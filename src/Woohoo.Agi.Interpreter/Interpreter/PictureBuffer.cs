// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

using Woohoo.Agi.Resources;

public class PictureBuffer
{
    private const byte DefaultVisualColor = 0x0f;
    private const byte DefaultPriorityColor = 0x04;

    private static readonly byte[][] Circles =
    [
        [0x80],
        [0x30],
        [0x5f, 0xf4],
        [0x66, 0xff, 0xf6, 0x60],
        [0x23, 0xbf, 0xff, 0xff, 0xee, 0x20],
        [0x31, 0xe7, 0x9e, 0xff, 0xff, 0xde, 0x79, 0xe3, 0x00],
        [0x38, 0xf9, 0xf3, 0xef, 0xff, 0xff, 0xff, 0xfe, 0xf9, 0xf3, 0xe3, 0x80],
        [0x18, 0x3c, 0x7e, 0x7e, 0x7e, 0xff, 0xff, 0xff, 0xff, 0xff, 0x7e, 0x7e, 0x7e, 0x3c, 0x18],
    ];

    private static readonly byte[] SplatterBrushMap =
    [
        0x20,
        0x94,
        0x02,
        0x24,
        0x90,
        0x82,
        0xa4,
        0xa2,
        0x82,
        0x09,
        0x0a,
        0x22,
        0x12,
        0x10,
        0x42,
        0x14,
        0x91,
        0x4a,
        0x91,
        0x11,
        0x08,
        0x12,
        0x25,
        0x10,
        0x22,
        0xa8,
        0x14,
        0x24,
        0x00,
        0x50,
        0x24,
        0x04,
    ];

    private static readonly byte[] SplatterBrushStart =
    [
        0x00,
        0x18,
        0x30,
        0xc4,
        0xdc,
        0x65,
        0xeb,
        0x48,
        0x60,
        0xbd,
        0x89,
        0x05,
        0x0a,
        0xf4,
        0x7d,
        0x7d,
        0x85,
        0xb0,
        0x8e,
        0x95,
        0x1f,
        0x22,
        0x0d,
        0xdf,
        0x2a,
        0x78,
        0xd5,
        0x73,
        0x1c,
        0xb4,
        0x40,
        0xa1,
        0xb9,
        0x3c,
        0xca,
        0x58,
        0x92,
        0x34,
        0xcc,
        0xce,
        0xd7,
        0x42,
        0x90,
        0x0f,
        0x8b,
        0x7f,
        0x32,
        0xed,
        0x5c,
        0x9d,
        0xc8,
        0x99,
        0xad,
        0x4e,
        0x56,
        0xa6,
        0xf7,
        0x68,
        0xb7,
        0x25,
        0x82,
        0x37,
        0x3a,
        0x51,
        0x69,
        0x26,
        0x38,
        0x52,
        0x9e,
        0x9a,
        0x4f,
        0xa7,
        0x43,
        0x10,
        0x80,
        0xee,
        0x3d,
        0x59,
        0x35,
        0xcf,
        0x79,
        0x74,
        0xb5,
        0xa2,
        0xb1,
        0x96,
        0x23,
        0xe0,
        0xbe,
        0x05,
        0xf5,
        0x6e,
        0x19,
        0xc5,
        0x66,
        0x49,
        0xf0,
        0xd1,
        0x54,
        0xa9,
        0x70,
        0x4b,
        0xa4,
        0xe2,
        0xe6,
        0xe5,
        0xab,
        0xe4,
        0xd2,
        0xaa,
        0x4c,
        0xe3,
        0x06,
        0x6f,
        0xc6,
        0x4a,
        0xa4,
        0x75,
        0x97,
        0xe1,
    ];

    public PictureBuffer()
    {
        for (int i = 0; i < PictureResource.Width * PictureResource.Height; i++)
        {
            this.Visual[i] = this.VisualColor;
            this.Priority[i] = this.PriorityColor;
        }
    }

    public PictureBuffer(byte priority, byte visual)
    {
        for (int i = 0; i < PictureResource.Width * PictureResource.Height; i++)
        {
            this.Visual[i] = visual;
            this.Priority[i] = priority;
        }
    }

    public byte[] Visual { get; } = new byte[PictureResource.Width * PictureResource.Height];

    public byte[] Priority { get; } = new byte[PictureResource.Width * PictureResource.Height];

    public bool VisualEnabled { get; set; }

    public bool PriorityEnabled { get; set; }

    public byte VisualColor { get; set; } = DefaultVisualColor;

    public byte PriorityColor { get; set; } = DefaultPriorityColor;

    public GraphicsRendererDriver? GraphicsRendererDriver { get; set; }

    public void Clear(byte pixelVisual, byte pixelPriority)
    {
        for (int i = 0; i < PictureResource.Width * PictureResource.Height; i++)
        {
            this.Visual[i] = pixelVisual;
            this.Priority[i] = pixelPriority;
        }
    }

    public void DrawFill(int x1, int y1)
    {
        var queue = new Queue<int>();

        queue.Enqueue(x1);
        queue.Enqueue(y1);

        while (true)
        {
            if (queue.Count == 0)
            {
                break;
            }

            int x = (int)queue.Dequeue();

            if (queue.Count == 0)
            {
                break;
            }

            int y = (int)queue.Dequeue();

            if (this.ShouldFill(x, y))
            {
                this.DrawPixel(x, y);

                if (this.ShouldFill(x, y - 1) && (y != 0))
                {
                    queue.Enqueue(x);
                    queue.Enqueue(y - 1);
                }

                if (this.ShouldFill(x - 1, y) && (x != 0))
                {
                    queue.Enqueue(x - 1);
                    queue.Enqueue(y);
                }

                if (this.ShouldFill(x + 1, y) && (x < PictureResource.Width))
                {
                    queue.Enqueue(x + 1);
                    queue.Enqueue(y);
                }

                if (this.ShouldFill(x, y + 1) && (y < PictureResource.Height))
                {
                    queue.Enqueue(x);
                    queue.Enqueue(y + 1);
                }
            }
        }
    }

    public void DrawFillAlternate(int x1, int y1)
    {
        var queue = new Queue<PicturePoint>();

        queue.Enqueue(new PicturePoint(x1, y1));

        while (true)
        {
            if (queue.Count == 0)
            {
                break;
            }

            PicturePoint pt = queue.Dequeue();

            if (this.ShouldFill(pt.X, pt.Y))
            {
                // Find leftmost pixel to fill
                while (this.ShouldFill(pt.X - 1, pt.Y))
                {
                    pt.X--;
                }

                bool up = true;
                bool down = true;

                while (this.ShouldFill(pt.X, pt.Y))
                {
                    this.DrawPixel(pt.X, pt.Y);

                    if (this.ShouldFill(pt.X, pt.Y - 1))
                    {
                        if (up)
                        {
                            queue.Enqueue(new PicturePoint(pt.X, pt.Y - 1));
                            up = false;
                        }
                    }
                    else
                    {
                        up = true;
                    }

                    if (this.ShouldFill(pt.X, pt.Y + 1))
                    {
                        if (down)
                        {
                            queue.Enqueue(new PicturePoint(pt.X, pt.Y + 1));
                            down = false;
                        }
                    }
                    else
                    {
                        down = true;
                    }

                    pt.X++;
                }
            }
        }
    }

    public void DrawLine(int x1, int y1, int x2, int y2)
    {
        int height = y2 - y1;
        int width = x2 - x1;
        float addX = height == 0 ? height : (float)width / Math.Abs(height);
        float addY = width == 0 ? width : (float)height / Math.Abs(width);

        if (Math.Abs(width) > Math.Abs(height))
        {
            float y = y1;
            addX = width == 0 ? 0 : (width / Math.Abs(width));

            for (float x = x1; x != x2; x += addX)
            {
                this.DrawPixel(Round(x, addX), Round(y, addY));
                y += addY;
            }

            this.DrawPixel(x2, y2);
        }
        else
        {
            float x = x1;
            addY = height == 0 ? 0 : (height / Math.Abs(height));

            for (float y = y1; y != y2; y += addY)
            {
                this.DrawPixel(Round(x, addX), Round(y, addY));
                x += addX;
            }

            this.DrawPixel(x2, y2);
        }
    }

    public void DrawPixel(int x, int y)
    {
        if (this.GraphicsRendererDriver == null)
        {
            throw new InvalidOperationException();
        }

        if (x >= 0 && x < PictureResource.Width && y >= 0 && y < PictureResource.Height)
        {
            if (this.VisualEnabled)
            {
                byte color;
                DitheredColor dithered = this.GraphicsRendererDriver.DitherColor(this.VisualColor);

                if ((y & 1) == 0)
                {
                    color = dithered.Even;
                }
                else
                {
                    color = dithered.Odd;
                }

                this.Visual[(y * PictureResource.Width) + x] = color;
            }

            if (this.PriorityEnabled)
            {
                this.Priority[(y * PictureResource.Width) + x] = this.PriorityColor;
            }
        }
    }

    public void DrawPattern(int x, int y, int size, PictureBrushShape shape, PictureBrushPattern pattern, int patternNumber)
    {
        int penSize = size;
        int circlePos = 0;
        int pos = SplatterBrushStart[patternNumber];

        // Adjust X if brush would end up outside screen boundaries
        if (x < penSize)
        {
            x = penSize - 1;
        }

        // Adjust Y if brush would end up outside screen boundaries
        if (y < penSize)
        {
            y = penSize;
        }

        for (int y1 = y - penSize; y1 <= y + penSize; y1++)
        {
            for (int x1 = x - (int)Math.Ceiling(penSize / 2.0f); x1 <= (x + (int)Math.Floor(penSize / 2.0f)); x1++)
            {
                if (shape == PictureBrushShape.Rectangle)
                {
                    // Rectangle
                    this.DrawPatternPoint(x1, y1, pattern, ref pos);
                }
                else
                {
                    // Circle
                    if (((Circles[penSize][circlePos >> 3] >> (0x07 - (circlePos & 0x07))) & 0x01) != 0)
                    {
                        this.DrawPatternPoint(x1, y1, pattern, ref pos);
                    }

                    circlePos++;
                }
            }
        }
    }

    protected static int Round(float number, float direction)
    {
        if (direction < 0)
        {
            return (int)((number - Math.Floor(number) <= 0.501) ? Math.Floor(number) : Math.Ceiling(number));
        }

        return (int)((number - Math.Floor(number) < 0.499) ? Math.Floor(number) : Math.Ceiling(number));
    }

    protected bool ShouldFill(int x, int y)
    {
        if (!this.VisualEnabled && !this.PriorityEnabled)
        {
            return false;
        }

        if (!(x >= 0 && x < PictureResource.Width && y >= 0 && y < PictureResource.Height))
        {
            return false;
        }

        byte pixelVisualColor = this.Visual[(y * PictureResource.Width) + x];
        byte pixelPriorityColor = this.Priority[(y * PictureResource.Width) + x];

        Debug.Assert(pixelVisualColor <= 0x0f, "Color out of range.");
        Debug.Assert(pixelPriorityColor <= 0x0f, "Color out of range.");

        if (!this.PriorityEnabled && this.VisualEnabled && this.VisualColor != DefaultVisualColor)
        {
            return pixelVisualColor == DefaultVisualColor;
        }

        if (this.PriorityEnabled && !this.VisualEnabled && this.PriorityColor != DefaultPriorityColor)
        {
            return pixelPriorityColor == DefaultPriorityColor;
        }

        return this.VisualEnabled && pixelVisualColor == DefaultVisualColor && this.VisualColor != DefaultVisualColor;
    }

    protected void DrawPatternPoint(int x, int y, PictureBrushPattern pattern, ref int pos)
    {
        if (pattern == PictureBrushPattern.Splatter)
        {
            // Splatter brush
            if ((SplatterBrushMap[pos >> 3] >> (0x07 - (pos & 0x07)) & 0x01) != 0)
            {
                this.DrawPixel(x, y);
            }

            pos++;

            // Wrap at 0xff
            if (pos == 0xff)
            {
                pos = 0;
            }
        }
        else
        {
            // Solid brush
            this.DrawPixel(x, y);
        }
    }
}
