// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Resources.Serialization;

/// <summary>
/// LZW decompression class.
/// </summary>
public class LzwDecompress
{
    private const int StartBits = 9;
    private const int MaxBits = 12;
    private const int TableSize = 18041;
    private const int StackSize = 4000;

    private const int ResetCode = 0x100;
    private const int EndCode = 0x101;
    private const int FirstCode = 0x102;

    private readonly uint[] prefixCode = new uint[TableSize]; // prefix codes
    private readonly byte[] appendCharacter = new byte[TableSize]; // appended characters
    private readonly byte[] decodeStack = new byte[StackSize]; // holds the decoded string

    private int bits;
    private int maxValue;
    private int maxCode;
    private int inputBitCount; // number of bits in input bit buffer
    private uint inputBitBuffer;

    /// <summary>
    /// Decompress the data using LZW algorithm.
    /// </summary>
    /// <param name="encryptedData">Compressed data.</param>
    /// <param name="encryptedStartOffset">Compressed data start offset.</param>
    /// <param name="encryptedLength">Compressed data length.</param>
    /// <param name="decryptedLength">Decompressed data length.</param>
    /// <returns>Decompressed data.</returns>
    public byte[] Decompress(byte[] encryptedData, int encryptedStartOffset, int encryptedLength, int decryptedLength)
    {
        ArgumentNullException.ThrowIfNull(encryptedData);
        ArgumentOutOfRangeException.ThrowIfNegative(encryptedStartOffset);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(encryptedStartOffset, encryptedData.Length);
        ArgumentOutOfRangeException.ThrowIfNegative(encryptedLength);
        ArgumentOutOfRangeException.ThrowIfNegative(decryptedLength);

        if ((encryptedStartOffset + encryptedLength) > encryptedData.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(encryptedLength));
        }

        byte[] decryptedData = new byte[decryptedLength];

        uint nextCode;
        uint newCode;
        uint oldCode;
        uint character;
        bool bitsFull;
        int decodeStackOffset = 0;
        int decryptedOffset = 0;
        int encryptedOffset = encryptedStartOffset;
        int encryptedEndOffset = encryptedStartOffset + encryptedLength;

        this.inputBitCount = 0;
        this.inputBitBuffer = 0;

        // Starts at 9 bits
        bitsFull = this.SetBits(StartBits);
        Debug.Assert(!bitsFull, "Bits should not be full right at the start of decompression.");

        // Next available code to define
        nextCode = 257;

        // Read the first code
        oldCode = this.GetInputCode(encryptedData, ref encryptedOffset);
        character = oldCode;
        newCode = this.GetInputCode(encryptedData, ref encryptedOffset);

        // Loop until end code
        while (newCode != EndCode)
        {
            Debug.Assert(encryptedOffset <= (encryptedEndOffset + 1), "Unexpected end of the encrypted buffer.");

            // Code to start over
            if (newCode == ResetCode)
            {
                nextCode = FirstCode;
                bitsFull = this.SetBits(StartBits);
                Debug.Assert(!bitsFull, "Bits should not be full right after reset of decompression.");
                oldCode = this.GetInputCode(encryptedData, ref encryptedOffset);
                Debug.Assert(encryptedOffset <= (encryptedEndOffset + 1), "Unexpected end of the encrypted buffer.");
                character = oldCode;
                decryptedData[decryptedOffset] = (byte)character;
                decryptedOffset++;
                newCode = this.GetInputCode(encryptedData, ref encryptedOffset);
            }
            else
            {
                int stringOffset;

                // Handle special LZW scenario
                if (newCode >= nextCode)
                {
                    this.decodeStack[decodeStackOffset] = (byte)character;
                    stringOffset = this.DecodeString(decodeStackOffset + 1, oldCode);
                }
                else
                {
                    stringOffset = this.DecodeString(decodeStackOffset, newCode);
                }

                // Reverse order of decoded string and store in output
                character = this.decodeStack[stringOffset];
                while (stringOffset >= 0)
                {
                    decryptedData[decryptedOffset] = this.decodeStack[stringOffset];
                    decryptedOffset++;
                    stringOffset--;
                }

                if (nextCode > this.maxCode)
                {
                    _ = this.SetBits(this.bits + 1);
                }

                this.prefixCode[nextCode] = oldCode;
                this.appendCharacter[nextCode] = (byte)character;
                nextCode++;
                oldCode = newCode;
                newCode = this.GetInputCode(encryptedData, ref encryptedOffset);
            }
        }

        return decryptedData;
    }

    private bool SetBits(int val)
    {
        if (val == MaxBits)
        {
            return true;
        }

        this.bits = val;
        this.maxValue = (1 << this.bits) - 1;
        this.maxCode = this.maxValue - 1;

        return false;
    }

    private uint GetInputCode(byte[] buffer, ref int index)
    {
        uint retVal;

        while (this.inputBitCount <= 24)
        {
            byte v = (index < buffer.Length) ? buffer[index] : (byte)0;
            this.inputBitBuffer |= (uint)(v << this.inputBitCount);
            this.inputBitCount += 8;

            index++;
        }

        retVal = (uint)((this.inputBitBuffer & 0x7fff) % (1 << this.bits));
        this.inputBitBuffer >>= this.bits;
        this.inputBitCount -= this.bits;

        return retVal;
    }

    private int DecodeString(int index, uint code)
    {
        int i = 0;
        while (code > 255)
        {
            this.decodeStack[index] = this.appendCharacter[code];
            code = this.prefixCode[code];

            index++;

            i++;
            Debug.Assert(i < StackSize, "Error during code expansion.");
        }

        this.decodeStack[index] = (byte)code;

        return index;
    }
}
