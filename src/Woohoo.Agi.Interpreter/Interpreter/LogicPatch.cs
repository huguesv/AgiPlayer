// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

public class LogicPatch
{
    public LogicPatch(string[] gameIds, byte resourceIndex, byte[] original, byte[] patched)
    {
        this.GameIds = gameIds ?? throw new ArgumentException(nameof(gameIds));
        this.ResourceIndex = resourceIndex;
        this.Original = original ?? throw new ArgumentException(nameof(original));
        this.Patched = patched ?? throw new ArgumentException(nameof(patched));
    }

    /// <summary>
    /// Gets the game ids this patch applies to.
    /// </summary>
    public string[] GameIds { get; }

    /// <summary>
    /// Gets the index of logic resource.
    /// </summary>
    public byte ResourceIndex { get; }

    /// <summary>
    /// Gets the original code.
    /// </summary>
    public byte[] Original { get; }

    /// <summary>
    /// Gets the patched code.
    /// </summary>
    public byte[] Patched { get; }
}
