// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Resources.Serialization;

/// <summary>
/// Performs an XOR transformation on an input buffer.
/// </summary>
public class XorTransform
{
    private readonly byte[] key;

    /// <summary>
    /// Initializes a new instance of the <see cref="XorTransform"/> class.
    /// </summary>
    /// <param name="key">XOR encryption key.</param>
    public XorTransform(byte[] key)
    {
        this.key = key ?? throw new ArgumentNullException(nameof(key));
    }

    /// <summary>
    /// Computes the transformation for the specified region of the input byte array and copies the resulting transformation to the specified region of the output byte array.
    /// </summary>
    /// <param name="inputBuffer">The input on which to perform the operation.</param>
    /// <param name="inputOffset">The offset into the input byte array from which to begin using data.</param>
    /// <param name="inputCount">The number of bytes in the input byte array to use as data.</param>
    /// <param name="outputBuffer">The output to which to write the data.</param>
    /// <param name="outputOffset">The offset into the output byte array from which to begin writing data.</param>
    /// <returns>The number of bytes written.</returns>
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
        ArgumentNullException.ThrowIfNull(inputBuffer);
        ArgumentOutOfRangeException.ThrowIfNegative(inputOffset);
        ArgumentOutOfRangeException.ThrowIfNegative(inputCount);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(inputCount, inputBuffer.Length);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(inputOffset, inputBuffer.Length - inputCount);

        int keyLength = this.key.Length;
        for (int index = 0; index < inputCount; index++)
        {
            outputBuffer[outputOffset + index] = (byte)(inputBuffer[inputOffset + index] ^ this.key[index % keyLength]);
        }

        return inputCount;
    }

    /// <summary>
    /// Computes the transformation for the specified region of the specified byte array.
    /// </summary>
    /// <param name="inputBuffer">The input on which to perform the operation.</param>
    /// <param name="inputOffset">The offset into the byte array from which to begin using data.</param>
    /// <param name="inputCount">The number of bytes in the byte array to use as data.</param>
    /// <returns>The computed transform.</returns>
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
        ArgumentNullException.ThrowIfNull(inputBuffer);
        ArgumentOutOfRangeException.ThrowIfNegative(inputOffset);
        ArgumentOutOfRangeException.ThrowIfNegative(inputCount);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(inputCount, inputBuffer.Length);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(inputOffset, inputBuffer.Length - inputCount);

        int keyLength = this.key.Length;
        byte[] outputBuffer = new byte[inputCount];

        for (int index = 0; index < inputCount; index++)
        {
            outputBuffer[index] = (byte)(inputBuffer[inputOffset + index] ^ this.key[index % keyLength]);
        }

        return outputBuffer;
    }
}
