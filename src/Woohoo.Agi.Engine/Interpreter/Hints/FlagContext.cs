// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter.Hints;

public record class FlagContext
{
    public byte Number { get; set; }

    public bool Value { get; set; }
}
