// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources.Serialization
{
    using System;
    using System.Globalization;
    using Woohoo.Agi.Interpreter;

    /// <summary>
    /// Volume file decoding for AGI version 3.
    /// </summary>
    public sealed class VolumeDecoderV3 : IVolumeDecoder
    {
        private const string VolumeFileSuffix = "vol";
        private const string MapFileSuffix = "dir";
        private const string MapAmigaFile = "dirs";

        private readonly string gameId;
        private readonly Platform platform;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeDecoderV3"/> class.
        /// </summary>
        /// <param name="gameId">Game id.</param>
        /// <param name="platform">Game platform.</param>
        public VolumeDecoderV3(string gameId, Platform platform)
        {
            if (platform != Platform.Amiga)
            {
                if (gameId == null)
                {
                    throw new ArgumentNullException(nameof(gameId));
                }

                if (gameId.Length == 0)
                {
                    throw new ArgumentException(Errors.GameIdEmpty, nameof(gameId));
                }
            }

            this.gameId = gameId;
            this.platform = platform;
        }

        /// <summary>
        /// Get the volume file name for the specified volume index.
        /// </summary>
        /// <param name="volume">Volume index (0-255).</param>
        /// <returns>Volume file name.</returns>
        string IVolumeDecoder.GetVolumeFile(byte volume)
        {
            return this.gameId + VolumeFileSuffix + "." + volume.ToString(CultureInfo.InvariantCulture);
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
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (fileName.Length == 0)
            {
                throw new ArgumentException(Errors.FilePathEmpty, nameof(fileName));
            }

            if (dirEntry == null)
            {
                throw new ArgumentNullException(nameof(dirEntry));
            }

            byte[] volumeData = container.Read(fileName);

            if (dirEntry.Offset >= volumeData.Length)
            {
                throw new VolumeDecoderInvalidResourceOffsetException();
            }

            return VolumeDecoderV3.ExtractResource(volumeData, dirEntry.Offset, out wasCompressed);
        }

        /// <summary>
        /// Load the resource map located in the specified folder.
        /// </summary>
        /// <param name="container">Game container where the resource map file(s) are located.</param>
        /// <returns>Resource map.</returns>
        VolumeResourceMap IVolumeDecoder.LoadResourceMap(IGameContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            return VolumeDecoderV3.ReadResourceMap(container.Read(this.GetResourceMapFile()));
        }

        private static VolumeResourceMap ReadResourceMap(byte[] data)
        {
            var map = new VolumeResourceMap();

            int offset = 0;

            // [0][1] = Offset of logdir
            int logOffset = (data[offset + 1] * 0x100) + data[offset];
            offset += 2;

            // [2][3] = Offset of picdir
            int picOffset = (data[offset + 1] * 0x100) + data[offset];
            offset += 2;

            // [4][5] = Offset of viewdir
            int viewOffset = (data[offset + 1] * 0x100) + data[offset];
            offset += 2;

            // [6][7] = Offset of snddir
            int sndOffset = (data[offset + 1] * 0x100) + data[offset];
            offset += 2;

            VolumeDecoder.ReadResourceMap(data, logOffset, picOffset - logOffset, map.LogicResources);
            VolumeDecoder.ReadResourceMap(data, picOffset, viewOffset - picOffset, map.PictureResources);
            VolumeDecoder.ReadResourceMap(data, viewOffset, sndOffset - viewOffset, map.ViewResources);
            VolumeDecoder.ReadResourceMap(data, sndOffset, data.Length - sndOffset, map.SoundResources);

            return map;
        }

        private static byte[] ExtractResource(byte[] volumeData, int offset, out bool wasCompressed)
        {
            byte[] resourceData = null;

            wasCompressed = false;

            int volOffset = offset;

            // [0][1] = Signature (0x12-0x34)
            byte signature1 = volumeData[volOffset];
            byte signature2 = volumeData[volOffset + 1];
            volOffset += 2;

            if (signature1 != 0x12 || signature2 != 0x34)
            {
                throw new VolumeDecoderInvalidResourceSignatureException();
            }

            // [2] = Volume number for resource ORed with 0x80 (if a pic resource)
            bool pictureResource = (volumeData[volOffset] & 0x80) != 0;
            volOffset += 1;

            // [3][4] = Length of the resource after header (uncompressed)
            int resourceUncompressedLength = (volumeData[volOffset + 1] * 0x100) + volumeData[volOffset];
            volOffset += 2;

            // [5][6] = Length of the resource after header (compressed)
            int resourceLength = (volumeData[volOffset + 1] * 0x100) + volumeData[volOffset];
            volOffset += 2;

            if (pictureResource)
            {
                resourceData = PictureCompression.Decompress(volumeData, volOffset, resourceLength, resourceUncompressedLength);

                wasCompressed = true;
            }
            else
            {
                if (resourceUncompressedLength != resourceLength)
                {
                    // Resource is compressed
                    LzwDecompress decompressor = new LzwDecompress();
                    resourceData = decompressor.Decompress(volumeData, volOffset, resourceLength, resourceUncompressedLength);

                    wasCompressed = true;
                }
                else
                {
                    // Resource is not compressed, just copy the data directly
                    resourceData = new byte[resourceLength];
                    Array.Copy(volumeData, volOffset, resourceData, 0, resourceLength);
                }
            }

            return resourceData;
        }

        private string GetResourceMapFile()
        {
            return this.platform == Platform.Amiga ? MapAmigaFile : this.gameId + MapFileSuffix;
        }
    }
}
