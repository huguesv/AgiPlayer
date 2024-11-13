// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

using Woohoo.Agi.Engine.Resources;

public class ViewObject
{
    public ViewObject()
    {
    }

    public byte StepTime { get; set; }

    public byte StepCount { get; set; }

    public byte Number { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public byte ViewCur { get; set; }

    public ViewResource? ViewResource { get; set; }

    public byte LoopCur { get; set; }

    public byte LoopTotal { get; set; }

    public ViewLoop? ViewLoop { get; set; }

    public byte CelCur { get; set; }

    public byte CelTotal { get; set; }

    public ViewCel? ViewCel { get; set; }

    public byte CelPrevWidth { get; set; }

    public byte CelPrevHeight { get; set; }

    public Blit? Blit { get; set; }

    public int PreviousX { get; set; }

    public int PreviousY { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public byte StepSize { get; set; }

    public byte CycleTime { get; set; }

    public byte CycleCount { get; set; }

    public byte Direction { get; set; }

    public Motion Motion { get; set; }

    public CycleMode Cycle { get; set; }

    public byte Priority { get; set; }

    public int Flags { get; set; }

    public int MoveX { get; set; }

    public int MoveY { get; set; }

    public byte MoveStepSize { get; set; }

    public byte MoveFlag { get; set; }

    public byte FollowStepSize { get; set; }

    public byte FollowFlag { get; set; }

    public byte FollowCount { get; set; }

    public byte WanderCount { get; set; }

    public byte LoopFlag { get; set; }
}
