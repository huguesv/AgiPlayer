// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources
{
    using System;

    /// <summary>
    /// Represents a picture resource, which is a vector image.
    /// </summary>
    public class PictureResource
    {
        /// <summary>
        /// Picture width, in AGI picture coordinates.
        /// </summary>
        public const int Width = 160;

        /// <summary>
        /// Picture height, in AGI picture coordinates.
        /// </summary>
        public const int Height = 168;

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureResource"/> class.
        /// </summary>
        /// <param name="resourceIndex">Resource index (0-255).</param>
        /// <param name="data">Picture byte code.</param>
        public PictureResource(byte resourceIndex, byte[] data)
        {
            this.ResourceIndex = resourceIndex;
            this.Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// Gets resource index (0-255).
        /// </summary>
        public byte ResourceIndex { get; }

        /// <summary>
        /// Gets picture byte code.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Direct access to array items.")]
        public byte[] Data { get; }
    }
}
