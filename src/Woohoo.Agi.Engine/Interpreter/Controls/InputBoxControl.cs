// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable disable

namespace Woohoo.Agi.Engine.Interpreter.Controls;

public class InputBoxControl
{
    private const int DefaultWindowWidth = 0x22;
    private const int DefaultMaxTextLength = 0x22;

    private const int WindowForegroundColor = 0x00;
    private const int WindowBackgroundColor = 0x0f;

    private const int TextBoxForegroundColor = 0x0f;
    private const int TextBoxBackgroundColor = 0x00;

    public InputBoxControl(AgiInterpreter interpreter)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
        this.Title = string.Empty;
        this.Text = string.Empty;
        this.MaxTextLength = DefaultMaxTextLength;
        this.Width = DefaultWindowWidth;
    }

    public string Title { get; set; }

    public string Text { get; set; }

    public int MaxTextLength { get; set; }

    public int Width { get; set; }

    protected AgiInterpreter Interpreter { get; }

    protected WindowManager WindowManager => this.Interpreter.WindowManager;

    protected IInputDriver InputDriver => this.Interpreter.InputDriver;

    public bool DoModal()
    {
        this.WindowManager.PushTextColor();
        this.WindowManager.PushTextPosition();

        this.WindowManager.SetTextColor(WindowForegroundColor, WindowBackgroundColor);
        this.WindowManager.DisplayMessageBox(this.Title, 0, this.Width, toggle: true, isHint: false);
        this.WindowManager.GotoPosition(new TextPosition((byte)this.WindowManager.MessageState.TextLowRow, (byte)this.WindowManager.MessageState.TextLeftColumn));
        this.WindowManager.ClearWindow(new TextPosition((byte)this.WindowManager.MessageState.TextLowRow, (byte)this.WindowManager.MessageState.TextLeftColumn), new TextPosition((byte)this.WindowManager.MessageState.TextLowRow, (byte)(this.WindowManager.MessageState.TextRightColumn - 1)), 0);
        this.WindowManager.SetTextColor(TextBoxForegroundColor, TextBoxBackgroundColor);

        var textBox = new TextBoxControl(this.Interpreter)
        {
            Text = this.Text,
            MaxTextLength = this.MaxTextLength,
        };

        bool result = textBox.DoModal();
        this.Text = result ? textBox.Text : string.Empty;

        this.WindowManager.CloseWindow();

        this.WindowManager.PopTextPosition();
        this.WindowManager.PopTextColor();

        return result;
    }
}
