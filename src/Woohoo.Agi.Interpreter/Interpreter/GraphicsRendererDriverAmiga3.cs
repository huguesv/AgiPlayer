// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    public sealed class GraphicsRendererDriverAmiga3 : GraphicsRendererDriverFull
    {
        /// <summary>
        /// Amiga Palette 3 (as reproduced by WinUAE emulator).
        /// </summary>
        /// <remarks>
        /// Used by King's Quest III, Gold Rush!, Manhunter: New York,
        /// Manhunter: San Francisco, Police Quest I.
        /// </remarks>
        private static GraphicsColor[] amigaPalette3 = new GraphicsColor[]
        {
            new GraphicsColor(0x00, 0x00, 0x00), // black
            new GraphicsColor(0x00, 0x00, 0xbb), // blue
            new GraphicsColor(0x00, 0xbb, 0x00), // green
            new GraphicsColor(0x00, 0xbb, 0xbb), // cyan
            new GraphicsColor(0xbb, 0x00, 0x00), // red
            new GraphicsColor(0xbb, 0x00, 0xbb), // magenta
            new GraphicsColor(0xcc, 0x77, 0x00), // brown
            new GraphicsColor(0xbb, 0xbb, 0xbb), // gray
            new GraphicsColor(0x77, 0x77, 0x77), // dark gray
            new GraphicsColor(0x00, 0x00, 0xff), // bright blue
            new GraphicsColor(0x00, 0xff, 0x00), // bright green
            new GraphicsColor(0x00, 0xff, 0xff), // bright cyan
            new GraphicsColor(0xff, 0x00, 0x00), // bright red
            new GraphicsColor(0xff, 0x00, 0xff), // bright magenta
            new GraphicsColor(0xff, 0xff, 0x00), // yellow
            new GraphicsColor(0xff, 0xff, 0xff), // white
        };

        public GraphicsRendererDriverAmiga3()
        {
        }

        public override GraphicsColor[] GetPaletteColors(bool textMode)
        {
            return textMode ? this.GetPaletteTextColors() : amigaPalette3;
        }

        public override Font GetFont()
        {
            return new Font(Resources.FontAmiga8x8);
        }
    }
}
