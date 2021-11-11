// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    /// <summary>
    /// Menu hierarchical structure.
    /// </summary>
    public class Menu
    {
        public const int LastMenuColumn = 39;

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        public Menu()
        {
            this.Items = new List<MenuParentItem>();
        }

        /// <summary>
        /// Gets top-level menu items (parent items).
        /// </summary>
        public IList<MenuParentItem> Items { get; }

        /// <summary>
        /// Gets or sets a value indicating whether once a menu is submitted, no changes are made to its structure.
        /// </summary>
        public bool Submitted { get; set; }

        /// <summary>
        /// Enable all the menu items.
        /// </summary>
        public void EnableAllItems()
        {
            foreach (var parent in this.Items)
            {
                if (parent.Enabled)
                {
                    foreach (var item in parent.Items)
                    {
                        item.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Enable or disable the item(s) that use the specified controller.
        /// </summary>
        /// <param name="controller">Controller to search for.</param>
        /// <param name="enable">Enable/disable menu item(s).</param>
        public void SetItemEnabled(byte controller, bool enable)
        {
            foreach (var parentItem in this.Items)
            {
                foreach (var item in parentItem.Items)
                {
                    if (item.Controller == controller)
                    {
                        item.Enabled = enable;
                    }
                }
            }
        }

        /// <summary>
        /// Append a parent menu item.
        /// </summary>
        /// <param name="text">Parent menu item text.</param>
        public void AppendParentItem(string text)
        {
            byte column = 1;

            foreach (var current in this.Items)
            {
                column += (byte)(current.Text.Length + 1);
            }

            var parentItem = new MenuParentItem();
            parentItem.Column = column;
            parentItem.Row = 0;
            parentItem.Text = text;
            parentItem.Enabled = true;

            this.Items.Add(parentItem);
        }
    }
}
