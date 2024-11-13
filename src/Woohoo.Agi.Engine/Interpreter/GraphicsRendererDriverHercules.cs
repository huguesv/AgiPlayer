// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

using Woohoo.Agi.Engine.Resources;

public sealed class GraphicsRendererDriverHercules : GraphicsRendererDriver
{
    private readonly byte[] line0Column0 =
    [
        0x00,
        0x20,
        0x40,
        0xA0,
        0x28,
        0x80,
        0x12,
        0xA5,
        0x80,
        0x7f,
        0xAD,
        0xFF,
        0xFA,
        0x7B,
        0xDF,
        0xFF,
    ];

    private readonly byte[] line0Column1 =
    [
        0x00,
        0x20,
        0x08,
        0xA0,
        0x28,
        0x80,
        0x12,
        0xA5,
        0x80,
        0xdf,
        0xAD,
        0x7E,
        0xFA,
        0x7B,
        0xDF,
        0xFF,
    ];

    private readonly byte[] line1Column0 =
    [
        0x00,
        0x00,
        0x80,
        0xA0,
        0x28,
        0x80,
        0x48,
        0xA5,
        0x20,
        0xdf,
        0x57,
        0xDF,
        0xFA,
        0xDE,
        0xFF,
        0xFF,
    ];

    private readonly byte[] line1Column1 =
    [
        0x00,
        0x00,
        0x04,
        0xA0,
        0x28,
        0x80,
        0x48,
        0xA5,
        0x20,
        0x7f,
        0x57,
        0xFD,
        0xFA,
        0xDE,
        0xFF,
        0xFF,
    ];

    private readonly byte[] line2Column0 =
    [
        0x00,
        0x80,
        0x20,
        0xA0,
        0x28,
        0x80,
        0x12,
        0xA5,
        0x80,
        0x7f,
        0xAD,
        0xE7,
        0xFA,
        0x7B,
        0x7F,
        0xFF,
    ];

    private readonly byte[] line2Column1 =
    [
        0x00,
        0x80,
        0x01,
        0xA0,
        0x28,
        0x80,
        0x12,
        0xA5,
        0x80,
        0xdf,
        0xAD,
        0xFF,
        0xFA,
        0x7B,
        0x7F,
        0xFF,
    ];

    private readonly byte[] line3Column0 =
    [
        0x00,
        0x00,
        0x10,
        0xA0,
        0x28,
        0x80,
        0x48,
        0xA5,
        0x20,
        0xdf,
        0x57,
        0xFB,
        0xFA,
        0xDE,
        0xFF,
        0xFF,
    ];

    private readonly byte[] line3Column1 =
    [
        0x00,
        0x00,
        0x02,
        0xA0,
        0x28,
        0x80,
        0x48,
        0xA5,
        0x20,
        0x7f,
        0x57,
        0xBF,
        0xFA,
        0xDE,
        0xFF,
        0xFF,
    ];

    private readonly GraphicsColor whiteColor;

    public GraphicsRendererDriverHercules(GraphicsColor whiteColor)
    {
        this.whiteColor = whiteColor;
        this.CreateRenderBuffer(640, 336);
    }

    public override byte DisplayType => Woohoo.Agi.Engine.Interpreter.DisplayType.Hercules;

    public override Palette Palette => Palette.BlackWhite;

    public override int Width => 640;

    public override int Height => 336;

    public override int RenderScaleX => 2;

    public override int RenderScaleY => 2;

    public override byte MessageBoxBackground => 0x0f;

    public override byte MessageBoxBorder => 0x00;

    public override Font GetFont()
    {
        return new Font(PlayerResources.FontHercules16x16);
    }

    public override byte CombineTextColors(byte fg, byte bg, bool textMode)
    {
        byte combine;

        if (textMode)
        {
            combine = (byte)(fg | (bg << 4));
        }
        else
        {
            if (bg != 0)
            {
                combine = 0x8f;
            }
            else
            {
                combine = fg;
            }
        }

        return combine;
    }

    public override byte CalculateTextBackground(byte bg, bool textMode)
    {
        if (!textMode && bg != 0)
        {
            return 0xff;
        }

        return 0;
    }

    public override byte ConvertTextBackground(byte attrib, bool textMode)
    {
        attrib = (byte)((attrib & 0xf0) >> 4);
        if (textMode)
        {
            // textmode does not support bright backgrounds
            attrib = (byte)(attrib & 0x7);
        }

        return attrib;
    }

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
                this.whiteColor,
            ];
        }

        return colors;
    }

    public override DitheredColor DitherColor(byte color)
    {
        return new DitheredColor(color, color);
    }

    public override byte ConvertViewColor(byte color)
    {
        return color;
    }

    public override void RenderPictureBuffer(PictureRectangle rect, bool picBuffRotate)
    {
        if (rect.X < 0 || rect.X > PictureResource.Width ||
            rect.Y < 0 || rect.Y > PictureResource.Height ||
            rect.Width < 0 || rect.Height < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rect));
        }

        byte[] buffer;
        if (picBuffRotate)
        {
            buffer = this.PictureBuffer.Priority;
        }
        else
        {
            buffer = this.PictureBuffer.Visual;
        }

        for (int j = rect.Y; j > (rect.Y - rect.Height); j--)
        {
            for (int i = rect.X; i < (rect.X + rect.Width); i++)
            {
                byte color = (byte)(buffer[(PictureResource.Width * j) + i] & 0xf);

                this.RenderPixel(i, j, color);
            }
        }
    }

    public override void RenderSolidRectangle(PictureRectangle rect, byte color)
    {
        if (rect.X < 0 || rect.X > PictureResource.Width ||
            rect.Y < 0 || rect.Y > PictureResource.Height ||
            rect.Width < 0 || rect.Height < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rect));
        }

        for (int j = rect.Y; j > (rect.Y - rect.Height); j--)
        {
            for (int i = rect.X; i < (rect.X + rect.Width); i++)
            {
                this.RenderPixel(i, j, color);
            }
        }
    }

    protected override GraphicsColor[] GetPaletteTextColors()
    {
        return
        [
            new(0x00, 0x00, 0x00),
            this.whiteColor,
            this.whiteColor,
            this.whiteColor,
            this.whiteColor,
            this.whiteColor,
            this.whiteColor,
            this.whiteColor,
            this.whiteColor,
            this.whiteColor,
            this.whiteColor,
            this.whiteColor,
            this.whiteColor,
            this.whiteColor,
            this.whiteColor,
            this.whiteColor,
        ];
    }

    private void RenderPixel(int i, int j, byte color)
    {
        if ((j & 1) == 0)
        {
            if ((j & 2) == 0)
            {
                if ((i & 1) == 0)
                {
                    this.RenderBuffer.SetPixel(i * 4, j * 2, (byte)((this.line2Column1[color] >> 7) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, j * 2, (byte)((this.line2Column1[color] >> 6) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, j * 2, (byte)((this.line2Column1[color] >> 5) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, j * 2, (byte)((this.line2Column1[color] >> 4) & 0x01));
                    this.RenderBuffer.SetPixel(i * 4, (j * 2) + 1, (byte)((this.line2Column1[color] >> 3) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, (j * 2) + 1, (byte)((this.line2Column1[color] >> 2) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, (j * 2) + 1, (byte)((this.line2Column1[color] >> 1) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, (j * 2) + 1, (byte)(this.line2Column1[color] & 0x01));
                }
                else
                {
                    this.RenderBuffer.SetPixel(i * 4, j * 2, (byte)((this.line2Column0[color] >> 7) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, j * 2, (byte)((this.line2Column0[color] >> 6) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, j * 2, (byte)((this.line2Column0[color] >> 5) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, j * 2, (byte)((this.line2Column0[color] >> 4) & 0x01));
                    this.RenderBuffer.SetPixel(i * 4, (j * 2) + 1, (byte)((this.line2Column0[color] >> 3) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, (j * 2) + 1, (byte)((this.line2Column0[color] >> 2) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, (j * 2) + 1, (byte)((this.line2Column0[color] >> 1) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, (j * 2) + 1, (byte)(this.line2Column0[color] & 0x01));
                }
            }
            else
            {
                if ((i & 1) == 0)
                {
                    this.RenderBuffer.SetPixel(i * 4, j * 2, (byte)((this.line0Column1[color] >> 7) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, j * 2, (byte)((this.line0Column1[color] >> 6) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, j * 2, (byte)((this.line0Column1[color] >> 5) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, j * 2, (byte)((this.line0Column1[color] >> 4) & 0x01));
                    this.RenderBuffer.SetPixel(i * 4, (j * 2) + 1, (byte)((this.line0Column1[color] >> 3) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, (j * 2) + 1, (byte)((this.line0Column1[color] >> 2) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, (j * 2) + 1, (byte)((this.line0Column1[color] >> 1) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, (j * 2) + 1, (byte)(this.line0Column1[color] & 0x01));
                }
                else
                {
                    this.RenderBuffer.SetPixel(i * 4, j * 2, (byte)((this.line0Column0[color] >> 7) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, j * 2, (byte)((this.line0Column0[color] >> 6) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, j * 2, (byte)((this.line0Column0[color] >> 5) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, j * 2, (byte)((this.line0Column0[color] >> 4) & 0x01));
                    this.RenderBuffer.SetPixel(i * 4, (j * 2) + 1, (byte)((this.line0Column0[color] >> 3) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, (j * 2) + 1, (byte)((this.line0Column0[color] >> 2) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, (j * 2) + 1, (byte)((this.line0Column0[color] >> 1) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, (j * 2) + 1, (byte)(this.line0Column0[color] & 0x01));
                }
            }
        }
        else
        {
            if ((j & 2) == 0)
            {
                if ((i & 1) == 0)
                {
                    this.RenderBuffer.SetPixel(i * 4, j * 2, (byte)((this.line3Column1[color] >> 7) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, j * 2, (byte)((this.line3Column1[color] >> 6) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, j * 2, (byte)((this.line3Column1[color] >> 5) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, j * 2, (byte)((this.line3Column1[color] >> 4) & 0x01));
                    this.RenderBuffer.SetPixel(i * 4, (j * 2) + 1, (byte)((this.line3Column1[color] >> 3) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, (j * 2) + 1, (byte)((this.line3Column1[color] >> 2) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, (j * 2) + 1, (byte)((this.line3Column1[color] >> 1) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, (j * 2) + 1, (byte)(this.line3Column1[color] & 0x01));
                }
                else
                {
                    this.RenderBuffer.SetPixel(i * 4, j * 2, (byte)((this.line3Column0[color] >> 7) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, j * 2, (byte)((this.line3Column0[color] >> 6) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, j * 2, (byte)((this.line3Column0[color] >> 5) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, j * 2, (byte)((this.line3Column0[color] >> 4) & 0x01));
                    this.RenderBuffer.SetPixel(i * 4, (j * 2) + 1, (byte)((this.line3Column0[color] >> 3) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, (j * 2) + 1, (byte)((this.line3Column0[color] >> 2) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, (j * 2) + 1, (byte)((this.line3Column0[color] >> 1) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, (j * 2) + 1, (byte)(this.line3Column0[color] & 0x01));
                }
            }
            else
            {
                if ((i & 1) == 0)
                {
                    this.RenderBuffer.SetPixel(i * 4, j * 2, (byte)((this.line1Column1[color] >> 7) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, j * 2, (byte)((this.line1Column1[color] >> 6) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, j * 2, (byte)((this.line1Column1[color] >> 5) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, j * 2, (byte)((this.line1Column1[color] >> 4) & 0x01));
                    this.RenderBuffer.SetPixel(i * 4, (j * 2) + 1, (byte)((this.line1Column1[color] >> 3) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, (j * 2) + 1, (byte)((this.line1Column1[color] >> 2) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, (j * 2) + 1, (byte)((this.line1Column1[color] >> 1) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, (j * 2) + 1, (byte)(this.line1Column1[color] & 0x01));
                }
                else
                {
                    this.RenderBuffer.SetPixel(i * 4, j * 2, (byte)((this.line1Column0[color] >> 7) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, j * 2, (byte)((this.line1Column0[color] >> 6) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, j * 2, (byte)((this.line1Column0[color] >> 5) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, j * 2, (byte)((this.line1Column0[color] >> 4) & 0x01));
                    this.RenderBuffer.SetPixel(i * 4, (j * 2) + 1, (byte)((this.line1Column0[color] >> 3) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 1, (j * 2) + 1, (byte)((this.line1Column0[color] >> 2) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 2, (j * 2) + 1, (byte)((this.line1Column0[color] >> 1) & 0x01));
                    this.RenderBuffer.SetPixel((i * 4) + 3, (j * 2) + 1, (byte)(this.line1Column0[color] & 0x01));
                }
            }
        }
    }
}
