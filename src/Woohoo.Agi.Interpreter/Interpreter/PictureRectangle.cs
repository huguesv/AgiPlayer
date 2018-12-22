// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    /// <summary>
    /// Rectangle in the native picture/view system (x = 0 to 160, y = 0 to 168).
    /// </summary>
    public struct PictureRectangle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PictureRectangle"/> struct.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public PictureRectangle(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Gets or sets x coordinate.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets y coordinate.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// The equality operator (==) returns true if the values of its operands are equal, false otherwise.
        /// </summary>
        /// <param name="a">First object to compare.</param>
        /// <param name="b">Second object to compare.</param>
        /// <returns>true if the values are equal.</returns>
        public static bool operator ==(PictureRectangle a, PictureRectangle b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// The equality operator (!=) returns true if the values of its operands are not equal, false otherwise.
        /// </summary>
        /// <param name="a">First object to compare.</param>
        /// <param name="b">Second object to compare.</param>
        /// <returns>true if the values are not equal.</returns>
        public static bool operator !=(PictureRectangle a, PictureRectangle b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified Object represent the same type and value.
        /// </summary>
        /// <param name="obj">An object to compare to this instance.</param>
        /// <returns>true if obj is the same type and value; otherwise, false. </returns>
        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                PictureRectangle o = (PictureRectangle)obj;

                if (this.X != o.X)
                {
                    return false;
                }

                if (this.Y != o.Y)
                {
                    return false;
                }

                if (this.Width != o.Width)
                {
                    return false;
                }

                if (this.Height != o.Height)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return this.X ^ this.Y ^ this.Width ^ this.Height;
        }
    }
}
