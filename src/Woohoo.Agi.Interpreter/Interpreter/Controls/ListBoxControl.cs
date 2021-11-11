// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.Controls
{
    public class ListBoxControl
    {
        private const byte ListBoxForegroundColor = 0x00;
        private const byte ListBoxBackgroundColor = 0x0f;
        private string[] items;
        private int selectedItemIndex;

        public ListBoxControl(AgiInterpreter interpreter)
        {
            this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
            this.Title = string.Empty;
            this.Width = 10;
            this.Height = 10;
            this.items = new string[0];
        }

        public string Title { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int SelectedItemIndex
        {
            get
            {
                return this.selectedItemIndex;
            }

            set
            {
                if (value < -1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.selectedItemIndex = value;
            }
        }

        protected AgiInterpreter Interpreter { get; }

        protected WindowManager WindowManager => this.Interpreter.WindowManager;

        protected IInputDriver InputDriver => this.Interpreter.InputDriver;

        public void SetItems(string[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            this.items = (string[])items.Clone();
        }

        public bool DoModal()
        {
            int firstItemIndex = this.SelectedItemIndex;

            this.WindowManager.PushTextColor();
            this.WindowManager.PushTextPosition();
            this.WindowManager.SetTextColor(ListBoxForegroundColor, ListBoxBackgroundColor);

            int currentItemIndex = firstItemIndex;
            if (currentItemIndex >= this.items.Length)
            {
                currentItemIndex = this.items.Length - 1;
            }

            if (currentItemIndex < 0)
            {
                currentItemIndex = 0;
            }

            this.WindowManager.DisplayMessageBox(this.Title, this.Height, this.Width, true);

            int height = this.WindowManager.MessageState.TextLowRow - this.WindowManager.MessageState.TextTopRow - this.WindowManager.MessageState.PrintedHeight;
            int width = this.WindowManager.MessageState.TextWidth;

            if (this.items.Length > height)
            {
                width -= 2;
            }

            if (this.items.Length < height)
            {
                height = this.items.Length;
            }

            int listRow = this.WindowManager.MessageState.TextTopRow + this.WindowManager.MessageState.PrintedHeight + 1;
            int listColumn = this.WindowManager.MessageState.TextLeftColumn;

            int itemTop = 0;
            if (this.items.Length > height)
            {
                itemTop = currentItemIndex - ((height - 1) / 2);
                if (itemTop < 0)
                {
                    itemTop = 0;
                }

                if (itemTop > (this.items.Length - height))
                {
                    itemTop = this.items.Length - height;
                }
            }

            this.DrawItems(this.items, itemTop, listRow, listColumn, width, height);
            this.DrawSelector((byte)(listRow + currentItemIndex - itemTop));

            this.SelectedItemIndex = this.Poll(this.items, currentItemIndex, itemTop, width, height, listRow, listColumn);

            this.WindowManager.PopTextPosition();
            this.WindowManager.PopTextColor();

            this.WindowManager.CloseWindow();

            return this.SelectedItemIndex >= 0;
        }

        private static bool IsUpButton(string[] items, int width, int height, int listRow, int listColumn, TextPosition pos)
        {
            bool up = false;
            bool status = items.Length > height;

            if (status)
            {
                var abovePos = new TextPosition((byte)listRow, (byte)(listColumn + width + 1));
                up = abovePos == pos;
            }

            return up;
        }

        private static bool IsDownButton(string[] items, int width, int height, int listRow, int listColumn, TextPosition pos)
        {
            bool down = false;
            bool status = items.Length > height;

            if (status)
            {
                var belowPos = new TextPosition((byte)(listRow + height - 1), (byte)(listColumn + width + 1));
                down = belowPos == pos;
            }

            return down;
        }

        private int Poll(string[] items, int currentItemIndex, int itemTop, int width, int height, int listRow, int listColumn)
        {
            while (true)
            {
                this.Interpreter.WindowManager.UpdateTextRegion();

                var e = this.InputDriver.PollEvent();

                switch (e.Type)
                {
                    case InputEventType.Mouse:
                        switch (e.Data)
                        {
                            case MouseButton.Left:
                                var pt = this.Interpreter.GraphicsDriver.ScreenToRenderPoint(new ScreenPoint(e.X, e.Y));
                                var pos = new TextPosition((byte)(pt.Y / this.Interpreter.GraphicsRenderer.RenderFontHeight), (byte)(pt.X / this.Interpreter.GraphicsRenderer.RenderFontWidth));

                                if (IsDownButton(items, width, height, listRow, listColumn, pos))
                                {
                                    if (currentItemIndex < (items.Length - 1))
                                    {
                                        this.DrawNoSelector((byte)(listRow + currentItemIndex - itemTop));
                                        currentItemIndex = itemTop + height - 1;
                                        this.AfterKeyDown(items, ref currentItemIndex, ref itemTop, width, height, listRow, listColumn, 4);
                                    }
                                }
                                else if (IsUpButton(items, width, height, listRow, listColumn, pos))
                                {
                                    if (currentItemIndex > 0)
                                    {
                                        this.DrawNoSelector((byte)(listRow + currentItemIndex - itemTop));
                                        currentItemIndex = itemTop;
                                        this.AfterKeyDown(items, ref currentItemIndex, ref itemTop, width, height, listRow, listColumn, 2);
                                    }
                                }
                                else if (pos.Row >= listRow && pos.Row < (listRow + height))
                                {
                                    if (pos.Column >= listColumn && pos.Column < (listColumn + width))
                                    {
                                        this.DrawNoSelector((byte)(listRow + currentItemIndex - itemTop));
                                        currentItemIndex = itemTop + (pos.Row - listRow);
                                        this.AfterKeyDown(items, ref currentItemIndex, ref itemTop, width, height, listRow, listColumn, 0);
                                        return currentItemIndex;
                                    }
                                }

                                break;
                        }

                        break;
                    case InputEventType.Ascii:
                        switch (e.Data)
                        {
                            case InputEventAscii.Enter:
                                return currentItemIndex;
                            case InputEventAscii.Esc:
                                return -1;
                        }

                        break;
                    case InputEventType.Direction:
                        {
                            switch (e.Data)
                            {
                                case InputEventDirection.Up:
                                    if (currentItemIndex > 0)
                                    {
                                        this.DrawNoSelector((byte)(listRow + currentItemIndex - itemTop));
                                        currentItemIndex--;
                                        this.AfterKeyDown(items, ref currentItemIndex, ref itemTop, width, height, listRow, listColumn, 0);
                                    }

                                    break;
                                case InputEventDirection.Down:
                                    if (currentItemIndex < (items.Length - 1))
                                    {
                                        this.DrawNoSelector((byte)(listRow + currentItemIndex - itemTop));
                                        currentItemIndex++;
                                        this.AfterKeyDown(items, ref currentItemIndex, ref itemTop, width, height, listRow, listColumn, 0);
                                    }

                                    break;
                                case InputEventDirection.PageUp:
                                    if (currentItemIndex > 0)
                                    {
                                        this.DrawNoSelector((byte)(listRow + currentItemIndex - itemTop));
                                        currentItemIndex = itemTop;
                                        this.AfterKeyDown(items, ref currentItemIndex, ref itemTop, width, height, listRow, listColumn, 2);
                                    }

                                    break;
                                case InputEventDirection.PageDown:
                                    if (currentItemIndex < (items.Length - 1))
                                    {
                                        this.DrawNoSelector((byte)(listRow + currentItemIndex - itemTop));
                                        currentItemIndex = itemTop + height - 1;
                                        this.AfterKeyDown(items, ref currentItemIndex, ref itemTop, width, height, listRow, listColumn, 4);
                                    }

                                    break;
                            }
                        }

                        break;
                }
            }
        }

        private void DrawItems(string[] items, int itemTop, int listRow, int listColumn, int width, int height)
        {
            bool status = items.Length > height;
            bool up = itemTop > 0;
            bool down = (itemTop + height) < items.Length;

            this.DrawItems(items, itemTop, new TextPosition((byte)listRow, (byte)listColumn), (byte)width, (byte)height, status, up, down);
        }

        private void DrawItems(string[] items, int startItemIndex, TextPosition pos, byte width, byte height, bool status, bool up, bool down)
        {
            this.WindowManager.ClearWindow(new TextPosition(pos.Row, pos.Column), new TextPosition((byte)(pos.Row + height - 1), (byte)(pos.Column + width - 1)), 0xff);

            for (int i = 0; i < height; i++)
            {
                this.WindowManager.GotoPosition(new TextPosition((byte)(pos.Row + i), pos.Column));

                string itemText = items[startItemIndex + i];

                if (itemText.Length > 550)
                {
                    this.WindowManager.PrintFormatted(UserInterface.ListBoxItemTooLong);
                }
                else
                {
                    string text = string.Format(CultureInfo.CurrentCulture, UserInterface.ListBoxItemFormat, itemText);
                    if (text.Length > width)
                    {
                        text = text.Substring(0, width);
                    }

                    this.WindowManager.PrintFormatted(text);
                }
            }

            if (status)
            {
                this.WindowManager.GotoPosition(new TextPosition(pos.Row, (byte)(pos.Column + width + 1)));
                if (up)
                {
                    this.WindowManager.PrintFormatted(UserInterface.ListBoxScrollbarUp);
                }
                else
                {
                    this.WindowManager.PrintFormatted(UserInterface.ListBoxScrollbarUpHidden);
                }

                this.WindowManager.GotoPosition(new TextPosition((byte)(pos.Row + height - 1), (byte)(pos.Column + width + 1)));
                if (down)
                {
                    this.WindowManager.PrintFormatted(UserInterface.ListBoxScrollbarDown);
                }
                else
                {
                    this.WindowManager.PrintFormatted(UserInterface.ListBoxScrollbarDownHidden);
                }
            }
        }

        private void DrawSelector(byte row)
        {
            this.WindowManager.GotoPosition(new TextPosition(row, (byte)this.WindowManager.MessageState.TextLeftColumn));
            this.WindowManager.DisplayCharacter((char)0x1a);
        }

        private void DrawNoSelector(byte row)
        {
            this.WindowManager.GotoPosition(new TextPosition(row, (byte)this.WindowManager.MessageState.TextLeftColumn));
            this.WindowManager.DisplayCharacter(' ');
        }

        private void AfterKeyDown(string[] items, ref int currentItemIndex, ref int itemTop, int width, int height, int listRow, int listColumn, int force)
        {
            if (currentItemIndex < 0)
            {
                currentItemIndex = 0;
            }
            else if (currentItemIndex >= items.Length)
            {
                currentItemIndex = items.Length - 1;
            }

            if (currentItemIndex > (itemTop + height - 1) || force == 4)
            {
                itemTop = currentItemIndex - (height / 4);
                if (itemTop < 0)
                {
                    itemTop = 0;
                }

                if ((itemTop + height) > items.Length)
                {
                    itemTop = items.Length - height;
                }

                this.DrawItems(items, itemTop, listRow, listColumn, width, height);
            }
            else if (currentItemIndex < itemTop || force == 2)
            {
                itemTop = currentItemIndex - ((height * 3) / 4);
                if (itemTop < 0)
                {
                    itemTop = 0;
                }

                if ((itemTop + height) > items.Length)
                {
                    itemTop = items.Length - height;
                }

                this.DrawItems(items, itemTop, listRow, listColumn, width, height);
            }

            this.DrawSelector((byte)(listRow + currentItemIndex - itemTop));
        }
    }
}
