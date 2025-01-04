// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.UnitTest.Infrastructure;

using System.Collections.Generic;

internal class GameFolderBuilder
{
    private readonly List<string> emptyFiles = [];

    public GameFolderBuilder WithEmptyFiles(params IEnumerable<string> files)
    {
        this.emptyFiles.AddRange(files);
        return this;
    }

    public GameFolder Build(string parentFolder, string name)
    {
        var gameFolder = Path.Combine(parentFolder, name);
        Directory.CreateDirectory(gameFolder);

        foreach (var file in this.emptyFiles)
        {
            File.WriteAllBytes(Path.Combine(gameFolder, file), []);
        }

        return new GameFolder(gameFolder);
    }
}
