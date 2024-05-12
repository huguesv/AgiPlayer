// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

public class LogicFunctionTable
{
    private readonly LogicCommand[] functions;

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
        LogicCommand[] cmds =
        [
            new(0, []),
            new(LogicFunctionCode.EqualN, LogicArgumentType.Variable, LogicArgumentType.Number),
            new(LogicFunctionCode.EqualV, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicFunctionCode.LessN, LogicArgumentType.Variable, LogicArgumentType.Number),
            new(LogicFunctionCode.LessV, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicFunctionCode.GreaterN, LogicArgumentType.Variable, LogicArgumentType.Number),
            new(LogicFunctionCode.GreaterV, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicFunctionCode.IsSet, LogicArgumentType.Flag),
            new(LogicFunctionCode.IsSetV, LogicArgumentType.Variable),
            new(LogicFunctionCode.Has, LogicArgumentType.InventoryObject),
            new(LogicFunctionCode.ObjInRoom, LogicArgumentType.InventoryObject, LogicArgumentType.Variable),
            new(LogicFunctionCode.PosN, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number),
            new(LogicFunctionCode.Controller, LogicArgumentType.Control),
            new(LogicFunctionCode.HaveKey, []),
            new(LogicFunctionCode.Said, []),
            new(LogicFunctionCode.CompareStrings, LogicArgumentType.String, LogicArgumentType.String),
            new(LogicFunctionCode.ObjInBox, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number),
            new(LogicFunctionCode.CenterPosN, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number),
            new(LogicFunctionCode.RightPosN, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number),
        ];

        return cmds;
    }
}
