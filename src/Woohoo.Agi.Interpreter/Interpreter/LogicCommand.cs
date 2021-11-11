// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

public class LogicCommand
{
    private LogicArgumentType[] parameterTypes;

    public LogicCommand(byte code, params LogicArgumentType[] parameterTypes)
    {
        this.Code = code;
        this.parameterTypes = parameterTypes;
    }

    public byte Code { get; }

    public int ParameterCount => this.parameterTypes.Length;

    public LogicArgumentType GetParameterType(int index)
    {
        return this.parameterTypes[index];
    }
}
