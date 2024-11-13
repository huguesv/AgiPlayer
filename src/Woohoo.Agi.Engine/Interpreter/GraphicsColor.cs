// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// Represents a 24-bit color.
/// </summary>
public readonly record struct GraphicsColor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphicsColor"/> struct.
    /// </summary>
    /// <param name="r">Red component.</param>
    /// <param name="g">Green component.</param>
    /// <param name="b">Blue component.</param>
    public GraphicsColor(byte r, byte g, byte b)
    {
        this.R = r;
        this.G = g;
        this.B = b;
    }

    /// <summary>
    /// Gets red component.
    /// </summary>
    public byte R { get; }

    /// <summary>
    /// Gets green component.
    /// </summary>
    public byte G { get; }

    /// <summary>
    /// Gets blue component.
    /// </summary>
    public byte B { get; }
}
