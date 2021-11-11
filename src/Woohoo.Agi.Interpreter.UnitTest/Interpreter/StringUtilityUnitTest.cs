// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.UnitTest;

using Woohoo.Agi.Interpreter;

[TestClass]
public class StringUtilityUnitTest
{
    [TestMethod]
    public void Reverse()
    {
        var input = "Hello";

        var output = StringUtility.Reverse(input);
        output.Should().Be("olleH");
    }

    [TestMethod]
    public void NumberToString()
    {
        var input = 45;

        var output = StringUtility.NumberToString(input);
        output.Should().Be("45");
    }

    [TestMethod]
    public void NumberToHexString()
    {
        var input = 45;

        var output = StringUtility.NumberToHexString(input);
        output.Should().Be("2D");
    }

    [TestMethod]
    public void PadWithZeros()
    {
        var input = "45";

        var output = StringUtility.PadWithZeros(input, 5);
        output.Should().Be("00045");
    }
}
