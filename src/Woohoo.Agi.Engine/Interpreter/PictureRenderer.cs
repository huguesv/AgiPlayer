// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

public class PictureRenderer
{
    private const byte DefaultVisualColor = 0x0f;
    private const byte DefaultPriorityColor = 0x04;

    private readonly PictureBuffer buffer;

    public PictureRenderer(PictureBuffer pictureBuffer)
    {
        this.buffer = pictureBuffer ?? throw new ArgumentNullException(nameof(pictureBuffer));
    }

    public bool VisualEnabled { get; set; }

    public bool PriorityEnabled { get; set; }

    public byte VisualColor { get; set; } = DefaultVisualColor;

    public byte PriorityColor { get; set; } = DefaultPriorityColor;

    public void Clear()
    {
        this.buffer.Clear(this.VisualColor, this.PriorityColor);
    }

    public void DrawLine(PicturePoint[] points)
    {
        ArgumentNullException.ThrowIfNull(points);

        this.buffer.PriorityEnabled = this.PriorityEnabled;
        this.buffer.PriorityColor = this.PriorityColor;
        this.buffer.VisualEnabled = this.VisualEnabled;
        this.buffer.VisualColor = this.VisualColor;

        if (points.Length > 0)
        {
            int x1 = points[0].X;
            int y1 = points[0].Y;

            this.buffer.DrawPixel(x1, y1);

            for (int i = 1; i < points.Length; i++)
            {
                int x2 = points[i].X;
                int y2 = points[i].Y;

                this.buffer.DrawLine(x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }
        }
    }

    public void DrawFill(PicturePoint point)
    {
        this.buffer.PriorityEnabled = this.PriorityEnabled;
        this.buffer.PriorityColor = this.PriorityColor;
        this.buffer.VisualEnabled = this.VisualEnabled;
        this.buffer.VisualColor = this.VisualColor;

        this.buffer.DrawFill(point.X, point.Y);
    }

    public void DrawBrush(PicturePoint point, byte size, PictureBrushShape shape, PictureBrushPattern pattern, int patternNumber)
    {
        this.buffer.PriorityEnabled = this.PriorityEnabled;
        this.buffer.PriorityColor = this.PriorityColor;
        this.buffer.VisualEnabled = this.VisualEnabled;
        this.buffer.VisualColor = this.VisualColor;

        this.buffer.DrawPattern(point.X, point.Y, size, shape, pattern, patternNumber);
    }
}
