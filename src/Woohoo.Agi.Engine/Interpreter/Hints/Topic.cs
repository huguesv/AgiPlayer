// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter.Hints;

using System.Collections.Generic;

public record class Topic
{
    public string Title { get; set; } = string.Empty;

    public List<byte> Rooms { get; init; } = [];

    public List<FlagContext> Flags { get; init; } = [];

    public List<ItemContext> Items { get; init; } = [];

    public List<string> Messages { get; init; } = [];
}
