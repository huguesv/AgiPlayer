// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources.Serialization.UnitTest
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Woohoo.Agi.Resources.Serialization;

    [TestClass]
    public class PictureCompressionUnitTest
    {
        private byte[] uncompressed00 = new byte[] { 0xF0, 0x06 };
        private byte[] compressed00 = new byte[] { 0xF0, 0x60 };

        private byte[] uncompressed01 = new byte[] { 0xF0, 0x06, 0xF8, 0x12, 0x45, 0xF0, 0x07, 0xF2, 0x05, 0xF8, 0x14, 0x67 };
        private byte[] compressed01 = new byte[] { 0xF0, 0x6F, 0x81, 0x24, 0x5F, 0x07, 0xF2, 0x5F, 0x81, 0x46, 0x70 };

        private byte[] uncompressed02 = new byte[] { 0xF8, 0x12, 0x45 };
        private byte[] compressed02 = new byte[] { 0xF8, 0x12, 0x45 };

        [TestMethod]
        public void DecompressNull()
        {
            Action act = () => PictureCompression.Decompress(null, 0, 0, 0);

            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Decompress00()
        {
            this.Decompress(this.compressed00, this.uncompressed00);
        }

        [TestMethod]
        public void Decompress01()
        {
            this.Decompress(this.compressed01, this.uncompressed01);
        }

        [TestMethod]
        public void Decompress02()
        {
            this.Decompress(this.compressed02, this.uncompressed02);
        }

        private void Decompress(byte[] compressed, byte[] expected)
        {
            var uncompressed = PictureCompression.Decompress(compressed, 0, compressed.Length, expected.Length);
            uncompressed.Should().BeEquivalentTo(expected);
        }
    }
}
