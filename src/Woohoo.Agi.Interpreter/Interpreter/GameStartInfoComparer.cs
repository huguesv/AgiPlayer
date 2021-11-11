// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Comparer for alphabetical sorting of games.
/// </summary>
public sealed class GameStartInfoComparer : IComparer<GameStartInfo>
{
    int IComparer<GameStartInfo>.Compare(GameStartInfo? x, GameStartInfo? y)
    {
        if (x is null && y is null)
        {
            return 0;
        }

        if (x is null)
        {
            return 1;
        }

        if (y is null)
        {
            return -1;
        }

        string nameX = x.Name + x.Version;
        string nameY = y.Name + y.Version;

        return nameX.CompareTo(nameY);
    }
}
