// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// Mapping of a key and a controller.
/// </summary>
public class ControlEntry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ControlEntry"/> class.
    /// </summary>
    public ControlEntry()
    {
    }

    /// <summary>
    /// Gets or sets key.
    /// </summary>
    public int Key { get; set; }

    /// <summary>
    /// Gets or sets controller.
    /// </summary>
    public int Number { get; set; }
}
