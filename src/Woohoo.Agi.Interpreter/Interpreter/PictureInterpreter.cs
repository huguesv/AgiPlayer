// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    using System;
    using System.Collections.Generic;
    using Woohoo.Agi.Resources;

    /// <summary>
    /// Picture byte code interpreter.
    /// </summary>
    public class PictureInterpreter
    {
        private byte pattern;

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureInterpreter"/> class.
        /// </summary>
        public PictureInterpreter()
        {
        }

        private PictureRenderer Renderer { get; set; }

        /// <summary>
        /// Execute the specified picture resource.
        /// </summary>
        /// <param name="resource">Picture to execute.</param>
        /// <param name="renderer">Picture renderer.</param>
        public void Execute(PictureResource resource, PictureRenderer renderer)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            this.Renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            this.pattern = 0;

            byte[] pictureData = resource.Data;
            int offset = 0;

            while (offset < pictureData.Length && pictureData[offset] != PictureCode.End)
            {
                byte instruction = pictureData[offset];
                offset++;

                switch (instruction)
                {
                    case PictureCode.VisualEnabled:
                        this.Renderer.VisualEnabled = true;
                        this.Renderer.VisualColor = pictureData[offset];
                        offset++;
                        break;
                    case PictureCode.VisualDisabled:
                        this.Renderer.VisualEnabled = false;
                        break;
                    case PictureCode.PriorityEnabled:
                        this.Renderer.PriorityEnabled = true;
                        this.Renderer.PriorityColor = pictureData[offset];
                        offset++;
                        break;
                    case PictureCode.PriorityDisabled:
                        this.Renderer.PriorityEnabled = false;
                        break;
                    case PictureCode.YCorner:
                        offset = this.ReadYCorner(pictureData, offset);
                        break;
                    case PictureCode.XCorner:
                        offset = this.ReadXCorner(pictureData, offset);
                        break;
                    case PictureCode.AbsoluteLine:
                        offset = this.ReadAbsoluteLine(pictureData, offset);
                        break;
                    case PictureCode.RelativeLine:
                        offset = this.ReadRelativeLine(pictureData, offset);
                        break;
                    case PictureCode.Fill:
                        offset = this.ReadFill(pictureData, offset);
                        break;
                    case PictureCode.Pattern:
                        this.pattern = pictureData[offset];
                        offset++;
                        break;
                    case PictureCode.Brush:
                        offset = this.ReadBrush(pictureData, offset);
                        break;
                    default:
                        break;
                }
            }
        }

        private int ReadYCorner(byte[] pictureData, int offset)
        {
            byte x1 = pictureData[offset];
            offset++;

            byte y1 = pictureData[offset];
            offset++;

            var points = new List<PicturePoint>();
            points.Add(new PicturePoint(x1, y1));

            while (offset < pictureData.Length && pictureData[offset] < 0xf0)
            {
                byte y2 = pictureData[offset];
                offset++;

                points.Add(new PicturePoint(x1, y2));

                y1 = y2;

                // Important to check if it is the end of the corner instruction
                if (pictureData[offset] >= 0xf0)
                {
                    break;
                }

                byte x2 = pictureData[offset];
                offset++;

                points.Add(new PicturePoint(x2, y1));

                x1 = x2;
            }

            this.Renderer.DrawLine(points.ToArray());

            return offset;
        }

        private int ReadXCorner(byte[] pictureData, int offset)
        {
            byte x1 = pictureData[offset];
            offset++;

            byte y1 = pictureData[offset];
            offset++;

            var points = new List<PicturePoint>();
            points.Add(new PicturePoint(x1, y1));

            while (offset < pictureData.Length && pictureData[offset] < 0xf0)
            {
                byte x2 = pictureData[offset];
                offset++;

                points.Add(new PicturePoint(x2, y1));

                x1 = x2;

                // Important to check if it is the end of the corner instruction
                if (pictureData[offset] >= 0xf0)
                {
                    break;
                }

                byte y2 = pictureData[offset];
                offset++;

                points.Add(new PicturePoint(x1, y2));

                y1 = y2;
            }

            this.Renderer.DrawLine(points.ToArray());

            return offset;
        }

        private int ReadAbsoluteLine(byte[] pictureData, int offset)
        {
            byte x1 = pictureData[offset];
            offset++;

            byte y1 = pictureData[offset];
            offset++;

            var points = new List<PicturePoint>();
            points.Add(new PicturePoint(x1, y1));

            while (offset < pictureData.Length && pictureData[offset] < 0xf0)
            {
                byte x2 = pictureData[offset];
                offset++;

                byte y2 = pictureData[offset];
                offset++;

                points.Add(new PicturePoint(x2, y2));

                x1 = x2;
                y1 = y2;
            }

            this.Renderer.DrawLine(points.ToArray());

            return offset;
        }

        private int ReadRelativeLine(byte[] pictureData, int offset)
        {
            byte x1 = pictureData[offset];
            offset++;

            byte y1 = pictureData[offset];
            offset++;

            var points = new List<PicturePoint>();
            points.Add(new PicturePoint(x1, y1));

            while (offset < pictureData.Length && pictureData[offset] < 0xf0)
            {
                byte data = pictureData[offset];

                // high 4 bits are dx
                int dx = ((data & 0xf0) >> 4) & 0x0f;

                // low 4 bits are dy
                int dy = data & 0x0f;

                // If sign bit is set, then negate the value
                if ((dx & 0x08) != 0)
                {
                    dx = -1 * (dx & 0x07);
                }

                // If sign bit is set, then negate the value
                if ((dy & 0x08) != 0)
                {
                    dy = -1 * (dy & 0x07);
                }

                offset++;

                points.Add(new PicturePoint((byte)(x1 + dx), (byte)(y1 + dy)));

                x1 += (byte)dx;
                y1 += (byte)dy;
            }

            this.Renderer.DrawLine(points.ToArray());

            return offset;
        }

        private int ReadFill(byte[] pictureData, int offset)
        {
            while (offset < pictureData.Length && pictureData[offset] < 0xf0)
            {
                byte x1 = pictureData[offset];
                offset++;

                byte y1 = pictureData[offset];
                offset++;

                this.Renderer.DrawFill(new PicturePoint(x1, y1));
            }

            return offset;
        }

        private int ReadBrush(byte[] pictureData, int offset)
        {
            while (pictureData[offset] < 0xf0 && offset < pictureData.Length)
            {
                var patternNumber = 0;
                var size = (byte)(this.pattern & 0x07);
                var shape = ((this.pattern & 0x10) != 0) ? PictureBrushShape.Rectangle : PictureBrushShape.Circle;
                var pattern = ((this.pattern & 0x20) != 0) ? PictureBrushPattern.Splatter : PictureBrushPattern.Solid;

                // If the selected pattern is a splatter brush (bit 5 is set)
                // then the there is an additional argument, the texture number
                if ((this.pattern & 0x20) != 0)
                {
                    // Pattern number bit 0 is not used, so right-shift by one bit
                    patternNumber = (byte)((pictureData[offset] >> 1) & 0x7f);
                    offset++;
                }

                byte x1 = pictureData[offset];
                offset++;

                byte y1 = pictureData[offset];
                offset++;

                this.Renderer.DrawBrush(new PicturePoint(x1, y1), size, shape, pattern, patternNumber);
            }

            return offset;
        }
    }
}
