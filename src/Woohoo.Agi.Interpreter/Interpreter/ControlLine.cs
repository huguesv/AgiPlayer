// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    /// <summary>
    /// Control line enumeration.
    /// </summary>
    public static class ControlLine
    {
        /// <summary>
        /// Unconditional obstacle.
        /// </summary>
        public const byte Obstacle = 0;

        /// <summary>
        /// Conditional obstacle.
        /// </summary>
        public const byte Conditional = 1;

        /// <summary>
        /// Alarm line.
        /// </summary>
        public const byte Alarm = 2;

        /// <summary>
        /// Typically used for water, or something that a view can be confined
        /// to being on.
        /// </summary>
        public const byte Water = 3;
    }
}
