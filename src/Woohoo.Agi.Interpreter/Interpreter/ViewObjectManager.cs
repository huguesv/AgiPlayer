// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

using Woohoo.Agi.Resources;

public class ViewObjectManager
{
    private const int LoopRight = 0;
    private const int LoopLeft = 1;
    private const int LoopDown = 2;
    private const int LoopUp = 3;
    private const int LoopIgnore = 4;

    private static readonly byte[] DirectionConversion = { 8, 1, 2, 7, 0, 3, 6, 5, 4 };
    private static readonly int[] DirectionXMultiplier = { 0, 0, 1, 1, 1, 0, -1, -1, -1 };
    private static readonly int[] DirectionYMultiplier = { 0, -1, -1, 0, 1, 1, 1, 0, -1 };
    private static readonly byte[] LoopSmall = new byte[] { LoopIgnore, LoopIgnore, LoopRight, LoopRight, LoopRight, LoopIgnore, LoopLeft, LoopLeft, LoopLeft, LoopRight };
    private static readonly byte[] LoopLarge = new byte[] { LoopIgnore, LoopUp, LoopRight, LoopRight, LoopRight, LoopDown, LoopLeft, LoopLeft, LoopLeft, LoopRight };

    public ViewObjectManager(AgiInterpreter interpreter, AgiError error)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
        this.Error = error;
    }

    public LoopUpdate LoopUpdate { get; set; }

    protected AgiInterpreter Interpreter { get; }

    protected AgiError Error { get; }

    protected State State => this.Interpreter.State;

    protected ViewObjectTable ObjectTable => this.Interpreter.ObjectTable;

    protected Random Randomizer => this.Interpreter.Randomizer;

    protected PictureBuffer PictureBuffer => this.Interpreter.GraphicsRenderer.PictureBuffer;

    protected PriorityTable PriorityTable => this.Interpreter.PriorityTable;

    public static bool IsUpdated(ViewObject view)
    {
        if (view is null)
        {
            throw new ArgumentNullException(nameof(view));
        }

        return (view.Flags & (ViewObjectFlags.Drawn | ViewObjectFlags.Update | ViewObjectFlags.Animate)) == (ViewObjectFlags.Drawn | ViewObjectFlags.Update | ViewObjectFlags.Animate);
    }

    public static bool IsStatic(ViewObject view)
    {
        if (view is null)
        {
            throw new ArgumentNullException(nameof(view));
        }

        return (view.Flags & (ViewObjectFlags.Drawn | ViewObjectFlags.Update | ViewObjectFlags.Animate)) == (ViewObjectFlags.Drawn | ViewObjectFlags.Animate);
    }

    public static void SetLoopData(ViewObject view, byte loopNum)
    {
        if (view is null)
        {
            throw new ArgumentNullException(nameof(view));
        }

        view.LoopCur = loopNum;
        view.ViewLoop = view.ViewResource.Loops[loopNum];
        view.CelTotal = (byte)view.ViewLoop.Cels.Length;

        if (view.CelCur >= view.CelTotal)
        {
            view.CelCur = 0;
        }
    }

    public static void SetCelData(ViewObject view, byte celNum)
    {
        if (view is null)
        {
            throw new ArgumentNullException(nameof(view));
        }

        view.CelCur = celNum;
        view.ViewCel = view.ViewLoop.Cels[celNum];
        view.Width = view.ViewCel.Width;
        view.Height = view.ViewCel.Height;
    }

    public static bool IsObjectInside(int left, int right, int y, int x1, int y1, int x2, int y2)
    {
        return !(left < x1 || y < y1 || right > x2 || y > y2);
    }

    public void MoveEgo(int egoX, int egoY)
    {
        // v3 function that moves the ego.. presumably for mouse support
        if (this.State.EgoControl == EgoControl.Player)
        {
            ViewObject view = this.ObjectTable.GetAt(ViewObjectTable.EgoIndex);

            view.Motion = Motion.Ego;
            if (egoX < (view.Width / 2))
            {
                view.MoveX = -1;
            }
            else
            {
                view.MoveX = egoX - (view.Width / 2);
            }

            view.MoveY = egoY;
            view.MoveStepSize = view.StepSize;
        }
    }

    public void MoveUpdate(ViewObject view)
    {
        if (view is null)
        {
            throw new ArgumentNullException(nameof(view));
        }

        view.Direction = ViewObjectManager.GetDirection(view.X, view.Y, view.MoveX, view.MoveY, view.StepSize);
        if (this.ObjectTable.GetAt(ViewObjectTable.EgoIndex) == view)
        {
            this.State.Variables[Variables.Direction] = view.Direction;
        }

        if (view.Direction == Direction.Motionless)
        {
            // Reached destination
            this.StopMove(view);
        }
    }

    public void MotionUpdateAll()
    {
        for (int index = 0; index < this.ObjectTable.Length; index++)
        {
            ViewObject view = this.ObjectTable.GetAt(index);
            if ((view.Flags & (ViewObjectFlags.Drawn | ViewObjectFlags.Animate | ViewObjectFlags.Update)) == (ViewObjectFlags.Drawn | ViewObjectFlags.Animate | ViewObjectFlags.Update))
            {
                if (view.StepCount == 1)
                {
                    this.MotionUpdate(view);
                }
            }
        }
    }

    public void ShufflePosition(ViewObject view)
    {
        if (view is null)
        {
            throw new ArgumentNullException(nameof(view));
        }

        if ((view.Y <= this.State.Horizon) && ((view.Flags & ViewObjectFlags.IgnoreHorizon) == 0))
        {
            view.Y = this.State.Horizon + 1;
        }

        if (this.CheckWalkingArea(view))
        {
            // Walkable
            if (!this.CheckContact(view))
            {
                // No contact with others
                if (this.CheckControlLines(view))
                {
                    // No control ine
                    return;
                }
            }
        }

        ShiftDirection shiftDir = ShiftDirection.Left;
        int shiftCount = 1;
        int shiftSize = 1;

        while (!this.CheckWalkingArea(view) || this.CheckContact(view) || !this.CheckControlLines(view))
        {
            switch (shiftDir)
            {
                case ShiftDirection.Left:
                    view.X--;
                    shiftCount--;
                    if (shiftCount == 0)
                    {
                        shiftDir = ShiftDirection.Down;
                        shiftCount = shiftSize;
                    }

                    break;

                case ShiftDirection.Down:
                    view.Y++;
                    shiftCount--;
                    if (shiftCount == 0)
                    {
                        shiftDir = ShiftDirection.Right;
                        shiftSize++;
                        shiftCount = shiftSize;
                    }

                    break;

                case ShiftDirection.Right:
                    view.X++;
                    shiftCount--;
                    if (shiftCount == 0)
                    {
                        shiftDir = ShiftDirection.Up;
                        shiftCount = shiftSize;
                    }

                    break;

                case ShiftDirection.Up:
                    view.Y--;
                    shiftCount--;
                    if (shiftCount == 0)
                    {
                        shiftDir = ShiftDirection.Left;
                        shiftSize++;
                        shiftCount = shiftSize;
                    }

                    break;
            }
        }
    }

    public void StepUpdateAll()
    {
        this.State.Variables[Variables.ObjectBorder] = BorderType.None;
        this.State.Variables[Variables.Object] = 0;
        this.State.Variables[Variables.Border] = BorderType.None;

        for (int index = 0; index < this.ObjectTable.Length; index++)
        {
            ViewObject view = this.ObjectTable.GetAt(index);

            if ((view.Flags & (ViewObjectFlags.Drawn | ViewObjectFlags.Update | ViewObjectFlags.Animate)) == (ViewObjectFlags.Drawn | ViewObjectFlags.Update | ViewObjectFlags.Animate))
            {
                if (view.StepCount <= 1)
                {
                    view.StepCount = view.StepTime;
                    byte borderCode = BorderType.None;
                    int originalX = view.X;
                    int originalY = view.Y;

                    if ((view.Flags & ViewObjectFlags.Repositioned) == 0)
                    {
                        view.X += view.StepSize * DirectionXMultiplier[view.Direction];
                        view.Y += view.StepSize * DirectionYMultiplier[view.Direction];
                    }

                    if (view.X < 0)
                    {
                        // left edge
                        view.X = 0;
                        borderCode = BorderType.ScreenLeftEdge;
                    }
                    else if ((view.X + view.Width) > PictureResource.Width)
                    {
                        // right edge
                        view.X = PictureResource.Width - view.Width;
                        borderCode = BorderType.ScreenRightEdge;
                    }

                    if ((view.Y - view.Height) < -1)
                    {
                        // top/horizon edge
                        view.Y = view.Height - 1;
                        borderCode = BorderType.ScreenTopEdgeOrHorizon;
                    }
                    else if (view.Y > (PictureResource.Height - 1))
                    {
                        // bottom edge
                        view.Y = PictureResource.Height - 1;
                        borderCode = BorderType.ScreenBottomEdge;
                    }
                    else if (((view.Flags & ViewObjectFlags.IgnoreHorizon) == 0) && (this.State.Horizon > view.Y))
                    {
                        // top/horizon edge
                        view.Y = this.State.Horizon + 1;
                        borderCode = BorderType.ScreenTopEdgeOrHorizon;
                    }

                    // check if the new position doesn't contact anything else
                    if (this.CheckContact(view) || !this.CheckControlLines(view))
                    {
                        view.X = originalX;
                        view.Y = originalY;
                        borderCode = 0;
                        this.ShufflePosition(view);
                    }

                    if (borderCode != BorderType.None)
                    {
                        if (view.Number == 0)
                        {
                            this.State.Variables[Variables.Border] = borderCode;
                        }
                        else
                        {
                            this.State.Variables[Variables.ObjectBorder] = borderCode;
                            this.State.Variables[Variables.Object] = view.Number;
                        }

                        if (view.Motion == Motion.Move)
                        {
                            this.StopMove(view);
                        }
                    }

                    view.Flags &= ~ViewObjectFlags.Repositioned;
                }
                else
                {
                    view.StepCount--;
                }
            }
        }
    }

    public void SetView(ViewObject view, ViewResource resource)
    {
        if (view is null)
        {
            throw new ArgumentNullException(nameof(view));
        }

        view.ViewResource = resource ?? throw new ArgumentNullException(nameof(resource));
        view.ViewCur = resource.ResourceIndex;
        view.LoopTotal = (byte)resource.Loops.Length;

        if (view.LoopCur >= view.LoopTotal)
        {
            this.SetViewLoop(view, 0);
        }
        else
        {
            this.SetViewLoop(view, view.LoopCur);
        }
    }

    public void SetViewCel(ViewObject view, byte celNumber)
    {
        if (view is null)
        {
            throw new ArgumentNullException(nameof(view));
        }

        if (view.ViewResource is null)
        {
            this.Error(ErrorCodes.ObjectCelSetViewNotSet, this.ObjectTable.IndexOf(view));
        }

        if (view.CelTotal <= celNumber)
        {
            this.Error(ErrorCodes.ObjectCelSetCelOutOfRange, this.ObjectTable.IndexOf(view));
        }

        ViewObjectManager.SetCelData(view, celNumber);

        if (view.X + view.Width > PictureResource.Width)
        {
            view.Flags |= ViewObjectFlags.Repositioned;
            view.X = PictureResource.Width - view.Width;
        }

        if (view.Y - view.Height < -1)
        {
            view.Flags |= ViewObjectFlags.Repositioned;
            view.Y = view.Height - 1;
            if (view.Y <= this.State.Horizon && (view.Flags & ViewObjectFlags.IgnoreHorizon) == 0)
            {
                view.Y = this.State.Horizon + 1;
            }
        }
    }

    public void SetViewLoop(ViewObject view, byte loopNumber)
    {
        if (view is null)
        {
            throw new ArgumentNullException(nameof(view));
        }

        if (view.ViewResource is null)
        {
            this.Error(ErrorCodes.ObjectLoopSetViewNotSet, this.ObjectTable.IndexOf(view));
        }

        if (loopNumber > view.LoopTotal)
        {
            this.Error(ErrorCodes.ObjectLoopSetLoopOutOfRange, this.ObjectTable.IndexOf(view));
        }

        if (loopNumber == view.LoopTotal)
        {
            // fixes bug: just out of range
            loopNumber = (byte)(view.LoopTotal - 1);
        }

        ViewObjectManager.SetLoopData(view, loopNumber);
        this.SetViewCel(view, view.CelCur);
    }

    public void ViewLoopUpdate(ViewObject view)
    {
        if (view is null)
        {
            throw new ArgumentNullException(nameof(view));
        }

        if ((view.Flags & ViewObjectFlags.SkipUpdate) != 0)
        {
            view.Flags &= ~ViewObjectFlags.SkipUpdate;
        }
        else
        {
            byte currentCel = view.CelCur;
            byte max = (byte)(view.CelTotal - 1);
            switch (view.Cycle)
            {
                case CycleMode.Normal:
                    currentCel++;
                    if (currentCel > max)
                    {
                        currentCel = 0;
                    }

                    break;

                case CycleMode.NormalEnd:
                    if (currentCel < max)
                    {
                        currentCel++;
                        if (currentCel != max)
                        {
                            break;
                        }
                    }

                    this.State.Flags[view.LoopFlag] = true;
                    view.Flags &= ~ViewObjectFlags.Cycle;
                    view.Direction = Direction.Motionless;
                    view.Cycle = CycleMode.Normal;
                    break;

                case CycleMode.ReverseEnd:
                    if (currentCel != 0)
                    {
                        currentCel--;
                        if (currentCel != 0)
                        {
                            break;
                        }
                    }

                    this.State.Flags[view.LoopFlag] = true;
                    view.Flags &= ~ViewObjectFlags.Cycle;
                    view.Direction = Direction.Motionless;
                    view.Cycle = CycleMode.Normal;
                    break;

                case CycleMode.Reverse:
                    if (currentCel != 0)
                    {
                        currentCel--;
                    }
                    else
                    {
                        currentCel = max;
                    }

                    break;
            }

            this.SetViewCel(view, currentCel);
        }
    }

    public bool UpdateAll()
    {
        bool modified = false;

        for (int index = 0; index < this.ObjectTable.Length; index++)
        {
            ViewObject view = this.ObjectTable.GetAt(index);

            if ((view.Flags & (ViewObjectFlags.Drawn | ViewObjectFlags.Animate | ViewObjectFlags.Update)) == (ViewObjectFlags.Drawn | ViewObjectFlags.Animate | ViewObjectFlags.Update))
            {
                modified = true;

                byte newLoop = LoopIgnore;

                // if loop released (ie, agi picks the loop depending on direction)
                if ((view.Flags & ViewObjectFlags.LoopFixed) == 0)
                {
                    if ((view.LoopTotal == 2) || (view.LoopTotal == 3))
                    {
                        newLoop = LoopSmall[view.Direction];
                    }
                    else if (view.LoopTotal == 4)
                    {
                        newLoop = LoopLarge[view.Direction];
                    }
                    else if (this.LoopUpdate != LoopUpdate.Four)
                    {
                        if (this.LoopUpdate == LoopUpdate.All && view.LoopTotal > 4)
                        {
                            newLoop = LoopLarge[view.Direction];
                        }
                        else if (this.LoopUpdate == LoopUpdate.Flag)
                        {
                            if (this.State.Flags[0x14] && view.LoopTotal > 4)
                            {
                                newLoop = LoopLarge[view.Direction];
                            }
                        }
                    }
                }

                if (view.StepCount == 1)
                {
                    if (newLoop != LoopIgnore)
                    {
                        if (view.LoopCur != newLoop)
                        {
                            this.SetViewLoop(view, newLoop);
                        }
                    }
                }

                if ((view.Flags & ViewObjectFlags.Cycle) != 0)
                {
                    if (view.CycleCount != 0)
                    {
                        view.CycleCount--;
                        if (view.CycleCount == 0)
                        {
                            this.ViewLoopUpdate(view);
                            view.CycleCount = view.CycleTime;
                        }
                    }
                }
            }
        }

        return modified;
    }

    private static byte GetDirection(int x, int y, int newX, int newY, int stepSize)
    {
        // find a new direction depending on the direction and size of the step
        byte di = ViewObjectManager.GetDirectionIndex(newX - x, stepSize);
        di += (byte)(ViewObjectManager.GetDirectionIndex(newY - y, stepSize) * 3);
        return DirectionConversion[di];
    }

    private static byte GetDirectionIndex(int disp, int step)
    {
        if (-step >= disp)
        {
            return 0;
        }
        else if (step <= disp)
        {
            return 2;
        }

        return 1;
    }

    private void StopMove(ViewObject view)
    {
        view.StepSize = view.MoveStepSize;

        // V3
        if (view.Motion != Motion.Ego)
        {
            this.State.Flags[view.MoveFlag] = true;
        }

        view.Motion = Motion.Normal;
        if (this.ObjectTable.GetAt(ViewObjectTable.EgoIndex) == view)
        {
            this.State.EgoControl = EgoControl.Player;
            this.State.Variables[Variables.Direction] = Direction.Motionless;
        }
    }

    private void WanderUpdate(ViewObject view)
    {
        int originalWanderCount = view.WanderCount;

        view.WanderCount--;
        if ((originalWanderCount == 0) || ((view.Flags & ViewObjectFlags.MotionLess) != 0))
        {
            view.Direction = this.RandomDirection();
            if (view == this.ObjectTable.GetAt(ViewObjectTable.EgoIndex))
            {
                this.State.Variables[Variables.Direction] = view.Direction;
            }

            while (view.WanderCount < 6)
            {
                view.WanderCount = (byte)(this.Randomizer.Next() % 0x33);
            }
        }
    }

    private byte RandomDirection()
    {
        return (byte)(this.Randomizer.Next() % 9);
    }

    private void FollowUpdate(ViewObject view)
    {
        ViewObject egoView = this.ObjectTable.GetAt(ViewObjectTable.EgoIndex);
        int egoX = egoView.X + (egoView.Width / 2);
        int viewX = view.X + (view.Width / 2);
        byte direction = ViewObjectManager.GetDirection(viewX, view.Y, egoX, egoView.Y, view.FollowStepSize);

        if (direction == Direction.Motionless)
        {
            view.Direction = Direction.Motionless;
            view.Motion = Motion.Normal;
            this.State.Flags[view.FollowFlag] = true;
        }
        else
        {
            if ((view.FollowCount != 0xff) && ((view.Flags & ViewObjectFlags.MotionLess) != 0))
            {
                do
                {
                    view.Direction = this.RandomDirection();
                }
                while (view.Direction == Direction.Motionless);

                int temp = ((Math.Abs(view.Y - egoView.Y) + Math.Abs(view.X - egoX)) / 2) + 1;

                if (temp <= view.StepSize)
                {
                    view.FollowCount = view.StepSize;
                }
                else
                {
                    do
                    {
                        view.FollowCount = (byte)(this.Randomizer.Next() % 8);
                    }
                    while (view.FollowCount < view.StepSize);
                }
            }
            else
            {
                if (view.FollowCount == 0xff)
                {
                    view.FollowCount = 0;
                }

                if (view.FollowCount != 0)
                {
                    if (view.FollowCount > view.StepSize)
                    {
                        view.FollowCount -= view.StepSize;
                    }
                    else
                    {
                        view.FollowCount = 0;
                    }
                }
                else
                {
                    view.Direction = direction;
                }
            }
        }
    }

    private void CheckBlock(ViewObject view)
    {
        // check for x1 change in the block state
        // you can stay in the block and not get out (or stay out and not get in)
        int x = view.X;
        int y = view.Y;

        bool state = this.CheckBlockPosition(x, y);

        x += view.StepSize * DirectionXMultiplier[view.Direction];
        y += view.StepSize * DirectionYMultiplier[view.Direction];

        if (state == this.CheckBlockPosition(x, y))
        {
            view.Flags &= ~ViewObjectFlags.Block;
        }
        else
        {
            view.Flags |= ViewObjectFlags.Block;
            view.Direction = Direction.Motionless;
            if (view == this.ObjectTable.GetAt(ViewObjectTable.EgoIndex))
            {
                this.State.Variables[Variables.Direction] = Direction.Motionless;
            }
        }
    }

    private bool CheckBlockPosition(int x, int y)
    {
        return (this.State.BlockX1 < x) && (this.State.BlockX2 > x) && (this.State.BlockY1 < y) && (this.State.BlockY2 > y);
    }

    private bool CheckWalkingArea(ViewObject view)
    {
        if (view.X < 0)
        {
            return false;
        }

        if ((view.X + view.Width) > PictureResource.Width)
        {
            return false;
        }

        if ((view.Y - view.Height) < -1)
        {
            return false;
        }

        if (view.Y > (PictureResource.Height - 1))
        {
            return false;
        }

        if (((view.Flags & ViewObjectFlags.IgnoreHorizon) == 0) && (view.Y <= this.State.Horizon))
        {
            return false;
        }

        // Object is within walking area (including horizon)
        return true;
    }

    private bool CheckContact(ViewObject view)
    {
        if ((view.Flags & ViewObjectFlags.IgnoreObjects) == 0)
        {
            for (int index = 0; index < this.ObjectTable.Length; index++)
            {
                ViewObject current = this.ObjectTable.GetAt(index);

                if ((current.Flags & (ViewObjectFlags.Drawn | ViewObjectFlags.Animate)) != (ViewObjectFlags.Drawn | ViewObjectFlags.Animate))
                {
                    continue;
                }

                if ((current.Flags & ViewObjectFlags.IgnoreObjects) != 0)
                {
                    continue;
                }

                if (current.Number == view.Number)
                {
                    continue;
                }

                if ((view.X + view.Width) < current.X)
                {
                    continue;
                }

                if ((current.X + current.Width) < view.X)
                {
                    continue;
                }

                if (view.Y == current.Y)
                {
                    return true;
                }

                if (view.Y > current.Y && view.PreviousY < current.PreviousY)
                {
                    return true;
                }

                if (view.Y < current.Y && view.PreviousY > current.PreviousY)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void MotionUpdate(ViewObject view)
    {
        switch (view.Motion)
        {
            case Motion.Wander:
                this.WanderUpdate(view);
                break;

            case Motion.Follow:
                this.FollowUpdate(view);
                break;

            case Motion.Ego:
            case Motion.Move:
                this.MoveUpdate(view);
                break;
        }

        if (this.State.BlockIsSet)
        {
            if (((view.Flags & ViewObjectFlags.BlockIgnore) == 0) && (view.Direction != Direction.Motionless))
            {
                this.CheckBlock(view);
            }
        }
        else
        {
            view.Flags &= ~ViewObjectFlags.Block;
        }
    }

    private bool CheckControlLines(ViewObject view)
    {
        bool water = false;
        bool signal = false;
        bool control = true;

        if ((view.Flags & ViewObjectFlags.PriorityFixed) == 0)
        {
            view.Priority = this.PriorityTable.GetPriorityAt(view.Y);
        }

        if (view.Priority != 0x0f)
        {
            water = true;
            signal = false;

            for (int x = 0; x < view.ViewCel.Width; x++)
            {
                byte priority = this.PictureBuffer.Priority[(view.Y * PictureResource.Width) + view.X + x];

                // obstacle
                if (priority == ControlLine.Obstacle)
                {
                    control = false;
                    goto check_finish;
                }

                // water
                if (priority != ControlLine.Water)
                {
                    water = false;

                    // conditional
                    if (priority == ControlLine.Conditional)
                    {
                        if ((view.Flags & ViewObjectFlags.BlockIgnore) == 0)
                        {
                            control = false;
                            goto check_finish;
                        }
                    }
                    else if (priority == ControlLine.Alarm)
                    {
                        signal = true;
                    }
                }
            }

            if (!water)
            {
                if ((view.Flags & ViewObjectFlags.Water) != 0)
                {
                    control = false;
                }
            }
            else if ((view.Flags & ViewObjectFlags.Land) != 0)
            {
                control = false;
            }
        }

    check_finish:

        if (view.Number == 0)
        {
            this.State.Flags[Flags.EgoSignal] = signal;
            this.State.Flags[Flags.EgoOnWater] = water;
        }

        return control;
    }
}
