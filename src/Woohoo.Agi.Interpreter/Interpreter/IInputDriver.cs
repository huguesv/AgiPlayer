// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    public interface IInputDriver
    {
        void ClockInitStartThread();

        void ClockDenitStopThread();

        void InitializeEvents();

        void InitializeDelay();

        byte CharacterPollLoop();

        void ClearEvents();

        void DoDelay();

        int Tick();

        int TickScale();

        void Sleep(int milliseconds);

        int WaitCharacter();

        bool WriteEvent(int type, int data);

        void PollMouse(out int button, out int screenScaledX, out int screenScaledY);

        bool PollAcceptOrCancel(int timeout);

        InputEvent PollEvent();

        InputEvent ReadEvent();
    }
}
