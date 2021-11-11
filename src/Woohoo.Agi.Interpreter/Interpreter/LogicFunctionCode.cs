// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Function code.
/// </summary>
public sealed class LogicFunctionCode
{
    /// <summary>
    /// EqualN.
    /// </summary>
    public const byte EqualN = 0x01;

    /// <summary>
    /// EqualV.
    /// </summary>
    public const byte EqualV = 0x02;

    /// <summary>
    /// LessN.
    /// </summary>
    public const byte LessN = 0x03;

    /// <summary>
    /// LessV.
    /// </summary>
    public const byte LessV = 0x04;

    /// <summary>
    /// GreaterN.
    /// </summary>
    public const byte GreaterN = 0x05;

    /// <summary>
    /// GreaterV.
    /// </summary>
    public const byte GreaterV = 0x06;

    /// <summary>
    /// IsSet.
    /// </summary>
    public const byte IsSet = 0x07;

    /// <summary>
    /// IsSetV.
    /// </summary>
    public const byte IsSetV = 0x08;

    /// <summary>
    /// Has.
    /// </summary>
    public const byte Has = 0x09;

    /// <summary>
    /// ObjInRoom.
    /// </summary>
    public const byte ObjInRoom = 0x0a;

    /// <summary>
    /// PosN.
    /// </summary>
    public const byte PosN = 0x0b;

    /// <summary>
    /// Controller.
    /// </summary>
    public const byte Controller = 0x0c;

    /// <summary>
    /// HaveKey.
    /// </summary>
    public const byte HaveKey = 0x0d;

    /// <summary>
    /// Said.
    /// </summary>
    public const byte Said = 0x0e;

    /// <summary>
    /// CompareStrings.
    /// </summary>
    public const byte CompareStrings = 0x0f;

    /// <summary>
    /// ObjInBox.
    /// </summary>
    public const byte ObjInBox = 0x10;

    /// <summary>
    /// CenterPosN.
    /// </summary>
    public const byte CenterPosN = 0x11;

    /// <summary>
    /// RightPosN.
    /// </summary>
    public const byte RightPosN = 0x12;

    /// <summary>
    /// Unknown19.
    /// </summary>
    public const byte Unknown19 = 0x13;

    /// <summary>
    /// First function code.
    /// </summary>
    public const byte First = 0x01;

    /// <summary>
    /// Last function code.
    /// </summary>
    public const byte Last = 0x13;
}
