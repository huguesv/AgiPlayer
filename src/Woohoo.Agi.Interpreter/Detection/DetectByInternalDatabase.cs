// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Detection;

using Woohoo.Agi.Interpreter;

/// <summary>
/// Game detection algorithm which uses MD5 checksum to identify
/// the game using an internal database.
/// </summary>
public sealed class DetectByInternalDatabase : IGameDetectorAlgorithm
{
    private readonly Database database;

    /// <summary>
    /// Initializes a new instance of the <see cref="DetectByInternalDatabase"/> class.
    /// </summary>
    public DetectByInternalDatabase()
    {
        this.database = new Database();
        this.database.LoadFromXml(Databases.Agi);
    }

    /// <summary>
    /// Detect a game in the specified folder.
    /// </summary>
    /// <param name="container">Game container.</param>
    /// <returns>Detection result.</returns>
    GameDetectorResult? IGameDetectorAlgorithm.Detect(IGameContainer container)
    {
        // Look in the current directory for all game files
        var files = Database.GetFolderGameFiles(container);
        if (files.Count > 0)
        {
            // Find a game match
            var match = this.database.FindMatch(files);
            if (match is not null)
            {
                return new GameDetectorResult(match.Name, GameInfoParser.ParseInterpreterVersion(match.Interpreter), GameInfoParser.ParsePlatform(match.Platform), match.Version);
            }
        }

        return null;
    }
}
