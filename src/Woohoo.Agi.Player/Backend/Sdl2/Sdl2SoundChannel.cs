// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl2;

#if USE_SDL2
using Woohoo.Agi.Interpreter;

internal class Sdl2SoundChannel
{
    public ToneChannel ToneChannel { get; set; }

    public int Available { get; set; }

    public AudioCallback Callback { get; set; }

    public int Handle { get; set; }
}
#endif
