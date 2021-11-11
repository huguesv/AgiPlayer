// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Top level menu item.
/// </summary>
public class MenuParentItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MenuParentItem"/> class.
    /// </summary>
    public MenuParentItem()
    {
        this.Items = new List<MenuItem>();
        this.Text = string.Empty;
    }

    /// <summary>
    /// Gets child menu items.
    /// </summary>
    public IList<MenuItem> Items { get; }

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
    /// Gets or sets child item that is currently selected.
    /// </summary>
    public int SelectedIndex { get; set; }

    /// <summary>
    /// Append a menu item.
    /// </summary>
    /// <param name="itemText">Menu item text.</param>
    /// <param name="itemController">Menu item controller.</param>
    public void AppendItem(string itemText, byte itemController)
    {
        byte itemRow = (byte)(1 + this.Items.Count + 1);
        byte itemColumn;

        if (this.Items.Count == 0)
        {
            // The first menu item determines the column
            if ((itemText.Length + this.Column) < Menu.LastMenuColumn)
            {
                itemColumn = this.Column;
            }
            else
            {
                itemColumn = (byte)(Menu.LastMenuColumn - itemText.Length);
            }
        }
        else
        {
            // The remaining items use the same column as the first item
            // (so everything is left aligned)
            itemColumn = this.Items[0].Column;
        }

        var item = new MenuItem
        {
            Text = itemText,
            Controller = itemController,
            Row = itemRow,
            Column = itemColumn,
            Enabled = true,
        };

        this.Items.Add(item);
    }
}
