// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Resources.Serialization;

using Woohoo.Agi.Engine.Resources;

/// <summary>
/// Sound resource decoder.
/// </summary>
public static class SoundDecoder
{
    /// <summary>
    /// Decode the sound resource from byte array.
    /// </summary>
    /// <param name="resourceIndex">Resource index.</param>
    /// <param name="data">Sound data.</param>
    /// <returns>Sound resource.</returns>
    public static SoundResource ReadSound(byte resourceIndex, byte[] data)
    {
        int soundOffset = 0;

        ////////////////
        // Sound header

        // [0][1] = Offset of first voice data
        int firstVoiceOffset = (data[soundOffset + 1] * 0x100) + data[soundOffset];
        Debug.Assert(firstVoiceOffset == 0x08, "Invalid magic value.");
        soundOffset += 2;

        // [2][3] = Offset of second voice data
        int secondVoiceOffset = (data[soundOffset + 1] * 0x100) + data[soundOffset];
        soundOffset += 2;

        // [4][5] = Offset of third voice data
        int thirdVoiceOffset = (data[soundOffset + 1] * 0x100) + data[soundOffset];
        soundOffset += 2;

        // [6][7] = Offset of noise voice data
        int noiseVoiceOffset = (data[soundOffset + 1] * 0x100) + data[soundOffset];

        int lastOffset = data.Length - 1;

        byte[] channel1Data = new byte[secondVoiceOffset - firstVoiceOffset];
        byte[] channel2Data = new byte[thirdVoiceOffset - secondVoiceOffset];
        byte[] channel3Data = new byte[noiseVoiceOffset - thirdVoiceOffset];
        byte[] channel4Data = new byte[lastOffset - noiseVoiceOffset];

        Array.Copy(data, firstVoiceOffset, channel1Data, 0, secondVoiceOffset - firstVoiceOffset);
        Array.Copy(data, secondVoiceOffset, channel2Data, 0, thirdVoiceOffset - secondVoiceOffset);
        Array.Copy(data, thirdVoiceOffset, channel3Data, 0, noiseVoiceOffset - thirdVoiceOffset);
        Array.Copy(data, noiseVoiceOffset, channel4Data, 0, lastOffset - noiseVoiceOffset);

        return new SoundResource(resourceIndex, channel1Data, channel2Data, channel3Data, channel4Data);
    }
}
