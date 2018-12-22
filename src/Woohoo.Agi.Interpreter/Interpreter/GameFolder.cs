// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    public sealed class GameFolder : IGameContainer
    {
        private string folderPath;

        public GameFolder(string folderPath)
        {
            this.folderPath = folderPath;
        }

        public string Name => Path.GetFileName(this.folderPath);

        public byte[] Read(string file)
        {
            byte[] data = new byte[0];

            using (FileStream stream = new FileStream(Path.Combine(this.folderPath, file), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
            }

            return data;
        }

        public bool Exists(string file)
        {
            return File.Exists(Path.Combine(this.folderPath, file));
        }

        public string GetGameId()
        {
            string id = string.Empty;

            string[] files = Directory.GetFiles(this.folderPath, "*vol.0");
            if (files.Length > 0)
            {
                string name = Path.GetFileNameWithoutExtension(files[0]);
                if (name.Length >= 3 && name.EndsWith("vol", StringComparison.InvariantCultureIgnoreCase))
                {
                    id = name.Substring(0, name.Length - 3);
                }
            }

            return id;
        }

        public string[] GetGameFiles()
        {
            // Hashtable that holds (upper filename, filename) of all game files
            Dictionary<string, string> filesTable = new Dictionary<string, string>();

            string[] includePatterns = new string[]
            {
                "OBJECT",
                "WORDS.TOK",
                "*VOL.*",
                "*DIR",
                "DIRS",
            };

            // Get the list of files to include
            foreach (string pattern in includePatterns)
            {
                string[] files = Directory.GetFiles(this.folderPath, pattern);
                foreach (string file in files)
                {
                    // File name only, no path
                    string filename = Path.GetFileName(file).ToUpper(CultureInfo.InvariantCulture);

                    // Put the file in the dictionary, this ensures the file is present only once
                    filesTable[filename] = Path.GetFileName(file);
                }
            }

            List<string> gameFiles = new List<string>();
            foreach (string gameFile in filesTable.Values)
            {
                gameFiles.Add(gameFile);
            }

            return gameFiles.ToArray();
        }

        public string[] GetFilesByExtension(string ext)
        {
            return Directory.GetFiles(this.folderPath, "*" + ext);
        }
    }
}
