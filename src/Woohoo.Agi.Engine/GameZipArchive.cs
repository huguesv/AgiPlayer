// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine;

using System.IO.Compression;

internal sealed class GameZipArchive : IGameContainer
{
    private readonly string archivePath;
    private readonly ZipArchive archive;

    public GameZipArchive(string archivePath)
    {
        this.archivePath = archivePath;
        this.archive = new ZipArchive(new FileStream(archivePath, FileMode.Open, FileAccess.Read), ZipArchiveMode.Read);
    }

    public string Name => Path.GetFileNameWithoutExtension(this.archivePath);

    public byte[] Read(string file)
    {
        byte[] data = [];

        var entry = this.GetEntryOrdinalIgnoreCase(file);
        if (entry is not null)
        {
            using (var stream = entry.Open())
            {
                data = new byte[entry.Length];
                stream.ReadExactly(data, 0, data.Length);
            }
        }

        return data;
    }

    public bool Exists(string file)
    {
        return this.GetEntryOrdinalIgnoreCase(file) is not null;
    }

    public string GetGameId()
    {
        var id = string.Empty;

        foreach (var entry in this.archive.Entries)
        {
            if (entry.Length > 0)
            {
                var fileName = Path.GetFileNameWithoutExtension(entry.Name);
                var ext = Path.GetExtension(entry.Name);

                if (fileName.EndsWith("vol", StringComparison.OrdinalIgnoreCase) && ext == ".0" && fileName.Length > 3)
                {
                    id = fileName[..^3];
                }
            }
        }

        return id;
    }

    public string[] GetGameFiles()
    {
        List<string> files = [];
        foreach (var entry in this.archive.Entries)
        {
            if (entry.Length > 0)
            {
                if (string.Equals(entry.Name, "object", StringComparison.OrdinalIgnoreCase))
                {
                    files.Add(entry.Name);
                }
                else if (string.Equals(entry.Name, "words.tok", StringComparison.OrdinalIgnoreCase))
                {
                    files.Add(entry.Name);
                }
                else if (string.Equals(entry.Name, "dirs", StringComparison.OrdinalIgnoreCase))
                {
                    files.Add(entry.Name);
                }
                else
                {
                    var fileName = Path.GetFileNameWithoutExtension(entry.Name);
                    var ext = Path.GetExtension(entry.Name);

                    if (fileName.EndsWith("vol", StringComparison.OrdinalIgnoreCase) && int.TryParse(ext[1..], out _))
                    {
                        files.Add(entry.Name);
                    }
                    else if (fileName.EndsWith("dir", StringComparison.OrdinalIgnoreCase) && ext.Length == 0)
                    {
                        files.Add(entry.Name);
                    }
                }
            }
        }

        return [.. files];
    }

    public string[] GetFilesByExtension(string ext)
    {
        List<string> files = [];
        foreach (var entry in this.archive.Entries)
        {
            if (entry.Length > 0)
            {
                if (string.Equals(Path.GetExtension(entry.Name), ext, StringComparison.OrdinalIgnoreCase))
                {
                    files.Add(entry.Name);
                }
            }
        }

        return [.. files];
    }

    private ZipArchiveEntry? GetEntryOrdinalIgnoreCase(string entryName)
    {
        foreach (var entry in this.archive.Entries)
        {
            if (string.Equals(entry.Name, entryName, StringComparison.OrdinalIgnoreCase))
            {
                return entry;
            }
        }

        return null;
    }
}
