// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Interpreter;

using Woohoo.Agi.Engine.Interpreter;

public class StringUtilityUnitTest
{
    [Fact]
    public void Reverse()
    {
        // Arrange
        var input = "Hello";

        // Act
        var output = StringUtility.Reverse(input);

        // Assert
        output.Should().Be("olleH");
    }

    [Fact]
    public void NumberToString()
    {
        // Arrange
        var input = 45;

        // Act
        var output = StringUtility.NumberToString(input);

        // Assert
        output.Should().Be("45");
    }

    [Fact]
    public void NumberToHexString()
    {
        // Arrange
        var input = 45;

        // Act
        var output = StringUtility.NumberToHexString(input);

        // Assert
        output.Should().Be("2D");
    }

    [Theory]
    [InlineData("45", 0, "45")]
    [InlineData("45", 1, "45")]
    [InlineData("45", 2, "45")]
    [InlineData("45", 3, "045")]
    [InlineData("45", 4, "0045")]
    [InlineData("45", 5, "00045")]
    public void PadWithZeros(string input, int size, string expected)
    {
        // Act
        var output = StringUtility.PadWithZeros(input, size);

        // Assert
        output.Should().Be(expected);
    }
}
