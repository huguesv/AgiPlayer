// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

using Woohoo.Agi.Resources;

public abstract class GraphicsRendererDriverCga : GraphicsRendererDriver
{
    private readonly byte[] cgaTextConversion =
    [
        0,
        1,
        1,
        1,
        2,
        2,
        2,
        3,
        3,
        1,
        1,
        1,
        2,
        2,
        2,
    ];

    protected GraphicsRendererDriverCga()
    {
        this.CreateRenderBuffer(320, 168);
    }

    public override byte DisplayType => Woohoo.Agi.Interpreter.DisplayType.Cga;

    public override int Width => 320;

    public override int Height => 168;

    public override int RenderScaleX => 1;

    public override int RenderScaleY => 1;

    public override byte MessageBoxBackground => 0x0f;

    public override byte MessageBoxBorder => 0x04;

    public override Font GetFont()
    {
        return new Font(Resources.FontEga8x8);
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
                combine = 0x83;
            }
            else if (fg > 0xe)
            {
                combine = 0x03;
            }
            else
            {
                combine = this.cgaTextConversion[fg];
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

        if (!textMode)
        {
            attrib = (byte)(attrib & 0x3);
        }

        return attrib;
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
                byte visual = buffer[(PictureResource.Width * j) + i];

                byte pixel1 = (byte)((visual & 0xc) >> 2);
                byte pixel2 = (byte)(visual & 0x3);
                this.RenderBuffer.SetPixel(i * 2, j, pixel1);
                this.RenderBuffer.SetPixel((i * 2) + 1, j, pixel2);
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

        DitheredColor dithered = this.DitherColor(color);

        if ((rect.Width & 1) != 0)
        {
            for (int j = rect.Y - rect.Height + 1; j <= rect.Y; j++)
            {
                int i = rect.X * 2;

                this.RenderBuffer.SetPixel(i++, j, (byte)((dithered.Odd & 0xc) >> 2));
                this.RenderBuffer.SetPixel(i++, j, (byte)(dithered.Odd & 0x3));

                for (int count = 0; count < ((rect.Width - 1) / 2); count++)
                {
                    this.RenderBuffer.SetPixel(i++, j, (byte)((dithered.Even & 0xc) >> 2));
                    this.RenderBuffer.SetPixel(i++, j, (byte)(dithered.Even & 0x3));
                    this.RenderBuffer.SetPixel(i++, j, (byte)((dithered.Odd & 0xc) >> 2));
                    this.RenderBuffer.SetPixel(i++, j, (byte)(dithered.Odd & 0x3));
                }
            }
        }
        else
        {
            for (int j = rect.Y - rect.Height + 1; j <= rect.Y; j++)
            {
                int i = rect.X * 2;

                for (int count = 0; count < (rect.Width / 2); count++)
                {
                    this.RenderBuffer.SetPixel(i++, j, (byte)((dithered.Even & 0xc) >> 2));
                    this.RenderBuffer.SetPixel(i++, j, (byte)(dithered.Even & 0x3));
                    this.RenderBuffer.SetPixel(i++, j, (byte)((dithered.Odd & 0xc) >> 2));
                    this.RenderBuffer.SetPixel(i++, j, (byte)(dithered.Odd & 0x3));
                }
            }
        }
    }
}
