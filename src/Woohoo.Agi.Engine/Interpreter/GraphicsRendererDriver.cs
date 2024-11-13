// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

public abstract class GraphicsRendererDriver
{
    protected GraphicsRendererDriver()
    {
        this.PictureBuffer = new PictureBuffer(0x04, 0x00)
        {
            GraphicsRendererDriver = this,
        };
    }

    public RenderBuffer RenderBuffer { get; private set; }

    public PictureBuffer PictureBuffer { get; }

    public abstract byte DisplayType
    {
        get;
    }

    public abstract Palette Palette
    {
        get;
    }

    public abstract int Width
    {
        get;
    }

    public abstract int Height
    {
        get;
    }

    public abstract int RenderScaleX
    {
        get;
    }

    public abstract int RenderScaleY
    {
        get;
    }

    public abstract byte MessageBoxBackground
    {
        get;
    }

    public abstract byte MessageBoxBorder
    {
        get;
    }

    public abstract byte CombineTextColors(byte fg, byte bg, bool textMode);

    public abstract byte CalculateTextBackground(byte bg, bool textMode);

    public abstract byte ConvertTextBackground(byte attrib, bool textMode);

    public abstract GraphicsColor[] GetPaletteColors(bool textMode);

    public abstract Font GetFont();

    public abstract DitheredColor DitherColor(byte color);

    public abstract byte ConvertViewColor(byte color);

    public abstract void RenderPictureBuffer(PictureRectangle rect, bool picBuffRotate);

    public abstract void RenderSolidRectangle(PictureRectangle rect, byte color);

    protected void CreateRenderBuffer(int width, int height)
    {
        this.RenderBuffer = new RenderBuffer(width, height);
    }

    protected virtual GraphicsColor[] GetPaletteTextColors()
    {
        return
        [
            new(0x00, 0x00, 0x00),
            new(0x00, 0x00, 0xa0),
            new(0x00, 0xa0, 0x00),
            new(0x00, 0xa0, 0xa0),
            new(0xa0, 0x00, 0x00),
            new(0x80, 0x00, 0xa0),
            new(0xa0, 0x50, 0x00),
            new(0xa0, 0xa0, 0xa0),
            new(0x50, 0x50, 0x50),
            new(0x50, 0x50, 0xff),
            new(0x00, 0xff, 0x50),
            new(0x50, 0xff, 0xff),
            new(0xff, 0x50, 0x50),
            new(0xff, 0x50, 0xff),
            new(0xff, 0xff, 0x50),
            new(0xff, 0xff, 0xff),
        ];
    }
}
