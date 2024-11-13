// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Resources.Serialization;

/// <summary>
/// Decoding of binary data into strings.
/// </summary>
public static class StringDecoder
{
    /// <summary>
    /// Read an agi string from binary data (byte array).
    /// </summary>
    /// <param name="data">Source array.</param>
    /// <param name="offset">Source textIndex.</param>
    /// <returns>Agi string.</returns>
    public static string GetNullTerminatedString(byte[] data, int offset)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(offset, data.Length);

        var str = new StringBuilder();

        int i = offset;
        while (i < data.Length && data[i] != 0)
        {
            str.Append((char)data[i]);
            i++;
        }

        return str.ToString();
    }
}
