// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// View object flags.
/// </summary>
public static class ViewObjectFlags
{
    /// <summary>
    /// Object has been drawn.
    /// </summary>
    public const int Drawn = 0x0001;

    /// <summary>
    /// Ignore blocks and condition lines.
    /// </summary>
    public const int BlockIgnore = 0x0002;

    /// <summary>
    /// Fixes priority.  Agi cannot change it based on position.
    /// </summary>
    public const int PriorityFixed = 0x0004;

    /// <summary>
    /// Ignore horizon.
    /// </summary>
    public const int IgnoreHorizon = 0x0008;

    /// <summary>
    /// Update every cycle.
    /// </summary>
    public const int Update = 0x0010;

    /// <summary>
    /// The object cycles.
    /// </summary>
    public const int Cycle = 0x0020;

    /// <summary>
    /// Animated.
    /// </summary>
    public const int Animate = 0x0040;

    /// <summary>
    /// Resting on a block.
    /// </summary>
    public const int Block = 0x0080;

    /// <summary>
    /// Only allowed on water.
    /// </summary>
    public const int Water = 0x0100;

    /// <summary>
    /// Ignore other objects when determining contacts.
    /// </summary>
    public const int IgnoreObjects = 0x0200;

    /// <summary>
    /// Set whenever object is repositioned.  Interpreter doesn't check its next movement for one cycle.
    /// </summary>
    public const int Repositioned = 0x0400;

    /// <summary>
    /// Only allowed on land.
    /// </summary>
    public const int Land = 0x0800;

    /// <summary>
    /// Does not update object for one cycle.
    /// </summary>
    public const int SkipUpdate = 0x1000;

    /// <summary>
    /// Agi cannot set the loop depending on direction.
    /// </summary>
    public const int LoopFixed = 0x2000;

    /// <summary>
    /// No movement.  If position is same as position in last cycle then this flag is set.
    /// Follow/wander code can then create a new direction (if it hits a wall or something).
    /// </summary>
    public const int MotionLess = 0x4000;
}
