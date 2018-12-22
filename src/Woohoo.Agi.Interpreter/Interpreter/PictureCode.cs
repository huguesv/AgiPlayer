// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    /// <summary>
    /// Picture codec codes.
    /// </summary>
    public static class PictureCode
    {
        /// <summary>
        /// Enable drawing on the visual surface.
        /// </summary>
        public const byte VisualEnabled = 0xf0;

        /// <summary>
        /// Disable drawing on the visual surface.
        /// </summary>
        public const byte VisualDisabled = 0xf1;

        /// <summary>
        /// Enable drawing on the priority surface.
        /// </summary>
        public const byte PriorityEnabled = 0xf2;

        /// <summary>
        /// Disable drawing on the priority surface.
        /// </summary>
        public const byte PriorityDisabled = 0xf3;

        /// <summary>
        /// Draw corners specified by x and y deltas, starting with an y delta.
        /// </summary>
        public const byte YCorner = 0xf4;

        /// <summary>
        /// Draw corners specified by x and y deltas, starting with an x delta.
        /// </summary>
        public const byte XCorner = 0xf5;

        /// <summary>
        /// Draw lines specified by absolute points.
        /// </summary>
        public const byte AbsoluteLine = 0xf6;

        /// <summary>
        /// Draw lines specified by x and y deltas.
        /// </summary>
        public const byte RelativeLine = 0xf7;

        /// <summary>
        /// Fill the surface at the specified points.
        /// </summary>
        public const byte Fill = 0xf8;

        /// <summary>
        /// Specify the brush type to use for brush instructions.
        /// </summary>
        public const byte Pattern = 0xf9;

        /// <summary>
        /// Draw a brush using the current brush type at the specified points.
        /// </summary>
        public const byte Brush = 0xfa;

        /// <summary>
        /// End of picture instructions.
        /// </summary>
        public const byte End = 0xff;
    }
}
