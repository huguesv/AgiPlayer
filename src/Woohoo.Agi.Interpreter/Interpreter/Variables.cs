// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    /// <summary>
    /// Constants for the state variables.
    /// </summary>
    public static class Variables
    {
        /// <summary>
        /// Current room number.
        /// </summary>
        public const int CurrentRoom = 0;

        /// <summary>
        /// Previous room number.
        /// </summary>
        public const int PreviousRoom = 1;

        /// <summary>
        /// Code of the border touched by ego.
        /// </summary>
        public const int Border = 2;

        /// <summary>
        /// Current score.
        /// </summary>
        public const int Score = 3;

        /// <summary>
        /// Number of object, other than ego, that touched the border.
        /// </summary>
        public const int Object = 4;

        /// <summary>
        /// The code of border touched by the object in variable 4.
        /// </summary>
        public const int ObjectBorder = 5;

        /// <summary>
        /// Direction of ego's motion.
        /// </summary>
        public const int Direction = 6;

        /// <summary>
        /// Maximum score.
        /// </summary>
        public const int MaxScore = 7;

        /// <summary>
        /// Number of free 256-byte pages of the interpreter's memory.
        /// </summary>
        public const int FreeMemory = 8;

        /// <summary>
        /// Number of the word in the user message that was not found in the dictionary.
        /// </summary>
        public const int BadWord = 9;

        /// <summary>
        /// Time delay between interpreter cycles in 1/20 second intervals.
        /// </summary>
        public const int Delay = 10;

        /// <summary>
        /// Seconds (interpreter's internal clock).
        /// </summary>
        public const int Seconds = 11;

        /// <summary>
        /// Minutes (interpreter's internal clock).
        /// </summary>
        public const int Minutes = 12;

        /// <summary>
        /// Hours (interpreter's internal clock).
        /// </summary>
        public const int Hours = 13;

        /// <summary>
        /// Days (interpreter's internal clock).
        /// </summary>
        public const int Days = 14;

        /// <summary>
        /// Joystick sensitivity.
        /// </summary>
        public const int JoystickSensitivity = 15;

        /// <summary>
        /// ID number of the view resource associated with ego.
        /// </summary>
        public const int EgoViewResource = 16;

        /// <summary>
        /// Interpreter error code.
        /// </summary>
        public const int Error = 17;

        /// <summary>
        /// Additional information that goes with the error code.
        /// </summary>
        public const int Error2 = 18;

        /// <summary>
        /// Key pressed on the keyboard.
        /// </summary>
        public const int KeyPressed = 19;

        /// <summary>
        /// Computer type.
        /// </summary>
        public const int ComputerType = 20;

        /// <summary>
        /// Window close timer (in 1/2 seconds).
        /// </summary>
        public const int WindowTimer = 21;

        /// <summary>
        /// Sound generator type.
        /// </summary>
        public const int SoundType = 22;

        /// <summary>
        /// Sound volume.
        /// </summary>
        public const int SoundVolume = 23;

        /// <summary>
        /// Maximum number of characters that can be entered on the input line.
        /// </summary>
        public const int InputLength = 24;

        /// <summary>
        /// ID number of the item selected using status comand of 0xFF if ESC was pressed.
        /// </summary>
        public const int StatusSelectedItem = 25;

        /// <summary>
        /// Monitor (display) type.
        /// </summary>
        public const int DisplayType = 26;

        /// <summary>
        /// Mouse button that is currently clicked.
        /// </summary>
        public const int BrianMouseButton = 27;

        /// <summary>
        /// Mouse x position.
        /// </summary>
        public const int BrianMouseX = 28;

        /// <summary>
        /// Mouse y position.
        /// </summary>
        public const int BrianMouseY = 29;
    }
}
