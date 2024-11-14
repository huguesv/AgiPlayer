// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Resources.Serialization;

using Woohoo.Agi.Engine.Resources.Serialization;

public class PictureCompressionUnitTest
{
    [Fact]
    public void DecompressNull()
    {
        // Act
        Action act = () => PictureCompression.Decompress(null, 0, 0, 0);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(new byte[] { 0xF0, 0x60 }, new byte[] { 0xF0, 0x06 })]
    [InlineData(new byte[] { 0xF0, 0x6F, 0x81, 0x24, 0x5F, 0x07, 0xF2, 0x5F, 0x81, 0x46, 0x70 }, new byte[] { 0xF0, 0x06, 0xF8, 0x12, 0x45, 0xF0, 0x07, 0xF2, 0x05, 0xF8, 0x14, 0x67 })]
    [InlineData(new byte[] { 0xF8, 0x12, 0x45 }, new byte[] { 0xF8, 0x12, 0x45 })]
    public void Decompress(byte[] compressed, byte[] expected)
    {
        // Act
        var uncompressed = PictureCompression.Decompress(compressed, 0, compressed.Length, expected.Length);

        // Assert
        uncompressed.Should().BeEquivalentTo(expected);
    }
}
