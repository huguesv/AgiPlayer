// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Detection
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Xml;

    /// <summary>
    /// Collection of FileCrc objects.
    /// </summary>
    internal class GameFileCollection : Collection<GameFile>
    {
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="filename">Name of file.</param>
        internal GameFile this[string filename]
        {
            get
            {
                foreach (GameFile file in this)
                {
                    if (string.Compare(file.Name, filename, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return file;
                    }
                }

                return null;
            }
        }
    }
}
