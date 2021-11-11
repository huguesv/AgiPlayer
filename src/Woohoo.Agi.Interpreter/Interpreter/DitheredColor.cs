// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// A dithered AGI pixel is made of 2 pixels of 2 different colors.
/// </summary>
public struct DitheredColor
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

    /// <summary>
    /// The equality operator (==) returns true if the values of its operands are equal, false otherwise.
    /// </summary>
    /// <param name="a">First object to compare.</param>
    /// <param name="b">Second object to compare.</param>
    /// <returns>true if the values are equal.</returns>
    public static bool operator ==(DitheredColor a, DitheredColor b)
    {
        return a.Equals(b);
    }

    /// <summary>
    /// The equality operator (!=) returns true if the values of its operands are not equal, false otherwise.
    /// </summary>
    /// <param name="a">First object to compare.</param>
    /// <param name="b">Second object to compare.</param>
    /// <returns>true if the values are not equal.</returns>
    public static bool operator !=(DitheredColor a, DitheredColor b)
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
        return this.Odd == this.Even;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
        return this.Odd ^ this.Even;
    }
}
