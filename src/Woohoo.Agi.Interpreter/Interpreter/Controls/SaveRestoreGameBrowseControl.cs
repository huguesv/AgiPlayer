// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.Controls
{
    using System;
    using System.Globalization;

    public class SaveRestoreGameBrowseControl
    {
        private const byte ListBoxForegroundColor = 0x00;
        private const byte ListBoxBackgroundColor = 0x0f;
        private const int DefaultWindowTop = 5;
        private const int DefaultWindowWidth = 34;
        private string[] descriptions;
        private int[] slotNumbers;
        private int slotCount;

        public SaveRestoreGameBrowseControl(AgiInterpreter interpreter)
        {
            this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
            this.Title = string.Empty;
            this.descriptions = new string[0];
            this.slotNumbers = new int[0];
        }

        public string Title { get; set; }

        public int SelectedSlotIndex { get; set; }

        protected AgiInterpreter Interpreter { get; }

        protected WindowManager WindowManager => this.Interpreter.WindowManager;

        protected IInputDriver InputDriver => this.Interpreter.InputDriver;

        public void SetSlotInformation(int[] slotNumbers, string[] descriptions, int slotCount)
        {
            if (slotNumbers == null)
            {
                throw new ArgumentNullException(nameof(slotNumbers));
            }

            if (descriptions == null)
            {
                throw new ArgumentNullException(nameof(descriptions));
            }

            this.slotNumbers = (int[])slotNumbers.Clone();
            this.descriptions = (string[])descriptions.Clone();
            this.slotCount = slotCount;
        }

        public bool DoModal()
        {
            int textRow = DefaultWindowTop;
            int textHeight = textRow + this.slotCount;
            int textWidth = DefaultWindowWidth;
            int current = this.SelectedSlotIndex;

            this.WindowManager.PushTextColor();
            this.WindowManager.PushTextPosition();
            this.WindowManager.SetTextColor(ListBoxForegroundColor, ListBoxBackgroundColor);

            this.WindowManager.DisplayMessageBox(this.Title, textHeight, textWidth, true);

            textRow += (byte)this.WindowManager.MessageState.TextTopRow;

            for (int i = 0; i < this.slotCount; i++)
            {
                this.WindowManager.GotoPosition(new TextPosition((byte)(textRow + i), (byte)this.WindowManager.MessageState.TextLeftColumn));
                this.WindowManager.PrintFormatted(string.Format(CultureInfo.CurrentCulture, UserInterface.ListBoxItemFormat, this.descriptions[i]));
            }

            this.DrawSelector((byte)(current + textRow));

            bool result = this.Poll(current, textRow);

            this.WindowManager.PopTextPosition();
            this.WindowManager.PopTextColor();

            return result;
        }

        private bool Poll(int current, int textRow)
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
                                if (pos.Row >= textRow && pos.Row < (textRow + this.slotCount))
                                {
                                    if (pos.Column >= this.WindowManager.MessageState.TextLeftColumn && pos.Column < (this.WindowManager.MessageState.TextLeftColumn + DefaultWindowWidth))
                                    {
                                        int newCurrent = pos.Row - textRow;
                                        if (newCurrent >= 0 && newCurrent < this.slotCount)
                                        {
                                            this.DrawNoSelector((byte)(current + textRow));
                                            current = newCurrent;
                                            this.DrawSelector((byte)(current + textRow));
                                            return this.Accept(current);
                                        }
                                    }
                                }

                                break;
                        }

                        break;
                    case InputEventType.Ascii:
                        switch (e.Data)
                        {
                            case InputEventAscii.Enter:
                                return this.Accept(current);
                            case InputEventAscii.Esc:
                                return this.Cancel();
                        }

                        break;

                    case InputEventType.Direction:
                        this.DrawNoSelector((byte)(current + textRow));
                        switch (e.Data)
                        {
                            case InputEventDirection.Up:
                                if (current == 0)
                                {
                                    current = this.slotCount - 1;
                                }
                                else
                                {
                                    current = current - 1;
                                }

                                break;

                            case InputEventDirection.Down:
                                if (current == (this.slotCount - 1))
                                {
                                    current = 0;
                                }
                                else
                                {
                                    current++;
                                }

                                break;
                        }

                        this.DrawSelector((byte)(current + textRow));

                        break;
                }
            }
        }

        private void DrawSelector(byte row)
        {
            this.WindowManager.GotoPosition(new TextPosition(row, (byte)this.WindowManager.MessageState.TextLeftColumn));
            this.WindowManager.DisplayCharacter((char)0x1a);
            this.WindowManager.UpdateTextRegion();
        }

        private void DrawNoSelector(byte row)
        {
            this.WindowManager.GotoPosition(new TextPosition(row, (byte)this.WindowManager.MessageState.TextLeftColumn));
            this.WindowManager.DisplayCharacter(' ');
            this.WindowManager.UpdateTextRegion();
        }

        private bool Accept(int current)
        {
            this.SelectedSlotIndex = current;
            this.WindowManager.CloseWindow();
            return true;
        }

        private bool Cancel()
        {
            this.SelectedSlotIndex = -1;
            this.WindowManager.CloseWindow();
            return false;
        }
    }
}
