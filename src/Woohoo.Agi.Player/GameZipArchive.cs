// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player;

#if USE_ZIP
using ICSharpCode.SharpZipLib.Zip;
using Woohoo.Agi.Interpreter;

internal sealed class GameZipArchive : IGameContainer
{
    private readonly string archivePath;
    private readonly ZipFile zipFile;

    public GameZipArchive(string archivePath)
    {
        this.archivePath = archivePath;
        this.zipFile = new ZipFile(archivePath);
    }

    public string Name => Path.GetFileNameWithoutExtension(this.archivePath);

    public byte[] Read(string file)
    {
        byte[] data = [];

        int entryIndex = this.zipFile.FindEntry(file, true);
        if (entryIndex != -1)
        {
            ZipEntry entry = this.zipFile[entryIndex];
            if (entry is not null)
            {
                using (Stream stream = this.zipFile.GetInputStream(entry))
                {
                    data = new byte[entry.Size];
                    stream.ReadExactly(data, 0, data.Length);
                }
            }
        }

        return data;
    }

    public bool Exists(string file)
    {
        int entryIndex = this.zipFile.FindEntry(file, true);
        return entryIndex != -1;
    }

    public string GetGameId()
    {
        string id = string.Empty;

        foreach (ZipEntry entry in this.zipFile)
        {
            if (entry.IsFile)
            {
                string lowerFileNameNoExt = Path.GetFileNameWithoutExtension(entry.Name).ToLower(CultureInfo.InvariantCulture);
                string ext = Path.GetExtension(entry.Name);

                if (lowerFileNameNoExt.EndsWith("vol") && ext == ".0")
                {
                    if (lowerFileNameNoExt.Length > 3)
                    {
                        id = lowerFileNameNoExt[..^3];
                    }
                }
            }
        }

        return id;
    }

    public string[] GetGameFiles()
    {
        List<string> files = [];
        foreach (ZipEntry entry in this.zipFile)
        {
            if (entry.IsFile)
            {
                if (string.Compare(entry.Name, "object", true) == 0)
                {
                    files.Add(entry.Name);
                }
                else if (string.Compare(entry.Name, "words.tok", true) == 0)
                {
                    files.Add(entry.Name);
                }
                else if (string.Compare(entry.Name, "dirs", true) == 0)
                {
                    files.Add(entry.Name);
                }
                else
                {
                    string lowerFileNameNoExt = Path.GetFileNameWithoutExtension(entry.Name).ToLower(CultureInfo.InvariantCulture);
                    string ext = Path.GetExtension(entry.Name);

                    if (lowerFileNameNoExt.EndsWith("vol"))
                    {
                        files.Add(entry.Name);
                    }
                    else if (lowerFileNameNoExt.EndsWith("dir") && ext.Length == 0)
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
        foreach (ZipEntry entry in this.zipFile)
        {
            if (entry.IsFile)
            {
                if (string.Compare(Path.GetExtension(entry.Name), ext, true) == 0)
                {
                    files.Add(entry.Name);
                }
            }
        }

        return [.. files];
    }
}
#endif
