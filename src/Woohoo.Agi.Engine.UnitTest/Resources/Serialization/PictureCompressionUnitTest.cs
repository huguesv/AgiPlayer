// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Resources.Serialization;

using Woohoo.Agi.Engine.Resources.Serialization;

[TestClass]
public class PictureCompressionUnitTest
{
    private readonly byte[] uncompressed00 = [0xF0, 0x06];
    private readonly byte[] compressed00 = [0xF0, 0x60];

    private readonly byte[] uncompressed01 = [0xF0, 0x06, 0xF8, 0x12, 0x45, 0xF0, 0x07, 0xF2, 0x05, 0xF8, 0x14, 0x67];
    private readonly byte[] compressed01 = [0xF0, 0x6F, 0x81, 0x24, 0x5F, 0x07, 0xF2, 0x5F, 0x81, 0x46, 0x70];

    private readonly byte[] uncompressed02 = [0xF8, 0x12, 0x45];
    private readonly byte[] compressed02 = [0xF8, 0x12, 0x45];

    [TestMethod]
    public void DecompressNull()
    {
        Action act = () => PictureCompression.Decompress(null, 0, 0, 0);

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Decompress00()
    {
        Decompress(this.compressed00, this.uncompressed00);
    }

    [TestMethod]
    public void Decompress01()
    {
        Decompress(this.compressed01, this.uncompressed01);
    }

    [TestMethod]
    public void Decompress02()
    {
        Decompress(this.compressed02, this.uncompressed02);
    }

    private static void Decompress(byte[] compressed, byte[] expected)
    {
        var uncompressed = PictureCompression.Decompress(compressed, 0, compressed.Length, expected.Length);
        uncompressed.Should().BeEquivalentTo(expected);
    }
}
