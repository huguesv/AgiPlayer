// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Rectangle in the native picture/view system (x = 0 to 160, y = 0 to 168).
/// </summary>
public record struct PictureRectangle
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PictureRectangle"/> struct.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    public PictureRectangle(int x, int y, int width, int height)
    {
        this.X = x;
        this.Y = y;
        this.Width = width;
        this.Height = height;
    }

    /// <summary>
    /// Gets or sets x coordinate.
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Gets or sets y coordinate.
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Gets or sets width.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets height.
    /// </summary>
    public int Height { get; set; }
}
