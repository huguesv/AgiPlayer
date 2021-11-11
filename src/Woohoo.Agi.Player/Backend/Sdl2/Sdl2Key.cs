// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl2;

#if USE_SDL2
using static Woohoo.Agi.Player.Backend.Sdl2.NativeMethods;

internal class Sdl2Key
{
    public Sdl2Key(SDL_Keycode symbol, int value)
    {
        this.Symbol = symbol;
        this.Value = value;
    }

    public SDL_Keycode Symbol { get; }

    public int Value { get; set; }
}
#endif
