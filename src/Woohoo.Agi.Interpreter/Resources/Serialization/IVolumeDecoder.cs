// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources.Serialization
{
    using Woohoo.Agi.Interpreter;

    /// <summary>
    /// Supports decoding of a volume file.
    /// </summary>
    public interface IVolumeDecoder
    {
        /// <summary>
        /// Extract the resource from the volume.
        /// </summary>
        /// <param name="container">Game container.</param>
        /// <param name="fileName">Volume file name.</param>
        /// <param name="dirEntry">Resource map entry for the resource to extract.</param>
        /// <param name="wasCompressed">Resource was compressed in the volume, so it was decompressed.</param>
        /// <returns>Binary data for the resource.  This is always uncompressed.  If the data is compressed in the volume, the volume decoder must decompress it.</returns>
        byte[] ExtractResource(IGameContainer container, string fileName, VolumeResourceMapEntry dirEntry, out bool wasCompressed);

        /// <summary>
        /// Load the resource map located in the specified folder.
        /// </summary>
        /// <param name="container">Folder where the resource map file(s) are located.</param>
        /// <returns>Resource map.</returns>
        VolumeResourceMap LoadResourceMap(IGameContainer container);

        /// <summary>
        /// Get the volume file name for the specified volume index.
        /// </summary>
        /// <param name="volume">Volume index (0-255).</param>
        /// <returns>Volume file name.</returns>
        string GetVolumeFile(byte volume);
    }
}
