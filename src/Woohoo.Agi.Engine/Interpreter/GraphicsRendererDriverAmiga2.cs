// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

public sealed class GraphicsRendererDriverAmiga2 : GraphicsRendererDriverFull
{
    /// <summary>
    /// Amiga Palette 2 (as reproduced by WinUAE emulator).
    /// </summary>
    /// <remarks>
    /// Exactly like Palette 1 except for bright magenta.
    /// Used by Space Quest II.
    /// </remarks>
    private static readonly GraphicsColor[] AmigaPalette2 =
    [
        new(0x00, 0x00, 0x00), // black
        new(0x00, 0x00, 0xff), // blue
        new(0x00, 0x88, 0x00), // green
        new(0x00, 0xdd, 0xbb), // cyan
        new(0xcc, 0x00, 0x00), // red
        new(0xbb, 0x77, 0xdd), // magenta
        new(0x88, 0x55, 0x00), // brown
        new(0xbb, 0xbb, 0xbb), // gray
        new(0x77, 0x77, 0x77), // dark gray
        new(0x00, 0xbb, 0xff), // bright blue
        new(0x00, 0xee, 0x00), // bright green
        new(0x00, 0xff, 0xdd), // bright cyan
        new(0xff, 0x99, 0x88), // bright red
        new(0xdd, 0x00, 0xff), // bright magenta (really is bright magenta)
        new(0xee, 0xee, 0x00), // yellow
        new(0xff, 0xff, 0xff), // white
    ];

    public GraphicsRendererDriverAmiga2()
    {
    }

    public override GraphicsColor[] GetPaletteColors(bool textMode)
    {
        return textMode ? this.GetPaletteTextColors() : AmigaPalette2;
    }

    public override Font GetFont()
    {
        return new Font(PlayerResources.FontAmiga8x8);
    }
}
