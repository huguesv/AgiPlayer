// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Detection;

public class GameFinder
{
    public static GameStartInfo? FindGame(string folder)
    {
        var detector = CreateDetector();

        return FindGame(detector, new GameFolder(folder));
    }

    public static GameStartInfo[] FindGames(string folder, bool recursive)
    {
        var detector = CreateDetector();

        return FindGames(detector, folder, recursive);
    }

    public static GameStartInfo[] FindGames(GameDetector detector, string folder, bool recursive)
    {
        var games = new List<GameStartInfo>();

        if (Directory.Exists(folder))
        {
            foreach (string subfolder in Directory.GetDirectories(folder))
            {
                var game = FindGame(detector, new GameFolder(subfolder));
                if (game is not null)
                {
                    // A game was found, so we do not go any further down that folder
                    games.Add(game);
                }
                else if (recursive)
                {
                    games.AddRange(FindGames(detector, subfolder, true));
                }
            }

            foreach (string zipFile in Directory.GetFiles(folder, "*.zip"))
            {
                var game = FindGame(detector, new GameZipArchive(zipFile));
                if (game is not null)
                {
                    games.Add(game);
                }
            }
        }

        return [.. games];
    }

    private static GameStartInfo? FindGame(GameDetector detector, IGameContainer container)
    {
        GameStartInfo? startInfo = null;

        var result = detector.Detect(container);
        if (result is not null)
        {
            string id = container.GetGameId();

            startInfo = new GameStartInfo(container, id, result.Platform, result.Interpreter, result.Name ?? string.Empty, result.Version ?? string.Empty);
        }

        return startInfo;
    }

    private static GameDetector CreateDetector()
    {
        IGameDetectorAlgorithm[] algorithms =
        [
            new DetectByInternalDatabase(),
            new DetectByAgiDesignerGameInfo(),
            new DetectByWinAgiGameInfo(),
            new DetectByFileNames(),
        ];

        return new GameDetector(algorithms);
    }
}
