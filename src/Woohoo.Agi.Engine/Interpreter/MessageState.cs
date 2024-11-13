// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

public class MessageState
{
    public MessageState()
    {
        this.WantedWidth = 0xffff;
        this.WantedRow = 0xffff;
        this.WantedColumn = 0xffff;
        this.NewlineChar = '\\';
    }

    public int WantedWidth { get; set; }

    public int WantedRow { get; set; }

    public int WantedColumn { get; set; }

    public bool DialogueOpen { get; set; }

    public char NewlineChar { get; set; }

    public bool Active { get; set; }

    public int TextTopRow { get; set; }

    public int TextLeftColumn { get; set; }

    public int TextLowRow { get; set; }

    public int TextRightColumn { get; set; }

    public int TextWidth { get; set; }

    public int TextHeight { get; set; }

    public int PrintedHeight { get; set; }

    public int BackgroundLowRow { get; set; }

    public int BackgroundLeftColumn { get; set; }

    public int BackgroundWidth { get; set; }

    public int BackgroundHeight { get; set; }
}
