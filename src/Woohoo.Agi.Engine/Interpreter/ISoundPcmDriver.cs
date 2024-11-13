// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

public delegate int AudioCallback(ToneChannel ch, short[] buffer, int count);

public interface ISoundPcmDriver
{
    void SetInterpreter(AgiInterpreter interpreter);

    int Initialize(int freq, int format);

    void Shutdown();

    int Open(AudioCallback callback, ToneChannel tc);

    void Close(int handle);

    void SetState(bool playing);

    void Lock();

    void Unlock();
}
