// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

public sealed class GraphicsRendererDriverAmiga3 : GraphicsRendererDriverFull
{
    /// <summary>
    /// Amiga Palette 3 (as reproduced by WinUAE emulator).
    /// </summary>
    /// <remarks>
    /// Used by King's Quest III, Gold Rush!, Manhunter: New York,
    /// Manhunter: San Francisco, Police Quest I.
    /// </remarks>
    private static readonly GraphicsColor[] AmigaPalette3 =
    [
        new(0x00, 0x00, 0x00), // black
        new(0x00, 0x00, 0xbb), // blue
        new(0x00, 0xbb, 0x00), // green
        new(0x00, 0xbb, 0xbb), // cyan
        new(0xbb, 0x00, 0x00), // red
        new(0xbb, 0x00, 0xbb), // magenta
        new(0xcc, 0x77, 0x00), // brown
        new(0xbb, 0xbb, 0xbb), // gray
        new(0x77, 0x77, 0x77), // dark gray
        new(0x00, 0x00, 0xff), // bright blue
        new(0x00, 0xff, 0x00), // bright green
        new(0x00, 0xff, 0xff), // bright cyan
        new(0xff, 0x00, 0x00), // bright red
        new(0xff, 0x00, 0xff), // bright magenta
        new(0xff, 0xff, 0x00), // yellow
        new(0xff, 0xff, 0xff), // white
    ];

    public GraphicsRendererDriverAmiga3()
    {
    }

    public override GraphicsColor[] GetPaletteColors(bool textMode)
    {
        return textMode ? this.GetPaletteTextColors() : AmigaPalette3;
    }

    public override Font GetFont()
    {
        return new Font(PlayerResources.FontAmiga8x8);
    }
}
