// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Resources;

/// <summary>
/// Represents a logic resource.
/// </summary>
public class LogicResource
{
    private readonly byte[] code;
    private readonly string[] messages;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicResource"/> class.
    /// </summary>
    /// <param name="resourceIndex">Resource index (0-255).</param>
    /// <param name="code">Logic byte code.</param>
    /// <param name="messages">Resource messages.</param>
    public LogicResource(byte resourceIndex, byte[] code, string[] messages)
    {
        if (code is null)
        {
            throw new ArgumentNullException(nameof(code));
        }

        if (messages is null)
        {
            throw new ArgumentNullException(nameof(messages));
        }

        this.ResourceIndex = resourceIndex;
        this.code = (byte[])code.Clone();
        this.messages = (string[])messages.Clone();
    }

    /// <summary>
    /// Gets resource index (0-255).
    /// </summary>
    public byte ResourceIndex { get; }

    /// <summary>
    /// Gets or sets index in the byte code array where execution starts.
    /// The default is 0, but it can be changed by the logic script.
    /// </summary>
    public int ScanStart { get; set; }

    /// <summary>
    /// Gets number of messages in the resource.
    /// </summary>
    public int MessageCount => this.messages.Length;

    /// <summary>
    /// Get the message at the specified index.
    /// </summary>
    /// <param name="index">Index in the array of messages (1-based).</param>
    /// <returns>Message text, or null if out of bounds.</returns>
    public string? GetMessage(int index)
    {
        if (index >= 1 && (index - 1) < this.messages.Length)
        {
            return this.messages[index - 1];
        }

        return null;
    }

    /// <summary>
    /// Get a byte.
    /// </summary>
    /// <param name="index">Index in the byte code array.</param>
    /// <returns>Byte code value.</returns>
    public byte GetCode(int index)
    {
        return this.code[index];
    }

    /// <summary>
    /// Get a signed 16 bit integer stored as little endian.
    /// </summary>
    /// <param name="index">Index in the byte code array.</param>
    /// <returns>Byte code value.</returns>
    public int GetCodeLE16(int index)
    {
        if (index == int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        ushort temp = (ushort)((this.code[index + 1] << 8) | this.code[index]);
        return (short)temp;
    }

    /// <summary>
    /// Get a signed 16 bit integer stored as big endian.
    /// </summary>
    /// <param name="index">Index in the byte code array.</param>
    /// <returns>Byte code value.</returns>
    public int GetCodeBE16(int index)
    {
        if (index == int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return (this.code[index] << 8) | this.code[index + 1];
    }

    /// <summary>
    /// Patch the logic byte code if it matches the byte code passed in.
    /// </summary>
    /// <param name="original">Byte code to match.</param>
    /// <param name="patched">Byte code to replace with.</param>
    public void Patch(byte[] original, byte[] patched)
    {
        if (original is null)
        {
            throw new ArgumentNullException(nameof(original));
        }

        if (patched is null)
        {
            throw new ArgumentNullException(nameof(patched));
        }

        if (original.Length <= this.code.Length)
        {
            bool match = true;

            // See if this logic's byte code matches the one we want to patch
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] != this.code[i])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                // Modify this logic's byte code
                for (int i = 0; i < patched.Length; i++)
                {
                    this.code[i] = patched[i];
                }
            }
        }
    }
}
