// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    public sealed class GraphicsRendererDriverAtariST : GraphicsRendererDriverFull
    {
        /// <summary>
        /// Atari ST palette (as reproduced by Steem emulator).
        /// </summary>
        private static GraphicsColor[] atariPalette = new GraphicsColor[]
        {
            new GraphicsColor(0x00, 0x00, 0x00), // black
            new GraphicsColor(0x00, 0x00, 0xe0), // blue
            new GraphicsColor(0x00, 0x80, 0x00), // green
            new GraphicsColor(0x00, 0xa0, 0x80), // cyan
            new GraphicsColor(0xa0, 0x00, 0x00), // red
            new GraphicsColor(0xa0, 0x60, 0xc0), // magenta
            new GraphicsColor(0x80, 0x60, 0x00), // brown
            new GraphicsColor(0xa0, 0xa0, 0xa0), // gray
            new GraphicsColor(0x60, 0x60, 0x40), // dark gray
            new GraphicsColor(0x00, 0xa0, 0xe0), // bright blue
            new GraphicsColor(0x00, 0xc0, 0x00), // bright green
            new GraphicsColor(0x00, 0xe0, 0xc0), // bright cyan
            new GraphicsColor(0xe0, 0x40, 0x60), // bright red
            new GraphicsColor(0xe0, 0x80, 0xe0), // bright magenta
            new GraphicsColor(0xe0, 0xe0, 0x80), // yellow
            new GraphicsColor(0xe0, 0xe0, 0xe0), // white
        };

        public GraphicsRendererDriverAtariST()
        {
        }

        public override GraphicsColor[] GetPaletteColors(bool textMode)
        {
            return textMode ? this.GetPaletteTextColors() : atariPalette;
        }

        public override Font GetFont()
        {
            return new Font(Resources.FontAtariST8x8);
        }
    }
}
