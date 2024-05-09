// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Resources;

/// <summary>
/// Represents a view resource. The view resource contains multiple related loops of animation.
/// </summary>
public class ViewResource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewResource"/> class.
    /// </summary>
    /// <param name="resourceIndex">Resource index (0-255).</param>
    /// <param name="loops">Array of view loops.</param>
    /// <param name="description">Description of the view, used for inventory object description.</param>
    /// <param name="unknown1">Unknown 1.</param>
    /// <param name="unknown2">Unknown 2.</param>
    public ViewResource(byte resourceIndex, ViewLoop[] loops, string description, byte unknown1, byte unknown2)
    {
        this.ResourceIndex = resourceIndex;
        this.Loops = loops ?? throw new ArgumentNullException(nameof(loops));
        this.Description = description ?? throw new ArgumentNullException(nameof(description));
        this.Unknown1 = unknown1;
        this.Unknown2 = unknown2;
    }

    /// <summary>
    /// Gets resource index (0-255).
    /// </summary>
    public byte ResourceIndex { get; }

    /// <summary>
    /// Gets array of view loops.
    /// </summary>
    public ViewLoop[] Loops { get; }

    /// <summary>
    /// Gets description of the view, used for inventory object description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets unknown at this time.
    /// </summary>
    public byte Unknown1 { get; }

    /// <summary>
    /// Gets unknown at this time.
    /// </summary>
    public byte Unknown2 { get; }
}
