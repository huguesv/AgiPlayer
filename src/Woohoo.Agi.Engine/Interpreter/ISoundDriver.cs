// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

public interface ISoundDriver
{
    int Initialize();

    int Open(int channel);

    void Close(int handle);

    void Lock();

    void Unlock();

    void SetState(bool playing);

    void Shutdown();
}
