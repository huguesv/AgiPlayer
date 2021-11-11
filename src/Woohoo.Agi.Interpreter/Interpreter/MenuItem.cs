// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Menu item.
/// </summary>
public class MenuItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItem"/> class.
    /// </summary>
    public MenuItem()
    {
        this.Text = string.Empty;
    }

    /// <summary>
    /// Gets or sets caption of the item.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets row, in text coordinates, where the menu item is to be displayed.
    /// </summary>
    public byte Row { get; set; }

    /// <summary>
    /// Gets or sets column, in text coordinates, where the menu item is to be displayed.
    /// </summary>
    public byte Column { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether state of the menu item.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets controller that is set when the item is clicked.
    /// </summary>
    public byte Controller { get; set; }
}
