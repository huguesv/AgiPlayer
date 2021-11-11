// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.Controls;

public abstract class InputControl
{
    protected InputControl(AgiInterpreter interpreter)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
        this.Input = string.Empty;
        this.InputPrevious = string.Empty;
    }

    public string InputPrevious { get; set; }

    public bool InputEditEnabled { get; set; }

    public SoundManager SoundManager => this.Interpreter.SoundManager;

    public Preferences Preferences => this.Interpreter.Preferences;

    protected IInputDriver InputDriver => this.Interpreter.InputDriver;

    protected string Input { get; set; }

    protected AgiInterpreter Interpreter { get; }

    protected State State => this.Interpreter.State;

    protected ViewObjectTable ObjectTable => this.Interpreter.ObjectTable;

    protected WindowManager WindowManager => this.Interpreter.WindowManager;

    protected GraphicsRenderer GraphicsRenderer => this.Interpreter.GraphicsRenderer;

    protected ResourceManager ResourceManager => this.Interpreter.ResourceManager;

    public bool ProcessEvent(InputEvent e)
    {
        if (e is null)
        {
            throw new ArgumentNullException(nameof(e));
        }

        if (e.Type == InputEventType.Ascii)
        {
            this.ProcessKey(e);
            return true;
        }

        return false;
    }

    public abstract void EnableInput();

    public abstract void DisableInput();

    public abstract void RedrawInput();

    public abstract void RepeatPreviousInput();

    public abstract void CancelInput();

    public abstract string GetString(string message, int maxLength, byte row, byte column);

    public abstract string GetNumber(string message);

    protected abstract void ProcessKey(InputEvent e);

    protected void InputPollKeyPressed(int key)
    {
        this.State.Variables[Variables.KeyPressed] = (byte)key;
        if (this.State.InputEnabled)
        {
            this.AddInputCharacter((char)key);
        }
    }

    protected void AddInputCharacter(char c)
    {
        byte max;

        if (this.WindowManager.MessageState.DialogueOpen)
        {
            max = 0x24;
        }
        else
        {
            max = (byte)(0x28 - this.State.Strings[0].Length);
        }

        if (this.State.Cursor.Length > 0)
        {
            max--;
        }

        if (this.State.Variables[Variables.InputLength] < max)
        {
            max = this.State.Variables[Variables.InputLength];
        }

        this.EnableInput();

        switch (c)
        {
            case (char)InputEventAscii.Backspace:
                if (this.Input.Length > 0)
                {
                    this.Input = this.Input.Substring(0, this.Input.Length - 1);
                    this.WindowManager.DisplayCharacter(c);
                    this.WindowManager.UpdateTextRegion();
                }

                break;

            case (char)InputEventAscii.Newline:
                break;

            case (char)InputEventAscii.Enter:
                if (this.Input.Length > 0)
                {
                    this.InputPrevious = this.Input;
                    this.Interpreter.ParseText(this.Input);
                    this.Input = string.Empty;
                    this.RedrawInput();
                }

                break;

            default:
                if (max > this.Input.Length && c != 0)
                {
                    this.Input += c;
                    this.WindowManager.DisplayCharacter(c);
                    this.WindowManager.UpdateTextRegion();
                }

                break;
        }

        this.DisableInput();
    }
}
