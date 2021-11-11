// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

public static class LogicCommandTable
{
    public static int GetArgumentTypeLength(LogicArgumentType argument)
    {
        int length = 1;

        switch (argument)
        {
            case LogicArgumentType.Number:
                length = 1;
                break;
            case LogicArgumentType.Variable:
                length = 1;
                break;
            case LogicArgumentType.Flag:
                length = 1;
                break;
            case LogicArgumentType.Message:
                length = 1;
                break;
            case LogicArgumentType.ScreenObject:
                length = 1;
                break;
            case LogicArgumentType.InventoryObject:
                length = 1;
                break;
            case LogicArgumentType.String:
                length = 1;
                break;
            case LogicArgumentType.Word:
                length = 2;
                break;
            case LogicArgumentType.Control:
                length = 1;
                break;
        }

        return length;
    }
}
