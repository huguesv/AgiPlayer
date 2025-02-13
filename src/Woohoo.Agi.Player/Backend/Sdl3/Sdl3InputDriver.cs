// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl3;

#if USE_SDL3

using System;
using Woohoo.Agi.Engine.Interpreter;
using static SDL3.SDL;

internal class Sdl3InputDriver : IInputDriver
{
    private const int DelayMultiplier = 50;
    private const int SdlTickScale = 50;

    private static int[] systemAltAtoZMap =
    [
        30, 48, 46, 32, 18, 33, 34, 35, 23, 36, 37, 38, 50,
        49, 24, 25, 16, 19, 31, 20, 22, 47, 17, 45, 21, 44,
    ];

    private static Sdl3Key[] dirMap =
    [
        new Sdl3Key(SDL_Keycode.SDLK_UP, InputEventDirection.Up),
        new Sdl3Key(SDL_Keycode.SDLK_PAGEUP, InputEventDirection.PageUp),
        new Sdl3Key(SDL_Keycode.SDLK_RIGHT, InputEventDirection.Right),
        new Sdl3Key(SDL_Keycode.SDLK_PAGEDOWN, InputEventDirection.PageDown),
        new Sdl3Key(SDL_Keycode.SDLK_DOWN, InputEventDirection.Down),
        new Sdl3Key(SDL_Keycode.SDLK_END, InputEventDirection.End),
        new Sdl3Key(SDL_Keycode.SDLK_LEFT, InputEventDirection.Left),
        new Sdl3Key(SDL_Keycode.SDLK_HOME, InputEventDirection.Home),
        new Sdl3Key(SDL_Keycode.SDLK_KP_8, InputEventDirection.Up),
        new Sdl3Key(SDL_Keycode.SDLK_KP_9, InputEventDirection.PageUp),
        new Sdl3Key(SDL_Keycode.SDLK_KP_6, InputEventDirection.Right),
        new Sdl3Key(SDL_Keycode.SDLK_KP_3, InputEventDirection.PageDown),
        new Sdl3Key(SDL_Keycode.SDLK_KP_2, InputEventDirection.Down),
        new Sdl3Key(SDL_Keycode.SDLK_KP_1, InputEventDirection.End),
        new Sdl3Key(SDL_Keycode.SDLK_KP_4, InputEventDirection.Left),
        new Sdl3Key(SDL_Keycode.SDLK_KP_7, InputEventDirection.Home),
    ];

    private static Sdl3Key[] keySpecial =
    [
        new Sdl3Key(SDL_Keycode.SDLK_HOME, 0),
        new Sdl3Key(SDL_Keycode.SDLK_UP, 0),
        new Sdl3Key(SDL_Keycode.SDLK_PAGEUP, 0),
        new Sdl3Key(SDL_Keycode.SDLK_LEFT, 0),
        new Sdl3Key(SDL_Keycode.SDLK_RIGHT, 0),
        new Sdl3Key(SDL_Keycode.SDLK_END, 0),
        new Sdl3Key(SDL_Keycode.SDLK_DOWN, 0),
        new Sdl3Key(SDL_Keycode.SDLK_PAGEDOWN, 0),
    ];

    private AgiInterpreter interpreter;
    private ulong tick;
    private nint clockThread;
    private InputEvent stopEgoEvent;
    private int joystickX;
    private int joystickY;

    public Sdl3InputDriver()
    {
        this.stopEgoEvent = new InputEvent
        {
            Type = InputEventType.Direction,
            Data = InputEventDirection.None,
        };
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

    void IInputDriver.ClearEvents()
    {
        while (SDL_PollEvent(out _))
        {
        }
    }

    void IInputDriver.ClockInitStartThread()
    {
        this.clockThread = SDL_CreateThreadRuntime(this.ClockThread, "Clock", 0, 0, 0);
    }

    void IInputDriver.ClockDenitStopThread()
    {
        SDL_WaitThread(this.clockThread, 0);
    }

    void IInputDriver.DoDelay()
    {
        SDL_PumpEvents();
        this.interpreter.PollInput();
        SDL_Delay(1);

        while (((ulong)(this.interpreter.State.Variables[Variables.Delay] * DelayMultiplier) > (SDL_GetTicks() - this.tick)) && !this.interpreter.State.Flags[Flags.PlayerCommandLine])
        {
            SDL_PumpEvents();
            this.interpreter.PollInput();
            SDL_Delay(5);
        }

        this.tick = SDL_GetTicks();
    }

    void IInputDriver.InitializeDelay()
    {
        this.tick = SDL_GetTicks();
    }

    void IInputDriver.InitializeEvents()
    {
        SDL_SetEventEnabled((uint)SDL_EventType.SDL_EVENT_USER, false);
        SDL_SetEventEnabled((uint)SDL_EventType.SDL_EVENT_JOYSTICK_BALL_MOTION, false);
        SDL_SetEventEnabled((uint)SDL_EventType.SDL_EVENT_JOYSTICK_HAT_MOTION, false);

        (this as IInputDriver).ClearEvents();
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

    InputEvent IInputDriver.PollEvent()
    {
        InputEvent e = this.EventWait(true);
        this.JoyButtonMap(e);
        return e;
    }

    void IInputDriver.PollMouse(out int button, out int screenScaledX, out int screenScaledY)
    {
        button = (int)SDL_GetMouseState(out float x, out float y);
        screenScaledX = (int)x;
        screenScaledY = (int)y;
    }

    InputEvent IInputDriver.ReadEvent()
    {
        InputEvent e = this.EventRead(false);
        return e;
    }

    void IInputDriver.Sleep(int milliseconds)
    {
        SDL_Delay((uint)milliseconds);
    }

    int IInputDriver.Tick()
    {
        return (int)SDL_GetTicks();
    }

    int IInputDriver.TickScale()
    {
        return SdlTickScale;
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
        SDL_Event ev = default(SDL_Event);
        ev.type = (uint)SDL_EventType.SDL_EVENT_USER;
        ev.user.data1 = new IntPtr(type);
        ev.user.data2 = new IntPtr(data);

        if (SDL_PushEvent(ref ev))
        {
            return true;
        }

        return false;
    }

    internal void SetInterpreter(AgiInterpreter interpreter)
    {
        this.interpreter = interpreter;
    }

    private int ClockThread(IntPtr data)
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
        InputEvent e = this.EventRead(false);
        if (e is null)
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
        return (int)(SDL_GetTicks() / DelayMultiplier);
    }

    private InputEvent UserEventDecode(IntPtr data1, IntPtr data2)
    {
        InputEvent e = new InputEvent();
        e.Type = data1.ToInt32();
        e.Data = data2.ToInt32();

        return e;
    }

    private int DirKeyMap(SDL_Keycode sym)
    {
        // map directions to key symbols
        for (int i = 0; i < dirMap.Length; i++)
        {
            if (dirMap[i].Symbol == sym)
            {
                return dirMap[i].Value;
            }
        }

        return 0xffff;
    }

    private int SystemKeyMap(SDL_Keycode sym, SDL_Keymod mod)
    {
        if (sym >= SDL_Keycode.SDLK_F1 && sym <= SDL_Keycode.SDLK_F10)
        {
            return (int)(sym - SDL_Keycode.SDLK_F1 + 0x3b) << 8;
        }
        else if ((mod & SDL_Keymod.SDL_KMOD_ALT) != 0)
        {
            if (sym >= SDL_Keycode.SDLK_A && sym <= SDL_Keycode.SDLK_Z)
            {
                return systemAltAtoZMap[sym - SDL_Keycode.SDLK_A] << 8;
            }
        }
        else if ((mod & SDL_Keymod.SDL_KMOD_CTRL) != 0)
        {
            if (sym >= SDL_Keycode.SDLK_A && sym <= SDL_Keycode.SDLK_Z)
            {
                return (int)(sym - SDL_Keycode.SDLK_A + 1);
            }
        }
        else
        {
            switch (sym)
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
                    return (int)((int)sym & 0xff);
            }
        }

        return 0;
    }

    private InputEvent KeyParse(SDL_Keycode sym, SDL_Keymod mod)
    {
        // if the key is a direction, then map it to that
        // else, return the ascii thing back
        InputEvent e = new InputEvent();

        int direction = this.DirKeyMap(sym);
        if (direction != 0xffff)
        {
            e.Type = InputEventType.Direction;
            e.Data = direction;
        }
        else
        {
            e.Type = InputEventType.Ascii;
            e.Data = this.SystemKeyMap(sym, mod);
        }

        return e;
    }

    private InputEvent EventKeyUp(SDL_Keycode sym, SDL_Keymod mod)
    {
        InputEvent e = null;

        for (int i = 0; i < keySpecial.Length; i++)
        {
            if (keySpecial[i].Symbol == sym && keySpecial[i].Value != 0)
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

    private InputEvent EventKeyDown(SDL_Keycode sym, SDL_Keymod mod)
    {
        InputEvent e = null;

        switch (sym)
        {
            case SDL_Keycode.SDLK_KP_5:
            case SDL_Keycode.SDLK_CLEAR:
                e = this.stopEgoEvent;
                break;

            case SDL_Keycode.SDLK_SCROLLLOCK:
                this.interpreter.ToggleTrace();
                break;

            default:
                int index = -1;
                for (int i = 0; i < keySpecial.Length; i++)
                {
                    if (keySpecial[i].Symbol == sym)
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
                        e = this.KeyParse(sym, mod);
                    }
                }
                else
                {
                    // normal key
                    e = this.KeyParse(sym, mod);
                }

                break;
        }

        return e;
    }

    private InputEvent EventMouseButton(int button, int x, int y)
    {
        InputEvent e = new InputEvent();
        e.Type = InputEventType.Mouse;

        switch ((uint)button)
        {
            case (uint)SDL_MouseButtonFlags.SDL_BUTTON_LMASK:
                e.Data = MouseButton.Left;
                break;

            case (uint)SDL_MouseButtonFlags.SDL_BUTTON_MMASK:
                e.Data = MouseButton.Middle;
                break;

            case (uint)SDL_MouseButtonFlags.SDL_BUTTON_RMASK:
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
        InputEvent e = new InputEvent();
        e.Type = InputEventType.Ascii;

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
                e = new InputEvent();
                e.Type = InputEventType.Direction;
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
                e = new InputEvent();
                e.Type = InputEventType.Direction;
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

    private InputEvent EventRead(bool includeJoystickAxis)
    {
        // AgiEvent e = this.previous_event_joy_axis();
        InputEvent e = null;
        if (e is null)
        {
            SDL_Event evt;
            while (SDL_PollEvent(out evt))
            {
                switch (evt.type)
                {
                    case (uint)SDL_EventType.SDL_EVENT_KEY_DOWN:
                        e = this.EventKeyDown((SDL_Keycode)evt.key.key, evt.key.mod);
                        break;
                    case (uint)SDL_EventType.SDL_EVENT_KEY_UP:
                        e = this.EventKeyUp((SDL_Keycode)evt.key.key, evt.key.mod);
                        break;
                    case (uint)SDL_EventType.SDL_EVENT_USER:
                        e = this.UserEventDecode(evt.user.data1, evt.user.data2);
                        break;
                    case (uint)SDL_EventType.SDL_EVENT_MOUSE_BUTTON_DOWN:
                        e = this.EventMouseButton(evt.button.button, (int)evt.button.x, (int)evt.button.y);
                        break;
                    case (uint)SDL_EventType.SDL_EVENT_QUIT:
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
            if (e is null)
            {
                (this as IInputDriver).Sleep(10);
            }
        }
        while (e is null);

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

#endif // USE_SDL_3
