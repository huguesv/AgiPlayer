// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Interpreter;

using Woohoo.Agi.Engine.Interpreter;
using Woohoo.Agi.Engine.Resources;
using Woohoo.Agi.Engine.UnitTest.Infrastructure;

[TestClass]
public class ParserUnitTest
{
    [TestMethod]
    public void CreateNullVocabulary()
    {
        Action act = () => _ = new Parser(null);

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ParseNullText()
    {
        var resource = CreateEmptyVocabulary();
        var parser = new Parser(resource);

        Action act = () => parser.Parse(null);

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ParseSimpleWordEmptyVocabulary()
    {
        var resource = CreateEmptyVocabulary();
        var parser = new Parser(resource);

        var actual = parser.Parse("get");
        var expected = new ParserResult[] { new("get", VocabularyResource.NoFamily) };
        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void ParseSimpleWordsSimpleVocabulary()
    {
        var resource = CreateSimpleVocabulary();
        var parser = new Parser(resource);

        var actual = parser.Parse("get key");
        var expected = new ParserResult[] { new("get", 50), new("key", 51) };
        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void ParseSeparatorsSimpleVocabulary()
    {
        var resource = CreateSimpleVocabulary();
        var parser = new Parser(resource);

        var actual = parser.Parse("get,key");
        var expected = new ParserResult[] { new("get", 50), new("key", 51) };
        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void ParseIllegalSeparatorsSimpleVocabulary()
    {
        var resource = CreateSimpleVocabulary();
        var parser = new Parser(resource);

        var actual = parser.Parse("ge-t key");
        var expected = new ParserResult[] { new("get", 50), new("key", 51) };
        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void ParseComplexWordsComplexVocabulary()
    {
        var resource = CreateComplexVocabulary();
        var parser = new Parser(resource);

        var actual = parser.Parse("get the blue key");
        var expected = new ParserResult[] { new("get", 50), new("blue key", 53) };
        actual.Should().BeEquivalentTo(expected);
    }

    private static VocabularyResource CreateEmptyVocabulary()
    {
        return new VocabularyBuilder().Build();
    }

    private static VocabularyResource CreateSimpleVocabulary()
    {
        return new VocabularyBuilder()
            .WithFamily(50, "get")
            .WithFamily(51, "key")
            .Build();
    }

    private static VocabularyResource CreateComplexVocabulary()
    {
        return new VocabularyBuilder()
            .WithFamily(50, "get")
            .WithFamily(51, "blue")
            .WithFamily(52, "key")
            .WithFamily(53, "blue key")
            .WithFamily(0, "a", "an", "the", "this")
            .Build();
    }
}
