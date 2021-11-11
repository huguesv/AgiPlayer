// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Character position in text coordinates (row, column) = (0, 0).
/// </summary>
public struct TextPosition
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

    /// <summary>
    /// The equality operator (==) returns true if the values of its operands are equal, false otherwise.
    /// </summary>
    /// <param name="a">First object to compare.</param>
    /// <param name="b">Second object to compare.</param>
    /// <returns>true if the values are equal.</returns>
    public static bool operator ==(TextPosition a, TextPosition b)
    {
        return a.Equals(b);
    }

    /// <summary>
    /// The equality operator (!=) returns true if the values of its operands are not equal, false otherwise.
    /// </summary>
    /// <param name="a">First object to compare.</param>
    /// <param name="b">Second object to compare.</param>
    /// <returns>true if the values are not equal.</returns>
    public static bool operator !=(TextPosition a, TextPosition b)
    {
        return !(a == b);
    }

    /// <summary>
    /// Returns a value indicating whether this instance and a specified Object represent the same type and value.
    /// </summary>
    /// <param name="obj">An object to compare to this instance.</param>
    /// <returns>true if obj is the same type and value; otherwise, false. </returns>
    public override bool Equals(object obj)
    {
        if (obj is not null)
        {
            TextPosition o = (TextPosition)obj;

            if (this.Row != o.Row)
            {
                return false;
            }

            if (this.Column != o.Column)
            {
                return false;
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
        return this.Row ^ this.Column;
    }
}
