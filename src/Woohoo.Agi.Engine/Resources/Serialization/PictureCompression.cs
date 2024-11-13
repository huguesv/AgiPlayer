// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Resources.Serialization;

/// <summary>
/// Performs Agi version 3 picture resource decompression.
/// </summary>
public static class PictureCompression
{
    /// <summary>
    /// Decompresses the picture binary data.
    /// </summary>
    /// <param name="compressedData">Source array.</param>
    /// <param name="compressedIndex">Source textIndex.</param>
    /// <param name="compressedLength">Source length.</param>
    /// <param name="uncompressedLength">Expected length of uncompressed data (must be exact).</param>
    /// <returns>Uncompressed array of bytes.</returns>
    public static byte[] Decompress(byte[] compressedData, int compressedIndex, int compressedLength, int uncompressedLength)
    {
        ArgumentNullException.ThrowIfNull(compressedData);
        ArgumentOutOfRangeException.ThrowIfNegative(compressedIndex);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(compressedIndex, compressedData.Length);
        ArgumentOutOfRangeException.ThrowIfNegative(compressedLength);
        ArgumentOutOfRangeException.ThrowIfNegative(uncompressedLength);

        if ((compressedIndex + compressedLength) > compressedData.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(compressedLength));
        }

        byte[] uncompressedData = new byte[uncompressedLength];

        bool normal = true;
        byte previousData = 0;
        int uncompressedIndex = 0;

        for (int index = 0; index < compressedLength; index++)
        {
            byte data = compressedData[compressedIndex + index];
            byte outdata;

            if (normal)
            {
                outdata = data;
            }
            else
            {
                outdata = (byte)(((data & 0xf0) >> 4) + ((previousData & 0x0f) << 4));
            }

            uncompressedData[uncompressedIndex] = outdata;
            uncompressedIndex++;

            if ((outdata == 0xf0) || (outdata == 0xf2))
            {
                if (normal)
                {
                    index++;
                    data = compressedData[compressedIndex + index];

                    uncompressedData[uncompressedIndex] = (byte)((data & 0xf0) >> 4);
                    uncompressedIndex++;
                    normal = false;
                }
                else
                {
                    uncompressedData[uncompressedIndex] = (byte)(data & 0x0f);
                    uncompressedIndex++;
                    normal = true;
                }
            }

            previousData = data;
        }

        return uncompressedData;
    }
}
