// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// Rectangle point in the render buffer system (x = 0 to 320/640, y = 0 to 200/400).
/// The system maximum X and Y values depend on the render buffer that is used
/// (low resolution vs. high resolution).
/// </summary>
public record struct RenderPoint
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RenderPoint"/> struct.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    public RenderPoint(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    /// <summary>
    /// Gets or sets x coordinate.
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Gets or sets y coordinate.
    /// </summary>
    public int Y { get; set; }
}
