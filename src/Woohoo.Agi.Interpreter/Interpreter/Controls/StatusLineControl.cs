// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.Controls;

public class StatusLineControl
{
    private const int TextForegroundColor = 0x00;
    private const int TextBackgroundColor = 0x0f;

    public StatusLineControl(AgiInterpreter interpreter)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
    }

    public GameStartInfo GameInfo => this.Interpreter.GameInfo;

    protected AgiInterpreter Interpreter { get; }

    protected State State => this.Interpreter.State;

    protected WindowManager WindowManager => this.Interpreter.WindowManager;

    public void Display()
    {
        this.WindowManager.PushTextPosition();
        this.WindowManager.PushTextColor();

        if (this.State.StatusVisible)
        {
            byte soundColumn;
            string soundTextFormat;
            string scoreTextFormat;
            if (this.GameInfo.Interpreter > InterpreterVersion.V2089)
            {
                soundColumn = 0x1e;
                soundTextFormat = UserInterface.SoundStatusNew;
                scoreTextFormat = UserInterface.ScoreStatusNew;
            }
            else
            {
                soundColumn = 0x1b;
                soundTextFormat = UserInterface.SoundStatusOld;
                scoreTextFormat = UserInterface.ScoreStatusOld;
            }

            this.WindowManager.ClearLine(this.State.StatusLineRow, 0xff);
            this.WindowManager.SetTextColor(TextForegroundColor, TextBackgroundColor);
            this.WindowManager.GotoPosition(new TextPosition(this.State.StatusLineRow, 1));
            this.WindowManager.PrintFormatted(scoreTextFormat, this.State.Variables[Variables.Score], this.State.Variables[Variables.MaxScore]);
            this.WindowManager.GotoPosition(new TextPosition(this.State.StatusLineRow, soundColumn));

            if (this.State.Flags[Flags.SoundOn])
            {
                this.WindowManager.PrintFormatted(soundTextFormat, UserInterface.SoundOn);
            }
            else
            {
                this.WindowManager.PrintFormatted(soundTextFormat, UserInterface.SoundOff);
            }
        }

        this.WindowManager.PopTextColor();
        this.WindowManager.PopTextPosition();
        this.WindowManager.UpdateTextRegion();
    }

    public bool ProcessEvent(InputEvent e)
    {
        ArgumentNullException.ThrowIfNull(e);

        if (this.State.StatusVisible)
        {
            // TODO: handle mouse event on the status line
        }

        return false;
    }
}
