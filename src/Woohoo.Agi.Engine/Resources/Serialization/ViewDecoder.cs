// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Resources.Serialization;

using Woohoo.Agi.Engine.Resources;

/// <summary>
/// View resource decoder.
/// </summary>
public static class ViewDecoder
{
    /// <summary>
    /// Decode the view resource from byte array.
    /// </summary>
    /// <param name="resourceIndex">Resource index.</param>
    /// <param name="viewData">View data.</param>
    /// <returns>View resource.</returns>
    public static ViewResource ReadView(byte resourceIndex, byte[] viewData)
    {
        var mapLoopHeaderPosToLoopNum = new Dictionary<int, int>();

        // View header
        int viewOffset = 0;

        // [0] = unknown
        byte unknown1 = viewData[viewOffset];
        viewOffset += 1;

        // [1] = unknown
        byte unknown2 = viewData[viewOffset];
        viewOffset += 1;

        // [2] = number of loops
        int numLoops = viewData[viewOffset];
        viewOffset += 1;

        // [3][4] = position of description
        int descPos = (viewData[viewOffset + 1] * 0x100) + viewData[viewOffset];
        viewOffset += 2;

        string description = string.Empty;

        if (descPos > 0)
        {
            description = StringDecoder.GetNullTerminatedString(viewData, descPos);
        }

        var loops = new ViewLoop[numLoops];
        for (int loop = 0; loop < numLoops; loop++)
        {
            ViewCel[] cels = [];
            int mirrorOfIndex = -1;

            // [0][1] = position of loop header
            int loopHeaderPos = (viewData[viewOffset + 1] * 0x100) + viewData[viewOffset];
            viewOffset += 2;

            if (mapLoopHeaderPosToLoopNum.TryGetValue(loopHeaderPos, out var value))
            {
                // We have already seen this loop, looks like this is a mirror
                mirrorOfIndex = value;
            }
            else
            {
                mapLoopHeaderPosToLoopNum.Add(loopHeaderPos, loop);

                // Loop header
                int loopOffset = loopHeaderPos;

                // [0] = number of cels in loop
                int numCels = viewData[loopOffset];
                loopOffset += 1;

                cels = new ViewCel[numCels];
                for (int cel = 0; cel < numCels; cel++)
                {
                    // [0][1] = position of first cel header, relative to start of loop
                    int celHeaderPos = (viewData[loopOffset + 1] * 0x100) + viewData[loopOffset];
                    loopOffset += 2;

                    // Cel header
                    int celOffset = loopHeaderPos + celHeaderPos;

                    // [0] = width of cel in pixels (1 agi pixels = 2 ega pixels)
                    byte celWidth = viewData[celOffset];
                    celOffset += 1;

                    // [1] = height of cel
                    byte celHeight = viewData[celOffset];
                    celOffset += 1;

                    // [2] = transparency and cel mirroring
                    // (high 4 bits are mirror info, low 4 bits are transparent color)
                    byte transparentColor = (byte)(viewData[celOffset] & 0x0f);
                    byte mirrorInfo = (byte)((viewData[celOffset] & 0xf0) >> 4);
                    bool mirror = (mirrorInfo & 0x08) != 0;
                    byte mirrorLoopNumber = (byte)(mirrorInfo & 0x07);
                    celOffset += 1;

                    // [+] = RLE encoding of pixels
                    byte[] pixels = RleCompression.Decompress(viewData, celOffset, celWidth, celHeight, transparentColor);

                    cels[cel] = new ViewCel(celWidth, celHeight, transparentColor, mirror, mirrorLoopNumber, pixels);
                }
            }

            loops[loop] = new ViewLoop(cels, mirrorOfIndex);
        }

        // Go through all the loops and process the ones that are marked as mirrors
        for (int loop = 0; loop < loops.Length; loop++)
        {
            if (loops[loop].IsMirror)
            {
                // Create the mirrored cels for this loop
                var originalLoop = loops[loops[loop].MirrorOfIndex];

                var cels = new ViewCel[originalLoop.Cels.Length];
                for (int cel = 0; cel < cels.Length; cel++)
                {
                    ViewCel originalCel = originalLoop.Cels[cel];
                    cels[cel] = new ViewCel(originalCel.Width, originalCel.Height, originalCel.TransparentColor, originalCel.Mirror, originalCel.MirrorLoopNumber, originalCel.MirrorPixels());
                }

                loops[loop] = new ViewLoop(cels, -1);
            }
        }

        return new ViewResource(resourceIndex, loops, description, unknown1, unknown2);
    }
}
