// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Size in the render buffer system (x = 0 to 320/640, y = 0 to 200/400).
/// The system maximum X and Y values depend on the render buffer that is used
/// (low resolution vs. high resolution).
/// </summary>
public readonly record struct RenderSize
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RenderSize"/> struct.
    /// </summary>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    public RenderSize(int width, int height)
    {
        this.Width = width;
        this.Height = height;
    }

    /// <summary>
    /// Gets width.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets height.
    /// </summary>
    public int Height { get; }
}
