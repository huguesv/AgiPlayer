// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// Text font.
/// </summary>
public class Font
{
    private readonly byte[] data;
    private readonly byte charSize;

    /// <summary>
    /// Initializes a new instance of the <see cref="Font"/> class.
    /// </summary>
    /// <param name="data">Font binary data.  Data is in NAGI font format.</param>
    public Font(byte[] data)
    {
        this.Width = data[0];
        this.Height = data[1];
        this.charSize = data[2];

        this.data = new byte[this.charSize * 256];

        int i = 3;
        while (i < data.Length)
        {
            char c = (char)data[i++];
            if (c == 0xff)
            {
                break;
            }

            Array.Copy(data, i, this.data, c * this.charSize, this.charSize);
            i += this.charSize;
        }
    }

    /// <summary>
    /// Gets font width, in pixels.
    /// </summary>
    public byte Width { get; }

    /// <summary>
    /// Gets font height, in pixels.
    /// </summary>
    public byte Height { get; }

    public byte[]? GetPixels(char c, byte color, bool inverted, bool shaded, bool textMode)
    {
        if ((c * this.charSize) >= this.data.Length)
        {
            return null;
        }

        byte maskXor = 0;
        byte maskOr = 0;

        if (inverted || ((color & 0x80) != 0 && !textMode))
        {
            maskXor = 0xff;
        }

        if (shaded)
        {
            maskOr = 0xaa;
        }

        byte[] characterData = new byte[this.charSize];
        Array.Copy(this.data, c * this.charSize, characterData, 0, this.charSize);

        if ((maskOr | maskXor) != 0)
        {
            int lineSize = this.charSize / this.Height;
            int index = 0;
            for (int j = 0; j < this.Height; j++)
            {
                for (int i = 0; i < lineSize; i++)
                {
                    characterData[index] ^= maskXor;
                    characterData[index] |= maskOr;
                    index++;
                }

                if (maskOr != 0)
                {
                    maskOr ^= 0xff;
                }
            }
        }

        byte[] pixels = new byte[this.Width * this.Height];

        int dataIndex = 0;
        int pixelIndex = 0;
        for (int j = 0; j < this.Height; j++)
        {
            byte b = 0x80;
            byte d = characterData[dataIndex++];

            for (int i = 0; i < this.Width; i++)
            {
                if (b == 0)
                {
                    d = characterData[dataIndex++];
                    b = 0x80;
                }

                if ((d & b) != 0)
                {
                    pixels[pixelIndex] = (byte)(color & 0x0f);
                }
                else
                {
                    pixels[pixelIndex] = (byte)((color & 0x70) >> 4);
                }

                pixelIndex++;
                b >>= 1;
            }
        }

        return pixels;
    }
}
