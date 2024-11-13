// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// Text color components.
/// </summary>
public readonly record struct TextColor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextColor"/> struct.
    /// </summary>
    /// <param name="foreground">Foreground color.</param>
    /// <param name="background">Background color.</param>
    /// <param name="combine">Combined color.</param>
    public TextColor(byte foreground, byte background, byte combine)
    {
        this.Foreground = foreground;
        this.Background = background;
        this.Combine = combine;
    }

    /// <summary>
    /// Gets foreground color.
    /// </summary>
    public byte Foreground { get; }

    /// <summary>
    /// Gets background color.
    /// </summary>
    public byte Background { get; }

    /// <summary>
    /// Gets combined color.
    /// </summary>
    public byte Combine { get; }
}
