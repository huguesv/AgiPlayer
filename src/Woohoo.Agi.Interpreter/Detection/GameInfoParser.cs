// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Detection
{
    using System.Globalization;
    using System.Text;

    internal static class GameInfoParser
    {
        internal static Platform ParsePlatform(string platformName)
        {
            switch (platformName.Replace(" ", string.Empty).ToLower(CultureInfo.CurrentCulture))
            {
                case "pc":
                case "ibm":
                case "dos":
                case "msdos":
                case "ms-dos":
                    return Platform.PC;

                case "amiga":
                case "commodore":
                    return Platform.Amiga;

                case "appleiigs":
                case "apple2gs":
                case "2gs":
                    return Platform.AppleIIgs;

                case "appleii":
                case "apple2":
                    return Platform.AppleII;

                case "atarist":
                case "atari":
                case "st":
                    return Platform.AtariST;

                case "macintosh":
                case "mac":
                    return Platform.Macintosh;
            }

            return Platform.PC;
        }

        internal static InterpreterVersion ParseInterpreterVersion(string interpreterVersionText)
        {
            var s = new StringBuilder();
            foreach (char c in interpreterVersionText)
            {
                if (char.IsDigit(c))
                {
                    s.Append(c);
                }
            }

            switch (s.ToString())
            {
                case "2089":
                    return InterpreterVersion.V2089;

                case "2272":
                    return InterpreterVersion.V2272;

                case "2411":
                    return InterpreterVersion.V2411;

                case "2425":
                    return InterpreterVersion.V2425;

                case "2426":
                    return InterpreterVersion.V2426;

                case "2435":
                    return InterpreterVersion.V2435;

                case "2439":
                    return InterpreterVersion.V2439;

                case "2440":
                    return InterpreterVersion.V2440;

                case "2903":
                    return InterpreterVersion.V2903;

                case "2912":
                    return InterpreterVersion.V2912;

                case "2915":
                    return InterpreterVersion.V2915;

                case "2917":
                    return InterpreterVersion.V2917;

                case "2936":
                    return InterpreterVersion.V2936;

                case "3002086":
                    return InterpreterVersion.V3002086;

                case "3002098":
                    return InterpreterVersion.V3002098;

                case "3002102":
                    return InterpreterVersion.V3002102;

                case "3002107":
                    return InterpreterVersion.V3002107;

                case "3002149":
                    return InterpreterVersion.V3002149;
            }

            return InterpreterVersion.V2917;
        }
    }
}
