// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources.Serialization.UnitTest;

[TestClass]
public class RleCompressionUnitTest
{
    private readonly byte[] uncompressed00 = [0, 0, 5, 1, 2, 3, 1, 2, 3, 5, 0, 0];
    private readonly byte[] compressed00 = [0x02, 0x51, 0x11, 0x21, 0x31, 0x00, 0x11, 0x21, 0x31, 0x51, 0x00];

    [TestMethod]
    public void DecompressNull()
    {
        Action act = () => RleCompression.Decompress(null, 0, 1, 1, 0);

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Decompress00()
    {
        Decompress(this.compressed00, this.uncompressed00, 6, 2, 0);
    }

    private static void Decompress(byte[] compressed, byte[] expected, int width, int height, byte transparentColor)
    {
        var uncompressed = RleCompression.Decompress(compressed, 0, width, height, transparentColor);
        uncompressed.Should().BeEquivalentTo(expected);
    }
}
