// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

public class RenderBuffer
{
    private readonly byte[,] pixels;

    public RenderBuffer(int width, int height)
    {
        this.pixels = new byte[width, height];
    }

    public void SetPixel(int x, int y, byte color)
    {
        this.pixels[x, y] = color;
    }

    public byte[,] GetBuffer()
    {
        return this.pixels;
    }
}
