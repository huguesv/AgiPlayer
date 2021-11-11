// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Logic argument type.
/// </summary>
public enum LogicArgumentType
{
    /// <summary>
    /// Number.
    /// </summary>
    Number,

    /// <summary>
    /// Variable.
    /// </summary>
    Variable,

    /// <summary>
    /// Flag.
    /// </summary>
    Flag,

    /// <summary>
    /// Message.
    /// </summary>
    Message,

    /// <summary>
    /// Screen object.
    /// </summary>
    ScreenObject,

    /// <summary>
    /// Inventory object.
    /// </summary>
    InventoryObject,

    /// <summary>
    /// String.
    /// </summary>
    String,

    /// <summary>
    /// Word.
    /// </summary>
    Word,

    /// <summary>
    /// Control.
    /// </summary>
    Control,
}
