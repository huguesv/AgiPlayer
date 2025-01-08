// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

using System;
using System.Collections.Generic;
using Woohoo.Agi.Engine.Resources;

public class LogicVocabularyScraper
{
    private readonly LogicFunctionTable functionTable;
    private readonly LogicProcedureTable procedureTable;

    public LogicVocabularyScraper(InterpreterVersion version)
    {
        this.functionTable = new LogicFunctionTable();
        this.procedureTable = new LogicProcedureTable(version);
    }

    public int[] FindWordReferences(LogicResource resource)
    {
        var result = new HashSet<int>();

        this.ParseStatements(resource, 0, result);

        return result.ToArray();
    }

    public void FindWordReferences(LogicResource resource, HashSet<int> result)
    {
        this.ParseStatements(resource, 0, result);
    }

    private int ParseStatements(LogicResource resource, int index, HashSet<int> result)
    {
        while (index < resource.CodeLength)
        {
            byte op = resource.GetCode(index++);
            if (op == LogicControlCode.If)
            {
                index = this.ParseIfStatement(resource, index, result);
            }
            else if (op == LogicControlCode.Else)
            {
                // Skip the length of the code block
                index += 2;

                index = this.ParseStatements(resource, index, result);
            }
            else
            {
                this.ParseProcedure(resource, ref index, op, result);
            }
        }

        return index;
    }

    private int ParseIfStatement(LogicResource resource, int index, HashSet<int> result)
    {
        while (true)
        {
            byte op = resource.GetCode(index++);

            if (op == LogicControlCode.Or || op == LogicControlCode.Not)
            {
                // Nothing to do
            }
            else if (op == LogicControlCode.If)
            {
                // Skip the length of code block
                index += 2;
                break;
            }
            else
            {
                index = this.ParseFunction(resource, index, op, result);
            }
        }

        return index;
    }

    private int ParseFunction(LogicResource resource, int index, byte op, HashSet<int> result)
    {
        if (op == LogicFunctionCode.Said)
        {
            int paramCount = resource.GetCode(index++);
            for (int paramIndex = 0; paramIndex < paramCount; paramIndex++)
            {
                result.Add(resource.GetCodeLE16(index));
                index += 2;
            }
        }
        else
        {
            LogicCommand cmd = this.functionTable.GetAt(op);
            for (int param = 0; param < cmd.ParameterCount; param++)
            {
                index += LogicCommandTable.GetArgumentTypeLength(cmd.GetParameterType(param));
            }
        }

        return index;
    }

    private void ParseProcedure(LogicResource resource, ref int index, byte op, HashSet<int> result)
    {
        LogicCommand cmd = this.procedureTable.GetAt(op);
        for (int param = 0; param < cmd.ParameterCount; param++)
        {
            index += LogicCommandTable.GetArgumentTypeLength(cmd.GetParameterType(param));
        }
    }
}
