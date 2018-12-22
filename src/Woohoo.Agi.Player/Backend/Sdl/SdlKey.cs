// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl
{
#if USE_SDL
    internal class SdlKey
    {
        public SdlKey(int symbol, int value)
        {
            this.Symbol = symbol;
            this.Value = value;
        }

        public int Symbol { get; }

        public int Value { get; set; }
    }
#endif
}
