// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

public class InputEvent
{
    public int Type { get; set; }

    public int Data { get; set; }

    public int X { get; set; }

    public int Y { get; set; }
}
