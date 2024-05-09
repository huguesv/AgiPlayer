// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

public class Blit
{
    public Blit()
    {
        this.VisualBuffer = [];
        this.PriorityBuffer = [];
    }

    public int X { get; set; }

    public int Y { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public ViewObject View { get; set; }

    public byte[] VisualBuffer { get; private set; }

    public byte[] PriorityBuffer { get; private set; }

    public void CreateBuffer(int size)
    {
        this.VisualBuffer = new byte[size];
        this.PriorityBuffer = new byte[size];
    }
}
