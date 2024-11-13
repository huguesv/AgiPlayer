// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

public static class ScriptCodes
{
    public const byte LoadLogic = 0;
    public const byte LoadView = 1;
    public const byte LoadPicture = 2;
    public const byte LoadSound = 3;
    public const byte DrawPicture = 4;
    public const byte AddToPicture = 5;
    public const byte DiscardPicture = 6;
    public const byte DiscardView = 7;
    public const byte OverlayPicture = 8;
}
