// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Resources;

/// <summary>
/// Represents a sound resource.
/// </summary>
public class SoundResource
{
    private readonly byte[] channel1Data;
    private readonly byte[] channel2Data;
    private readonly byte[] channel3Data;
    private readonly byte[] channel4Data;

    /// <summary>
    /// Initializes a new instance of the <see cref="SoundResource"/> class.
    /// </summary>
    /// <param name="resourceIndex">Resource index (0-255).</param>
    /// <param name="channel1Data">Channel 1 sound byte code.</param>
    /// <param name="channel2Data">Channel 2 sound byte code.</param>
    /// <param name="channel3Data">Channel 3 sound byte code.</param>
    /// <param name="channel4Data">Channel 4 sound byte code.</param>
    public SoundResource(byte resourceIndex, byte[] channel1Data, byte[] channel2Data, byte[] channel3Data, byte[] channel4Data)
    {
        ArgumentNullException.ThrowIfNull(channel1Data);
        ArgumentNullException.ThrowIfNull(channel2Data);
        ArgumentNullException.ThrowIfNull(channel3Data);
        ArgumentNullException.ThrowIfNull(channel4Data);

        this.ResourceIndex = resourceIndex;
        this.channel1Data = (byte[])channel1Data.Clone();
        this.channel2Data = (byte[])channel2Data.Clone();
        this.channel3Data = (byte[])channel3Data.Clone();
        this.channel4Data = (byte[])channel4Data.Clone();
    }

    /// <summary>
    /// Gets resource index (0-255).
    /// </summary>
    public byte ResourceIndex { get; }

    /// <summary>
    /// Get the byte code for the specified channel.
    /// </summary>
    /// <param name="channel">Channel index (0-3).</param>
    /// <returns>Sound byte code.</returns>
    public byte[] GetChannelData(int channel)
    {
        return channel switch
        {
            0 => (byte[])this.channel1Data.Clone(),
            1 => (byte[])this.channel2Data.Clone(),
            2 => (byte[])this.channel3Data.Clone(),
            3 => (byte[])this.channel4Data.Clone(),
            _ => throw new ArgumentOutOfRangeException(nameof(channel)),
        };
    }
}
