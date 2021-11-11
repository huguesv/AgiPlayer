// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

public class Blit
{
    public Blit()
    {
        this.VisualBuffer = new byte[0];
        this.PriorityBuffer = new byte[0];
    }

    public int X { get; set; }

    public int Y { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public ViewObject View { get; set; }

    [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Direct access to array items.")]
    public byte[] VisualBuffer { get; private set; }

    [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Direct access to array items.")]
    public byte[] PriorityBuffer { get; private set; }

    public void CreateBuffer(int size)
    {
        this.VisualBuffer = new byte[size];
        this.PriorityBuffer = new byte[size];
    }
}
