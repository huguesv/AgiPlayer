// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Resources.Serialization;

using Woohoo.Agi.Engine;

/// <summary>
/// Volume file decoding for AGI version 2.
/// </summary>
public sealed class VolumeDecoderV2 : IVolumeDecoder
{
    private const string VolumeFile = "vol";
    private const string LogMapFile = "logdir";
    private const string ViewMapFile = "viewdir";
    private const string PicMapFile = "picdir";
    private const string SndMapFile = "snddir";

    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeDecoderV2"/> class.
    /// </summary>
    public VolumeDecoderV2()
    {
    }

    /// <summary>
    /// Get the volume file name for the specified volume index.
    /// </summary>
    /// <param name="volume">Volume index (0-255).</param>
    /// <returns>Volume file name.</returns>
    string IVolumeDecoder.GetVolumeFile(byte volume)
    {
        return VolumeFile + "." + volume.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Extract the resource from the volume.
    /// </summary>
    /// <param name="container">Game container.</param>
    /// <param name="fileName">Volume file name.</param>
    /// <param name="dirEntry">Resource map entry for the resource to extract.</param>
    /// <param name="wasCompressed">Resource was compressed in the volume, so it was decompressed.</param>
    /// <returns>Binary data for the resource.  This is always uncompressed.  If the data is compressed in the volume, the volume decoder must decompress it.</returns>
    byte[] IVolumeDecoder.ExtractResource(IGameContainer container, string fileName, VolumeResourceMapEntry dirEntry, out bool wasCompressed)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentNullException.ThrowIfNull(dirEntry);

        wasCompressed = false;

        byte[] volumeData = container.Read(fileName);

        if (dirEntry.Offset >= volumeData.Length)
        {
            throw new VolumeDecoderInvalidResourceOffsetException();
        }

        return VolumeDecoderV2.ExtractResource(volumeData, dirEntry.Offset);
    }

    /// <summary>
    /// Load the resource map located in the specified folder.
    /// </summary>
    /// <param name="container">Game container where the resource map file(s) are located.</param>
    /// <returns>Resource map.</returns>
    VolumeResourceMap IVolumeDecoder.LoadResourceMap(IGameContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        var map = new VolumeResourceMap();

        VolumeDecoderV2.LoadResourceMapFile(container.Read(LogMapFile), map.LogicResources);
        VolumeDecoderV2.LoadResourceMapFile(container.Read(ViewMapFile), map.ViewResources);
        VolumeDecoderV2.LoadResourceMapFile(container.Read(PicMapFile), map.PictureResources);
        VolumeDecoderV2.LoadResourceMapFile(container.Read(SndMapFile), map.SoundResources);

        return map;
    }

    private static byte[] ExtractResource(byte[] volumeData, int offset)
    {
        byte[] resourceData;

        int volOffset = offset;

        // [0][1] = Signature (0x12-0x34)
        byte signature1 = volumeData[volOffset];
        byte signature2 = volumeData[volOffset + 1];
        volOffset += 2;

        if (signature1 != 0x12 || signature2 != 0x34)
        {
            throw new VolumeDecoderInvalidResourceSignatureException();
        }

        // [2] = Volume number for resource
        volOffset += 1;

        // [3][4] = Length of the resource after header (uncompressed)
        int resourceLength = (volumeData[volOffset + 1] * 0x100) + volumeData[volOffset];
        volOffset += 2;

        // Resource is never compressed, just copy the data directly
        resourceData = new byte[resourceLength];
        for (int index = 0; index < resourceLength; index++)
        {
            resourceData[index] = volumeData[volOffset + index];
        }

        return resourceData;
    }

    private static void LoadResourceMapFile(byte[] data, VolumeResourceMapEntryCollection resourceMapEntries)
    {
        VolumeDecoder.ReadResourceMap(data, 0, data.Length, resourceMapEntries);
    }
}
