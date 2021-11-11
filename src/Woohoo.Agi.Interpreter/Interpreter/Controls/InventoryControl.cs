// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.Controls;

using Woohoo.Agi.Resources;

public class InventoryControl
{
    private const int NormalTextForegroundColor = 0x00;
    private const int NormalTextBackgroundColor = 0x0f;

    private const int SelectedTextForegroundColor = 0x0f;
    private const int SelectedTextBackgroundColor = 0x00;

    public InventoryControl(AgiInterpreter interpreter)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
    }

    public InventoryResource Inventory { get; set; }

    public byte SelectedInventoryNumber { get; set; }

    public bool SelectionEnabled { get; set; }

    protected AgiInterpreter Interpreter { get; }

    protected WindowManager WindowManager => this.Interpreter.WindowManager;

    protected IInputDriver InputDriver => this.Interpreter.InputDriver;

    public bool DoModal()
    {
        byte row = 2;
        byte count = 0;
        int selected = 0;

        var egoItems = new List<InventoryControlItem>();
        for (int i = 0; i < this.Inventory.Items.Length; i++)
        {
            var item = this.Inventory.Items[i];

            if (item.Location == 0xff)
            {
                var egoItem = new InventoryControlItem();
                egoItems.Add(egoItem);

                if (i == this.SelectedInventoryNumber)
                {
                    selected = egoItems.Count - 1;
                }

                egoItem.Num = (byte)i;
                egoItem.Name = item.Name;
                egoItem.Row = row;

                if (count % 2 == 0)
                {
                    egoItem.Column = 1;
                }
                else
                {
                    row++;
                    egoItem.Column = (byte)(39 - item.Name.Length);
                }

                count++;
            }
        }

        if (count == 0)
        {
            var egoItem = new InventoryControlItem
            {
                Num = 0,
                Name = UserInterface.InventoryNothing,
                Row = row,
                Column = 16,
            };

            egoItems.Add(egoItem);
        }

        var array = egoItems.ToArray();

        this.Display(array, selected);

        return this.Poll(array, selected);
    }

    private void Display(InventoryControlItem[] egoItems, int selected)
    {
        if (egoItems == null)
        {
            throw new ArgumentNullException(nameof(egoItems));
        }

        this.WindowManager.GotoPosition(new TextPosition(0, 11));
        this.WindowManager.PrintFormatted(UserInterface.InventoryCarrying);

        for (int i = 0; i < egoItems.Length; i++)
        {
            var egoItem = egoItems[i];

            this.WindowManager.GotoPosition(new TextPosition(egoItem.Row, egoItem.Column));
            if (i != selected || !this.SelectionEnabled)
            {
                this.WindowManager.SetTextColor(NormalTextForegroundColor, NormalTextBackgroundColor);
            }
            else
            {
                this.WindowManager.SetTextColor(15, 0);
            }

            this.WindowManager.PrintFormatted(egoItem.Name);
        }

        this.WindowManager.SetTextColor(0, 15);

        if (this.SelectionEnabled)
        {
            this.WindowManager.GotoPosition(new TextPosition(24, 2));
            this.WindowManager.PrintFormatted(UserInterface.InventoryStatusForItems);
        }
        else
        {
            this.WindowManager.GotoPosition(new TextPosition(24, 4));
            this.WindowManager.PrintFormatted(UserInterface.InventoryStatusNoItems);
        }
    }

    private bool Poll(InventoryControlItem[] egoItems, int selected)
    {
        if (this.SelectionEnabled)
        {
            while (true)
            {
                var e = this.InputDriver.PollEvent();

                switch (e.Type)
                {
                    case InputEventType.Mouse:
                        switch (e.Data)
                        {
                            case MouseButton.Left:
                                var pt = this.Interpreter.GraphicsDriver.ScreenToRenderPoint(new ScreenPoint(e.X, e.Y));
                                var pos = new TextPosition((byte)(pt.Y / this.Interpreter.GraphicsRenderer.RenderFontHeight), (byte)(pt.X / this.Interpreter.GraphicsRenderer.RenderFontWidth));
                                for (int i = 0; i < egoItems.Length; i++)
                                {
                                    var item = egoItems[i];
                                    if (pos.Row == item.Row && pos.Column >= item.Column)
                                    {
                                        if ((pos.Column - item.Column) < item.Name.Length)
                                        {
                                            this.ChangeSelection(egoItems[selected], egoItems[i]);
                                            selected = i;

                                            // Enter
                                            this.SelectedInventoryNumber = egoItems[selected].Num;
                                            return true;
                                        }
                                    }
                                }

                                break;
                        }

                        break;

                    case InputEventType.Ascii:
                        if (e.Data == InputEventAscii.Enter)
                        {
                            // Enter
                            this.SelectedInventoryNumber = egoItems[selected].Num;
                            return true;
                        }

                        if (e.Data == InputEventAscii.Esc)
                        {
                            // Esc
                            this.SelectedInventoryNumber = 0xff;
                            return false;
                        }

                        break;

                    case InputEventType.Direction:
                        this.ArrowKey(egoItems, ref selected, e.Data);
                        break;
                }
            }
        }
        else
        {
            this.InputDriver.PollEvent();
        }

        return true;
    }

    private void ArrowKey(InventoryControlItem[] egoItems, ref int selected, int direction)
    {
        int oldItem = selected;
        int newItem = selected;

        switch (direction)
        {
            case InputEventDirection.Up:
                newItem -= 2;
                break;

            case InputEventDirection.Right:
                newItem++;
                break;

            case InputEventDirection.Down:
                newItem += 2;
                break;

            case InputEventDirection.Left:
                newItem--;
                break;
        }

        if (newItem < 0 || newItem >= egoItems.Length)
        {
            newItem = oldItem;
        }

        this.ChangeSelection(egoItems[oldItem], egoItems[newItem]);

        selected = newItem;
    }

    private void ChangeSelection(InventoryControlItem oldItem, InventoryControlItem newItem)
    {
        if (oldItem == null)
        {
            throw new ArgumentNullException(nameof(oldItem));
        }

        if (newItem == null)
        {
            throw new ArgumentNullException(nameof(newItem));
        }

        if (oldItem != newItem)
        {
            this.WindowManager.SetTextColor(SelectedTextForegroundColor, SelectedTextBackgroundColor);
            this.WindowManager.GotoPosition(new TextPosition(newItem.Row, newItem.Column));
            this.WindowManager.PrintFormatted(newItem.Name);

            this.WindowManager.SetTextColor(NormalTextForegroundColor, NormalTextBackgroundColor);
            this.WindowManager.GotoPosition(new TextPosition(oldItem.Row, oldItem.Column));
            this.WindowManager.PrintFormatted(oldItem.Name);
        }
    }
}
