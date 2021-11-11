// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

public class MouseAreaPosition
{
    public MouseAreaPosition(MouseArea area, int x, int y, int width, int height)
    {
        this.Area = area;
        this.X = x;
        this.Y = y;
        this.Width = width;
        this.Height = height;
    }

    public MouseArea Area { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }
}
