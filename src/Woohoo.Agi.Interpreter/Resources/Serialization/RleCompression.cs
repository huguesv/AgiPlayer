// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources.Serialization;

/// <summary>
/// Performs Rle decompression on an array of bytes (bitmap).
/// </summary>
public static class RleCompression
{
    /// <summary>
    /// Decompresses an array of of bytes (bitmap) using Rle.
    /// </summary>
    /// <param name="compressedData">Source array.</param>
    /// <param name="compressedIndex">Source textIndex.</param>
    /// <param name="width">Width of bitmap.</param>
    /// <param name="height">Height of bitmap.</param>
    /// <param name="transparentColor">Transparent color.</param>
    /// <returns>Decompressed array of bytes (bitmap).</returns>
    public static byte[] Decompress(byte[] compressedData, int compressedIndex, int width, int height, byte transparentColor)
    {
        if (compressedData is null)
        {
            throw new ArgumentNullException(nameof(compressedData));
        }

        if (compressedIndex < 0 || compressedIndex >= compressedData.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(compressedIndex));
        }

        if (width < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }

        if (height < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }

        if (transparentColor < 0 || transparentColor > 0x0f)
        {
            throw new ArgumentOutOfRangeException(nameof(transparentColor));
        }

        // This will decode the 4 bits per pixel rle encoded data into 8 bits per pixel data
        // Each resulting byte in the uncompressed data contains a 4 bits value (0x00-0x0F)
        byte[] pixels = new byte[width * height];
        int line = 0;
        while (line < height)
        {
            int pixel = 0;

            // Each line is terminated with 0x00
            while (compressedData[compressedIndex] != 0x00)
            {
                // High 4 bits are the color
                byte color = (byte)((compressedData[compressedIndex] & 0xf0) >> 4);

                // Low 4 bits are the repeat count
                int repeatCount = compressedData[compressedIndex] & 0x0f;
                if (repeatCount == 0)
                {
                    throw new RleCompressionInvalidRepeatCountException(0);
                }

                // Fill in the pixels buffer
                for (int repeat = 0; repeat < repeatCount; repeat++)
                {
                    pixels[(line * width) + pixel] = color;
                    pixel++;
                }

                compressedIndex++;
            }

            compressedIndex++;

            // Line may be terminated before the line width
            // is reached, that's because the remaining pixels
            // are using the transparent color
            int remaining = width - pixel;
            for (int repeat = 0; repeat < remaining; repeat++)
            {
                pixels[(line * width) + pixel] = transparentColor;
                pixel++;
            }

            line++;
        }

        return pixels;
    }
}
