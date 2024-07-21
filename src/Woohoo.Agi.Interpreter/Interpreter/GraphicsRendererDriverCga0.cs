// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

public sealed class GraphicsRendererDriverCga0 : GraphicsRendererDriverCga
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

    private readonly byte[] cga0ViewPalette =
    [
        0x00,
        0x22,
        0x11,
        0x33,
        0x44,
        0x66,
        0x88,
        0x55,
        0xAA,
        0x77,
        0x99,
        0xBB,
        0xEE,
        0xCC,
        0xDD,
        0xFF,
    ];

    public GraphicsRendererDriverCga0()
    {
    }

    public override Palette Palette => Palette.Cga0;

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
                new(0x00, 0x00, 0x00),
                new(0x55, 0xff, 0xff),
                new(0xff, 0x55, 0xff),
                new(0xff, 0xff, 0xff),
            ];
        }

        return colors;
    }

    public override DitheredColor DitherColor(byte color)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(color, 0x0f);

        byte val = this.cgaColorPalette[3 * color];

        return new DitheredColor(val, val);
    }

    public override byte ConvertViewColor(byte color)
    {
        return (byte)(this.cga0ViewPalette[color] & 0x0f);
    }
}
