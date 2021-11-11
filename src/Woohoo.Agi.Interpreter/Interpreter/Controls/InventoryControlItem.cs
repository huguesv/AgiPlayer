// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.Controls;

/// <summary>
/// Inventory item in the inventory screen.
/// </summary>
public class InventoryControlItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryControlItem"/> class.
    /// </summary>
    public InventoryControlItem()
    {
    }

    /// <summary>
    /// Gets or sets item name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets item index in the inventory resource.
    /// </summary>
    public byte Num { get; set; }

    /// <summary>
    /// Gets or sets item column, in text coordinates.
    /// </summary>
    public byte Column { get; set; }

    /// <summary>
    /// Gets or sets item row, in text coordinates.
    /// </summary>
    public byte Row { get; set; }
}
