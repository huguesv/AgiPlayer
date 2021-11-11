// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Resources.Serialization;

/// <summary>
/// Collection of resource map entries.
/// </summary>
public class VolumeResourceMapEntryCollection : List<VolumeResourceMapEntry>
{
    /// <summary>
    /// Find the index in the collection of the resource map entry for the specified resource index.
    /// </summary>
    /// <param name="resourceIndex">Resource index.</param>
    /// <returns>Index in the resource map entry collection.</returns>
    public int IndexOf(int resourceIndex)
    {
        if (resourceIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(resourceIndex));
        }

        for (int i = 0; i < this.Count; i++)
        {
            if (this[i].Index == resourceIndex)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Get the resource map entry for the specified resource index.
    /// </summary>
    /// <param name="resourceIndex">Resource index.  This is not the same as the index in the collection.</param>
    /// <returns>Resource map entry.</returns>
    public VolumeResourceMapEntry? GetEntry(int resourceIndex)
    {
        if (resourceIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(resourceIndex));
        }

        int i = this.IndexOf(resourceIndex);
        if (i >= 0)
        {
            return this[i];
        }

        return null;
    }
}
