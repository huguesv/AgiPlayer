// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Resources.Serialization;

/// <summary>
/// The resource map entry contains the location of a resource (in the volumes).
/// </summary>
public class VolumeResourceMapEntry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeResourceMapEntry"/> class.
    /// </summary>
    /// <remarks>
    /// The textIndex, volume and offset are all set to their default value of 0.
    /// </remarks>
    public VolumeResourceMapEntry()
        : this(0, 0, 0)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeResourceMapEntry"/> class.
    /// </summary>
    /// <param name="index">Resource index.</param>
    /// <param name="volume">Volume index.</param>
    /// <param name="offset">Location of the resource in the volume.</param>
    public VolumeResourceMapEntry(byte index, byte volume, int offset)
    {
        this.Index = index;
        this.Volume = volume;
        this.Offset = offset;
    }

    /// <summary>
    /// Gets or sets resource index.
    /// </summary>
    public byte Index { get; set; }

    /// <summary>
    /// Gets or sets volume index.
    /// </summary>
    public byte Volume { get; set; }

    /// <summary>
    /// Gets or sets location of the resource in the volume.
    /// </summary>
    public int Offset { get; set; }
}
