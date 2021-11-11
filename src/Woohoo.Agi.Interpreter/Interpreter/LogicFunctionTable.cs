// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

public class LogicFunctionTable
{
    private LogicCommand[] functions;

    public LogicFunctionTable()
    {
        this.functions = CreateFunctions();
    }

    public int Count => this.functions.Length;

    public LogicCommand GetAt(int index)
    {
        return this.functions[index];
    }

    private static LogicCommand[] CreateFunctions()
    {
        LogicCommand[] cmds = new LogicCommand[]
        {
            new LogicCommand(0),
            new LogicCommand(LogicFunctionCode.EqualN, LogicArgumentType.Variable, LogicArgumentType.Number),
            new LogicCommand(LogicFunctionCode.EqualV, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new LogicCommand(LogicFunctionCode.LessN, LogicArgumentType.Variable, LogicArgumentType.Number),
            new LogicCommand(LogicFunctionCode.LessV, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new LogicCommand(LogicFunctionCode.GreaterN, LogicArgumentType.Variable, LogicArgumentType.Number),
            new LogicCommand(LogicFunctionCode.GreaterV, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new LogicCommand(LogicFunctionCode.IsSet, LogicArgumentType.Flag),
            new LogicCommand(LogicFunctionCode.IsSetV, LogicArgumentType.Variable),
            new LogicCommand(LogicFunctionCode.Has, LogicArgumentType.InventoryObject),
            new LogicCommand(LogicFunctionCode.ObjInRoom, LogicArgumentType.InventoryObject, LogicArgumentType.Variable),
            new LogicCommand(LogicFunctionCode.PosN, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number),
            new LogicCommand(LogicFunctionCode.Controller, LogicArgumentType.Control),
            new LogicCommand(LogicFunctionCode.HaveKey),
            new LogicCommand(LogicFunctionCode.Said),
            new LogicCommand(LogicFunctionCode.CompareStrings, LogicArgumentType.String, LogicArgumentType.String),
            new LogicCommand(LogicFunctionCode.ObjInBox, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number),
            new LogicCommand(LogicFunctionCode.CenterPosN, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number),
            new LogicCommand(LogicFunctionCode.RightPosN, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number),
        };

        return cmds;
    }
}
