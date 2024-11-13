// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Detection;

/// <summary>
/// Collection of FileCrc objects.
/// </summary>
internal class GameFileCollection : Collection<GameFile>
{
    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="filename">Name of file.</param>
    internal GameFile? this[string filename]
    {
        get
        {
            foreach (GameFile file in this)
            {
                if (string.Equals(file.Name, filename, StringComparison.OrdinalIgnoreCase))
                {
                    return file;
                }
            }

            return null;
        }
    }
}
