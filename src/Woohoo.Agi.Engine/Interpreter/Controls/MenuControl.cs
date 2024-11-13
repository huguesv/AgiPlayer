// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter.Controls;

public class MenuControl
{
    private const byte MenuForegroundColor = 0x00;
    private const byte MenuBackgroundColor = 0x0f;
    private int menuSizeWidth;
    private int menuSizeHeight;
    private int menuPosX;
    private int menuPosY;

    public MenuControl(AgiInterpreter interpreter)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
    }

    public Menu Menu { get; set; }

    public int CurrentParentMenuItemIndex { get; set; }

    public int CurrentMenuItemIndex { get; set; }

    protected AgiInterpreter Interpreter { get; }

    protected State State => this.Interpreter.State;

    protected WindowManager WindowManager => this.Interpreter.WindowManager;

    protected GraphicsRenderer GraphicsRenderer => this.Interpreter.GraphicsRenderer;

    protected IInputDriver InputDriver => this.Interpreter.InputDriver;

    public bool DoModal()
    {
        this.WindowManager.PushTextPosition();
        this.WindowManager.PushTextColor();
        this.WindowManager.ClearLine(0, this.GraphicsRenderer.CalculateTextBackground(MenuBackgroundColor));
        this.WindowManager.UpdateTextRegion();

        foreach (MenuParentItem parent in this.Menu.Items)
        {
            this.DisplayUnselected(parent);
        }

        this.DisplayMenu(this.Menu.Items[this.CurrentParentMenuItemIndex]);

        bool accepted = this.Poll();

        this.Clear(this.Menu.Items[this.CurrentParentMenuItemIndex], this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);
        this.WindowManager.PopTextColor();
        this.WindowManager.PopTextPosition();

        this.WindowManager.ClearLine(0, MenuForegroundColor);
        this.WindowManager.UpdateTextRegion();

        return accepted;
    }

    private bool Poll()
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

                            // Check if it's a menu item under the currently selected parent menu
                            for (int j = 0; j < this.Menu.Items[this.CurrentParentMenuItemIndex].Items.Count; j++)
                            {
                                var item = this.Menu.Items[this.CurrentParentMenuItemIndex].Items[j];
                                if (item.Enabled)
                                {
                                    if (pos.Row == item.Row && pos.Column >= item.Column)
                                    {
                                        if ((pos.Column - item.Column) < item.Text.Length)
                                        {
                                            this.DisplayUnselected(this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);
                                            this.CurrentMenuItemIndex = j;
                                            this.DisplaySelected(this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);

                                            // Enter
                                            return this.Accept();
                                        }
                                    }
                                }
                            }

                            // Check if it's a parent menu
                            for (int i = 0; i < this.Menu.Items.Count; i++)
                            {
                                var parentItem = this.Menu.Items[i];
                                if (parentItem.Enabled && parentItem.Items.Count > 0)
                                {
                                    if (pos.Row == parentItem.Row && pos.Column >= parentItem.Column)
                                    {
                                        if ((pos.Column - parentItem.Column) < parentItem.Text.Length)
                                        {
                                            this.Clear(this.Menu.Items[this.CurrentParentMenuItemIndex], this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);
                                            this.CurrentParentMenuItemIndex = i;
                                            this.CurrentMenuItemIndex = this.Menu.Items[this.CurrentParentMenuItemIndex].SelectedIndex;
                                            this.DisplayMenu(this.Menu.Items[this.CurrentParentMenuItemIndex]);
                                            break;
                                        }
                                    }
                                }
                            }

                            break;
                        case MouseButton.Right:
                            return false;
                    }

                    break;
                case InputEventType.Ascii:
                    switch (e.Data)
                    {
                        case InputEventAscii.Enter:
                            return this.Accept();

                        case InputEventAscii.Esc:
                            return false;
                    }

                    break;

                case InputEventType.Direction:
                    switch (e.Data)
                    {
                        case InputEventDirection.Up:
                            this.Up();
                            break;

                        case InputEventDirection.PageUp:
                            this.PageUp();
                            break;

                        case InputEventDirection.Right:
                            this.Right();
                            break;

                        case InputEventDirection.PageDown:
                            this.PageDown();
                            break;

                        case InputEventDirection.Down:
                            this.Down();
                            break;

                        case InputEventDirection.End:
                            this.End();
                            break;

                        case InputEventDirection.Left:
                            this.Left();
                            break;

                        case InputEventDirection.Home:
                            this.Home();
                            break;

                        default:
                            break;
                    }

                    break;
            }
        }
    }

    private void CalculateSize(MenuParentItem parent)
    {
        this.menuSizeHeight = WindowManager.CharacterHeight * (parent.Items.Count + 2);
        this.menuSizeWidth = (parent.Items[0].Text.Length << 2) + 8;

        this.menuPosY = ((parent.Items.Count + 3 - this.State.WindowRowMin) * WindowManager.CharacterHeight) - 1;
        this.menuPosX = (parent.Items[0].Column - 1) << 2;
    }

    private void DisplayMenu(MenuParentItem parent)
    {
        this.DisplaySelected(parent);
        this.CalculateSize(parent);
        this.WindowManager.DisplayMessageBoxWindow(new PictureRectangle(this.menuPosX, this.menuPosY, this.menuSizeWidth, this.menuSizeHeight), MenuBackgroundColor, MenuForegroundColor);

        for (int i = 0; i < parent.Items.Count; i++)
        {
            var item = parent.Items[i];
            if (i == parent.SelectedIndex)
            {
                this.DisplaySelected(item);
            }
            else
            {
                this.DisplayUnselected(item);
            }
        }
    }

    private void DisplayUnselected(MenuParentItem parent)
    {
        this.WindowManager.SetTextColor(MenuForegroundColor, this.GraphicsRenderer.CalculateTextBackground(MenuBackgroundColor));
        this.WindowManager.GotoPosition(new TextPosition(parent.Row, parent.Column));

        if (!parent.Enabled)
        {
            this.WindowManager.TextShade = true;
        }

        this.WindowManager.PrintFormatted(parent.Text);
        this.WindowManager.TextShade = false;
    }

    private void DisplaySelected(MenuParentItem parent)
    {
        this.WindowManager.SetTextColor(MenuBackgroundColor, this.GraphicsRenderer.CalculateTextBackground(MenuForegroundColor));
        this.WindowManager.GotoPosition(new TextPosition(parent.Row, parent.Column));

        if (!parent.Enabled)
        {
            this.WindowManager.TextShade = true;
        }

        this.WindowManager.PrintFormatted(parent.Text);
        this.WindowManager.TextShade = false;
    }

    private void DisplayUnselected(MenuItem item)
    {
        this.WindowManager.SetTextColor(MenuForegroundColor, this.GraphicsRenderer.CalculateTextBackground(MenuBackgroundColor));
        this.WindowManager.GotoPosition(new TextPosition(item.Row, item.Column));

        if (!item.Enabled)
        {
            this.WindowManager.TextShade = true;
        }

        this.WindowManager.PrintFormatted(item.Text);
        this.WindowManager.TextShade = false;
    }

    private void DisplaySelected(MenuItem item)
    {
        this.WindowManager.SetTextColor(MenuBackgroundColor, this.GraphicsRenderer.CalculateTextBackground(MenuForegroundColor));
        this.WindowManager.GotoPosition(new TextPosition(item.Row, item.Column));

        if (!item.Enabled)
        {
            this.WindowManager.TextShade = true;
        }

        this.WindowManager.PrintFormatted(item.Text);
        this.WindowManager.TextShade = false;
    }

    private void Clear(MenuParentItem parent, MenuItem item)
    {
        parent.SelectedIndex = parent.Items.IndexOf(item);
        this.DisplayUnselected(parent);
        this.GraphicsRenderer.RenderPictureBuffer(new PictureRectangle(this.menuPosX, this.menuPosY, this.menuSizeWidth, this.menuSizeHeight), false, false);
    }

    private bool Accept()
    {
        return this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex].Enabled;
    }

    private void Up()
    {
        this.DisplayUnselected(this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);

        this.CurrentMenuItemIndex--;
        if (this.CurrentMenuItemIndex < 0)
        {
            this.CurrentMenuItemIndex = this.Menu.Items[this.CurrentParentMenuItemIndex].Items.Count - 1;
        }

        this.DisplaySelected(this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);
    }

    private void PageUp()
    {
        this.DisplayUnselected(this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);

        this.CurrentMenuItemIndex = 0;

        this.DisplaySelected(this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);
    }

    private void Right()
    {
        this.Clear(this.Menu.Items[this.CurrentParentMenuItemIndex], this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);

        do
        {
            this.CurrentParentMenuItemIndex++;
            if (this.CurrentParentMenuItemIndex >= this.Menu.Items.Count)
            {
                this.CurrentParentMenuItemIndex = 0;
            }
        }
        while (this.Menu.Items[this.CurrentParentMenuItemIndex].Items.Count == 0);
        this.CurrentMenuItemIndex = this.Menu.Items[this.CurrentParentMenuItemIndex].SelectedIndex;
        this.DisplayMenu(this.Menu.Items[this.CurrentParentMenuItemIndex]);
    }

    private void PageDown()
    {
        this.DisplayUnselected(this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);

        this.CurrentMenuItemIndex = this.Menu.Items[this.CurrentParentMenuItemIndex].Items.Count - 1;

        this.DisplaySelected(this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);
    }

    private void Down()
    {
        this.DisplayUnselected(this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);

        this.CurrentMenuItemIndex++;
        if (this.CurrentMenuItemIndex >= this.Menu.Items[this.CurrentParentMenuItemIndex].Items.Count)
        {
            this.CurrentMenuItemIndex = 0;
        }

        this.DisplaySelected(this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);
    }

    private void Home()
    {
        this.Clear(this.Menu.Items[this.CurrentParentMenuItemIndex], this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);

        this.CurrentParentMenuItemIndex = 0;
        this.CurrentMenuItemIndex = this.Menu.Items[this.CurrentParentMenuItemIndex].SelectedIndex;

        this.DisplayMenu(this.Menu.Items[this.CurrentParentMenuItemIndex]);
    }

    private void Left()
    {
        this.Clear(this.Menu.Items[this.CurrentParentMenuItemIndex], this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);

        do
        {
            this.CurrentParentMenuItemIndex--;
            if (this.CurrentParentMenuItemIndex < 0)
            {
                this.CurrentParentMenuItemIndex = this.Menu.Items.Count - 1;
            }
        }
        while (this.Menu.Items[this.CurrentParentMenuItemIndex].Items.Count == 0);

        this.CurrentMenuItemIndex = this.Menu.Items[this.CurrentParentMenuItemIndex].SelectedIndex;
        this.DisplayMenu(this.Menu.Items[this.CurrentParentMenuItemIndex]);
    }

    private void End()
    {
        this.Clear(this.Menu.Items[this.CurrentParentMenuItemIndex], this.Menu.Items[this.CurrentParentMenuItemIndex].Items[this.CurrentMenuItemIndex]);

        this.CurrentParentMenuItemIndex = this.Menu.Items.Count - 1;
        this.CurrentMenuItemIndex = this.Menu.Items[this.CurrentParentMenuItemIndex].SelectedIndex;

        this.DisplayMenu(this.Menu.Items[this.CurrentParentMenuItemIndex]);
    }
}
