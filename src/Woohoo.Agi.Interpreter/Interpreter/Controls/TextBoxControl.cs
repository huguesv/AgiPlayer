// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.Controls;

public class TextBoxControl
{
    public TextBoxControl(AgiInterpreter interpreter)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
        this.Text = string.Empty;
        this.MaxTextLength = 10;
    }

    public string Text { get; set; }

    public int MaxTextLength { get; set; }

    protected AgiInterpreter Interpreter { get; }

    protected WindowManager WindowManager => this.Interpreter.WindowManager;

    protected IInputDriver InputDriver => this.Interpreter.InputDriver;

    public bool DoModal()
    {
        var editText = new StringBuilder(this.Text);
        var pos = this.WindowManager.GetPosition();
        var displayStartOffset = this.Display(editText.ToString(), pos, this.MaxTextLength, 75);

        while (true)
        {
            this.Interpreter.GameControl.InputControl.DisableInput();
            this.WindowManager.UpdateTextRegion();
            int c = this.InputDriver.WaitCharacter();
            this.Interpreter.GameControl.InputControl.EnableInput();

            switch (c)
            {
                case 0x3: // ctrl-x2
                case 0x18: // ctrl-x??
                    editText = new StringBuilder();
                    displayStartOffset = this.Display(editText.ToString(), pos, this.MaxTextLength, 0);
                    break;

                case InputEventAscii.Backspace:
                    if (editText.Length > 0)
                    {
                        editText.Remove(editText.Length - 1, 1);
                    }

                    if (editText.Length > displayStartOffset)
                    {
                        this.WindowManager.DisplayCharacter((char)c);
                    }
                    else
                    {
                        displayStartOffset = this.Display(editText.ToString(), pos, this.MaxTextLength, 90);
                    }

                    break;

                case InputEventAscii.Enter:
                    this.WindowManager.UpdateTextRegion();
                    this.Text = editText.ToString();
                    return true;

                case InputEventAscii.Esc:
                    this.WindowManager.UpdateTextRegion();
                    this.Text = string.Empty;
                    return false;

                default:
                    editText.Append((char)c);

                    if ((editText.Length - displayStartOffset) < this.MaxTextLength)
                    {
                        this.WindowManager.DisplayCharacter((char)c);
                    }
                    else
                    {
                        displayStartOffset = this.Display(editText.ToString(), pos, this.MaxTextLength, 20);
                    }

                    break;
            }
        }
    }

    private int Display(string editText, TextPosition pos, int displaySize, int percent)
    {
        int displayStartOffset = 0;

        if (editText.Length >= displaySize)
        {
            displayStartOffset = editText.Length - (percent * displaySize / 100);
        }

        this.WindowManager.GotoPosition(new TextPosition(pos.Row, pos.Column));

        if (displayStartOffset == 0)
        {
            this.Display2(editText, pos, displaySize, 0);
        }
        else
        {
            this.WindowManager.DisplayCharacter((char)0x1b);
            this.Display2(editText[displayStartOffset..], pos, displaySize, 1);
        }

        return displayStartOffset;
    }

    private void Display2(string displayText, TextPosition pos, int displaySize, int offset)
    {
        this.WindowManager.ClearWindow(new TextPosition(pos.Row, (byte)(pos.Column + offset)), new TextPosition(pos.Row, (byte)(pos.Column + displaySize - 1)), this.Interpreter.State.TextBackground);

        int start = 0;
        while (start < displayText.Length && offset != 0)
        {
            start++;
            offset--;
        }

        while (start < displayText.Length)
        {
            char c = displayText[start++];
            this.WindowManager.DisplayCharacter(c);
        }
    }
}
