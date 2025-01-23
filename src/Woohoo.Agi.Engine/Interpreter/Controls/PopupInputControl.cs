// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable disable

namespace Woohoo.Agi.Engine.Interpreter.Controls;

public class PopupInputControl : InputControl
{
    public PopupInputControl(AgiInterpreter interpreter)
        : base(interpreter)
    {
    }

    public override void EnableInput()
    {
        if (!this.InputEditEnabled)
        {
            this.InputEditEnabled = true;
        }
    }

    public override void DisableInput()
    {
        if (this.InputEditEnabled)
        {
            this.InputEditEnabled = false;
        }
    }

    public override void RedrawInput()
    {
    }

    public override void RepeatPreviousInput()
    {
        if (this.Input.Length < this.InputPrevious.Length)
        {
            this.InputBoxPollStart(this.InputPrevious);
        }
    }

    public override void CancelInput()
    {
    }

    protected override void ProcessKey(InputEvent e)
    {
        this.InputBoxPollKeypressed(e.Data);
    }

    private void InputBoxPollStart(string text)
    {
        bool inputEditDisabled = !this.InputEditEnabled;
        this.EnableInput();
        this.SoundManager.StopPlaying();

        var inputBox = new InputBoxControl(this.Interpreter)
        {
            Title = UserInterface.InputBox,
            Text = text,
        };

        bool result = inputBox.DoModal();

        if (inputEditDisabled)
        {
            this.DisableInput();
        }

        if (result)
        {
            this.Input = inputBox.Text;
            this.InputPrevious = this.Input;
            this.Interpreter.ParseText(this.Input);
            this.Input = string.Empty;
            this.RedrawInput();
        }
    }

    private void InputBoxPollKeypressed(int key)
    {
        this.State.Variables[Variables.KeyPressed] = (byte)key;
        if (this.State.InputEnabled)
        {
            if (key != 8 && key != 13 && key != 0)
            {
                char c = (char)key;

                this.InputBoxPollStart(c.ToString());
            }
        }
    }
}
