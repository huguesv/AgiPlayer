// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine;

public sealed class GameFolder : IGameContainer
{
    private readonly string folderPath;

    public GameFolder(string folderPath)
    {
        this.folderPath = folderPath;
    }

    public string FolderPath => this.folderPath;

    public string Name => Path.GetFileName(this.folderPath);

    public byte[] Read(string file)
    {
        return File.ReadAllBytes(Path.Combine(this.folderPath, file));
    }

    public bool Exists(string file)
    {
        return File.Exists(Path.Combine(this.folderPath, file));
    }

    public string GetGameId()
    {
        var id = string.Empty;

        var files = Directory.GetFiles(this.folderPath, "*VOL.0");
        if (files.Length > 0)
        {
            var name = Path.GetFileNameWithoutExtension(files[0]);
            if (name.Length >= 3 && name.EndsWith("VOL", StringComparison.InvariantCultureIgnoreCase))
            {
                id = name[..^3];
            }
        }

        return id;
    }

    public string[] GetGameFiles()
    {
        string[] includePatterns =
        [
            "OBJECT",
            "WORDS.TOK",
            "*VOL.*",
            "*DIR",
            "DIRS",
        ];

        return this.GetFiles(includePatterns);
    }

    public string[] GetInterpreterFiles()
    {
        string[] includePatterns =
        [
            "AGI",
            "AGIFONT",
            "BUSY",
            "POINTER",
            "SIERRASTANDARD",
            "*.PRG",
        ];

        return this.GetFiles(includePatterns);
    }

    public string[] GetFilesByExtension(string ext)
    {
        return Directory.GetFiles(this.folderPath, "*" + ext);
    }

    private string[] GetFiles(string[] includePatterns)
    {
        // Hashtable that holds (upper filename, filename) of all game files
        var filesTable = new Dictionary<string, string>();

        // Get the list of files to include
        foreach (var pattern in includePatterns)
        {
            var files = Directory.GetFiles(this.folderPath, pattern, SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                // File name only, no path
                var filename = Path.GetFileName(file).ToUpper(CultureInfo.InvariantCulture);

                // Put the file in the dictionary, this ensures the file is present only once
                filesTable[filename] = Path.GetFileName(file);
            }
        }

        var gameFiles = new List<string>();
        foreach (var gameFile in filesTable.Values)
        {
            gameFiles.Add(gameFile);
        }

        return [.. gameFiles];
    }
}
