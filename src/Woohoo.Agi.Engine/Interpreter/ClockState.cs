// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// State of the internal clock.
/// </summary>
public enum ClockState
{
    /// <summary>
    /// Clock is running.
    /// </summary>
    Normal,

    /// <summary>
    /// Clock is paused.
    /// </summary>
    Pause,

    /// <summary>
    /// Clock stoppage requested.
    /// </summary>
    TurnOff,
}
