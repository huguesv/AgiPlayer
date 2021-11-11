// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Constants for the interpreter error code.
/// </summary>
public static class ErrorCodes
{
    public const int SoundResourceIndexNotFound = 0x00;
    public const int ViewDiscardViewResourceNotLoaded = 0x01;
    public const int ObjectViewSetViewResourceNotLoaded = 0x03;
    public const int ObjectLoopSetLoopOutOfRange = 0x05;
    public const int ObjectLoopSetViewNotSet = 0x06;
    public const int ObjectCelSetCelOutOfRange = 0x08;
    public const int ObjectCelSetViewNotSet = 0x0a;
    public const int ScriptFull = 0x0b;
    public const int AnimateObjectOutOfRange = 0x0d;
    public const int InvalidMessageIndex = 0x0e;
    public const int PictureDrawResourceNotLoaded = 0x12;
    public const int PictureOverlayResourceNotLoaded = 0x12;
    public const int ObjectEraseViewObjectOutOfRange = 0x12;
    public const int ObjectDrawViewObjectOutOfRange = 0x13;
    public const int ObjectDrawViewObjectCelNull = 0x14;
    public const int PictureDiscardPictureResourceNotLoaded = 0x15;
    public const int InventoryItemOverRange = 0x17;
}
