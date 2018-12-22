// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    public sealed class GraphicsRendererDriverEga : GraphicsRendererDriverFull
    {
        /// <summary>
        /// EGA palette as reproduced by DOSBox.
        /// </summary>
        private static GraphicsColor[] egaPalette = new GraphicsColor[]
        {
            new GraphicsColor(0x00, 0x00, 0x00),
            new GraphicsColor(0x00, 0x00, 0xaa),
            new GraphicsColor(0x00, 0xaa, 0x00),
            new GraphicsColor(0x00, 0xaa, 0xaa),
            new GraphicsColor(0xaa, 0x00, 0x00),
            new GraphicsColor(0xaa, 0x00, 0xaa),
            new GraphicsColor(0xaa, 0x55, 0x00),
            new GraphicsColor(0xaa, 0xaa, 0xaa),
            new GraphicsColor(0x55, 0x55, 0x55),
            new GraphicsColor(0x55, 0x55, 0xff),
            new GraphicsColor(0x55, 0xff, 0x55),
            new GraphicsColor(0x55, 0xff, 0xff),
            new GraphicsColor(0xff, 0x55, 0x55),
            new GraphicsColor(0xff, 0x55, 0xff),
            new GraphicsColor(0xff, 0xff, 0x55),
            new GraphicsColor(0xff, 0xff, 0xff),
        };

        public GraphicsRendererDriverEga()
        {
        }

        public override GraphicsColor[] GetPaletteColors(bool textMode)
        {
            return textMode ? this.GetPaletteTextColors() : egaPalette;
        }

        public override Font GetFont()
        {
            return new Font(Resources.FontEga8x8);
        }
    }
}
