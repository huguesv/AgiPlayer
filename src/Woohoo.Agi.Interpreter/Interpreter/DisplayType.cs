// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Display hardware type used by the DisplayType state variable.
/// </summary>
public static class DisplayType
{
    /// <summary>
    /// CGA (4 colors).
    /// </summary>
    public const byte Cga = 0;

    /// <summary>
    /// RGB.
    /// </summary>
    public const byte Rgb = 1;

    /// <summary>
    /// Hercules (2 colors).
    /// </summary>
    public const byte Hercules = 2;

    /// <summary>
    /// EGA (16 colors).
    /// </summary>
    public const byte Ega = 3;

    /// <summary>
    /// Unknown.  Does not support shake.screen (according to gr-apple2gs).
    /// </summary>
    public const byte MonoVga = 4;
}
