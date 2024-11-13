// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

using Woohoo.Agi.Engine.Resources;

public abstract class GraphicsRendererDriverFull : GraphicsRendererDriver
{
    protected GraphicsRendererDriverFull()
    {
        this.CreateRenderBuffer(320, 168);
    }

    public override byte DisplayType => Woohoo.Agi.Engine.Interpreter.DisplayType.Ega;

    public override Palette Palette => Palette.Ega;

    public override int Width => 320;

    public override int Height => 168;

    public override int RenderScaleX => 1;

    public override int RenderScaleY => 1;

    public override byte MessageBoxBackground => 0x0f;

    public override byte MessageBoxBorder => 0x04;

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

    public override DitheredColor DitherColor(byte color)
    {
        if (color > 0x0f)
        {
            throw new ArgumentOutOfRangeException(nameof(color));
        }

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
                byte pixel = (byte)(buffer[(PictureResource.Width * j) + i] & 0xf);
                this.RenderBuffer.SetPixel(i * 2, j, pixel);
                this.RenderBuffer.SetPixel((i * 2) + 1, j, pixel);
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
                this.RenderBuffer.SetPixel(i * 2, j, color);
                this.RenderBuffer.SetPixel((i * 2) + 1, j, color);
            }
        }
    }
}
