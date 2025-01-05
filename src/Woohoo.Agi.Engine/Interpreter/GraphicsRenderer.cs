// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable disable

namespace Woohoo.Agi.Engine.Interpreter;

using Woohoo.Agi.Engine.Resources;

public class GraphicsRenderer
{
    private readonly List<GraphicsRendererDriver> renderDrivers;
    private readonly IGraphicsDriver graphicsDriver;
    private int renderDriverIndex;

    public GraphicsRenderer(IGraphicsDriver graphicsDriver, State state, Preferences prefs)
    {
        this.graphicsDriver = graphicsDriver;
        this.State = state;

        this.renderDrivers =
        [
            new GraphicsRendererDriverEga(),
            new GraphicsRendererDriverCga0(),
            new GraphicsRendererDriverCga1(),
            new GraphicsRendererDriverHercules(new GraphicsColor(0xff, 0xff, 0xff)),
            new GraphicsRendererDriverHercules(new GraphicsColor(0x00, 0x80, 0x00)), // alternative: 0x00, 0x9a, 0x00
            new GraphicsRendererDriverHercules(new GraphicsColor(0xb9, 0x80, 0x00)), // alternative: 0xdf, 0xa2, 0x00
            new GraphicsRendererDriverAtariST(),
            new GraphicsRendererDriverAmiga1(),
            new GraphicsRendererDriverAmiga2(),
            new GraphicsRendererDriverAmiga3(),
            new GraphicsRendererDriverAppleIIgs(),
        ];

        switch (prefs.Theme)
        {
            case UserInterfaceTheme.Ega:
                this.renderDriverIndex = 0;
                break;
            case UserInterfaceTheme.Cga1:
                this.renderDriverIndex = 1;
                break;
            case UserInterfaceTheme.Cga2:
                this.renderDriverIndex = 2;
                break;
            case UserInterfaceTheme.Hercules:
                this.renderDriverIndex = 3;
                break;
            case UserInterfaceTheme.HerculesGreen:
                this.renderDriverIndex = 4;
                break;
            case UserInterfaceTheme.HerculesAmber:
                this.renderDriverIndex = 5;
                break;
            case UserInterfaceTheme.AtariST:
                this.renderDriverIndex = 6;
                break;
            case UserInterfaceTheme.Amiga1:
                this.renderDriverIndex = 7;
                break;
            case UserInterfaceTheme.Amiga2:
                this.renderDriverIndex = 8;
                break;
            case UserInterfaceTheme.Amiga3:
                this.renderDriverIndex = 9;
                break;
            case UserInterfaceTheme.AppleIIgs:
                this.renderDriverIndex = 10;
                break;
        }
    }

    public PictureBuffer PictureBuffer => this.renderDrivers[this.renderDriverIndex].PictureBuffer;

    public Palette Palette => this.renderDrivers[this.renderDriverIndex].Palette;

    public bool TextMode { get; set; }

    public int RenderScaleX => this.renderDrivers[this.renderDriverIndex].RenderScaleX;

    public int RenderScaleY => this.renderDrivers[this.renderDriverIndex].RenderScaleY;

    public int RenderFontWidth => 8 * this.RenderScaleX;

    public int RenderFontHeight => 8 * this.RenderScaleY;

    public byte MessageBoxBackground => this.renderDrivers[this.renderDriverIndex].MessageBoxBackground;

    public byte MessageBoxBorder => this.renderDrivers[this.renderDriverIndex].MessageBoxBorder;

    public byte HintBoxBorder => this.renderDrivers[this.renderDriverIndex].HintBoxBorder;

    protected State State { get; }

    public bool GraphicsUpdateNeeded()
    {
        return !this.TextMode;
    }

    public void NextDriver()
    {
        this.renderDriverIndex++;
        if (this.renderDriverIndex >= this.renderDrivers.Count)
        {
            this.renderDriverIndex = 0;
        }
    }

    public byte CombineTextColors(byte fg, byte bg)
    {
        return this.renderDrivers[this.renderDriverIndex].CombineTextColors(fg, bg, this.TextMode);
    }

    public byte CalculateTextBackground(byte bg)
    {
        return this.renderDrivers[this.renderDriverIndex].CalculateTextBackground(bg, this.TextMode);
    }

    public byte ConvertTextBackground(byte attrib)
    {
        return this.renderDrivers[this.renderDriverIndex].ConvertTextBackground(attrib, this.TextMode);
    }

    public GraphicsColor[] GetPaletteColors()
    {
        return this.renderDrivers[this.renderDriverIndex].GetPaletteColors(this.TextMode);
    }

    public Font GetFont()
    {
        return this.renderDrivers[this.renderDriverIndex].GetFont();
    }

    public void RenderSolidRectangle(PictureRectangle rect, byte color)
    {
        if (!Clip(ref rect))
        {
            this.renderDrivers[this.renderDriverIndex].RenderSolidRectangle(rect, color);
            this.Update(rect, false);
        }
    }

    public void RenderPictureBuffer(PictureRectangle rect, bool picBuffRotate, bool fade)
    {
        if (!Clip(ref rect))
        {
            this.renderDrivers[this.renderDriverIndex].RenderPictureBuffer(rect, picBuffRotate);
            this.Update(rect, fade);
        }
    }

    public void DrawPictureToPictureBuffer(PictureResource resource, bool overlay)
    {
        PictureRenderer renderer = new PictureRenderer(this.PictureBuffer);
        PictureInterpreter interpreter = new PictureInterpreter(renderer);

        if (!overlay)
        {
            renderer.Clear();
        }

        interpreter.Execute(resource);
    }

    public void DrawViewToPictureBuffer(ViewObject view)
    {
        bool invisible = true;

        byte transparentColor = view.ViewCel.TransparentColor;
        byte viewPriority = (byte)(view.Priority << 4);

        for (int j = 0; j < view.ViewCel.Height; j++)
        {
            int y = view.Y - view.ViewCel.Height + j + 1;

            for (int i = 0; i < view.ViewCel.Width; i++)
            {
                int x = view.X + i;

                byte color = view.ViewCel.GetPixel(i, j);
                if (color != transparentColor)
                {
                    byte picturePriority = (byte)(this.PictureBuffer.Priority[(y * PictureResource.Width) + x] << 4);

                    if (picturePriority <= 0x20)
                    {
                        byte ch = 0;
                        int tempY = y;
                        while (tempY < (PictureResource.Height - 1) && ch <= 0x20)
                        {
                            tempY++;
                            ch = (byte)(this.PictureBuffer.Priority[(tempY * PictureResource.Width) + x] << 4);
                        }

                        if (ch > viewPriority)
                        {
                            picturePriority = 0xff;
                        }
                    }
                    else
                    {
                        if (picturePriority > viewPriority)
                        {
                            picturePriority = 0xff;
                        }
                        else
                        {
                            picturePriority = viewPriority;
                        }
                    }

                    if (picturePriority != 0xff)
                    {
                        byte visual = this.renderDrivers[this.renderDriverIndex].ConvertViewColor(color);

                        this.PictureBuffer.Priority[(y * PictureResource.Width) + x] = (byte)(picturePriority >> 4);
                        this.PictureBuffer.Visual[(y * PictureResource.Width) + x] = visual;
                        invisible = false;
                    }
                }
            }
        }

        if (view.Number == 0)
        {
            this.State.Flags[Flags.EgoInvisible] = invisible;
        }
    }

    private static bool Clip(ref PictureRectangle rect)
    {
        int maxWidth = PictureResource.Width;
        int maxHeight = PictureResource.Height;

        // check if any of it is actually within window
        if (rect.X >= maxWidth || (rect.X + (rect.Width - 1)) < 0 || rect.Y < 0 || (rect.Y - (rect.Height - 1) >= maxHeight))
        {
            return true;
        }

        // top
        if ((rect.Y - rect.Height + 1) < 0)
        {
            rect.Height = rect.Y + 1;
        }

        // bottom
        if (rect.Y >= maxHeight)
        {
            rect.Height -= rect.Y - (maxHeight - 1);
            rect.Y = maxHeight - 1;
        }

        // left
        if (rect.X < 0)
        {
            rect.Width += rect.X;
            rect.X = 0;
        }

        // right
        if ((rect.X + rect.Width - 1) >= maxWidth)
        {
            rect.Width = maxWidth - rect.X;
        }

        return false;
    }

    private void Update(PictureRectangle rect, bool fade)
    {
        int left = rect.X * 2 * this.RenderScaleX;
        int right = (rect.X + rect.Width) * 2 * this.RenderScaleX;
        int top = (rect.Y - rect.Height + 1) * this.RenderScaleY;
        int bottom = (rect.Y * this.RenderScaleY) + this.RenderScaleY;

        this.graphicsDriver.RenderToScreen(this.renderDrivers[this.renderDriverIndex].RenderBuffer, this.State.WindowRowMin * this.RenderFontHeight, new RenderPoint(left, top), new RenderPoint(right, bottom), fade);
    }
}
