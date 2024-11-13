// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

public class WindowManager
{
    public const int CharacterHeight = 8;
    public const int CharacterWidth = 4;

    private const byte TextInvertBit = 0x1;
    private const byte TextShadeBit = 0x2;
    private const int HeightMax = 20;
    private const int TextWrapMaxLength = 40;

    private readonly Stack<TextColor> textAttributeStack;
    private readonly Stack<TextPosition> textPositionStack;

    private TextPosition textPosition;
    private Font font;
    private RenderRectangle invalidated;
    private byte windowColumn;
    private int dispCharCur;
    private int dispWidthMax;
    private int dispLastWordIndex = -1;

    /*
     Interpreter.State variables used
     - TextForeground
     - TextBackground
     - TextCombine
     - Variables (for string formatting)
     - Strings (for string formatting)
     - WindowRowMin
    */
    public WindowManager(AgiInterpreter interpreter)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));

        this.MessageState = new MessageState();
        this.textAttributeStack = new Stack<TextColor>();
        this.textPositionStack = new Stack<TextPosition>();
        this.textPosition = new TextPosition(0, 0);
    }

    public bool TextShade { get; set; }

    public MessageState MessageState { get; }

    protected AgiInterpreter Interpreter { get; }

    public TextPosition GetPosition()
    {
        return new TextPosition(this.textPosition.Row, this.textPosition.Column);
    }

    public void GotoPosition(TextPosition position)
    {
        this.textPosition.Row = position.Row;
        this.textPosition.Column = position.Column;
    }

    public void InitializeFont()
    {
        this.font = this.Interpreter.GraphicsRenderer.GetFont();
    }

    public void ShutdownFont()
    {
        this.textPosition.Row = 0;
        this.textPosition.Column = 0;
    }

    public void InitializeText()
    {
        this.Interpreter.State.WindowRowMin = 2;
        this.Interpreter.State.WindowRowMax = 23;
        this.Interpreter.State.StatusLineRow = 0;

        this.Initialize();
    }

    public void ShutdownText()
    {
        this.Interpreter.State.WindowRowMin = 0;
        this.Interpreter.State.WindowRowMax = 0;
        this.Interpreter.State.StatusLineRow = State.DefaultStatusLineRow;
    }

    public void CloseWindow()
    {
        if (this.MessageState.Active)
        {
            this.Interpreter.GraphicsRenderer.RenderPictureBuffer(new PictureRectangle(this.MessageState.BackgroundLeftColumn, this.MessageState.BackgroundLowRow, this.MessageState.BackgroundWidth, this.MessageState.BackgroundHeight), false, false);
        }

        this.MessageState.DialogueOpen = false;
        this.MessageState.Active = false;
    }

    public void PopTextColor()
    {
        if (this.textAttributeStack.Count > 0)
        {
            var attr = this.textAttributeStack.Pop();

            this.Interpreter.State.TextForeground = attr.Foreground;
            this.Interpreter.State.TextBackground = attr.Background;
            this.Interpreter.State.TextCombine = attr.Combine;
        }
    }

    public void PushTextColor()
    {
        this.textAttributeStack.Push(new TextColor(this.Interpreter.State.TextForeground, this.Interpreter.State.TextBackground, this.Interpreter.State.TextCombine));
    }

    public void PushTextPosition()
    {
        var pos = this.GetPosition();
        this.textPositionStack.Push(pos);
    }

    public void PopTextPosition()
    {
        if (this.textPositionStack.Count > 0)
        {
            this.GotoPosition(this.textPositionStack.Pop());
        }
    }

    public void ClearWindow(TextPosition upperLeft, TextPosition lowerRight, byte attr)
    {
        var oldPosition = this.GetPosition();
        this.EraseText(upperLeft, lowerRight, attr);
        this.GotoPosition(oldPosition);
    }

    public void ClearLine(byte row, byte attr)
    {
        this.ClearWindowPortion(row, row, attr);
    }

    public void ClearWindowPortion(byte upper, byte lower, byte attr)
    {
        this.ClearWindow(new TextPosition(upper, 0), new TextPosition(lower, 39), attr);
    }

    public bool MessageBox(string text)
    {
        bool accepted = true;

        this.DisplayMessageBox(text, 0, 0, false);

        if (this.Interpreter.State.Flags[Flags.PrintMode])
        {
            this.Interpreter.State.Flags[Flags.PrintMode] = false;
        }
        else
        {
            accepted = this.Interpreter.InputDriver.PollAcceptOrCancel(this.Interpreter.State.Variables[Variables.WindowTimer]);
            this.Interpreter.State.Variables[Variables.WindowTimer] = 0;
            this.CloseWindow();
        }

        return accepted;
    }

    public void DisplayMessageBox(string text, int row, int width, bool toggle)
    {
        ArgumentNullException.ThrowIfNull(text);

        if (this.MessageState.Active)
        {
            this.CloseWindow();
        }

        this.PushTextColor();
        this.PushTextPosition();
        this.SetTextColor(0, 0x0f);

        if (this.MessageState.WantedWidth == 0xffff && width == 0)
        {
            width = 30;
        }
        else if (this.MessageState.WantedWidth != 0xffff)
        {
            width = this.MessageState.WantedWidth;
        }

        string message;

        while (true)
        {
            message = this.WrapText(text, width);
            this.MessageState.PrintedHeight = this.MessageState.TextHeight;

            if (toggle)
            {
                this.MessageState.TextWidth = width;
                if (row != 0)
                {
                    this.MessageState.TextHeight = row;
                }
            }

            if (this.MessageState.TextHeight < HeightMax)
            {
                break;
            }

            text = string.Format(CultureInfo.CurrentCulture, UserInterface.MessageTooVerboseFormat, text[..20]);
        }

        int deltaY;
        if (this.MessageState.WantedRow == 0xffff)
        {
            deltaY = ((HeightMax - this.MessageState.TextHeight - 1) / 2) + 1;
        }
        else
        {
            deltaY = this.MessageState.WantedRow;
        }

        this.MessageState.TextTopRow = deltaY + this.Interpreter.State.WindowRowMin;
        this.MessageState.TextLowRow = this.MessageState.TextHeight + this.MessageState.TextTopRow - 1;

        if (this.MessageState.WantedColumn == 0xffff)
        {
            this.MessageState.TextLeftColumn = (40 - this.MessageState.TextWidth) / 2;
        }
        else
        {
            this.MessageState.TextLeftColumn = this.MessageState.WantedColumn;
        }

        this.windowColumn = (byte)this.MessageState.TextLeftColumn;

        this.MessageState.TextRightColumn = this.MessageState.TextLeftColumn + this.MessageState.TextWidth;

        this.GotoPosition(new TextPosition((byte)this.MessageState.TextTopRow, (byte)this.MessageState.TextLeftColumn));

        this.MessageState.BackgroundWidth = (this.MessageState.TextWidth * CharacterWidth) + 10;
        this.MessageState.BackgroundHeight = (this.MessageState.TextHeight * CharacterHeight) + 10;
        this.MessageState.BackgroundLowRow = ((this.MessageState.TextLowRow - this.Interpreter.State.WindowRowMin + 1) * CharacterHeight) + 4;
        this.MessageState.BackgroundLeftColumn = (this.MessageState.TextLeftColumn * CharacterWidth) - 5;

        this.DisplayMessageBoxWindow(new PictureRectangle(this.MessageState.BackgroundLeftColumn, this.MessageState.BackgroundLowRow, this.MessageState.BackgroundWidth, this.MessageState.BackgroundHeight), this.Interpreter.GraphicsRenderer.MessageBoxBackground, this.Interpreter.GraphicsRenderer.MessageBoxBorder);

        this.MessageState.Active = true;
        this.PrintFormatted(message);
        this.windowColumn = 0;

        this.PopTextPosition();
        this.PopTextColor();

        this.MessageState.DialogueOpen = true;
    }

    public void PrintFormatted(string text, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(text);

        int currentArgument = 0;
        int currentCharacter = 0;
        while (currentCharacter < text.Length)
        {
            char c = text[currentCharacter];

            if (c != '%')
            {
                this.DisplayCharacter(c);
            }
            else
            {
                currentCharacter++;
                c = text[currentCharacter];
                switch (c)
                {
                    case 's':
                        {
                            // string
                            string arg = (string)args[currentArgument];
                            currentArgument++;
                            this.DisplayString(arg);
                            break;
                        }

                    case 'd':
                        {
                            // decimal
                            short arg = short.Parse(args[currentArgument].ToString(), NumberStyles.Integer, CultureInfo.CurrentCulture);
                            currentArgument++;
                            if (arg < 0)
                            {
                                this.DisplayCharacter('-');
                                this.DisplayString(StringUtility.NumberToString(arg * -1));
                            }
                            else
                            {
                                this.DisplayString(StringUtility.NumberToString(arg));
                            }

                            break;
                        }

                    case 'u':
                        {
                            // unsigned decimal
                            ushort arg = ushort.Parse(args[currentArgument].ToString(), NumberStyles.Integer, CultureInfo.CurrentCulture);
                            currentArgument++;
                            this.DisplayString(StringUtility.NumberToString(arg));
                            break;
                        }

                    case 'x':
                        {
                            // hex number
                            ushort arg = ushort.Parse(args[currentArgument].ToString(), NumberStyles.Integer, CultureInfo.CurrentCulture);
                            currentArgument++;
                            this.DisplayString(StringUtility.NumberToHexString(arg));
                            break;
                        }

                    case 'c':
                        {
                            // character
                            char arg = '\0';
                            string s = args[currentArgument].ToString();
                            if (s.Length > 0)
                            {
                                arg = s[0];
                            }

                            currentArgument++;
                            this.DisplayCharacter(arg);
                            break;
                        }

                    default:
                        {
                            // not recognised
                            this.DisplayCharacter('%');
                            currentCharacter--;
                            break;
                        }
                }
            }

            currentCharacter++;
        }

        this.UpdateTextRegion();
    }

    public void DisplayMessageBoxWindow(PictureRectangle rect, byte backgroundColor, byte lineColor)
    {
        // Draws the familiar white/red boxes in the picture buffer
        this.Interpreter.GraphicsRenderer.RenderSolidRectangle(new PictureRectangle(rect.X, rect.Y, rect.Width, rect.Height), backgroundColor);
        this.Interpreter.GraphicsRenderer.RenderSolidRectangle(new PictureRectangle(rect.X + 1, rect.Y - 1, rect.Width - 2, 1), lineColor);
        this.Interpreter.GraphicsRenderer.RenderSolidRectangle(new PictureRectangle(rect.X + rect.Width - 2, rect.Y - 2, 1, rect.Height - 4), lineColor);
        this.Interpreter.GraphicsRenderer.RenderSolidRectangle(new PictureRectangle(rect.X + 1, rect.Y - rect.Height + 2, rect.Width - 2, 1), lineColor);
        this.Interpreter.GraphicsRenderer.RenderSolidRectangle(new PictureRectangle(rect.X + 1, rect.Y - 2, 1, rect.Height - 4), lineColor);
    }

    public void DisplayAt(string text, TextPosition pos)
    {
        ArgumentNullException.ThrowIfNull(text);

        this.PushTextPosition();
        this.GotoPosition(pos);

        text = this.WrapText(text, TextWrapMaxLength);
        this.PrintFormatted(text);

        this.PopTextPosition();
    }

    public void DisplayCharacter(char c)
    {
        var pos = this.GetPosition();

        if (c == (char)0x08)
        {
            // backspace
            if (pos.Column != 0)
            {
                pos.Column--;
            }
            else if (pos.Row > 21)
            {
                pos.Column = 39;
                pos.Row--;
            }

            this.EraseText(pos, pos, this.Interpreter.State.TextBackground);
            this.GotoPosition(pos);
        }
        else if (c == (char)0x0d || c == (char)0x0a)
        {
            // return/linefeed
            if (pos.Row < 24)
            {
                pos.Row++;
            }

            pos.Column = this.windowColumn;
            this.GotoPosition(pos);
        }
        else
        {
            byte conversion = 0;

            if (!this.Interpreter.GraphicsRenderer.TextMode)
            {
                if ((this.Interpreter.State.TextCombine & 0x80) != 0)
                {
                    conversion = (byte)(conversion | TextInvertBit);
                }

                if (this.TextShade)
                {
                    conversion = (byte)(conversion | TextShadeBit);
                }
            }

            this.DrawCharacter(c, this.Interpreter.State.TextCombine, conversion);

            pos.Column++;
            if (pos.Column <= 39)
            {
                this.GotoPosition(pos);
            }
            else
            {
                // Add carriage return
                this.DisplayCharacter((char)0xd);
            }
        }
    }

    public void UpdateTextRegion()
    {
        this.Interpreter.GraphicsDriver.Update(this.invalidated);
        this.invalidated = default;
    }

    public string WrapText(string text, int count)
    {
        ArgumentNullException.ThrowIfNull(text);

        this.dispCharCur = 0;
        this.dispWidthMax = count;
        this.dispLastWordIndex = -1;

        this.MessageState.TextWidth = 0;
        this.MessageState.TextHeight = 0;

        StringBuilder message = new StringBuilder();
        this.Display(text, message);
        this.NewLine();

        return message.ToString();
    }

    public void SetTextColor(byte foreground, byte background)
    {
        this.Interpreter.State.TextForeground = foreground;
        this.Interpreter.State.TextBackground = this.Interpreter.GraphicsRenderer.CalculateTextBackground(background);
        this.Interpreter.State.TextCombine = this.Interpreter.GraphicsRenderer.CombineTextColors(foreground, background);
    }

    public void PrintAt(string text, TextPosition pos, byte width)
    {
        ArgumentNullException.ThrowIfNull(text);

        this.MessageState.WantedRow = pos.Row;
        this.MessageState.WantedColumn = pos.Column;
        this.MessageState.WantedWidth = width;

        if (this.MessageState.WantedWidth == 0)
        {
            this.MessageState.WantedWidth = 30;
        }

        this.MessageBox(text);

        this.MessageState.WantedColumn = 0xffff;
        this.MessageState.WantedRow = 0xffff;
        this.MessageState.WantedWidth = 0xffff;
    }

    public void ScrollWindow(TextPosition upperLeft, TextPosition lowerRight, byte attrib)
    {
        this.ScrollText(upperLeft, lowerRight, 1, attrib);
        upperLeft.Row = lowerRight.Row;
        this.GotoPosition(upperLeft);
    }

    private void Initialize()
    {
        this.PushTextPosition();
        this.PushTextColor();
        this.ClearLine(0, 0xff);
        this.SetTextColor(0, 0x0f);
        this.GotoPosition(new TextPosition(0, (byte)((40 - UserInterface.PlayerName.Length) / 2)));
        this.PrintFormatted(UserInterface.PlayerName);
        this.PopTextColor();
        this.PopTextPosition();
    }

    private void DisplayString(string text)
    {
        foreach (char c in text)
        {
            this.DisplayCharacter(c);
        }
    }

    private void EraseText(TextPosition pos1, TextPosition pos2, byte attrib)
    {
        int x = pos1.Column * this.Interpreter.GraphicsRenderer.RenderFontWidth;
        int y = pos1.Row * this.Interpreter.GraphicsRenderer.RenderFontHeight;
        int width = (pos2.Column - pos1.Column + 1) * this.Interpreter.GraphicsRenderer.RenderFontWidth;
        int height = (pos2.Row - pos1.Row + 1) * this.Interpreter.GraphicsRenderer.RenderFontHeight;

        attrib = this.Interpreter.GraphicsRenderer.ConvertTextBackground(attrib);

        this.Interpreter.GraphicsDriver.Fill(new RenderRectangle(x, y, width, height), attrib);
        this.InvalidateTextRegion(new RenderRectangle(x, y, width, height));
    }

    private void DrawCharacter(char ch, byte color, byte flags)
    {
        byte[] pixels = this.font.GetPixels(ch, color, (flags & TextInvertBit) != 0, (flags & TextShadeBit) != 0, this.Interpreter.GraphicsRenderer.TextMode);
        if (pixels is not null)
        {
            this.Interpreter.GraphicsDriver.RenderCharacter(new RenderPoint(this.textPosition.Column * this.Interpreter.GraphicsRenderer.RenderFontWidth, this.textPosition.Row * this.Interpreter.GraphicsRenderer.RenderFontHeight), new RenderSize(this.Interpreter.GraphicsRenderer.RenderFontWidth, this.Interpreter.GraphicsRenderer.RenderFontHeight), flags, pixels, this.font.Width, this.font.Height);
            this.InvalidateTextRegion(new RenderRectangle(this.textPosition.Column * this.Interpreter.GraphicsRenderer.RenderFontWidth, this.textPosition.Row * this.Interpreter.GraphicsRenderer.RenderFontHeight, this.Interpreter.GraphicsRenderer.RenderFontWidth, this.Interpreter.GraphicsRenderer.RenderFontHeight));
        }
    }

    private void InvalidateTextRegion(RenderRectangle rect)
    {
        if (rect.Width != 0 || rect.Height != 0)
        {
            int x1;
            int y1;
            int x2;
            int y2;

            // upper y
            if (rect.Y < this.invalidated.Y)
            {
                y1 = rect.Y;
            }
            else
            {
                y1 = this.invalidated.Y;
            }

            // lower y
            if ((rect.Y + rect.Height) > (this.invalidated.Y + this.invalidated.Height))
            {
                y2 = rect.Y + rect.Height;
            }
            else
            {
                y2 = this.invalidated.Y + this.invalidated.Height;
            }

            // left x
            if (rect.X < this.invalidated.X)
            {
                x1 = rect.X;
            }
            else
            {
                x1 = this.invalidated.X;
            }

            // right x
            if ((rect.X + rect.Width) > (this.invalidated.X + this.invalidated.Width))
            {
                x2 = rect.X + rect.Width;
            }
            else
            {
                x2 = this.invalidated.X + this.invalidated.Width;
            }

            this.invalidated.X = x1;
            this.invalidated.Y = y1;
            this.invalidated.Width = x2 - x2;
            this.invalidated.Height = y2 - y1;
        }
        else
        {
            this.invalidated.X = rect.X;
            this.invalidated.Y = rect.Y;
            this.invalidated.Width = rect.Width;
            this.invalidated.Height = rect.Height;
        }
    }

    private void NewLine()
    {
        this.MessageState.TextHeight++;

        if (this.dispCharCur > this.MessageState.TextWidth)
        {
            this.MessageState.TextWidth = this.dispCharCur;
        }

        this.dispCharCur = 0;
    }

    private void Display(string text, StringBuilder message)
    {
        int textIndex = 0;

        while (textIndex < text.Length && this.MessageState.TextHeight <= (HeightMax - 1))
        {
            while (this.dispCharCur < this.dispWidthMax)
            {
                if (textIndex >= text.Length)
                {
                    return;
                }

                if (text[textIndex] == this.MessageState.NewlineChar)
                {
                    textIndex++;
                    message.Append(text[textIndex]);
                    textIndex++;
                    this.dispCharCur++;
                }
                else
                {
                    switch (text[textIndex])
                    {
                        case (char)0x0a:
                            // linefeed
                            message.Append(text[textIndex]);
                            textIndex++;
                            this.NewLine();
                            break;

                        case (char)0x20:
                            // space
                            this.dispLastWordIndex = message.Length;
                            message.Append(text[textIndex]);
                            textIndex++;
                            this.dispCharCur++;
                            break;

                        case (char)0x25:
                            // % control
                            textIndex++;
                            string embedded;
                            int num;

                            switch (text[textIndex++])
                            {
                                case 'g':
                                    num = StringUtility.ParseNumber(text, ref textIndex);
                                    embedded = this.Interpreter.ResourceManager.LogicResources[0].GetMessage(num);
                                    if (embedded is not null)
                                    {
                                        this.Display(embedded, message);
                                    }

                                    break;
                                case 'm':
                                    num = StringUtility.ParseNumber(text, ref textIndex);
                                    embedded = this.Interpreter.LogicInterpreter.CurrentLogic.GetMessage(num);
                                    if (embedded is not null)
                                    {
                                        this.Display(embedded, message);
                                    }

                                    break;
                                case 'o':
                                    num = StringUtility.ParseNumber(text, ref textIndex);
                                    embedded = this.Interpreter.ResourceManager.InventoryResource.Items[this.Interpreter.State.Variables[num]].Name;
                                    this.Display(embedded, message);
                                    break;
                                case 's':
                                    num = StringUtility.ParseNumber(text, ref textIndex);
                                    embedded = this.Interpreter.State.Strings[num];
                                    this.Display(embedded, message);
                                    break;
                                case 'v':
                                    num = StringUtility.ParseNumber(text, ref textIndex);
                                    embedded = StringUtility.NumberToString(this.Interpreter.State.Variables[num]);
                                    if (textIndex < text.Length && text[textIndex] == '|')
                                    {
                                        textIndex++;
                                        int paddingSize = StringUtility.ParseNumber(text, ref textIndex);
                                        embedded = StringUtility.PadWithZeros(embedded, paddingSize);
                                    }

                                    this.Display(embedded, message);
                                    break;
                                case 'w':
                                    num = StringUtility.ParseNumber(text, ref textIndex);
                                    num--;
                                    embedded = this.Interpreter.ParserResults[num].Word;
                                    this.Display(embedded, message);
                                    break;
                                default:
                                    break;
                            }

                            break;

                        default:
                            // normal character
                            message.Append(text[textIndex]);
                            textIndex++;
                            this.dispCharCur++;
                            break;
                    }
                }
            }

            if (this.dispLastWordIndex == -1)
            {
                message.Append((char)0x0a);
                this.NewLine();
            }
            else
            {
                this.dispCharCur -= message.Length - this.dispLastWordIndex;
                this.NewLine();
                message[this.dispLastWordIndex] = (char)0x0a;

                this.dispCharCur = message.Length - this.dispLastWordIndex - 1;
                this.dispLastWordIndex = -1;
            }
        }
    }

    private void ScrollText(TextPosition pos1, TextPosition pos2, int scroll, byte attrib)
    {
        if (scroll == 0)
        {
            return;
        }

        int copyHeight = (pos2.Row - pos1.Row - scroll + 1) * this.Interpreter.GraphicsRenderer.RenderFontHeight;
        if (copyHeight <= 0)
        {
            // Clear the entire window
            this.EraseText(pos1, pos2, attrib);
        }
        else
        {
            int copyWidth = (pos2.Column - pos1.Column + 1) * this.Interpreter.GraphicsRenderer.RenderFontWidth;

            int x = pos1.Column * this.Interpreter.GraphicsRenderer.RenderFontWidth;
            int y = scroll > 0
                ? pos1.Row * this.Interpreter.GraphicsRenderer.RenderFontHeight
                : (pos1.Row + scroll) * this.Interpreter.GraphicsRenderer.RenderFontHeight;

            this.Interpreter.GraphicsDriver.Scroll(new RenderRectangle(x, y, copyWidth, copyHeight), scroll * this.Interpreter.GraphicsRenderer.RenderFontHeight);

            this.InvalidateTextRegion(new RenderRectangle(x, y, copyWidth, copyHeight));

            if (scroll > 0)
            {
                // Scrolling up
                // Clear bottom bit of screen
                var tempPos = new TextPosition((byte)(pos2.Row - scroll + 1), pos1.Column);
                this.EraseText(tempPos, pos2, attrib);
            }
            else
            {
                // Scrolling down
                // Clear top bit of area
                var tempPos = new TextPosition((byte)(pos1.Row + scroll - 1), pos2.Column);
                this.EraseText(pos1, tempPos, attrib);
            }
        }
    }
}
