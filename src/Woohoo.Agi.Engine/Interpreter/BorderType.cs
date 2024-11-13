// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// Constants for the object border state variable.
/// </summary>
public static class BorderType
{
    /// <summary>
    /// Ego does not touch any border.
    /// </summary>
    public const byte None = 0;

    /// <summary>
    /// Ego is touching the top edge or horizon.
    /// </summary>
    public const byte ScreenTopEdgeOrHorizon = 1;

    /// <summary>
    /// Ego is touching the right edge.
    /// </summary>
    public const byte ScreenRightEdge = 2;

    /// <summary>
    /// Ego is touching the bottom edge.
    /// </summary>
    public const byte ScreenBottomEdge = 3;

    /// <summary>
    /// Ego is touching the left edge.
    /// </summary>
    public const byte ScreenLeftEdge = 4;
}
