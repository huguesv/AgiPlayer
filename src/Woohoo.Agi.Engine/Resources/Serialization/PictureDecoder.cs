// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Resources.Serialization;

using Woohoo.Agi.Engine.Resources;

/// <summary>
/// Picture resource decoder.
/// </summary>
public static class PictureDecoder
{
    /// <summary>
    /// Decode the picture resource from byte array.
    /// </summary>
    /// <param name="resourceIndex">Resource index.</param>
    /// <param name="data">Source array.</param>
    /// <returns>Picture resource.</returns>
    public static PictureResource ReadPicture(byte resourceIndex, byte[] data)
    {
        return new PictureResource(resourceIndex, data);
    }
}
