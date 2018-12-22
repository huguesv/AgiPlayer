// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    public class LogicPatch
    {
        /// <summary>
        /// Gets the game ids this patch applies to.
        /// </summary>
        public string[] GameIds { get; internal set; }

        /// <summary>
        /// Gets the index of logic resource.
        /// </summary>
        public byte ResourceIndex { get; internal set; }

        /// <summary>
        /// Gets the original code.
        /// </summary>
        public byte[] Original { get; internal set; }

        /// <summary>
        /// Gets the patched code.
        /// </summary>
        public byte[] Patched { get; internal set; }
    }
}
