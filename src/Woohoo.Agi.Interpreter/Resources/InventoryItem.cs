// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources;

/// <summary>
/// Represents an inventory object.
/// </summary>
public class InventoryItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItem"/> class.
    /// </summary>
    /// <param name="name">Inventory object name.</param>
    /// <param name="location">Room for the object.</param>
    public InventoryItem(string name, byte location)
    {
        this.Name = name ?? throw new ArgumentNullException(nameof(name));
        this.Location = location;
    }

    /// <summary>
    /// Gets inventory object name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets room for the object.
    /// </summary>
    public byte Location { get; set; }
}
