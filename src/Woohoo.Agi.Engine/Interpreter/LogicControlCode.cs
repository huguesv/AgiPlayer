// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// Logic resource control codes.
/// </summary>
public static class LogicControlCode
{
    /// <summary>
    /// Or control code.
    /// </summary>
    public const byte Or = 0xfc;

    /// <summary>
    /// Not control code.
    /// </summary>
    public const byte Not = 0xfd;

    /// <summary>
    /// Else/goto control code.
    /// </summary>
    public const byte Else = 0xfe;

    /// <summary>
    /// If control code.
    /// </summary>
    public const byte If = 0xff;
}
