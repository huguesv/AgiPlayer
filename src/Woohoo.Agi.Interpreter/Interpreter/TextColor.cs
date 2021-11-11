// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Text color components.
/// </summary>
public struct TextColor
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

    /// <summary>
    /// The equality operator (==) returns true if the values of its operands are equal, false otherwise.
    /// </summary>
    /// <param name="a">First object to compare.</param>
    /// <param name="b">Second object to compare.</param>
    /// <returns>true if the values are equal.</returns>
    public static bool operator ==(TextColor a, TextColor b)
    {
        return a.Equals(b);
    }

    /// <summary>
    /// The equality operator (!=) returns true if the values of its operands are not equal, false otherwise.
    /// </summary>
    /// <param name="a">First object to compare.</param>
    /// <param name="b">Second object to compare.</param>
    /// <returns>true if the values are not equal.</returns>
    public static bool operator !=(TextColor a, TextColor b)
    {
        return !(a == b);
    }

    /// <summary>
    /// Returns a value indicating whether this instance and a specified Object represent the same type and value.
    /// </summary>
    /// <param name="obj">An object to compare to this instance.</param>
    /// <returns>true if obj is the same type and value; otherwise, false. </returns>
    public override bool Equals(object? obj)
    {
        if (obj is TextColor o)
        {
            if (this.Foreground != o.Foreground)
            {
                return false;
            }

            if (this.Background != o.Background)
            {
                return false;
            }

            if (this.Combine != o.Combine)
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
        return this.Foreground ^ this.Background ^ this.Combine;
    }
}
