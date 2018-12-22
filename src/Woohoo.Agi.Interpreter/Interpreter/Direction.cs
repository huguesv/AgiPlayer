// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    /// <summary>
    /// Direction in which a view is moving.  Used for the state variable
    /// Direction, and for the Direction property of the view object.
    /// </summary>
    public static class Direction
    {
        /// <summary>
        /// No movement.
        /// </summary>
        public const byte Motionless = 0;

        /// <summary>
        /// Moving north.
        /// </summary>
        public const byte North = 1;

        /// <summary>
        /// Moving north-east.
        /// </summary>
        public const byte NorthEast = 2;

        /// <summary>
        /// Moving east.
        /// </summary>
        public const byte East = 3;

        /// <summary>
        /// Moving south-east.
        /// </summary>
        public const byte SouthEast = 4;

        /// <summary>
        /// Moving south.
        /// </summary>
        public const byte South = 5;

        /// <summary>
        /// Moving south-west.
        /// </summary>
        public const byte SouthWest = 6;

        /// <summary>
        /// Moving west.
        /// </summary>
        public const byte West = 7;

        /// <summary>
        /// Moving north-west.
        /// </summary>
        public const byte NorthWest = 8;
    }
}
