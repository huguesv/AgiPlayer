// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    /// <summary>
    /// Constants for the state flags.
    /// </summary>
    public static class Flags
    {
        /// <summary>
        /// Ego base line is completely on pixels with priority = 3 (water surface).
        /// </summary>
        public const int EgoOnWater = 0;

        /// <summary>
        /// Ego is invisible on the screen (completely obscured by another object).
        /// </summary>
        public const int EgoInvisible = 1;

        /// <summary>
        /// The player has issued a command line.
        /// </summary>
        public const int PlayerCommandLine = 2;

        /// <summary>
        /// Ego base line has touched a pixel with priority 2 (signal).
        /// </summary>
        public const int EgoSignal = 3;

        /// <summary>
        /// Said command has accepted the user input.
        /// </summary>
        public const int SaidAccepted = 4;

        /// <summary>
        /// The new room is executed for the first time.
        /// </summary>
        public const int NewRoom = 5;

        /// <summary>
        /// Restart command has been executed.
        /// </summary>
        public const int Restart = 6;

        /// <summary>
        /// Writing to the script buffer is blocked.
        /// </summary>
        public const int ScriptBlock = 7;

        /// <summary>
        /// Variable 15 determines joystick sensitivity.
        /// </summary>
        public const int JoystickSensitivity = 8;

        /// <summary>
        /// Sound on/off.
        /// </summary>
        public const int SoundOn = 9;

        /// <summary>
        /// Built-in debugger.
        /// </summary>
        public const int Debug = 10;

        /// <summary>
        /// Multi channel sound enabled.
        /// </summary>
        public const int MultiChannelSound = 11;

        /// <summary>
        /// Restore command has been executed.
        /// </summary>
        public const int Restore = 12;

        /// <summary>
        /// Allows the status command to select items.
        /// </summary>
        public const int StatusSelect = 13;

        /// <summary>
        /// Allows the menu to work.
        /// </summary>
        public const int Menu = 14;

        /// <summary>
        /// Determines the output mode of print and print.at commands.
        /// </summary>
        public const int PrintMode = 15;

        /// <summary>
        /// Restart command does not prompt the user (if true).
        /// </summary>
        public const int RestartMode = 16;
    }
}
