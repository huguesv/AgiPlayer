// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

public sealed class GraphicsRendererDriverAtariST : GraphicsRendererDriverFull
{
    /// <summary>
    /// Atari ST palette (as reproduced by Steem emulator).
    /// </summary>
    private static readonly GraphicsColor[] AtariPalette =
    [
        new(0x00, 0x00, 0x00), // black
        new(0x00, 0x00, 0xe0), // blue
        new(0x00, 0x80, 0x00), // green
        new(0x00, 0xa0, 0x80), // cyan
        new(0xa0, 0x00, 0x00), // red
        new(0xa0, 0x60, 0xc0), // magenta
        new(0x80, 0x60, 0x00), // brown
        new(0xa0, 0xa0, 0xa0), // gray
        new(0x60, 0x60, 0x40), // dark gray
        new(0x00, 0xa0, 0xe0), // bright blue
        new(0x00, 0xc0, 0x00), // bright green
        new(0x00, 0xe0, 0xc0), // bright cyan
        new(0xe0, 0x40, 0x60), // bright red
        new(0xe0, 0x80, 0xe0), // bright magenta
        new(0xe0, 0xe0, 0x80), // yellow
        new(0xe0, 0xe0, 0xe0), // white
    ];

    public GraphicsRendererDriverAtariST()
    {
    }

    public override GraphicsColor[] GetPaletteColors(bool textMode)
    {
        return textMode ? this.GetPaletteTextColors() : AtariPalette;
    }

    public override Font GetFont()
    {
        return new Font(PlayerResources.FontAtariST8x8);
    }
}
