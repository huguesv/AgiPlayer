// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Resources.Serialization;

using Woohoo.Agi.Engine.Resources;

/// <summary>
/// Logic resource decoder.
/// </summary>
public static class LogicDecoder
{
    /// <summary>
    /// Key used to encrypt the messages. The key is the string "Avis Durgan".
    /// </summary>
    private static readonly byte[] Key = [0x41, 0x76, 0x69, 0x73, 0x20, 0x44, 0x75, 0x72, 0x67, 0x61, 0x6e];

    /// <summary>
    /// Decode the logic resource from byte array.
    /// </summary>
    /// <param name="resourceIndex">Resource index.</param>
    /// <param name="data">Logic data.</param>
    /// <param name="wasCompressed">Logic data was originally compressed.  This is necessary to determine if messages are encrypted or not.</param>
    /// <param name="gameCompression">Game resources are compressed (v2 are not compressed, v3 are compressed).</param>
    /// <returns>Logic resource.</returns>
    public static LogicResource ReadLogic(byte resourceIndex, byte[] data, bool wasCompressed, bool gameCompression)
    {
        int logicOffset = 0;

        ////////////////
        // Logic header

        // [0][1] = Length of script section
        int scriptLength = (data[logicOffset + 1] * 0x100) + data[logicOffset];
        logicOffset += 2;

        // [3...] = Script section
        byte[] scriptData = new byte[scriptLength];
        Array.Copy(data, logicOffset, scriptData, 0, scriptLength);
        logicOffset += scriptLength;

        // [3 + codeLength] = Messages section
        int messageLength = data.Length - logicOffset;
        byte[] messageData = new byte[messageLength];
        Array.Copy(data, logicOffset, messageData, 0, messageLength);

        var messages = DecompileMessages(messageData, !gameCompression || !wasCompressed);

        return new LogicResource(resourceIndex, scriptData, [.. messages]);
    }

    /// <summary>
    /// Reads all the messages from the message byte code.
    /// </summary>
    /// <param name="messageData">Message byte code.</param>
    /// <param name="encodedMessages">True if messages are encoded.</param>
    /// <returns>Collection of messages.</returns>
    private static List<string> DecompileMessages(byte[] messageData, bool encodedMessages)
    {
        var messages = new List<string>();

        int messagesOffset = 0;

        ////////////////
        // Messages header

        // [0] = Number of messages
        int numMessages = messageData[messagesOffset];
        messagesOffset += 1;

        // [1][2] = Offset of end of messages
        messagesOffset += 2;

        // [3+] = Offset of each message (as per number of messages)

        // [x+] = Message data
        int messageDataOffset = messagesOffset + (numMessages * 2);

        if (numMessages > 0)
        {
            if (encodedMessages)
            {
                // decrypt the data in-place, this modifies the buffer passed as input
                var xform = new XorTransform(Key);
                xform.TransformBlock(messageData, messageDataOffset, messageData.Length - messageDataOffset, messageData, messageDataOffset);
            }

            // Go back to array of offset of each message
            for (int i = 0; i < numMessages; i++)
            {
                // [0][1] = Offset of message
                int messageOffset = (messageData[messagesOffset + 1] * 0x100) + messageData[messagesOffset];
                messagesOffset += 2;

                string msg;

                if ((messageOffset - 2) >= 0)
                {
                    msg = StringDecoder.GetNullTerminatedString(messageData, messageOffset + 1);
                }
                else
                {
                    msg = string.Empty;
                }

                messages.Add(msg);
            }
        }

        return messages;
    }
}
