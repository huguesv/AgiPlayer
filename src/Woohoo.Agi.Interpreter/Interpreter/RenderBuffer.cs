// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

public class RenderBuffer
{
    [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Member", Justification = "Direct access to array items.")]
    private byte[,] pixels;

    public RenderBuffer(int width, int height)
    {
        this.pixels = new byte[width, height];
    }

    public void SetPixel(int x, int y, byte color)
    {
        this.pixels[x, y] = color;
    }

    [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return", Justification = "Direct access to array items.")]
    public byte[,] GetBuffer()
    {
        return this.pixels;
    }
}
