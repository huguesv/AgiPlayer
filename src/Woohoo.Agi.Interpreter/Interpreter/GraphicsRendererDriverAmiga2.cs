// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    public sealed class GraphicsRendererDriverAmiga2 : GraphicsRendererDriverFull
    {
        /// <summary>
        /// Amiga Palette 2 (as reproduced by WinUAE emulator).
        /// </summary>
        /// <remarks>
        /// Exactly like Palette 1 except for bright magenta.
        /// Used by Space Quest II.
        /// </remarks>
        private static GraphicsColor[] amigaPalette2 = new GraphicsColor[]
        {
            new GraphicsColor(0x00, 0x00, 0x00), // black
            new GraphicsColor(0x00, 0x00, 0xff), // blue
            new GraphicsColor(0x00, 0x88, 0x00), // green
            new GraphicsColor(0x00, 0xdd, 0xbb), // cyan
            new GraphicsColor(0xcc, 0x00, 0x00), // red
            new GraphicsColor(0xbb, 0x77, 0xdd), // magenta
            new GraphicsColor(0x88, 0x55, 0x00), // brown
            new GraphicsColor(0xbb, 0xbb, 0xbb), // gray
            new GraphicsColor(0x77, 0x77, 0x77), // dark gray
            new GraphicsColor(0x00, 0xbb, 0xff), // bright blue
            new GraphicsColor(0x00, 0xee, 0x00), // bright green
            new GraphicsColor(0x00, 0xff, 0xdd), // bright cyan
            new GraphicsColor(0xff, 0x99, 0x88), // bright red
            new GraphicsColor(0xdd, 0x00, 0xff), // bright magenta (really is bright magenta)
            new GraphicsColor(0xee, 0xee, 0x00), // yellow
            new GraphicsColor(0xff, 0xff, 0xff), // white
        };

        public GraphicsRendererDriverAmiga2()
        {
        }

        public override GraphicsColor[] GetPaletteColors(bool textMode)
        {
            return textMode ? this.GetPaletteTextColors() : amigaPalette2;
        }

        public override Font GetFont()
        {
            return new Font(Resources.FontAmiga8x8);
        }
    }
}
