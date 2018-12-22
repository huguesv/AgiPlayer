// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    public static class LogicPatches
    {
        /// <summary>
        /// Gets the patch for Gold Rush copy protection.
        /// </summary>
        public static LogicPatch GoldRush { get; } = new LogicPatch()
        {
            GameIds = new string[] { "GR" },
            ResourceIndex = 125,
            Original = new byte[]
            {
                0x0C, 0x04, 0xFF, 0x07, 0x05, 0xFF, 0x16, 0x00,
                0x0C, 0x96, 0x03, 0x0A, 0x00, 0x77, 0x83, 0x71,
                0x0D, 0xD9, 0x03, 0xDC, 0xBF, 0x18, 0xDC, 0x19,
                0xDC, 0x1B, 0xDC, 0x0C, 0x95, 0x1A,
            },
            Patched = new byte[]
            {
                0x0D, 0xE3, 0x03, 0x13, 0x00, 0x03, 0xF6, 0x01,
                0x0C, 0x0F, 0x12, 0x49,
            },
        };

        /// <summary>
        /// Gets the patch for King's Quest IV copy protection.
        /// </summary>
        public static LogicPatch Kq4 { get; } = new LogicPatch()
        {
            GameIds = new string[] { "KQ4" },
            ResourceIndex = 140,
            Original = new byte[]
            {
                0x0C, 0x04, 0xFF, 0x07, 0x05, 0xFF, 0x15, 0x00,
                0x03, 0x0A, 0x00, 0x77, 0x83, 0x71, 0x0D, 0x97,
                0x03, 0x98, 0xCE, 0x18, 0x98, 0x19, 0x98, 0x1B,
                0x98, 0x0C, 0x5A, 0x1A, 0x00,
            },
            Patched = new byte[]
            {
                0x03, 0x13, 0x0, 0x12, 0x60, 0x00,
            },
        };

        /// <summary>
        /// Gets the patch for Leisure Suit Larry I age question.
        /// </summary>
        public static LogicPatch Lsl1 { get; } = new LogicPatch()
        {
            GameIds = new string[] { "LLLLL" },
            ResourceIndex = 6,
            Original = new byte[]
            {
                0xFF, 0xFD, 0x07, 0x1E, 0xFC, 0x07, 0x6D, 0x01,
                0x5F, 0x03, 0xFC, 0xFF, 0x12, 0x00, 0x0C, 0x6D,
                0x78, 0x8A, 0x77, 0x69, 0x16, 0x18, 0x00, 0x0D,
                0x30, 0x0D, 0x55, 0x78, 0x65, 0x0A,
            },
            Patched = new byte[]
            {
                0x0C, 0x6D, 0x0D, 0x30, 0x0D, 0x55, 0x78, 0x12,
                0x0B,
            },
        };

        /// <summary>
        /// Gets the patch for Manhunter: New York copy protection.
        /// </summary>
        /// <remarks>
        /// Game id varies, it is empty for PC, and "MH" for AtariST.
        /// </remarks>
        public static LogicPatch Mh1 { get; } = new LogicPatch()
        {
            GameIds = new string[] { "MH", string.Empty },
            ResourceIndex = 159,
            Original = new byte[]
            {
                0xFF, 0x07, 0x05, 0xFF, 0xE6, 0x00,
                0x03, 0x0A, 0x02, 0x77, 0x83, 0x71,
                0x6F, 0x01, 0x17, 0x00, 0x03, 0x00,
                0x9F, 0x03, 0x37, 0x00, 0x03, 0x32,
                0x03, 0x03, 0x3B, 0x00, 0x6C, 0x03,
            },
            Patched = new byte[]
            {
                0x0C, 0x05, 0x16, 0x5A, 0x12, 0x99,
            },
        };

        /// <summary>
        /// Gets all patches.
        /// </summary>
        public static LogicPatch[] All { get; } = new LogicPatch[]
        {
            LogicPatches.GoldRush,
            LogicPatches.Kq4,
            LogicPatches.Lsl1,
            LogicPatches.Mh1,
        };
    }
}
