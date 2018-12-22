// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    using System.Collections.Generic;

    /// <summary>
    /// Comparer for alphabetical sorting of games.
    /// </summary>
    public sealed class GameStartInfoComparer : IComparer<GameStartInfo>
    {
        int IComparer<GameStartInfo>.Compare(GameStartInfo x, GameStartInfo y)
        {
            string nameX = x.Name + x.Version;
            string nameY = y.Name + y.Version;

            return nameX.CompareTo(nameY);
        }
    }
}
