// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Resources.Serialization;

using Woohoo.Agi.Engine.Resources.Serialization;

public class XorTransformUnitTest
{
    // The key is the string "Avis Durgan"
    private readonly byte[] key = [0x41, 0x76, 0x69, 0x73, 0x20, 0x44, 0x75, 0x72, 0x67, 0x61, 0x6e];

    [Fact]
    public void DecryptNull()
    {
        // Arrange
        var transform = new XorTransform(this.key);

        // Act
        Action act = () => transform.TransformFinalBlock(null!, 0, 0);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(new byte[] { 0x41, 0x76, 0x6c, 0x72, 0x22, 0x47, 0x74, 0x70, 0x64, 0x64, 0x6e, 0x41 }, new byte[] { 0, 0, 5, 1, 2, 3, 1, 2, 3, 5, 0, 0 })]
    public void Decrypt(byte[] encrypted, byte[] expected)
    {
        // Arrange
        var transform = new XorTransform(this.key);

        // Act
        var unencrypted = transform.TransformFinalBlock(encrypted, 0, encrypted.Length);

        // Assert
        unencrypted.Should().BeEquivalentTo(expected);
    }
}
