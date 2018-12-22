// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    /// <summary>
    /// View cycling mode.
    /// </summary>
    public enum CycleMode
    {
        /// <summary>
        /// Normal cycle.
        /// </summary>
        Normal,

        /// <summary>
        /// Normal cycle, stop at the end.
        /// </summary>
        NormalEnd,

        /// <summary>
        /// Reverse cycle, stop at the end.
        /// </summary>
        ReverseEnd,

        /// <summary>
        /// Reverse cycle.
        /// </summary>
        Reverse,
    }
}
