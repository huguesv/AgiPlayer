// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Mouse screen area.
/// </summary>
public enum MouseArea
{
    /// <summary>
    /// Unknown area.
    /// </summary>
    Unknown,

    /// <summary>
    /// Command entry section at the bottom of the screen.
    /// </summary>
    CommandEntry,

    /// <summary>
    /// Picture area.
    /// </summary>
    Game,

    /// <summary>
    /// Status bar at the top of the screen.
    /// </summary>
    Status,
}
