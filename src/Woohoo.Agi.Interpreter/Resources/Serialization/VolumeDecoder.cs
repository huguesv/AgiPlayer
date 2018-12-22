// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Volume file decoding class.
    /// </summary>
    internal static class VolumeDecoder
    {
        /// <summary>
        /// Decode the volume resource map.
        /// </summary>
        /// <param name="data">Resource map data.</param>
        /// <param name="startOffset">Start offset in resource map data.</param>
        /// <param name="length">Length of resource map data (in bytes).</param>
        /// <param name="resourceMapEntries">Resource map entry collection to add decoded entries to.</param>
        internal static void ReadResourceMap(byte[] data, int startOffset, int length, VolumeResourceMapEntryCollection resourceMapEntries)
        {
            int count = length / 3;
            if (count > 255)
            {
                // There can't be more than 255 resources for a particular type of resource
                // If the resource map is longer than that, it is probably padded with junk
                count = 255;
            }

            for (int index = 0; index < count; index++)
            {
                int dataOffset = startOffset + (index * 3);

                byte b0 = data[dataOffset + 0];
                byte b1 = data[dataOffset + 1];
                byte b2 = data[dataOffset + 2];

                // [0][1][2] = volume file number and position in that volume file
                // [0] = 76543210   [1] = 76543210   [2] = 76543210
                // [0] = VVVVPPPP   [1] = PPPPPPPP   [2] = PPPPPPPP

                // When all three bytes are 0xff it means the resource doesn't exist
                if (!(b0 == 0xff && b1 == 0xff && b2 == 0xff))
                {
                    int volume = (b0 & 0xf0) >> 4;
                    int resOffset = ((b0 & 0x0f) * 0x10000) + (b1 * 0x100) + b2;

                    var entry = new VolumeResourceMapEntry((byte)index, (byte)volume, resOffset);
                    resourceMapEntries.Add(entry);
                }
            }
        }
    }
}
