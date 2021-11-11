// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player;

using Woohoo.Agi.Detection;
using Woohoo.Agi.Interpreter;
using Woohoo.Agi.Resources.Serialization;

internal class GameFinder
{
    public static GameStartInfo FindGame(string folder)
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

#if USE_ZIP
            foreach (string zipFile in Directory.GetFiles(folder, "*.zip"))
            {
                var game = FindGame(detector, new GameZipArchive(zipFile));
                if (game is not null)
                {
                    games.Add(game);
                }
            }
#endif
        }

        return games.ToArray();
    }

    private static GameStartInfo FindGame(GameDetector detector, IGameContainer container)
    {
        GameStartInfo startInfo = null;

        var result = detector.Detect(container);
        if (result.Detected)
        {
            string id = ResourceLoader.GetGameId(container);

            startInfo = new GameStartInfo(container, id, result.Platform, result.Interpreter, result.Name, result.Version);
        }

        return startInfo;
    }

    private static GameDetector CreateDetector()
    {
        var algorithms = new IGameDetectorAlgorithm[]
        {
            new DetectByInternalDatabase(),
            new DetectByAgiDesignerGameInfo(),
            new DetectByWinAgiGameInfo(),
            new DetectByFileNames(),
        };

        return new GameDetector(algorithms);
    }
}
