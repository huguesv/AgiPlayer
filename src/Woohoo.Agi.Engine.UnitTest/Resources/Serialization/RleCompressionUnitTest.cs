// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Resources.Serialization;

using Woohoo.Agi.Engine.Resources.Serialization;

public class RleCompressionUnitTest
{
    [Fact]
    public void DecompressNull()
    {
        // Act
        Action act = () => RleCompression.Decompress(null!, 0, 1, 1, 0);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(6, 2, 0, new byte[] { 0x02, 0x51, 0x11, 0x21, 0x31, 0x00, 0x11, 0x21, 0x31, 0x51, 0x00 }, new byte[] { 0, 0, 5, 1, 2, 3, 1, 2, 3, 5, 0, 0 })]
    public void Decompress(int width, int height, byte transparentColor, byte[] compressed, byte[] expected)
    {
        // Act
        var uncompressed = RleCompression.Decompress(compressed, 0, width, height, transparentColor);

        // Assert
        uncompressed.Should().BeEquivalentTo(expected);
    }
}
