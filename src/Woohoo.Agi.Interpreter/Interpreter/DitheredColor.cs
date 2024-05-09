// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// A dithered AGI pixel is made of 2 pixels of 2 different colors.
/// </summary>
public readonly record struct DitheredColor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DitheredColor"/> struct.
    /// </summary>
    /// <param name="odd">Odd column color.</param>
    /// <param name="even">Even column color.</param>
    public DitheredColor(byte odd, byte even)
    {
        this.Odd = odd;
        this.Even = even;
    }

    /// <summary>
    /// Gets odd column color.
    /// </summary>
    public byte Odd { get; }

    /// <summary>
    /// Gets even column color.
    /// </summary>
    public byte Even { get; }
}
