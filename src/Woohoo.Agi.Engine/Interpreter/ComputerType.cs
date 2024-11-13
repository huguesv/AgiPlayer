// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// Computer hardware constants used by the ComputerType state variable.
/// </summary>
public static class ComputerType
{
    /// <summary>
    /// PC.
    /// </summary>
    public const int PC = 0;

    /// <summary>
    /// PC Junior.
    /// </summary>
    public const int PCJunior = 1;

    /// <summary>
    /// Tandy 1000.
    /// </summary>
    public const int Tandy = 2;

    /// <summary>
    /// Atari ST (confirmed from Gold Rush logic).
    /// </summary>
    public const int AtariST = 4;

    /// <summary>
    /// Amiga (confirmed from Gold Rush logic).
    /// </summary>
    public const int Amiga = 5;

    /// <summary>
    /// AppleIIgs (confirmed from Larry logic).
    /// </summary>
    public const int AppleIIgs = 7;

    /// <summary>
    /// IBM PS/2.
    /// </summary>
    public const int PS2 = 8;
}
