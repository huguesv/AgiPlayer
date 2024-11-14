// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Infrastructure;

using Woohoo.Agi.Engine.Interpreter;

internal class InterpreterBuilder
{
    public AgiInterpreter Build()
    {
        var result = new AgiInterpreter(null, null, null);
        result.CreateState();
        return result;
    }
}
