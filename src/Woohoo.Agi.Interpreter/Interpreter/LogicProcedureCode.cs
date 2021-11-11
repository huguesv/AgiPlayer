// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Procedure code.
/// </summary>
public static class LogicProcedureCode
{
    /// <summary>
    /// ReturnFalse.
    /// </summary>
    public const byte ReturnFalse = 0x00;

    /// <summary>
    /// Increment.
    /// </summary>
    public const byte Increment = 0x01;

    /// <summary>
    /// Decrement.
    /// </summary>
    public const byte Decrement = 0x02;

    /// <summary>
    /// AssignN.
    /// </summary>
    public const byte AssignN = 0x03;

    /// <summary>
    /// AssignV.
    /// </summary>
    public const byte AssignV = 0x04;

    /// <summary>
    /// AddN.
    /// </summary>
    public const byte AddN = 0x05;

    /// <summary>
    /// AddV.
    /// </summary>
    public const byte AddV = 0x06;

    /// <summary>
    /// SubN.
    /// </summary>
    public const byte SubN = 0x07;

    /// <summary>
    /// SubV.
    /// </summary>
    public const byte SubV = 0x08;

    /// <summary>
    /// LIndirectV.
    /// </summary>
    public const byte LIndirectV = 0x09;

    /// <summary>
    /// RIndirect.
    /// </summary>
    public const byte RIndirect = 0x0a;

    /// <summary>
    /// LIndirectN.
    /// </summary>
    public const byte LIndirectN = 0x0b;

    /// <summary>
    /// Set.
    /// </summary>
    public const byte Set = 0x0c;

    /// <summary>
    /// Reset.
    /// </summary>
    public const byte Reset = 0x0d;

    /// <summary>
    /// Toggle.
    /// </summary>
    public const byte Toggle = 0x0e;

    /// <summary>
    /// SetV.
    /// </summary>
    public const byte SetV = 0x0f;

    /// <summary>
    /// ResetV.
    /// </summary>
    public const byte ResetV = 0x10;

    /// <summary>
    /// ToggleV.
    /// </summary>
    public const byte ToggleV = 0x11;

    /// <summary>
    /// NewRoom.
    /// </summary>
    public const byte NewRoom = 0x12;

    /// <summary>
    /// NewRoomV.
    /// </summary>
    public const byte NewRoomV = 0x13;

    /// <summary>
    /// LoadLogics.
    /// </summary>
    public const byte LoadLogics = 0x14;

    /// <summary>
    /// LoadLogicsV.
    /// </summary>
    public const byte LoadLogicsV = 0x15;

    /// <summary>
    /// Call.
    /// </summary>
    public const byte Call = 0x16;

    /// <summary>
    /// CallV.
    /// </summary>
    public const byte CallV = 0x17;

    /// <summary>
    /// LoadPicture.
    /// </summary>
    public const byte LoadPicture = 0x18;

    /// <summary>
    /// DrawPicture.
    /// </summary>
    public const byte DrawPicture = 0x19;

    /// <summary>
    /// ShowPicture.
    /// </summary>
    public const byte ShowPicture = 0x1a;

    /// <summary>
    /// DiscardPicture.
    /// </summary>
    public const byte DiscardPicture = 0x1b;

    /// <summary>
    /// OverlayPicture.
    /// </summary>
    public const byte OverlayPicture = 0x1c;

    /// <summary>
    /// ShowPriScreen.
    /// </summary>
    public const byte ShowPriScreen = 0x1d;

    /// <summary>
    /// LoadView.
    /// </summary>
    public const byte LoadView = 0x1e;

    /// <summary>
    /// LoadViewV.
    /// </summary>
    public const byte LoadViewV = 0x1f;

    /// <summary>
    /// DiscardView.
    /// </summary>
    public const byte DiscardView = 0x20;

    /// <summary>
    /// AnimateObj.
    /// </summary>
    public const byte AnimateObj = 0x21;

    /// <summary>
    /// UnanimateAll.
    /// </summary>
    public const byte UnanimateAll = 0x22;

    /// <summary>
    /// Draw.
    /// </summary>
    public const byte Draw = 0x23;

    /// <summary>
    /// Erase.
    /// </summary>
    public const byte Erase = 0x24;

    /// <summary>
    /// Position.
    /// </summary>
    public const byte Position = 0x25;

    /// <summary>
    /// PositionV.
    /// </summary>
    public const byte PositionV = 0x26;

    /// <summary>
    /// GetPosition.
    /// </summary>
    public const byte GetPosition = 0x27; // NOTE: renamed from GetPositionN

    /// <summary>
    /// Reposition.
    /// </summary>
    public const byte Reposition = 0x28;

    /// <summary>
    /// SetView.
    /// </summary>
    public const byte SetView = 0x29;

    /// <summary>
    /// SetViewV.
    /// </summary>
    public const byte SetViewV = 0x2a;

    /// <summary>
    /// SetLoop.
    /// </summary>
    public const byte SetLoop = 0x2b;

    /// <summary>
    /// SetLoopV.
    /// </summary>
    public const byte SetLoopV = 0x2c;

    /// <summary>
    /// FixLoop.
    /// </summary>
    public const byte FixLoop = 0x2d;

    /// <summary>
    /// ReleaseLoop.
    /// </summary>
    public const byte ReleaseLoop = 0x2e;

    /// <summary>
    /// SetCel.
    /// </summary>
    public const byte SetCel = 0x2f;

    /// <summary>
    /// SetCelV.
    /// </summary>
    public const byte SetCelV = 0x30;

    /// <summary>
    /// LastCel.
    /// </summary>
    public const byte LastCel = 0x31;

    /// <summary>
    /// CurrentCel.
    /// </summary>
    public const byte CurrentCel = 0x32;

    /// <summary>
    /// CurrentLoop.
    /// </summary>
    public const byte CurrentLoop = 0x33;

    /// <summary>
    /// CurrentView.
    /// </summary>
    public const byte CurrentView = 0x34;

    /// <summary>
    /// NumberOfLoops.
    /// </summary>
    public const byte NumberOfLoops = 0x35;

    /// <summary>
    /// SetPriority.
    /// </summary>
    public const byte SetPriority = 0x36;

    /// <summary>
    /// SetPriorityV.
    /// </summary>
    public const byte SetPriorityV = 0x37;

    /// <summary>
    /// ReleasePriority.
    /// </summary>
    public const byte ReleasePriority = 0x38;

    /// <summary>
    /// GetPriority.
    /// </summary>
    public const byte GetPriority = 0x39;

    /// <summary>
    /// StopUpdate.
    /// </summary>
    public const byte StopUpdate = 0x3a;

    /// <summary>
    /// StartUpdate.
    /// </summary>
    public const byte StartUpdate = 0x3b;

    /// <summary>
    /// ForceUpdate.
    /// </summary>
    public const byte ForceUpdate = 0x3c;

    /// <summary>
    /// IgnoreHorizon.
    /// </summary>
    public const byte IgnoreHorizon = 0x3d;

    /// <summary>
    /// ObserveHorizon.
    /// </summary>
    public const byte ObserveHorizon = 0x3e;

    /// <summary>
    /// SetHorizon.
    /// </summary>
    public const byte SetHorizon = 0x3f;

    /// <summary>
    /// ObjectOnWater.
    /// </summary>
    public const byte ObjectOnWater = 0x40;

    /// <summary>
    /// ObjectOnLand.
    /// </summary>
    public const byte ObjectOnLand = 0x41;

    /// <summary>
    /// ObjectOnAnything.
    /// </summary>
    public const byte ObjectOnAnything = 0x42;

    /// <summary>
    /// IgnoreObjects.
    /// </summary>
    public const byte IgnoreObjects = 0x43;

    /// <summary>
    /// ObserveObjects.
    /// </summary>
    public const byte ObserveObjects = 0x44;

    /// <summary>
    /// Distance.
    /// </summary>
    public const byte Distance = 0x45;

    /// <summary>
    /// StopCycling.
    /// </summary>
    public const byte StopCycling = 0x46;

    /// <summary>
    /// StartCycling.
    /// </summary>
    public const byte StartCycling = 0x47;

    /// <summary>
    /// NormalCycle.
    /// </summary>
    public const byte NormalCycle = 0x48;

    /// <summary>
    /// EndOfLoop.
    /// </summary>
    public const byte EndOfLoop = 0x49;

    /// <summary>
    /// ReverseCycle.
    /// </summary>
    public const byte ReverseCycle = 0x4a;

    /// <summary>
    /// ReverseLoop.
    /// </summary>
    public const byte ReverseLoop = 0x4b;

    /// <summary>
    /// CycleTime.
    /// </summary>
    public const byte CycleTime = 0x4c;

    /// <summary>
    /// StopMotion.
    /// </summary>
    public const byte StopMotion = 0x4d;

    /// <summary>
    /// StartMotion.
    /// </summary>
    public const byte StartMotion = 0x4e;

    /// <summary>
    /// StepSize.
    /// </summary>
    public const byte StepSize = 0x4f;

    /// <summary>
    /// StepTime.
    /// </summary>
    public const byte StepTime = 0x50;

    /// <summary>
    /// MoveObj.
    /// </summary>
    public const byte MoveObj = 0x51;

    /// <summary>
    /// MoveObjV.
    /// </summary>
    public const byte MoveObjV = 0x52;

    /// <summary>
    /// FollowEgo.
    /// </summary>
    public const byte FollowEgo = 0x53;

    /// <summary>
    /// Wander.
    /// </summary>
    public const byte Wander = 0x54;

    /// <summary>
    /// NormalMotion.
    /// </summary>
    public const byte NormalMotion = 0x55;

    /// <summary>
    /// SetDir.
    /// </summary>
    public const byte SetDir = 0x56;

    /// <summary>
    /// GetDir.
    /// </summary>
    public const byte GetDir = 0x57;

    /// <summary>
    /// IgnoreBlocks.
    /// </summary>
    public const byte IgnoreBlocks = 0x58;

    /// <summary>
    /// ObserveBlocks.
    /// </summary>
    public const byte ObserveBlocks = 0x59;

    /// <summary>
    /// Block.
    /// </summary>
    public const byte Block = 0x5a;

    /// <summary>
    /// Unblock.
    /// </summary>
    public const byte Unblock = 0x5b;

    /// <summary>
    /// Get.
    /// </summary>
    public const byte Get = 0x5c;

    /// <summary>
    /// GetV.
    /// </summary>
    public const byte GetV = 0x5d;

    /// <summary>
    /// Drop.
    /// </summary>
    public const byte Drop = 0x5e;

    /// <summary>
    /// Put.
    /// </summary>
    public const byte Put = 0x5f;

    /// <summary>
    /// PutV.
    /// </summary>
    public const byte PutV = 0x60;

    /// <summary>
    /// GetRoomV.
    /// </summary>
    public const byte GetRoomV = 0x61;

    /// <summary>
    /// LoadSound.
    /// </summary>
    public const byte LoadSound = 0x62;

    /// <summary>
    /// Sound.
    /// </summary>
    public const byte Sound = 0x63;

    /// <summary>
    /// StopSound.
    /// </summary>
    public const byte StopSound = 0x64;

    /// <summary>
    /// Print.
    /// </summary>
    public const byte Print = 0x65;

    /// <summary>
    /// PrintV.
    /// </summary>
    public const byte PrintV = 0x66;

    /// <summary>
    /// Display.
    /// </summary>
    public const byte Display = 0x67;

    /// <summary>
    /// DisplayV.
    /// </summary>
    public const byte DisplayV = 0x68;

    /// <summary>
    /// ClearLines.
    /// </summary>
    public const byte ClearLines = 0x69;

    /// <summary>
    /// TextScreen.
    /// </summary>
    public const byte TextScreen = 0x6a;

    /// <summary>
    /// Graphics.
    /// </summary>
    public const byte Graphics = 0x6b;

    /// <summary>
    /// SetCursorChar.
    /// </summary>
    public const byte SetCursorChar = 0x6c;

    /// <summary>
    /// SetTextAttribute.
    /// </summary>
    public const byte SetTextAttribute = 0x6d;

    /// <summary>
    /// ShakeScreen.
    /// </summary>
    public const byte ShakeScreen = 0x6e;

    /// <summary>
    /// ConfigureScreen.
    /// </summary>
    public const byte ConfigureScreen = 0x6f;

    /// <summary>
    /// StatusLineOn.
    /// </summary>
    public const byte StatusLineOn = 0x70;

    /// <summary>
    /// StatusLineOff.
    /// </summary>
    public const byte StatusLineOff = 0x71;

    /// <summary>
    /// SetString.
    /// </summary>
    public const byte SetString = 0x72;

    /// <summary>
    /// GetString.
    /// </summary>
    public const byte GetString = 0x73;

    /// <summary>
    /// WordToString.
    /// </summary>
    public const byte WordToString = 0x74;

    /// <summary>
    /// Parse.
    /// </summary>
    public const byte Parse = 0x75;

    /// <summary>
    /// GetNumber.
    /// </summary>
    public const byte GetNumber = 0x76;

    /// <summary>
    /// PreventInput.
    /// </summary>
    public const byte PreventInput = 0x77;

    /// <summary>
    /// AcceptInput.
    /// </summary>
    public const byte AcceptInput = 0x78;

    /// <summary>
    /// SetKey.
    /// </summary>
    public const byte SetKey = 0x79;

    /// <summary>
    /// AddToPicture.
    /// </summary>
    public const byte AddToPicture = 0x7a;

    /// <summary>
    /// AddToPictureV.
    /// </summary>
    public const byte AddToPictureV = 0x7b;

    /// <summary>
    /// Status.
    /// </summary>
    public const byte Status = 0x7c;

    /// <summary>
    /// SaveGame.
    /// </summary>
    public const byte SaveGame = 0x7d;

    /// <summary>
    /// RestoreGame.
    /// </summary>
    public const byte RestoreGame = 0x7e;

    /// <summary>
    /// InitDisk.
    /// </summary>
    public const byte InitDisk = 0x7f;

    /// <summary>
    /// RestartGame.
    /// </summary>
    public const byte RestartGame = 0x80;

    /// <summary>
    /// ShowObj.
    /// </summary>
    public const byte ShowObj = 0x81;

    /// <summary>
    /// Random.
    /// </summary>
    public const byte Random = 0x82;

    /// <summary>
    /// ProgramControl.
    /// </summary>
    public const byte ProgramControl = 0x83;

    /// <summary>
    /// PlayerControl.
    /// </summary>
    public const byte PlayerControl = 0x84;

    /// <summary>
    /// ObjectStatusV.
    /// </summary>
    public const byte ObjectStatusV = 0x85;

    /// <summary>
    /// Quit.
    /// </summary>
    public const byte Quit = 0x86;

    /// <summary>
    /// ShowMemory.
    /// </summary>
    public const byte ShowMemory = 0x87;

    /// <summary>
    /// Pause.
    /// </summary>
    public const byte Pause = 0x88;

    /// <summary>
    /// EchoLine.
    /// </summary>
    public const byte EchoLine = 0x89;

    /// <summary>
    /// CancelLine.
    /// </summary>
    public const byte CancelLine = 0x8a;

    /// <summary>
    /// InitJoy.
    /// </summary>
    public const byte InitJoy = 0x8b;

    /// <summary>
    /// ToggleMonitor.
    /// </summary>
    public const byte ToggleMonitor = 0x8c;

    /// <summary>
    /// Version.
    /// </summary>
    public const byte Version = 0x8d;

    /// <summary>
    /// ScriptSize.
    /// </summary>
    public const byte ScriptSize = 0x8e;

    /// <summary>
    /// SetGameId.
    /// </summary>
    public const byte SetGameId = 0x8f;

    /// <summary>
    /// Log.
    /// </summary>
    public const byte Log = 0x90;

    /// <summary>
    /// SetScanStart.
    /// </summary>
    public const byte SetScanStart = 0x91;

    /// <summary>
    /// ResetScanStart.
    /// </summary>
    public const byte ResetScanStart = 0x92;

    /// <summary>
    /// RepositionTo.
    /// </summary>
    public const byte RepositionTo = 0x93;

    /// <summary>
    /// RepositionToV.
    /// </summary>
    public const byte RepositionToV = 0x94;

    /// <summary>
    /// TraceOn.
    /// </summary>
    public const byte TraceOn = 0x95;

    /// <summary>
    /// TraceInfo.
    /// </summary>
    public const byte TraceInfo = 0x96;

    /// <summary>
    /// PrintAt.
    /// </summary>
    public const byte PrintAt = 0x97;

    /// <summary>
    /// PrintAtV.
    /// </summary>
    public const byte PrintAtV = 0x98;

    /// <summary>
    /// DiscardViewV.
    /// </summary>
    public const byte DiscardViewV = 0x99;

    /// <summary>
    /// ClearTextRectangle.
    /// </summary>
    public const byte ClearTextRectangle = 0x9a;

    /// <summary>
    /// SetUpperLeft.
    /// </summary>
    public const byte SetUpperLeft = 0x9b;

    /// <summary>
    /// SetMenu.
    /// </summary>
    public const byte SetMenu = 0x9c;

    /// <summary>
    /// SetMenuItem.
    /// </summary>
    public const byte SetMenuItem = 0x9d;

    /// <summary>
    /// SubmitMenu.
    /// </summary>
    public const byte SubmitMenu = 0x9e;

    /// <summary>
    /// EnableItem.
    /// </summary>
    public const byte EnableItem = 0x9f;

    /// <summary>
    /// DisableItem.
    /// </summary>
    public const byte DisableItem = 0xa0;

    /// <summary>
    /// MenuInput.
    /// </summary>
    public const byte MenuInput = 0xa1;

    /// <summary>
    /// ShowObjV.
    /// </summary>
    public const byte ShowObjV = 0xa2;

    /// <summary>
    /// OpenDialogue.
    /// </summary>
    public const byte OpenDialogue = 0xa3;

    /// <summary>
    /// CloseDialogue.
    /// </summary>
    public const byte CloseDialogue = 0xa4;

    /// <summary>
    /// MulN.
    /// </summary>
    public const byte MulN = 0xa5;

    /// <summary>
    /// MulV.
    /// </summary>
    public const byte MulV = 0xa6;

    /// <summary>
    /// DivN.
    /// </summary>
    public const byte DivN = 0xa7;

    /// <summary>
    /// DivV.
    /// </summary>
    public const byte DivV = 0xa8;

    /// <summary>
    /// CloseWindow.
    /// </summary>
    public const byte CloseWindow = 0xa9;

    /// <summary>
    /// SetSimple.
    /// </summary>
    public const byte SetSimple = 0xaa;

    /// <summary>
    /// PushScript.
    /// </summary>
    public const byte PushScript = 0xab;

    /// <summary>
    /// PopScript.
    /// </summary>
    public const byte PopScript = 0xac;

    /// <summary>
    /// HoldKey.
    /// </summary>
    public const byte HoldKey = 0xad;

    /// <summary>
    /// SetPriBase.
    /// </summary>
    public const byte SetPriBase = 0xae;

    /// <summary>
    /// DiscardSound.
    /// </summary>
    public const byte DiscardSound = 0xaf;

    /// <summary>
    /// HideMouse.
    /// </summary>
    public const byte HideMouse = 0xb0;

    /// <summary>
    /// AllowMenu.
    /// </summary>
    public const byte AllowMenu = 0xb1;

    /// <summary>
    /// ShowMouse.
    /// </summary>
    public const byte ShowMouse = 0xb2;

    /// <summary>
    /// FenceMouse.
    /// </summary>
    public const byte FenceMouse = 0xb3;

    /// <summary>
    /// MousePosN.
    /// </summary>
    public const byte MousePosN = 0xb4;

    /// <summary>
    /// ReleaseKey.
    /// </summary>
    public const byte ReleaseKey = 0xb5;

    /// <summary>
    /// AdjEgoMoveToXY.
    /// </summary>
    public const byte AdjEgoMoveToXY = 0xb6;

    /// <summary>
    /// First procedure code.
    /// </summary>
    public const byte First = 0x00;

    /// <summary>
    /// Last procedure code.
    /// </summary>
    public const byte Last = 0xb6;
}
