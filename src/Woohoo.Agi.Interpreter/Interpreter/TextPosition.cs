// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Character position in text coordinates (row, column) = (0, 0).
/// </summary>
public record struct TextPosition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextPosition"/> struct.
    /// </summary>
    /// <param name="row">Row, in text coordinates.</param>
    /// <param name="column">Column, in text coordinates.</param>
    public TextPosition(byte row, byte column)
    {
        this.Row = row;
        this.Column = column;
    }

    /// <summary>
    /// Gets or sets row, in text coordinates.
    /// </summary>
    public byte Row { get; set; }

    /// <summary>
    /// Gets or sets column, in text coordinates.
    /// </summary>
    public byte Column { get; set; }
}
