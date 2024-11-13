// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

public sealed class GraphicsRendererDriverEga : GraphicsRendererDriverFull
{
    /// <summary>
    /// EGA palette as reproduced by DOSBox.
    /// </summary>
    private static readonly GraphicsColor[] EgaPalette =
    [
        new(0x00, 0x00, 0x00),
        new(0x00, 0x00, 0xaa),
        new(0x00, 0xaa, 0x00),
        new(0x00, 0xaa, 0xaa),
        new(0xaa, 0x00, 0x00),
        new(0xaa, 0x00, 0xaa),
        new(0xaa, 0x55, 0x00),
        new(0xaa, 0xaa, 0xaa),
        new(0x55, 0x55, 0x55),
        new(0x55, 0x55, 0xff),
        new(0x55, 0xff, 0x55),
        new(0x55, 0xff, 0xff),
        new(0xff, 0x55, 0x55),
        new(0xff, 0x55, 0xff),
        new(0xff, 0xff, 0x55),
        new(0xff, 0xff, 0xff),
    ];

    public GraphicsRendererDriverEga()
    {
    }

    public override GraphicsColor[] GetPaletteColors(bool textMode)
    {
        return textMode ? this.GetPaletteTextColors() : EgaPalette;
    }

    public override Font GetFont()
    {
        return new Font(PlayerResources.FontEga8x8);
    }
}
