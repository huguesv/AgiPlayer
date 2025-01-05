// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter.Hints;

using System.Collections.Generic;

public record class HintBook
{
    public List<Topic> Topics { get; init; } = [];
}
