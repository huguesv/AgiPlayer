// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Coordinates (x, y) in the screen system (display scaling applied).
/// </summary>
public record struct ScreenPoint
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScreenPoint"/> struct.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    public ScreenPoint(int x, int y)
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
