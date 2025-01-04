// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Detection;

using Woohoo.Agi.Engine.Detection;
using Woohoo.Agi.Engine.UnitTest.Infrastructure;
using Xunit.Abstractions;

public class DetectByFileNamesTest
{
    private readonly ITestOutputHelper outputHelper;

    public DetectByFileNamesTest(ITestOutputHelper outputHelper)
    {
        this.outputHelper = outputHelper;
    }

    [Theory]
    [InlineData("AppleIIgsGame1", Platform.AppleIIgs, InterpreterVersion.V2936, new string[] { "OBJECT", "WORDS.TOK", "VOL.0", "LOGDIR", "PICDIR", "SNDDIR", "VIEWDIR", "SIERRASTANDARD", "AGIFONT" })]
    [InlineData("AtariSTGame1", Platform.AtariST, InterpreterVersion.V2936, new string[] { "OBJECT", "WORDS.TOK", "VOL.0", "LOGDIR", "PICDIR", "SNDDIR", "VIEWDIR", "SIERRA.PRG" })]
    [InlineData("PCGame1", Platform.PC, InterpreterVersion.V2936, new string[] { "OBJECT", "WORDS.TOK", "VOL.0", "LOGDIR", "PICDIR", "SNDDIR", "VIEWDIR", "AGI" })]
    [InlineData("PCGame2", Platform.PC, InterpreterVersion.V3002149, new string[] { "OBJECT", "WORDS.TOK", "GRVOL.0", "GRDIR" })]
    [InlineData("AmigaGame1", Platform.Amiga, InterpreterVersion.V2936, new string[] { "OBJECT", "WORDS.TOK", "VOL.0", "LOGDIR", "PICDIR", "SNDDIR", "VIEWDIR", "POINTER" })]
    [InlineData("AmigaGame2", Platform.Amiga, InterpreterVersion.V3002149, new string[] { "OBJECT", "WORDS.TOK", "VOL.0", "DIRS" })]
    public void DetectFolderWithGame(string name, Platform platform, InterpreterVersion interpreter, string[] fileNames)
    {
        // Arrange
        IGameDetectorAlgorithm detector = new DetectByFileNames();
        var gameFolder = new GameFolderBuilder()
            .WithEmptyFiles(fileNames)
            .Build(Path.Combine(Directory.GetCurrentDirectory(), "TestData", "Detection"), name);
        this.outputHelper.WriteLine(gameFolder.FolderPath);

        try
        {
            // Act
            var result = detector.Detect(gameFolder);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(name);
            result.Platform.Should().Be(platform);
            result.Interpreter.Should().Be(interpreter);
            result.Version.Should().Be(string.Empty);
            result.Detected.Should().BeTrue();
        }
        finally
        {
            Directory.Delete(gameFolder.FolderPath, true);
        }
    }

    [Theory]
    [InlineData("NoGame1", new string[] { "OBJECT", "WORDS.TOK", "VOL.0" })]
    [InlineData("NoGame2", new string[] { "VOL.0" })]
    public void DetectFolderWithoutGame(string name, string[] fileNames)
    {
        // Arrange
        IGameDetectorAlgorithm detector = new DetectByFileNames();
        var gameFolder = new GameFolderBuilder()
            .WithEmptyFiles(fileNames)
            .Build(Path.Combine(Directory.GetCurrentDirectory(), "TestData", "Detection"), name);
        this.outputHelper.WriteLine(gameFolder.FolderPath);

        try
        {
            // Act
            var result = detector.Detect(gameFolder);

            // Assert
            result.Should().BeNull();
        }
        finally
        {
            Directory.Delete(gameFolder.FolderPath, true);
        }
    }
}
