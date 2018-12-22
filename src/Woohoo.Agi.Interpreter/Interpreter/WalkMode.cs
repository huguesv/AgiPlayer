// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    /// <summary>
    /// Mode used to control ego.
    /// </summary>
    public enum WalkMode
    {
        /// <summary>
        /// Ego continues moving after key is released.
        /// </summary>
        ReleaseKey,

        /// <summary>
        /// Ego stops moving after key is released.
        /// </summary>
        HoldKey,
    }
}
