// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Resources.Serialization;

/// <summary>
/// The resource map contains the location (in the volumes) of all the game resources.
/// </summary>
/// <remarks>
/// Vocabulary and inventory resources are not stored in volumes, so there is no resource map entries for them.
/// </remarks>
public class VolumeResourceMap
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeResourceMap"/> class.
    /// </summary>
    public VolumeResourceMap()
    {
        this.SoundResources = [];
        this.ViewResources = [];
        this.LogicResources = [];
        this.PictureResources = [];
    }

    /// <summary>
    /// Gets resource map entries for the sound resources.
    /// </summary>
    public VolumeResourceMapEntryCollection SoundResources { get; }

    /// <summary>
    /// Gets resource map entries for the view resources.
    /// </summary>
    public VolumeResourceMapEntryCollection ViewResources { get; }

    /// <summary>
    /// Gets resource map entries for the logic resources.
    /// </summary>
    public VolumeResourceMapEntryCollection LogicResources { get; }

    /// <summary>
    /// Gets resource map entries for the picture resources.
    /// </summary>
    public VolumeResourceMapEntryCollection PictureResources { get; }
}
