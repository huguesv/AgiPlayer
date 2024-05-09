// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.Controls;

public class ClassicInputControl : InputControl
{
    public ClassicInputControl(AgiInterpreter interpreter)
        : base(interpreter)
    {
    }

    public override void EnableInput()
    {
        if (!this.InputEditEnabled)
        {
            this.InputEditEnabled = true;

            if (this.State.Cursor.Length > 0)
            {
                this.WindowManager.DisplayCharacter((char)0x08);
                this.WindowManager.UpdateTextRegion();
            }
        }
    }

    public override void DisableInput()
    {
        if (this.InputEditEnabled)
        {
            this.InputEditEnabled = false;

            if (this.State.Cursor.Length > 0)
            {
                this.WindowManager.DisplayCharacter(this.State.Cursor[0]);
                this.WindowManager.UpdateTextRegion();
            }
        }
    }

    public override void RedrawInput()
    {
        if (this.State.InputEnabled)
        {
            this.EnableInput();
            this.WindowManager.ClearLine(this.State.InputPosition, this.State.TextBackground);
            this.WindowManager.GotoPosition(new TextPosition(this.State.InputPosition, 0));

            string wrapped = this.WindowManager.WrapText(this.State.Strings[0], 0x28);
            this.WindowManager.PrintFormatted(wrapped);

            this.WindowManager.PrintFormatted(this.Input);

            this.DisableInput();
            this.WindowManager.UpdateTextRegion();
        }
    }

    public override void RepeatPreviousInput()
    {
        if (this.Input.Length < this.InputPrevious.Length)
        {
            this.EnableInput();
            this.Input += this.InputPrevious[this.Input.Length];
            this.WindowManager.DisplayCharacter(this.Input[^1]);

            while (this.Input.Length < this.InputPrevious.Length)
            {
                this.Input += this.InputPrevious[this.Input.Length];
                this.WindowManager.DisplayCharacter(this.Input[^1]);
            }

            this.WindowManager.UpdateTextRegion();
            this.DisableInput();
        }
    }

    public override void CancelInput()
    {
        // Delete all characters from input line
        // by adding a backspace until empty
        while (this.Input.Length > 0)
        {
            this.AddInputCharacter((char)0x08);
        }
    }

    public override string GetString(string message, int maxLength, byte row, byte column)
    {
        string result = string.Empty;

        bool inputEditDisabled = !this.InputEditEnabled;
        this.WindowManager.PushTextPosition();
        this.EnableInput();

        if (row < 0x19)
        {
            this.WindowManager.GotoPosition(new TextPosition(row, column));
        }

        string wrapped = this.WindowManager.WrapText(message, 0x40);
        this.WindowManager.PrintFormatted(wrapped);

        var textBox = new TextBoxControl(this.Interpreter)
        {
            Text = string.Empty,
            MaxTextLength = maxLength,
        };

        if (textBox.DoModal())
        {
            result = textBox.Text;
        }

        this.WindowManager.PopTextPosition();
        if (inputEditDisabled)
        {
            this.DisableInput();
        }

        return result;
    }

    public override string GetNumber(string message)
    {
        string result = string.Empty;

        this.EnableInput();
        this.WindowManager.GotoPosition(new TextPosition(this.State.InputPosition, 0));

        string wrapped = this.WindowManager.WrapText(message, 0x28);
        this.WindowManager.PrintFormatted(wrapped);

        this.DisableInput();

        var textBox = new TextBoxControl(this.Interpreter)
        {
            Text = string.Empty,
            MaxTextLength = 4,
        };

        if (textBox.DoModal())
        {
            result = textBox.Text;
        }

        this.RedrawInput();

        return result;
    }

    protected override void ProcessKey(InputEvent e)
    {
        this.InputPollKeyPressed(e.Data);
    }
}
