// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl
{
#if USE_SDL
    using System;
    using System.Diagnostics;
    using Woohoo.Agi.Interpreter;
    using static Woohoo.Agi.Player.Backend.Sdl.NativeMethods;

    internal class SdlInputDriver : IInputDriver
    {
        private const int DelayMultiplier = 50;
        private const int SdlTickScale = 50;

        private static readonly int[] SystemAltAtoZMap = new int[]
        {
            30, 48, 46, 32, 18, 33, 34, 35, 23, 36, 37, 38, 50,
            49, 24, 25, 16, 19, 31, 20, 22, 47, 17, 45, 21, 44,
        };

        private static SdlKey[] dirMap = new SdlKey[]
        {
            new SdlKey(SDL_Keycode.SDLK_UP, InputEventDirection.Up),
            new SdlKey(SDL_Keycode.SDLK_PAGEUP, InputEventDirection.PageUp),
            new SdlKey(SDL_Keycode.SDLK_RIGHT, InputEventDirection.Right),
            new SdlKey(SDL_Keycode.SDLK_PAGEDOWN, InputEventDirection.PageDown),
            new SdlKey(SDL_Keycode.SDLK_DOWN, InputEventDirection.Down),
            new SdlKey(SDL_Keycode.SDLK_END, InputEventDirection.End),
            new SdlKey(SDL_Keycode.SDLK_LEFT, InputEventDirection.Left),
            new SdlKey(SDL_Keycode.SDLK_HOME, InputEventDirection.Home),
            new SdlKey(SDL_Keycode.SDLK_KP8, InputEventDirection.Up),
            new SdlKey(SDL_Keycode.SDLK_KP9, InputEventDirection.PageUp),
            new SdlKey(SDL_Keycode.SDLK_KP6, InputEventDirection.Right),
            new SdlKey(SDL_Keycode.SDLK_KP3, InputEventDirection.PageDown),
            new SdlKey(SDL_Keycode.SDLK_KP2, InputEventDirection.Down),
            new SdlKey(SDL_Keycode.SDLK_KP1, InputEventDirection.End),
            new SdlKey(SDL_Keycode.SDLK_KP4, InputEventDirection.Left),
            new SdlKey(SDL_Keycode.SDLK_KP7, InputEventDirection.Home),
        };

        private static SdlKey[] keySpecial = new SdlKey[]
        {
            new SdlKey(SDL_Keycode.SDLK_HOME, 0),
            new SdlKey(SDL_Keycode.SDLK_UP, 0),
            new SdlKey(SDL_Keycode.SDLK_PAGEUP, 0),
            new SdlKey(SDL_Keycode.SDLK_LEFT, 0),
            new SdlKey(SDL_Keycode.SDLK_RIGHT, 0),
            new SdlKey(SDL_Keycode.SDLK_END, 0),
            new SdlKey(SDL_Keycode.SDLK_DOWN, 0),
            new SdlKey(SDL_Keycode.SDLK_PAGEDOWN, 0),
        };

        private readonly InputEvent stopEgoEvent;

        private AgiInterpreter interpreter;
        private int tick;
        private IntPtr clockThread;
        private IntPtr joystick;
        private int joystickX;
        private int joystickY;

        /// <summary>
        /// Initializes a new instance of the <see cref="SdlInputDriver"/> class.
        /// </summary>
        internal SdlInputDriver()
        {
            this.stopEgoEvent = new InputEvent
            {
                Type = InputEventType.Direction,
                Data = InputEventDirection.None,
            };
        }

        void IInputDriver.ClockInitStartThread()
        {
            this.clockThread = SDL_CreateThread(this.ClockThread, null);
        }

        void IInputDriver.ClockDenitStopThread()
        {
            SDL_WaitThread(this.clockThread, out _);
        }

        void IInputDriver.InitializeEvents()
        {
            // SDL_EventState(SDL_JOYAXISMOTION, SDL_IGNORE);
            SDL_EventState(SDL_EventType.SDL_SYSWMEVENT, SDL_IGNORE);
            SDL_EventState(SDL_EventType.SDL_VIDEORESIZE, SDL_IGNORE);
            SDL_EventState(SDL_EventType.SDL_USEREVENT, SDL_IGNORE);
            SDL_EventState(SDL_EventType.SDL_ACTIVEEVENT, SDL_IGNORE);
            SDL_EventState(SDL_EventType.SDL_JOYBALLMOTION, SDL_IGNORE);
            SDL_EventState(SDL_EventType.SDL_JOYHATMOTION, SDL_IGNORE);

            SDL_EnableUNICODE(1);

            SDL_EnableKeyRepeat(SDL_DEFAULT_REPEAT_DELAY, SDL_DEFAULT_REPEAT_INTERVAL);

            if (SDL_NumJoysticks() > 0)
            {
                SDL_JoystickEventState(SDL_ENABLE);
                this.joystick = SDL_JoystickOpen(0);
            }

            (this as IInputDriver).ClearEvents();
        }

        void IInputDriver.InitializeDelay()
        {
            this.tick = SDL_GetTicks();
        }

        void IInputDriver.ClearEvents()
        {
            int x;
            int oneCount = 0;
            while ((x = SDL_PollEvent(out SDL_Event ev)) != 0)
            {
                if (x == 1)
                {
                    oneCount++;
                }

                if (oneCount > 10)
                {
                    Trace.WriteLine("events_clear: one event left error");
                    break;
                }
            }
        }

        void IInputDriver.DoDelay()
        {
            SDL_PumpEvents();
            this.interpreter.PollInput();
            SDL_Delay(1);

            while (((this.interpreter.State.Variables[Variables.Delay] * DelayMultiplier) > (SDL_GetTicks() - this.tick)) && !this.interpreter.State.Flags[Flags.PlayerCommandLine])
            {
                SDL_PumpEvents();
                this.interpreter.PollInput();
                SDL_Delay(5);
            }

            this.tick = SDL_GetTicks();
        }

        int IInputDriver.Tick()
        {
            return SDL_GetTicks();
        }

        int IInputDriver.TickScale()
        {
            return SdlTickScale;
        }

        void IInputDriver.Sleep(int milliseconds)
        {
            SDL_Delay(milliseconds);
        }

        bool IInputDriver.PollAcceptOrCancel(int timeout)
        {
            (this as IInputDriver).ClearEvents();

            if (timeout == 0)
            {
                // wait for user to press enter (true) or esc (false)
                int reply = 0;
                while ((reply = this.HasUserReply()) == 0xffff)
                {
                    (this as IInputDriver).Sleep(10);
                }

                return reply == 1;
            }
            else
            {
                int reply = 0;
                int tick = this.CalculateAgiTick() + (timeout * 10);
                while ((this.CalculateAgiTick() < tick) && ((reply = this.HasUserReply()) == 0xffff))
                {
                    (this as IInputDriver).Sleep(50);
                }

                return reply == 1;
            }
        }

        byte IInputDriver.CharacterPollLoop()
        {
            int key = 0xffff;
            while (key == 0xffff)
            {
                key = this.PollCharacter();
            }

            return (byte)key;
        }

        int IInputDriver.WaitCharacter()
        {
            int c = 0;
            do
            {
                c = this.PollCharacter();
                if ((c == 0) || (c == 0xffff))
                {
                    (this as IInputDriver).Sleep(10);
                }
            }
            while ((c == 0) || (c == 0xffff));

            return c;
        }

        bool IInputDriver.WriteEvent(int type, int data)
        {
            SDL_Event ev;
            ev.type = SDL_EventType.SDL_USEREVENT;
            ev.user.data1 = new IntPtr(type);
            ev.user.data2 = new IntPtr(data);

            if (SDL_PushEvent(out ev) == 0)
            {
                return true;
            }

            return false;
        }

        void IInputDriver.PollMouse(out int button, out int x, out int y)
        {
            button = SDL_GetMouseState(out x, out y);
        }

        InputEvent IInputDriver.PollEvent()
        {
            InputEvent e = this.EventWait(true);
            this.JoyButtonMap(e);
            return e;
        }

        InputEvent IInputDriver.ReadEvent()
        {
            return this.EventRead(false);
        }

        internal void SetInterpreter(AgiInterpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        private int ClockThread()
        {
            this.interpreter.Clock();
            return 0;
        }

        private int HasUserReply()
        {
            switch (this.PollCharacter())
            {
                case 0x0d:
                    // enter
                    return 1;

                case 0x1b:
                    // esc
                    return 0;

                default:
                    return 0xffff;
            }
        }

        private int PollCharacter()
        {
            var e = this.EventRead(false);
            if (e == null)
            {
                return 0;
            }

            this.JoyButtonMap(e);
            this.MouseButtonMap(e);

            int di = e.Type;
            int si = e.Data;

            if (di == 4 || di == 5)
            {
                di = 1;
                si = 0x0d; // CR
            }

            return di == 1 ? si : 0xffff;
        }

        private int CalculateAgiTick()
        {
            return SDL_GetTicks() / DelayMultiplier;
        }

        private InputEvent UserEventDecode(IntPtr data1, IntPtr data2)
        {
            return new InputEvent
            {
                Type = data1.ToInt32(),
                Data = data2.ToInt32(),
            };
        }

        private int DirKeyMap(SDL_keysym keysym)
        {
            // map directions to key symbols
            for (int i = 0; i < dirMap.Length; i++)
            {
                if (dirMap[i].Symbol == keysym.sym)
                {
                    return dirMap[i].Value;
                }
            }

            return 0xffff;
        }

        private int SystemKeyMap(SDL_keysym keysym)
        {
            if (keysym.sym >= SDL_Keycode.SDLK_F1 && keysym.sym <= SDL_Keycode.SDLK_F10)
            {
                return (keysym.sym - SDL_Keycode.SDLK_F1 + 0x3b) << 8;
            }
            else if ((keysym.mod & SDL_Keymod.KMOD_ALT) != 0)
            {
                if (keysym.sym >= SDL_Keycode.SDLK_a && keysym.sym <= SDL_Keycode.SDLK_z)
                {
                    return SystemAltAtoZMap[keysym.sym - SDL_Keycode.SDLK_a] << 8;
                }
            }
            else if ((keysym.mod & SDL_Keymod.KMOD_CTRL) != 0)
            {
                if (keysym.sym >= SDL_Keycode.SDLK_a && keysym.sym <= SDL_Keycode.SDLK_z)
                {
                    return keysym.sym - SDL_Keycode.SDLK_a + 1;
                }
            }
            else
            {
                switch (keysym.sym)
                {
                    case SDL_Keycode.SDLK_TAB:
                        return InputEventAscii.Tab;

                    case SDL_Keycode.SDLK_ESCAPE:
                        return InputEventAscii.Esc;

                    case SDL_Keycode.SDLK_BACKSPACE:
                    case SDL_Keycode.SDLK_DELETE:
                        return InputEventAscii.Backspace;

                    case SDL_Keycode.SDLK_KP_ENTER:
                    case SDL_Keycode.SDLK_RETURN:
                        return InputEventAscii.Enter;

                    default:
                        if ((keysym.unicode & 0xff80) == 0 && keysym.unicode != 0)
                        {
                            return keysym.unicode & 0x7f;
                        }

                        return keysym.unicode & 0xff;
                }
            }

            return 0;
        }

        private InputEvent KeyParse(SDL_keysym keysym)
        {
            // if the key is a direction, then map it to that
            // else, return the ascii thing back
            var e = new InputEvent();

            int direction = this.DirKeyMap(keysym);
            if (direction != 0xffff)
            {
                e.Type = InputEventType.Direction;
                e.Data = direction;
            }
            else
            {
                e.Type = InputEventType.Ascii;
                e.Data = this.SystemKeyMap(keysym);
            }

            return e;
        }

        private InputEvent EventKeyUp(SDL_keysym keysym)
        {
            InputEvent e = null;

            for (int i = 0; i < keySpecial.Length; i++)
            {
                if (keySpecial[i].Symbol == keysym.sym && keySpecial[i].Value != 0)
                {
                    keySpecial[i].Value = 0;
                    if (this.interpreter.State.WalkMode == WalkMode.HoldKey)
                    {
                        e = this.stopEgoEvent;
                        break;
                    }
                }
            }

            return e;
        }

        private InputEvent EventKeyDown(SDL_keysym keysym)
        {
            InputEvent e = null;

            switch (keysym.sym)
            {
                case SDL_Keycode.SDLK_KP5:
                case SDL_Keycode.SDLK_CLEAR:
                    e = this.stopEgoEvent;
                    break;

                case SDL_Keycode.SDLK_SCROLLOCK:
                    this.interpreter.ToggleTrace();
                    break;

                default:
                    int index = -1;
                    for (int i = 0; i < keySpecial.Length; i++)
                    {
                        if (keySpecial[i].Symbol == keysym.sym)
                        {
                            index = i;
                            break;
                        }
                    }

                    if (index != -1)
                    {
                        if (keySpecial[index].Value == 0)
                        {
                            // first press (not repeat)
                            for (int i = 0; i < keySpecial.Length; i++)
                            {
                                keySpecial[i].Value = 0;
                            }

                            keySpecial[index].Value++;
                            e = this.KeyParse(keysym);
                        }
                    }
                    else
                    {
                        // normal key
                        e = this.KeyParse(keysym);
                    }

                    break;
            }

            return e;
        }

        private InputEvent EventMouseButton(int button, int x, int y)
        {
            var e = new InputEvent
            {
                Type = InputEventType.Mouse,
            };

            switch (button)
            {
                case SDL_BUTTON_LEFT:
                    e.Data = MouseButton.Left;
                    break;

                case SDL_BUTTON_MIDDLE:
                    e.Data = MouseButton.Middle;
                    break;

                case SDL_BUTTON_RIGHT:
                    e.Data = MouseButton.Right;
                    break;

                default:
                    e.Data = MouseButton.Unknown;
                    break;
            }

            e.X = x;
            e.Y = y;

            return e;
        }

        private InputEvent EventJoyButton(int button)
        {
            var e = new InputEvent
            {
                Type = InputEventType.Ascii,
            };

            switch (button)
            {
                case 0:
                    e.Data = 0x101;
                    break;

                case 1:
                    e.Data = 0x201;
                    break;

                case 2:
                    e.Data = 0x301;
                    break;

                case 3:
                    e.Data = 0x401;
                    break;

                default:
                    e.Data = 0x101;
                    break;
            }

            return e;
        }

        private InputEvent EventJoyAxis(int axis, int val)
        {
            InputEvent e = null;

            if ((val < -3200) || (val > 3200))
            {
                if (axis == 0)
                {
                    // Left-right
                    e = new InputEvent
                    {
                        Type = InputEventType.Direction,
                    };

                    if (val < 0)
                    {
                        e.Data = InputEventDirection.Left;
                    }
                    else
                    {
                        e.Data = InputEventDirection.Right;
                    }

                    this.joystickX = val;
                }

                if (axis == 1)
                {
                    // Up-Down
                    e = new InputEvent
                    {
                        Type = InputEventType.Direction,
                    };

                    if (val < 0)
                    {
                        e.Data = InputEventDirection.Up;
                    }
                    else
                    {
                        e.Data = InputEventDirection.Down;
                    }

                    this.joystickY = val;
                }

                Trace.WriteLine(string.Format("-----> joystick axis = {0}, value = {1}", axis, val));
                SDL_Delay(50);
            }
            else
            {
                if (axis == 0)
                {
                    this.joystickX = val;
                }

                if (axis == 1)
                {
                    this.joystickY = val;
                }
            }

            return e;
        }

        private InputEvent PreviousEventJoyAxis()
        {
            InputEvent e = null;

            SDL_PumpEvents();

            SDL_Event[] evts = new SDL_Event[1];
            int res = SDL_PeepEvents(evts, 1, SDL_eventaction.SDL_PEEKEVENT, SDL_ALLEVENTS);
            if (res > 0)
            {
                return null;
            }

            if (this.joystickX < -3200)
            {
                if (this.joystickY < -3200)
                {
                    // left-up
                    e = new InputEvent
                    {
                        Type = InputEventType.Direction,
                        Data = InputEventDirection.Home,
                    };
                }
                else if (this.joystickY > 3200)
                {
                    // left-down
                    e = new InputEvent
                    {
                        Type = InputEventType.Direction,
                        Data = InputEventDirection.End,
                    };
                }
                else
                {
                    // left
                    e = new InputEvent
                    {
                        Type = InputEventType.Direction,
                        Data = InputEventDirection.Left,
                    };
                }
            }
            else if (this.joystickX > 3200)
            {
                if (this.joystickY < -3200)
                {
                    // right-up
                    e = new InputEvent
                    {
                        Type = InputEventType.Direction,
                        Data = InputEventDirection.PageUp,
                    };
                }
                else if (this.joystickY > 3200)
                {
                    // right-down
                    e = new InputEvent
                    {
                        Type = InputEventType.Direction,
                        Data = InputEventDirection.PageDown,
                    };
                }
                else
                {
                    // right
                    e = new InputEvent
                    {
                        Type = InputEventType.Direction,
                        Data = InputEventDirection.Right,
                    };
                }
            }
            else
            {
                if (this.joystickY < -3200)
                {
                    // up
                    e = new InputEvent
                    {
                        Type = InputEventType.Direction,
                        Data = InputEventDirection.Up,
                    };
                }
                else if (this.joystickY > 3200)
                {
                    // down
                    e = new InputEvent
                    {
                        Type = InputEventType.Direction,
                        Data = InputEventDirection.Down,
                    };
                }
                else
                {
                    // none
                }
            }

            return e;
        }

        private InputEvent EventRead(bool includeJoystickAxis)
        {
            // AgiEvent e = this.previous_event_joy_axis();
            InputEvent e = null;
            if (e == null)
            {
                while (SDL_PollEvent(out SDL_Event evt) != 0 && e == null)
                {
                    switch (evt.type)
                    {
                        case SDL_EventType.SDL_KEYDOWN:
                            e = this.EventKeyDown(evt.key.keysym);
                            break;
                        case SDL_EventType.SDL_KEYUP:
                            e = this.EventKeyUp(evt.key.keysym);
                            break;
                        case SDL_EventType.SDL_USEREVENT:
                            e = this.UserEventDecode(evt.user.data1, evt.user.data2);
                            break;
                        case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                            e = this.EventMouseButton(evt.button.button, evt.button.x, evt.button.y);
                            break;
                        case SDL_EventType.SDL_JOYBUTTONDOWN:
                            e = this.EventJoyButton(evt.jbutton.button);
                            break;
                        case SDL_EventType.SDL_JOYAXISMOTION:
                            if (includeJoystickAxis)
                            {
                                e = this.EventJoyAxis(evt.jaxis.axis, evt.jaxis.val);
                            }

                            break;
                        case SDL_EventType.SDL_QUIT:
                            throw new AbortException();
                    }
                }
            }

            return e;
        }

        private InputEvent EventWait()
        {
            return this.EventWait(false);
        }

        private InputEvent EventWait(bool includeJoystickAxis)
        {
            InputEvent e = null;

            do
            {
                e = this.EventRead(includeJoystickAxis);
                if (e == null)
                {
                    (this as IInputDriver).Sleep(10);
                }
            }
            while (e == null);

            return e;
        }

        private void JoyButtonMap(InputEvent e)
        {
            if (e.Type == InputEventType.Ascii)
            {
                if (e.Data == 0x101 || e.Data == 0x301)
                {
                    // enter
                    e.Data = InputEventAscii.Enter;
                }
                else if (e.Data == 0x201 || e.Data == 0x401)
                {
                    // esc
                    e.Data = InputEventAscii.Esc;
                }
            }
        }

        private void MouseButtonMap(InputEvent e)
        {
            if (e.Type == InputEventType.Mouse)
            {
                if (e.Data == 1)
                {
                    // enter
                    e.Data = InputEventAscii.Enter;
                    e.Type = InputEventType.Ascii;
                }
                else if (e.Data == 2)
                {
                    // esc
                    e.Data = InputEventAscii.Esc;
                    e.Type = InputEventType.Ascii;
                }
            }
        }
    }
#endif
}
