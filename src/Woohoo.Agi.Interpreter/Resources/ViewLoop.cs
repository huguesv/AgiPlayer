// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources
{
    using System;

    /// <summary>
    /// Represents an animation loop, which is a made of cels displayed in a 'loop'.
    /// </summary>
    public class ViewLoop
    {
        public ViewLoop(ViewCel[] cels, int mirrorOfIndex)
        {
            if (mirrorOfIndex < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(mirrorOfIndex));
            }

            this.Cels = cels ?? throw new ArgumentNullException(nameof(cels));
            this.MirrorOfIndex = mirrorOfIndex;
        }

        /// <summary>
        /// Gets cels that make up this loop. Empty if this is loop is a mirror.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Direct access to array items.")]
        public ViewCel[] Cels { get; }

        /// <summary>
        /// Gets a value indicating whether indicate if this loop is a mirror of another loop.
        /// </summary>
        public bool IsMirror => this.MirrorOfIndex != -1;

        /// <summary>
        /// Gets index of the loop that this loop is a mirror of.
        /// </summary>
        public int MirrorOfIndex { get; }
    }
}
