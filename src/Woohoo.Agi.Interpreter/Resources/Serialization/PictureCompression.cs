// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources.Serialization;

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
        if (compressedData == null)
        {
            throw new ArgumentNullException(nameof(compressedData));
        }

        if (compressedIndex < 0 || compressedIndex >= compressedData.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(compressedIndex));
        }

        if (compressedLength < 0 || (compressedIndex + compressedLength) > compressedData.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(compressedLength));
        }

        if (uncompressedLength < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(uncompressedLength));
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
