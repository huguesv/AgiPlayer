// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl3;

#if USE_SDL3

using Woohoo.Agi.Engine.Interpreter;

internal class Sdl3SoundChannel
{
    public ToneChannel ToneChannel { get; set; }

    public int Available { get; set; }

    public AudioCallback Callback { get; set; }

    public int Handle { get; set; }
}

#endif // USE_SDL3
