// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Resources;

/// <summary>
/// Represents the set of inventory objects.
/// </summary>
/// <remarks>
/// The compiled version of the inventory resource is stored in the objects file.
/// </remarks>
public class InventoryResource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryResource"/> class.
    /// </summary>
    /// <param name="items">Inventory objects.</param>
    /// <param name="maxAnimatedObjects">Maximum number of animated objects.  Used to size the view object table.</param>
    public InventoryResource(InventoryItem[] items, int maxAnimatedObjects)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maxAnimatedObjects);

        this.Items = items ?? throw new ArgumentNullException(nameof(items));
        this.MaxAnimatedObjects = maxAnimatedObjects;
    }

    /// <summary>
    /// Gets inventory objects.
    /// </summary>
    public InventoryItem[] Items { get; }

    /// <summary>
    /// Gets maximum number of animated objects.  Used to size the view object table.
    /// </summary>
    public int MaxAnimatedObjects { get; }
}
