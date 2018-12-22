// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources.Serialization
{
    using System.Collections.Generic;
    using Woohoo.Agi.Resources;

    /// <summary>
    /// Inventory resource decoder.
    /// </summary>
    public static class InventoryDecoder
    {
        /// <summary>
        /// Key used to encrypt the inventory (using XOR). The key is the string "Avis Durgan".
        /// </summary>
        private static readonly byte[] XorKey = { 0x41, 0x76, 0x69, 0x73, 0x20, 0x44, 0x75, 0x72, 0x67, 0x61, 0x6e };

        /// <summary>
        /// Decode the inventory resource from byte array.
        /// </summary>
        /// <param name="data">Source array (data may be encrypted).</param>
        /// <param name="padded">Inventory data is padded.</param>
        /// <returns>Inventory resource.</returns>
        public static InventoryResource DecryptAndReadInventory(byte[] data, bool padded)
        {
            if (IsInventoryEncrypted(data, padded))
            {
                // Decrypt the data
                var xform = new XorTransform(XorKey);
                data = xform.TransformFinalBlock(data, 0, data.Length);
            }

            // Read the objects from the buffer
            return ReadInventory(data, padded);
        }

        /// <summary>
        /// Decode the inventory resource from byte array.
        /// </summary>
        /// <param name="data">Source array (data should NOT be encrypted).</param>
        /// <param name="padded">Inventory data is padded.</param>
        /// <returns>Inventory resource.</returns>
        private static InventoryResource ReadInventory(byte[] data, bool padded)
        {
            int offset = 0;

            // [0][1] = Offset of the start of inventory item names
            int namesOffset = (data[offset + 1] * 0x100) + data[offset];
            offset += 2;

            // [2] = Maximum number of animated objects (not sure what this is for)
            byte maxAnimatedObjects = data[offset];
            offset += 1;

            if (padded)
            {
                offset++;
            }

            var items = new List<InventoryItem>();

            // [3+] = three byte entry for each inventory item all of which conform to the following format
            while (offset <= namesOffset)
            {
                // [0][1] = Offset of inventory item name i
                int nameOffset = (data[offset + 1] * 0x100) + data[offset];
                offset += 2;

                // [2] = Starting room number for inventory item i or 255 carried
                byte room = data[offset];
                offset += 1;

                if (padded)
                {
                    offset++;
                }

                // [offset + 3]
                string name = StringDecoder.GetNullTerminatedString(data, nameOffset + (padded ? 4 : 3));

                items.Add(new InventoryItem(name, room));
            }

            return new InventoryResource(items.ToArray(), maxAnimatedObjects);
        }

        /// <summary>
        /// Determine if the inventory data is encrypted.
        /// </summary>
        /// <param name="data">Binary data.</param>
        /// <param name="padded">Inventory data is padded.</param>
        /// <returns>True if the data is encrypted.</returns>
        private static bool IsInventoryEncrypted(byte[] data, bool padded)
        {
            int fieldSize = padded ? 4 : 3;

            int offset = 0;

            // [0][1] = Offset of the start of inventory item names
            int namesOffset = (data[offset + 1] * 0x100) + data[offset];
            offset += 2;

            if (namesOffset == 0 && data.Length <= 4)
            {
                return false;
            }

            if (offset + fieldSize >= data.Length)
            {
                return true;
            }

            // [2] = Maximum number of animated objects (does not seem to be valid value)
            offset += 1;

            if (padded)
            {
                offset++;
            }

            // [3+] = three byte entry for each inventory item all of which conform to the following format
            while (offset <= namesOffset)
            {
                // [0][1] = Offset of inventory item name i
                int nameOffset = (data[offset + 1] * 0x100) + data[offset];
                offset += 2;

                // [2] = Starting room number for inventory item i or 255 carried
                offset += 1;

                if (padded)
                {
                    offset++;
                }

                if (nameOffset + fieldSize >= data.Length)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
