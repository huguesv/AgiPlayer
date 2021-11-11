// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources.Serialization.UnitTest
{
    [TestClass]
    public class LzwDecompressUnitTest
    {
        private byte[] compressed = new byte[] { 0x00, 0x01, 0x04, 0x08, 0x00, 0x00, 0xc0, 0x01, 0x81, 0x03, 0x00, 0x10, 0x48, 0x00, 0x40, 0x80, 0x98, 0x86, 0x0f, 0x01, 0x3c, 0x8c, 0x08, 0xb1, 0x62, 0x40 };
        private byte[] uncompressed = new byte[] { 0x00, 0x01, 0x01, 0x00, 0x00, 0x07, 0x00, 0x01, 0x03, 0x00, 0x04, 0x09, 0x00, 0x02, 0x62, 0x00, 0x02, 0x62, 0x00, 0x00, 0x62, 0x00, 0x62, 0x00, 0x00, 0x00, 0x02, 0x62, 0x00, 0x02, 0x62, 0x00 };

        [TestMethod]
        public void DecompressNull()
        {
            var decompress = new LzwDecompress();
            Action act = () => decompress.Decompress(null, 0, 0, 0);

            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void DecompressBcV3View18()
        {
            this.Decompress(this.compressed, this.uncompressed);
        }

        private void Decompress(byte[] compressed, byte[] expected)
        {
            var decompress = new LzwDecompress();
            var uncompressed = decompress.Decompress(compressed, 0, compressed.Length, expected.Length);
            uncompressed.Should().BeEquivalentTo(expected);
        }
    }
}
