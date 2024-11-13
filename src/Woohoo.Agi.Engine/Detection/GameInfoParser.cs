// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Detection;

internal static class GameInfoParser
{
    internal static Platform ParsePlatform(string platformName)
    {
        return platformName.Replace(" ", string.Empty).ToLower(CultureInfo.CurrentCulture) switch
        {
            "pc" or "ibm" or "dos" or "msdos" or "ms-dos" => Platform.PC,
            "amiga" or "commodore" => Platform.Amiga,
            "appleiigs" or "apple2gs" or "2gs" => Platform.AppleIIgs,
            "appleii" or "apple2" => Platform.AppleII,
            "atarist" or "atari" or "st" => Platform.AtariST,
            "macintosh" or "mac" => Platform.Macintosh,
            _ => Platform.PC,
        };
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

        return s.ToString() switch
        {
            "2089" => InterpreterVersion.V2089,
            "2272" => InterpreterVersion.V2272,
            "2411" => InterpreterVersion.V2411,
            "2425" => InterpreterVersion.V2425,
            "2426" => InterpreterVersion.V2426,
            "2435" => InterpreterVersion.V2435,
            "2439" => InterpreterVersion.V2439,
            "2440" => InterpreterVersion.V2440,
            "2903" => InterpreterVersion.V2903,
            "2912" => InterpreterVersion.V2912,
            "2915" => InterpreterVersion.V2915,
            "2917" => InterpreterVersion.V2917,
            "2936" => InterpreterVersion.V2936,
            "3002086" => InterpreterVersion.V3002086,
            "3002098" => InterpreterVersion.V3002098,
            "3002102" => InterpreterVersion.V3002102,
            "3002107" => InterpreterVersion.V3002107,
            "3002149" => InterpreterVersion.V3002149,
            _ => InterpreterVersion.V2917,
        };
    }
}
