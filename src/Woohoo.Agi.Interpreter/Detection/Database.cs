// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Detection;

using Woohoo.Agi.Interpreter;

/// <summary>
/// Game library database.
/// </summary>
internal class Database
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Database"/> class.
    /// </summary>
    internal Database()
    {
        this.Games = new Game[0];
    }

    /// <summary>
    /// Gets games in this database.
    /// </summary>
    internal IEnumerable<Game> Games { get; private set; }

    /// <summary>
    /// Get all the files in the specified container that are game data files and calculate
    /// the crc for them.
    /// </summary>
    /// <param name="container">Game container.</param>
    /// <returns>List of files.</returns>
    internal static GameFileCollection GetFolderGameFiles(IGameContainer container)
    {
        if (container == null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        string[] gameFiles = container.GetGameFiles();

        // Calculate the checksum for each game file
        GameFileCollection crcs = new GameFileCollection();
        foreach (string file in gameFiles)
        {
            GameFile crc = GameFile.FromFile(container, file);
            crcs.Add(crc);
        }

        return crcs;
    }

    /// <summary>
    /// Find a game in the database that matches the file set and checksums.
    /// </summary>
    /// <param name="files">File set.</param>
    /// <returns>Matched game, or null if no match.</returns>
    internal Game FindMatch(GameFileCollection files)
    {
        if (files == null)
        {
            throw new ArgumentNullException(nameof(files));
        }

        // If there are no game data files, then don't return a match
        if (files.Count == 0)
        {
            return null;
        }

        // Look at each game data for a match
        foreach (var curGame in this.Games)
        {
            // Check if the number of files match
            if (curGame.Files.Count == files.Count)
            {
                // Assume it's a match, we'll set it to false if any crc doesn't match
                bool match = true;

                // Check each crc, they have to all match
                foreach (GameFile curCrc in curGame.Files)
                {
                    // Check this crc
                    GameFile crc = files[curCrc.Name];
                    if (crc == null)
                    {
                        // Filename not found, continue with next game
                        match = false;
                        break;
                    }

                    if (crc.Sha1.Length > 0 && curCrc.Sha1.Length > 0)
                    {
                        if (crc.Sha1 != curCrc.Sha1)
                        {
                            // Checksum for this file doesn't match, continue with next game
                            match = false;
                            break;
                        }
                    }
                    else
                    {
                        match = false;
                        break;
                    }
                }

                // All crcs match for this game, so this is it
                if (match)
                {
                    return curGame;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Load the database from xml text.
    /// </summary>
    /// <param name="text">Text to load.</param>
    internal void LoadFromXml(string text)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        using (var textReader = new StringReader(text))
        {
            var settings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true,
            };

            var reader = XmlReader.Create(textReader, settings);

            this.Load(reader);
        }
    }

    /// <summary>
    /// Load the database.
    /// </summary>
    /// <param name="reader">Xml reader.</param>
    internal void Load(XmlReader reader)
    {
        if (reader == null)
        {
            throw new ArgumentNullException(nameof(reader));
        }

        reader.MoveToContent();
        reader.ReadStartElement("database");
        reader.Skip(); // skip over configuration

        this.Games = LoadGames(reader);

        reader.ReadEndElement();
    }

    private static Game[] LoadGames(XmlReader reader)
    {
        var games = new List<Game>();

        reader.ReadStartElement("games");

        while (reader.Name == "game")
        {
            var game = new Game
            {
                Name = reader.GetAttribute("name", string.Empty),
                Interpreter = reader.GetAttribute("interpreter", string.Empty),
                Platform = reader.GetAttribute("platform", string.Empty),
                Version = reader.GetAttribute("version", string.Empty),
            };

            reader.ReadStartElement("game");

            // Files
            while (reader.Name == "file")
            {
                var file = new GameFile
                {
                    Name = reader.GetAttribute("name", string.Empty),
                    Sha1 = reader.GetAttribute("sha1", string.Empty),
                };

                game.Files.Add(file);

                if (reader.IsEmptyElement)
                {
                    reader.Read();
                }
                else
                {
                    reader.ReadStartElement("file");
                    reader.ReadEndElement();
                }
            }

            games.Add(game);

            reader.ReadEndElement();
        }

        reader.ReadEndElement();

        return games.ToArray();
    }
}
