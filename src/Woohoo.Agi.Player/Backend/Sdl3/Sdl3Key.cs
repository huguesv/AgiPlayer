// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl3;

#if USE_SDL3

using static SDL3.SDL;

internal class Sdl3Key
{
    public Sdl3Key(SDL_Keycode symbol, int value)
    {
        this.Symbol = symbol;
        this.Value = value;
    }

    public SDL_Keycode Symbol { get; }

    public int Value { get; set; }
}

#endif // USE_SDL_3
