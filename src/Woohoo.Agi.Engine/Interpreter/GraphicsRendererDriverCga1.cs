// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

public sealed class GraphicsRendererDriverCga1 : GraphicsRendererDriverCga
{
    private readonly byte[] cgaColorPalette =
    [
        0x00,
        0x00,
        0x00,
        0x02,
        0x00,
        0x00,
        0x01,
        0x04,
        0x05,
        0x03,
        0x01,
        0x00,
        0x04,
        0x0A,
        0x0A,
        0x06,
        0x03,
        0x0C,
        0x08,
        0x0B,
        0x0E,
        0x05,
        0x03,
        0x04,
        0x0A,
        0x04,
        0x03,
        0x07,
        0x0D,
        0x00,
        0x09,
        0x01,
        0x04,
        0x0B,
        0x05,
        0x05,
        0x0E,
        0x0E,
        0x0E,
        0x0C,
        0x02,
        0x08,
        0x0D,
        0x0D,
        0x07,
        0x0F,
        0x0F,
        0x0F,
    ];

    private readonly byte[] cga1ViewPalette =
    [
        0x00,
        0x00,
        0xCC,
        0x11,
        0xAA,
        0x22,
        0x99,
        0xDD,
        0x00,
        0x33,
        0x55,
        0x77,
        0xEE,
        0xEE,
        0xFF,
        0xFF,
    ];

    public GraphicsRendererDriverCga1()
    {
    }

    public override Palette Palette => Palette.Cga1;

    public override GraphicsColor[] GetPaletteColors(bool textMode)
    {
        GraphicsColor[] colors;

        if (textMode)
        {
            colors = this.GetPaletteTextColors();
        }
        else
        {
            colors =
            [
                new(0x00, 0x00, 0xaa),
                new(0x00, 0xaa, 0x00),
                new(0xaa, 0x00, 0x00),
                new(0xaa, 0x55, 0x00),
            ];
        }

        return colors;
    }

    public override DitheredColor DitherColor(byte color)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(color, 0x0f);

        return new DitheredColor(this.cgaColorPalette[(3 * color) + 1], this.cgaColorPalette[(3 * color) + 2]);
    }

    public override byte ConvertViewColor(byte color)
    {
        return (byte)(this.cga1ViewPalette[color] & 0x0f);
    }
}
