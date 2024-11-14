// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Resources.Serialization;

using Woohoo.Agi.Engine.Resources.Serialization;

public class LzwDecompressUnitTest
{
    [Fact]
    public void DecompressNull()
    {
        // Arrange
        var decompress = new LzwDecompress();

        // Act
        Action act = () => decompress.Decompress(null, 0, 0, 0);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(new byte[] { 0x00, 0x01, 0x04, 0x08, 0x00, 0x00, 0xc0, 0x01, 0x81, 0x03, 0x00, 0x10, 0x48, 0x00, 0x40, 0x80, 0x98, 0x86, 0x0f, 0x01, 0x3c, 0x8c, 0x08, 0xb1, 0x62, 0x40 }, new byte[] { 0x00, 0x01, 0x01, 0x00, 0x00, 0x07, 0x00, 0x01, 0x03, 0x00, 0x04, 0x09, 0x00, 0x02, 0x62, 0x00, 0x02, 0x62, 0x00, 0x00, 0x62, 0x00, 0x62, 0x00, 0x00, 0x00, 0x02, 0x62, 0x00, 0x02, 0x62, 0x00 })] // BcV3View18
    public void Decompress(byte[] compressed, byte[] expected)
    {
        // Arrange
        var decompress = new LzwDecompress();

        // Act
        var uncompressed = decompress.Decompress(compressed, 0, compressed.Length, expected.Length);

        // Assert
        uncompressed.Should().BeEquivalentTo(expected);
    }
}
