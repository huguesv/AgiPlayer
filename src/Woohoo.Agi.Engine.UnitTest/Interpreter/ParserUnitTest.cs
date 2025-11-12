// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Interpreter;

using Woohoo.Agi.Engine.Interpreter;
using Woohoo.Agi.Engine.Resources;
using Woohoo.Agi.Engine.UnitTest.Infrastructure;

public class ParserUnitTest
{
    [Fact]
    public void CreateNullVocabulary()
    {
        // Act
        Action act = () => _ = new Parser(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ParseNullText()
    {
        // Arrange
        var resource = new VocabularyBuilder().Build();
        var parser = new Parser(resource);

        // Act
        Action act = () => parser.Parse(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("get")]
    [InlineData("key")]
    public void ParseEmptyVocabulary(string text)
    {
        // Arrange
        var resource = new VocabularyBuilder().Build();
        var parser = new Parser(resource);

        // Act
        var actual = parser.Parse(text);

        // Assert
        var expected = new ParserResult[] { new(text, VocabularyResource.NoFamily) };
        actual.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData("get key")]
    [InlineData("get,key")]
    [InlineData("ge-t key")]
    public void ParseSimpleVocabulary(string text)
    {
        // Arrange
        var resource = new VocabularyBuilder()
            .WithFamily(50, "get")
            .WithFamily(51, "key")
            .Build();
        var parser = new Parser(resource);

        // Act
        var actual = parser.Parse(text);

        // Assert
        var expected = new ParserResult[] { new("get", 50), new("key", 51) };
        actual.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData("get the blue key")]
    [InlineData("get this blue key")]
    [InlineData("get blue key")]
    public void ParseComplexVocabulary(string text)
    {
        // Arrange
        var resource = new VocabularyBuilder()
            .WithFamily(50, "get")
            .WithFamily(51, "blue")
            .WithFamily(52, "key")
            .WithFamily(53, "blue key")
            .WithFamily(0, "a", "an", "the", "this")
            .Build();
        var parser = new Parser(resource);

        // Act
        var actual = parser.Parse(text);

        // Assert
        var expected = new ParserResult[] { new("get", 50), new("blue key", 53) };
        actual.Should().BeEquivalentTo(expected);
    }
}
