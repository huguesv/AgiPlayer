// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

using Woohoo.Agi.Resources;

public partial class AgiInterpreter : IKernel
{
    private const byte ScreenTextWindowHeight = 21;

    bool IKernel.EqualN(byte variableA, byte numericB)
    {
        return this.State.Variables[variableA] == numericB;
    }

    bool IKernel.EqualV(byte variableA, byte variableB)
    {
        return this.State.Variables[variableA] == this.State.Variables[variableB];
    }

    bool IKernel.LessN(byte variableA, byte numericB)
    {
        return this.State.Variables[variableA] < numericB;
    }

    bool IKernel.LessV(byte variableA, byte variableB)
    {
        return this.State.Variables[variableA] < this.State.Variables[variableB];
    }

    bool IKernel.GreaterN(byte variableA, byte numericB)
    {
        return this.State.Variables[variableA] > numericB;
    }

    bool IKernel.GreaterV(byte variableA, byte variableB)
    {
        return this.State.Variables[variableA] > this.State.Variables[variableB];
    }

    bool IKernel.IsSet(byte flagNumber)
    {
        return this.State.Flags[flagNumber];
    }

    bool IKernel.IsSetV(byte variableFlagNumber)
    {
        return this.State.Flags[this.State.Variables[variableFlagNumber]];
    }

    bool IKernel.Has(byte inventoryNumber)
    {
        return this.ResourceManager.InventoryResource.Items[inventoryNumber].Location == 0xff;
    }

    bool IKernel.PosN(byte viewNumber, byte numericX1, byte numericY1, byte numericX2, byte numericY2)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        bool inside = ViewObjectManager.IsObjectInside(view.X, view.X, view.Y, numericX1, numericY1, numericX2, numericY2);
        return inside;
    }

    bool IKernel.CenterPosN(byte viewNumber, byte numericX1, byte numericY1, byte numericX2, byte numericY2)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        bool inside = ViewObjectManager.IsObjectInside(view.X + (view.Width / 2), view.X + (view.Width / 2), view.Y, numericX1, numericY1, numericX2, numericY2);
        return inside;
    }

    bool IKernel.RightPosN(byte viewNumber, byte numericX1, byte numericY1, byte numericX2, byte numericY2)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        bool inside = ViewObjectManager.IsObjectInside(view.X + view.Width - 1, view.X + view.Width - 1, view.Y, numericX1, numericY1, numericX2, numericY2);
        return inside;
    }

    bool IKernel.ObjInBox(byte viewNumber, byte numericX1, byte numericY1, byte numericX2, byte numericY2)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        bool inside = ViewObjectManager.IsObjectInside(view.X, view.X + view.Width - 1, view.Y, numericX1, numericY1, numericX2, numericY2);
        return inside;
    }

    bool IKernel.Controller(byte controllerNumber)
    {
        return this.controlState[controllerNumber];
    }

    bool IKernel.ObjInRoom(byte inventoryNumber, byte variableRoom)
    {
        bool inside = this.ResourceManager.InventoryResource.Items[inventoryNumber].Location == this.State.Variables[variableRoom];
        return inside;
    }

    bool IKernel.Said(int[] wordIds)
    {
        int remainingWords = wordIds.Length;
        int badWords = this.ParserResults.Length;
        if (badWords != 0)
        {
            if (!this.State.Flags[Flags.SaidAccepted])
            {
                if (this.State.Flags[Flags.PlayerCommandLine])
                {
                    int current = 0;
                    for (int i = 0; i < wordIds.Length; i++)
                    {
                        int id = wordIds[i];

                        remainingWords--;

                        if (id == VocabularyResource.Any)
                        {
                            badWords = 0;
                            break;
                        }

                        if (badWords == 0)
                        {
                            badWords++;
                            break;
                        }

                        // Any word
                        if (id != this.ParserResults[current].FamilyIdentifier && id != 1)
                        {
                            break;
                        }

                        current++;
                        badWords--;
                    }
                }
            }
        }

        bool said = false;
        if (badWords == 0 && remainingWords == 0)
        {
            this.State.Flags[Flags.SaidAccepted] = true;
            said = true;
        }

        return said;
    }

    bool IKernel.HaveKey()
    {
        byte key = this.State.Variables[Variables.KeyPressed];
        if (key == 0)
        {
            key = this.InputDriver.CharacterPollLoop();
        }

        bool haveKey = false;

        if (key != 0)
        {
            this.State.Variables[Variables.KeyPressed] = key;
            haveKey = true;
        }

        return haveKey;
    }

    bool IKernel.CompareStrings(byte stringA, byte stringB)
    {
        bool equal = string.Equals(this.State.Strings[stringA], this.State.Strings[stringB], StringComparison.OrdinalIgnoreCase);
        return equal;
    }

    void IKernel.ReturnFalse()
    {
        // Nothing to do
    }

    void IKernel.Increment(byte variableA)
    {
        if (this.State.Variables[variableA] < 0xff)
        {
            this.State.Variables[variableA]++;
        }
    }

    void IKernel.Decrement(byte variableA)
    {
        if (this.State.Variables[variableA] != 0)
        {
            this.State.Variables[variableA]--;
        }
    }

    void IKernel.AssignN(byte variableA, byte numericB)
    {
        this.State.Variables[variableA] = numericB;
    }

    void IKernel.AssignV(byte variableA, byte variableB)
    {
        this.State.Variables[variableA] = this.State.Variables[variableB];
    }

    void IKernel.AddN(byte variableA, byte numericB)
    {
        this.State.Variables[variableA] += numericB;
    }

    void IKernel.AddV(byte variableA, byte variableB)
    {
        this.State.Variables[variableA] += this.State.Variables[variableB];
    }

    void IKernel.SubN(byte variableA, byte numericB)
    {
        this.State.Variables[variableA] -= numericB;
    }

    void IKernel.SubV(byte variableA, byte variableB)
    {
        this.State.Variables[variableA] -= this.State.Variables[variableB];
    }

    void IKernel.LIndirectV(byte variableA, byte variableB)
    {
        this.State.Variables[this.State.Variables[variableA]] = this.State.Variables[variableB];
    }

    void IKernel.RIndirect(byte variableA, byte variableB)
    {
        this.State.Variables[variableA] = this.State.Variables[this.State.Variables[variableB]];
    }

    void IKernel.LIndirectN(byte variableA, byte numericB)
    {
        this.State.Variables[this.State.Variables[variableA]] = numericB;
    }

    void IKernel.Set(byte flagA)
    {
        this.State.Flags[flagA] = true;
    }

    void IKernel.Reset(byte flagA)
    {
        this.State.Flags[flagA] = false;
    }

    void IKernel.Toggle(byte flagA)
    {
        this.State.Flags[flagA] = !this.State.Flags[flagA];
    }

    void IKernel.SetV(byte variableA)
    {
        this.State.Flags[this.State.Variables[variableA]] = true;
    }

    void IKernel.ResetV(byte variableA)
    {
        this.State.Flags[this.State.Variables[variableA]] = false;
    }

    void IKernel.ToggleV(byte variableA)
    {
        this.State.Flags[this.State.Variables[variableA]] = !this.State.Flags[this.State.Variables[variableA]];
    }

    void IKernel.NewRoom(byte numericRoomNumber)
    {
        this.CallRoom(numericRoomNumber);
    }

    void IKernel.NewRoomV(byte variableRoomNumber)
    {
        this.CallRoom(this.State.Variables[variableRoomNumber]);
    }

    void IKernel.LoadLogics(byte numericLogicResourceIndex)
    {
        this.LoadLogic(numericLogicResourceIndex, true);
    }

    void IKernel.LoadLogicsV(byte variableLogicResourceIndex)
    {
        this.LoadLogic(this.State.Variables[variableLogicResourceIndex], true);
    }

    void IKernel.Call(byte numericLogicResourceIndex)
    {
        int savedLogicDataIndex = this.LogicInterpreter.CurrentLogicDataIndex;

        // TODO: this accesses 'private' data on logic interpreter
        if (this.CallLogic(numericLogicResourceIndex))
        {
            this.LogicInterpreter.CurrentLogicDataIndex = -1;
        }
        else
        {
            this.LogicInterpreter.CurrentLogicDataIndex = savedLogicDataIndex;
        }
    }

    void IKernel.CallV(byte variableLogicResourceIndex)
    {
        int savedLogicDataIndex = this.LogicInterpreter.CurrentLogicDataIndex;

        // TODO: this accesses 'private' data on logic interpreter
        if (this.CallLogic(this.State.Variables[variableLogicResourceIndex]))
        {
            this.LogicInterpreter.CurrentLogicDataIndex = -1;
        }
        else
        {
            this.LogicInterpreter.CurrentLogicDataIndex = savedLogicDataIndex;
        }
    }

    void IKernel.LoadPicture(byte variablePictureResourceIndex)
    {
        this.LoadPicture(this.State.Variables[variablePictureResourceIndex]);
    }

    void IKernel.DrawPicture(byte variablePictureResourceIndex)
    {
        this.DrawPicture(this.State.Variables[variablePictureResourceIndex]);
    }

    void IKernel.ShowPicture()
    {
        this.State.Flags[Flags.PrintMode] = false;
        this.WindowManager.CloseWindow();
        this.GraphicsRender(false, true);
        this.pictureVisible = true;
    }

    void IKernel.DiscardPicture(byte variablePictureResourceIndex)
    {
        this.DiscardPicture(this.State.Variables[variablePictureResourceIndex]);
    }

    void IKernel.OverlayPicture(byte variablePictureResourceIndex)
    {
        this.OverlayPicture(this.State.Variables[variablePictureResourceIndex]);
    }

    void IKernel.ShowPriScreen()
    {
        this.GraphicsRender(true, false);
        this.InputDriver.PollAcceptOrCancel(0);
        this.GraphicsRender(false, false);
    }

    void IKernel.LoadView(byte numericViewResourceIndex)
    {
        this.LoadView(numericViewResourceIndex, false);
    }

    void IKernel.LoadViewV(byte variableViewResourceIndex)
    {
        this.LoadView(this.State.Variables[variableViewResourceIndex], false);
    }

    void IKernel.DiscardView(byte numericViewResourceIndex)
    {
        this.DiscardView(numericViewResourceIndex);
    }

    void IKernel.AnimateObj(byte viewNumber)
    {
        if (viewNumber >= this.ObjectTable.Length)
        {
            this.ExecutionError(ErrorCodes.AnimateObjectOutOfRange, viewNumber);
        }

        var view = this.ObjectTable.GetAt(viewNumber);

        if ((view.Flags & ViewObjectFlags.Animate) == 0)
        {
            view.Flags = ViewObjectFlags.Update | ViewObjectFlags.Cycle | ViewObjectFlags.Animate;
            view.Motion = Motion.Normal;
            view.Cycle = CycleMode.Normal;
            view.Direction = Direction.Motionless;
        }
    }

    void IKernel.UnanimateAll()
    {
        this.BlistsErase();

        for (int i = 0; i < this.ObjectTable.Length; i++)
        {
            var view = this.ObjectTable.GetAt(i);
            view.Flags &= ~(ViewObjectFlags.Animate | ViewObjectFlags.Drawn);
        }
    }

    void IKernel.Draw(byte viewNumber)
    {
        this.DrawViewObject(viewNumber);
    }

    void IKernel.Erase(byte viewNumber)
    {
        this.EraseViewObject(viewNumber);
    }

    void IKernel.Position(byte viewNumber, byte numericX, byte numericY)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.PreviousX = numericX;
        view.X = view.PreviousX;
        view.PreviousY = numericY;
        view.Y = view.PreviousY;
    }

    void IKernel.PositionV(byte viewNumber, byte variableX, byte variableY)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.PreviousX = this.State.Variables[variableX];
        view.X = view.PreviousX;
        view.PreviousY = this.State.Variables[variableY];
        view.Y = view.PreviousY;
    }

    void IKernel.GetPosition(byte viewNumber, byte variableX, byte variableY)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        this.State.Variables[variableX] = (byte)view.X;
        this.State.Variables[variableY] = (byte)view.Y;
    }

    void IKernel.Reposition(byte viewNumber, byte variableX, byte variableY)
    {
        sbyte offsetX = unchecked((sbyte)this.State.Variables[variableX]);
        sbyte offsetY = unchecked((sbyte)this.State.Variables[variableY]);

        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags |= ViewObjectFlags.Repositioned;

        if (offsetX < 0 && view.X < (-offsetX))
        {
            view.X = 0;
        }
        else
        {
            view.X = (byte)(view.X + offsetX);
        }

        if (offsetY < 0 && view.Y < (-offsetY))
        {
            view.Y = 0;
        }
        else
        {
            view.Y = (byte)(view.Y + offsetY);
        }

        this.ObjectManager.ShufflePosition(view);
    }

    void IKernel.SetView(byte viewNumber, byte numericViewResourceIndex)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        this.ObjViewSet(view, numericViewResourceIndex);
    }

    void IKernel.SetViewV(byte viewNumber, byte variableViewResourceIndex)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        this.ObjViewSet(view, this.State.Variables[variableViewResourceIndex]);
    }

    void IKernel.SetLoop(byte viewNumber, byte numericLoopNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        this.ObjectManager.SetViewLoop(view, numericLoopNumber);
    }

    void IKernel.SetLoopV(byte viewNumber, byte variableLoopNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        this.ObjectManager.SetViewLoop(view, this.State.Variables[variableLoopNumber]);
    }

    void IKernel.FixLoop(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags |= ViewObjectFlags.LoopFixed;
    }

    void IKernel.ReleaseLoop(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags &= ~ViewObjectFlags.LoopFixed;
    }

    void IKernel.SetCel(byte viewNumber, byte numericCelNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);

        this.ObjectManager.SetViewCel(view, numericCelNumber);
        view.Flags &= ~ViewObjectFlags.SkipUpdate;
    }

    void IKernel.SetCelV(byte viewNumber, byte variableCelNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);

        this.ObjectManager.SetViewCel(view, this.State.Variables[variableCelNumber]);
        view.Flags &= ~ViewObjectFlags.SkipUpdate;
    }

    void IKernel.LastCel(byte viewNumber, byte variableCelNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        this.State.Variables[variableCelNumber] = (byte)(view.ViewLoop.Cels.Length - 1);
    }

    void IKernel.CurrentCel(byte viewNumber, byte variableCelNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        this.State.Variables[variableCelNumber] = view.CelCur;
    }

    void IKernel.CurrentLoop(byte viewNumber, byte variableLoopNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        this.State.Variables[variableLoopNumber] = view.LoopCur;
    }

    void IKernel.CurrentView(byte viewNumber, byte variableViewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        this.State.Variables[variableViewNumber] = view.ViewCur;
    }

    void IKernel.NumberOfLoops(byte viewNumber, byte variableTotalLoops)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        this.State.Variables[variableTotalLoops] = view.ViewCur;
    }

    void IKernel.SetPriority(byte viewNumber, byte numericPriority)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags |= ViewObjectFlags.PriorityFixed;
        view.Priority = numericPriority;
    }

    void IKernel.SetPriorityV(byte viewNumber, byte variablePriority)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags |= ViewObjectFlags.PriorityFixed;
        view.Priority = this.State.Variables[variablePriority];
    }

    void IKernel.ReleasePriority(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags &= ~ViewObjectFlags.PriorityFixed;
    }

    void IKernel.GetPriority(byte viewNumber, byte variablePriority)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        this.State.Variables[variablePriority] = view.Priority;
    }

    void IKernel.StopUpdate(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        this.ObjStopUpdate(view);
    }

    void IKernel.StartUpdate(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        this.ObjStartUpdate(view);
    }

    void IKernel.ForceUpdate(byte viewNumber)
    {
        // agi is meant to just update the one
        // but from at least 2.917 it updates them all
        this.BlistsErase();
        this.BlistsDraw();
        this.BlistsUpdate();
    }

    void IKernel.IgnoreHorizon(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags |= ViewObjectFlags.IgnoreHorizon;
    }

    void IKernel.ObserveHorizon(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags &= ~ViewObjectFlags.IgnoreHorizon;
    }

    void IKernel.SetHorizon(byte numericY)
    {
        this.State.Horizon = numericY;
    }

    void IKernel.ObjectOnWater(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags |= ViewObjectFlags.Water;
    }

    void IKernel.ObjectOnLand(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags |= ViewObjectFlags.Land;
    }

    void IKernel.ObjectOnAnything(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags &= ~(ViewObjectFlags.Land | ViewObjectFlags.Water);
    }

    void IKernel.IgnoreObjects(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags |= ViewObjectFlags.IgnoreObjects;
    }

    void IKernel.ObserveObjects(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags &= ~ViewObjectFlags.IgnoreObjects;
    }

    void IKernel.Distance(byte viewNumberA, byte viewNumberB, byte variableDistance)
    {
        var view1 = this.ObjectTable.GetAt(viewNumberA);
        var view2 = this.ObjectTable.GetAt(viewNumberB);

        if (((view1.Flags & ViewObjectFlags.Drawn) == 0) || ((view2.Flags & ViewObjectFlags.Drawn) == 0))
        {
            this.State.Variables[variableDistance] = 0xff;
        }
        else
        {
            int distance = Math.Abs(view1.Y - view2.Y);
            distance += Math.Abs(view1.X + (view1.Width / 2) - view2.X - (view2.Width / 2));

            if (distance > 0xfe)
            {
                this.State.Variables[variableDistance] = 0xfe;
            }
            else
            {
                this.State.Variables[variableDistance] = (byte)distance;
            }
        }
    }

    void IKernel.StopCycling(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags &= ~ViewObjectFlags.Cycle;
    }

    void IKernel.StartCycling(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags |= ViewObjectFlags.Cycle;
    }

    void IKernel.NormalCycle(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Cycle = CycleMode.Normal;
        view.Flags |= ViewObjectFlags.Cycle;
    }

    void IKernel.EndOfLoop(byte viewNumber, byte flagNotify)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Cycle = CycleMode.NormalEnd;
        view.Flags |= ViewObjectFlags.Update | ViewObjectFlags.Cycle | ViewObjectFlags.SkipUpdate;
        view.LoopFlag = flagNotify;
        this.State.Flags[view.LoopFlag] = false;
    }

    void IKernel.ReverseCycle(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Cycle = CycleMode.Reverse;
        view.Flags |= ViewObjectFlags.Cycle;
    }

    void IKernel.ReverseLoop(byte viewNumber, byte flagNotify)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Cycle = CycleMode.ReverseEnd;
        view.Flags |= ViewObjectFlags.Update | ViewObjectFlags.Cycle | ViewObjectFlags.SkipUpdate;
        view.LoopFlag = flagNotify;
        this.State.Flags[view.LoopFlag] = false;
    }

    void IKernel.CycleTime(byte viewNumber, byte variableTime)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.CycleTime = this.State.Variables[variableTime];
        view.CycleCount = view.CycleTime;
    }

    void IKernel.StopMotion(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Direction = Direction.Motionless;
        view.Motion = Motion.Normal;
        if (viewNumber == 0)
        {
            this.State.Variables[Variables.Direction] = Direction.Motionless;
            this.State.EgoControl = EgoControl.Computer;
        }
    }

    void IKernel.StartMotion(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Motion = Motion.Normal;
        if (viewNumber == 0)
        {
            this.State.Variables[Variables.Direction] = Direction.Motionless;
            this.State.EgoControl = EgoControl.Player;
        }
    }

    void IKernel.StepSize(byte viewNumber, byte variableStepSize)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.StepSize = this.State.Variables[variableStepSize];
    }

    void IKernel.StepTime(byte viewNumber, byte variableStepTime)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.StepCount = this.State.Variables[variableStepTime];
        view.StepTime = view.StepCount;
    }

    void IKernel.MoveObj(byte viewNumber, byte numericX, byte numericY, byte numericStepSize, byte flagDone)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Motion = Motion.Move;
        view.MoveX = numericX;
        view.MoveY = numericY;
        view.MoveStepSize = view.StepSize;

        if (numericStepSize != 0)
        {
            view.StepSize = numericStepSize;
        }

        view.MoveFlag = flagDone;
        this.State.Flags[flagDone] = false;
        view.Flags |= ViewObjectFlags.Update;
        if (viewNumber == 0)
        {
            this.State.EgoControl = EgoControl.Computer;
        }

        this.ObjectManager.MoveUpdate(view);
    }

    void IKernel.MoveObjV(byte viewNumber, byte variableX, byte variableY, byte variableStepSize, byte flagDone)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Motion = Motion.Move;
        view.MoveX = this.State.Variables[variableX];
        view.MoveY = this.State.Variables[variableY];
        view.MoveStepSize = view.StepSize;

        if (this.State.Variables[variableStepSize] != 0)
        {
            view.StepSize = this.State.Variables[variableStepSize];
        }

        view.MoveFlag = flagDone;
        this.State.Flags[flagDone] = false;
        view.Flags |= ViewObjectFlags.Update;
        if (viewNumber == 0)
        {
            this.State.EgoControl = EgoControl.Computer;
        }

        this.ObjectManager.MoveUpdate(view);
    }

    void IKernel.FollowEgo(byte viewNumber, byte numericStepSize, byte flagDone)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Motion = Motion.Follow;
        if (numericStepSize <= view.StepSize)
        {
            view.FollowStepSize = view.StepSize;
        }
        else
        {
            view.FollowStepSize = numericStepSize;
        }

        view.FollowFlag = flagDone;
        this.State.Flags[flagDone] = false;
        view.FollowCount = 0xff;
        view.Flags |= ViewObjectFlags.Update;
    }

    void IKernel.Wander(byte viewNumber)
    {
        if (viewNumber == 0)
        {
            this.State.EgoControl = EgoControl.Computer;
        }

        var view = this.ObjectTable.GetAt(viewNumber);
        view.Motion = Motion.Wander;
        view.Flags |= ViewObjectFlags.Update;
    }

    void IKernel.NormalMotion(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Motion = Motion.Normal;
    }

    void IKernel.SetDir(byte viewNumber, byte variableDirection)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Direction = this.State.Variables[variableDirection];
    }

    void IKernel.GetDir(byte viewNumber, byte variableDirection)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        this.State.Variables[variableDirection] = view.Direction;
    }

    void IKernel.IgnoreBlocks(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags |= ViewObjectFlags.BlockIgnore;
    }

    void IKernel.ObserveBlocks(byte viewNumber)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.Flags &= ~ViewObjectFlags.BlockIgnore;
    }

    void IKernel.Block(byte numericX1, byte numericY1, byte numericX2, byte numericY2)
    {
        this.State.BlockIsSet = true;
        this.State.BlockX1 = numericX1;
        this.State.BlockY1 = numericY1;
        this.State.BlockX2 = numericX2;
        this.State.BlockY2 = numericY2;
    }

    void IKernel.Unblock()
    {
        this.State.BlockIsSet = false;
    }

    void IKernel.Get(byte inventoryNumber)
    {
        if (inventoryNumber >= this.ResourceManager.InventoryResource.Items.Length)
        {
            this.ExecutionError(ErrorCodes.InventoryItemOverRange, inventoryNumber - this.ResourceManager.InventoryResource.Items.Length);
        }

        this.ResourceManager.InventoryResource.Items[inventoryNumber].Location = 0xff;
    }

    void IKernel.GetV(byte variableInventoryNumber)
    {
        if (this.State.Variables[variableInventoryNumber] >= this.ResourceManager.InventoryResource.Items.Length)
        {
            this.ExecutionError(ErrorCodes.InventoryItemOverRange, this.State.Variables[variableInventoryNumber] - this.ResourceManager.InventoryResource.Items.Length);
        }

        this.ResourceManager.InventoryResource.Items[this.State.Variables[variableInventoryNumber]].Location = 0xff;
    }

    void IKernel.Drop(byte inventoryNumber)
    {
        if (inventoryNumber >= this.ResourceManager.InventoryResource.Items.Length)
        {
            this.ExecutionError(ErrorCodes.InventoryItemOverRange, inventoryNumber - this.ResourceManager.InventoryResource.Items.Length);
        }

        this.ResourceManager.InventoryResource.Items[inventoryNumber].Location = 0x00;
    }

    void IKernel.Put(byte inventoryNumber, byte numericRoomNumber)
    {
        if (inventoryNumber >= this.ResourceManager.InventoryResource.Items.Length)
        {
            this.ExecutionError(ErrorCodes.InventoryItemOverRange, inventoryNumber - this.ResourceManager.InventoryResource.Items.Length);
        }

        this.ResourceManager.InventoryResource.Items[inventoryNumber].Location = numericRoomNumber;
    }

    void IKernel.PutV(byte inventoryNumber, byte variableRoomNumber)
    {
        if (this.State.Variables[inventoryNumber] >= this.ResourceManager.InventoryResource.Items.Length)
        {
            this.ExecutionError(ErrorCodes.InventoryItemOverRange, this.State.Variables[inventoryNumber] - this.ResourceManager.InventoryResource.Items.Length);
        }

        this.ResourceManager.InventoryResource.Items[this.State.Variables[inventoryNumber]].Location = this.State.Variables[variableRoomNumber];
    }

    void IKernel.GetRoomV(byte inventoryNumber, byte variableRoomNumber)
    {
        if (this.State.Variables[inventoryNumber] >= this.ResourceManager.InventoryResource.Items.Length)
        {
            this.ExecutionError(ErrorCodes.InventoryItemOverRange, this.State.Variables[inventoryNumber] - this.ResourceManager.InventoryResource.Items.Length);
        }

        this.State.Variables[variableRoomNumber] = this.ResourceManager.InventoryResource.Items[this.State.Variables[inventoryNumber]].Location;
    }

    void IKernel.LoadSound(byte numericSoundResourceIndex)
    {
        this.LoadSound(numericSoundResourceIndex);
    }

    void IKernel.Sound(byte numericSoundResourceIndex, byte flagDone)
    {
        this.SoundManager.StopPlaying();

        this.SoundManager.SoundFlag = flagDone;
        this.State.Flags[flagDone] = false;

        SoundResource resource = this.ResourceManager.FindSound(numericSoundResourceIndex);
        if (resource is null)
        {
            this.ExecutionError(ErrorCodes.SoundResourceIndexNotFound, numericSoundResourceIndex);
        }

        this.SoundManager.Play(resource);
    }

    void IKernel.StopSound()
    {
        this.SoundManager.StopPlaying();
    }

    void IKernel.Print(byte messageNumber)
    {
        string text = this.LogicInterpreter.CurrentLogic.GetMessage(messageNumber);
        this.WindowManager.MessageBox(text);
    }

    void IKernel.PrintV(byte variableMessageNumber)
    {
        string text = this.LogicInterpreter.CurrentLogic.GetMessage(this.State.Variables[variableMessageNumber]);
        this.WindowManager.MessageBox(text);
    }

    void IKernel.Display(byte numericRow, byte numericColumn, byte messageNumber)
    {
        string text = this.LogicInterpreter.CurrentLogic.GetMessage(messageNumber);
        this.WindowManager.DisplayAt(text, new TextPosition(numericRow, numericColumn));
    }

    void IKernel.DisplayV(byte numericRow, byte numericColumn, byte variableMessageNumber)
    {
        string text = this.LogicInterpreter.CurrentLogic.GetMessage(this.State.Variables[variableMessageNumber]);
        this.WindowManager.DisplayAt(text, new TextPosition(this.State.Variables[numericRow], this.State.Variables[numericColumn]));
    }

    void IKernel.ClearLines(byte numericRowTop, byte numericRowBottom, byte numericColor)
    {
        if (numericRowTop > numericRowBottom)
        {
            numericRowBottom = numericRowTop;
        }

        this.WindowManager.ClearWindowPortion(numericRowTop, numericRowBottom, this.GraphicsRenderer.CalculateTextBackground(numericColor));
        this.WindowManager.UpdateTextRegion();
    }

    void IKernel.TextScreen()
    {
        this.TextScreen();
    }

    void IKernel.Graphics()
    {
        this.GameControl.InputControl.EnableInput();
        this.GraphicsScreen();
    }

    void IKernel.SetCursorChar(byte messageCharacter)
    {
        string text = this.LogicInterpreter.CurrentLogic.GetMessage(messageCharacter);

        this.State.Cursor = text;
    }

    void IKernel.SetTextAttribute(byte numericForeground, byte numericBackground)
    {
        this.WindowManager.SetTextColor(numericForeground, numericBackground);
    }

    void IKernel.ShakeScreen(byte numericShakeCount)
    {
        this.GraphicsDriver.Shake(numericShakeCount);
    }

    void IKernel.ConfigureScreen(byte numericPlayTop, byte numericInputLine, byte numericStatusLine)
    {
        this.State.WindowRowMin = numericPlayTop;
        this.State.WindowRowMax = numericPlayTop + ScreenTextWindowHeight;
        this.State.InputPosition = numericInputLine;
        this.State.StatusLineRow = numericStatusLine;
    }

    void IKernel.StatusLineOn()
    {
        this.State.StatusVisible = true;
        this.GameControl.DisplayStatusLine();
    }

    void IKernel.StatusLineOff()
    {
        this.State.StatusVisible = false;
        this.WindowManager.ClearLine(this.State.StatusLineRow, 0);
        this.WindowManager.UpdateTextRegion();
    }

    void IKernel.SetString(byte stringDestination, byte messageSource)
    {
        var text = this.LogicInterpreter.CurrentLogic.GetMessage(messageSource);
        this.State.Strings[stringDestination] = text;
    }

    void IKernel.GetString(byte stringDestination, byte messageCaption, byte numericRow, byte numericColumn, byte numericMaxLength)
    {
        var message = this.LogicInterpreter.CurrentLogic.GetMessage(messageCaption);

        int maxLength = numericMaxLength + 1;
        if (maxLength > StringSize)
        {
            maxLength = StringSize;
        }

        string result = this.GameControl.InputControl.GetString(message, maxLength, numericRow, numericColumn);

        this.State.Strings[stringDestination] = result;
    }

    void IKernel.WordToString(byte stringDestination, byte numericWordNumber)
    {
        this.State.Strings[stringDestination] = this.ParserResults[numericWordNumber].Word;
    }

    void IKernel.Parse(byte stringInput)
    {
        this.State.Flags[Flags.PlayerCommandLine] = false;
        this.State.Flags[Flags.SaidAccepted] = false;
        if (stringInput < 12)
        {
            this.ParseText(this.State.Strings[stringInput]);
        }
    }

    void IKernel.GetNumber(byte messageCaption, byte variableNumber)
    {
        var message = this.LogicInterpreter.CurrentLogic.GetMessage(messageCaption);

        var result = this.GameControl.InputControl.GetNumber(message);

        this.State.Variables[variableNumber] = StringUtility.ParseNumber(result);
    }

    void IKernel.PreventInput()
    {
        this.State.InputEnabled = false;
        this.GameControl.InputControl.EnableInput();
        this.WindowManager.ClearLine(this.State.InputPosition, 0);
        this.WindowManager.UpdateTextRegion();
    }

    void IKernel.AcceptInput()
    {
        this.State.InputEnabled = true;
        this.GameControl.InputControl.RedrawInput();
    }

    void IKernel.SetKey(int numericCode, byte controllerNumber)
    {
        for (int i = 0; i < 39; i++)
        {
            if (this.State.Controls[i].Key == 0)
            {
                this.State.Controls[i].Key = numericCode;
                this.State.Controls[i].Number = controllerNumber;
                break;
            }
        }
    }

    void IKernel.AddToPicture(byte numericViewResourceIndex, byte numericLoop, byte numericCel, byte numericX, byte numericY, byte numericPriority, byte numericMargin)
    {
        this.AddViewToPicture(numericViewResourceIndex, numericLoop, numericCel, numericX, numericY, (byte)(numericPriority | (numericMargin << 4)));
    }

    void IKernel.AddToPictureV(byte variableViewResourceIndex, byte variableLoop, byte variableCel, byte variableX, byte variableY, byte variablePriority, byte variableMargin)
    {
        this.AddViewToPicture(this.State.Variables[variableViewResourceIndex], this.State.Variables[variableLoop], this.State.Variables[variableCel], this.State.Variables[variableX], this.State.Variables[variableY], (byte)(this.State.Variables[variablePriority] | (this.State.Variables[variableMargin] << 4)));
    }

    void IKernel.Status()
    {
        this.GameControl.InputControl.EnableInput();
        this.WindowManager.PushTextColor();
        this.WindowManager.SetTextColor(0, 0xf);
        this.TextScreen();
        this.DisplayInventoryScreen();
        this.WindowManager.PopTextColor();
        this.GraphicsScreen();
    }

    void IKernel.SaveGame()
    {
        this.SaveGame();
    }

    void IKernel.RestoreGame()
    {
        if (this.RestoreGame())
        {
            // TODO: this accesses 'private' data on logic interpreter
            this.LogicInterpreter.CurrentLogicDataIndex = -1;
        }
    }

    void IKernel.InitDisk()
    {
        // Nothing to do (incomplete)
    }

    void IKernel.RestartGame()
    {
        if (this.RestartGame())
        {
            // TODO: this accesses 'private' data on logic interpreter
            this.LogicInterpreter.CurrentLogicDataIndex = -1;
        }
    }

    void IKernel.ShowObj(byte numericViewResourceIndex)
    {
        this.ShowView(numericViewResourceIndex);
    }

    void IKernel.Random(byte numericStart, byte numericEnd, byte variableDestination)
    {
        byte val = (byte)((this.Randomizer.Next() % (numericEnd - numericStart + 1)) + numericStart);
        this.State.Variables[variableDestination] = val;
    }

    void IKernel.ProgramControl()
    {
        this.State.EgoControl = EgoControl.Computer;
    }

    void IKernel.PlayerControl()
    {
        this.State.EgoControl = EgoControl.Player;
        if (this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).Motion != Motion.Ego)
        {
            this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).Motion = Motion.Normal;
        }
    }

    void IKernel.ObjectStatusV(byte variableViewObjectNumber)
    {
        // Nothing to do (incomplete)
    }

    void IKernel.Quit(byte numericImmediate)
    {
        this.SoundManager.StopPlaying();
        if (numericImmediate == 1)
        {
            this.ExitAgi();
        }

        bool quit = this.Prompt(UserInterface.QuitQuery);
        if (quit)
        {
            this.ExitAgi();
        }
    }

    void IKernel.ShowMemory()
    {
        // Nothing to do (incomplete)
    }

    void IKernel.Pause()
    {
        this.clockState = ClockState.Pause;
        this.InputDriver.ClearEvents();
        this.SoundManager.StopPlaying();
        this.Prompt(UserInterface.Pause);
        this.clockState = ClockState.Normal;
    }

    void IKernel.EchoLine()
    {
        if (this.State.InputEnabled)
        {
            this.GameControl.InputControl.RepeatPreviousInput();
        }
    }

    void IKernel.CancelLine()
    {
        this.GameControl.InputControl.CancelInput();
    }

    void IKernel.InitJoy()
    {
        // Nothing to do (incomplete)
    }

    void IKernel.ToggleMonitor()
    {
        if (this.State.Variables[Variables.CurrentRoom] != 0)
        {
            this.SaveLogicScanStart();
            this.displayType ^= 1;
            this.WindowManager.PushTextPosition();
            this.GraphicsRenderer.NextDriver();
            this.ReinitializeGraphics();
            this.WindowManager.PopTextPosition();
            this.ReloadState();
        }
    }

    void IKernel.Version()
    {
        this.Prompt(UserInterface.KernelVersion1);
        this.Prompt(UserInterface.KernelVersion2(this.GameInfo.Name, this.GameInfo.Version, this.GameInfo.Id, this.GameInfo.Platform.ToString(), this.GameInfo.Interpreter.ToString()));
    }

    void IKernel.ScriptSize(byte numericSize)
    {
        this.State.ScriptSize = numericSize;

        this.BlistsErase();
        this.ScriptManager.Clear();
        this.BlistsDraw();
    }

    void IKernel.SetGameId(byte messageId)
    {
        if (messageId > this.LogicInterpreter.CurrentLogic.MessageCount)
        {
            this.State.Id = string.Empty;
        }
        else
        {
            string text = this.LogicInterpreter.CurrentLogic.GetMessage(messageId);
            if (text is null)
            {
                this.ExecutionError(ErrorCodes.InvalidMessageIndex, messageId);
            }

            this.State.Id = text;
        }
    }

    void IKernel.Log(byte messageNote)
    {
        string text = this.LogicInterpreter.CurrentLogic.GetMessage(messageNote);

        this.WriteLogEntry(text);
    }

    void IKernel.SetScanStart()
    {
        this.LogicInterpreter.CurrentLogic.ScanStart = this.LogicInterpreter.CurrentLogicDataIndex;
    }

    void IKernel.ResetScanStart()
    {
        this.LogicInterpreter.CurrentLogic.ScanStart = 0;
    }

    void IKernel.RepositionTo(byte viewNumber, byte numericX, byte numericY)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.X = numericX;
        view.Y = numericY;
        view.Flags |= ViewObjectFlags.Repositioned;

        this.ObjectManager.ShufflePosition(view);
    }

    void IKernel.RepositionToV(byte viewNumber, byte variableX, byte variableY)
    {
        var view = this.ObjectTable.GetAt(viewNumber);
        view.X = this.State.Variables[variableX];
        view.Y = this.State.Variables[variableY];
        view.Flags |= ViewObjectFlags.Repositioned;

        this.ObjectManager.ShufflePosition(view);
    }

    void IKernel.TraceOn()
    {
        // TODO: this accesses 'private' data on logic interpreter
        if (this.GameControl.TraceControl.TraceState != TraceState.Uninitialized)
        {
            this.LogicInterpreter.CurrentLogicDataIndex++;
        }
        else
        {
            this.InitializeTraceWindow();
        }
    }

    void IKernel.TraceInfo(byte numericLogicResourceIndex, byte numericTop, byte numericHeight)
    {
        this.GameControl.TraceControl.TraceLogicIndex = numericLogicResourceIndex;
        this.GameControl.TraceControl.TraceTopGiven = numericTop;
        this.GameControl.TraceControl.TraceHeight = numericHeight;
        if (this.GameControl.TraceControl.TraceHeight < 2)
        {
            this.GameControl.TraceControl.TraceHeight = 2;
        }
    }

    void IKernel.PrintAt(byte messageText, byte numericRow, byte numericColumn, byte numericWidth)
    {
        var text = this.LogicInterpreter.CurrentLogic.GetMessage(messageText);

        this.WindowManager.PrintAt(text, new TextPosition(numericRow, numericColumn), numericWidth);
    }

    void IKernel.PrintAtV(byte variableText, byte numericRow, byte numericColumn, byte numericWidth)
    {
        var text = this.LogicInterpreter.CurrentLogic.GetMessage(this.State.Variables[variableText]);

        this.WindowManager.PrintAt(text, new TextPosition(numericRow, numericColumn), numericWidth);
    }

    void IKernel.DiscardViewV(byte variableViewResourceIndex)
    {
        this.DiscardView(this.State.Variables[variableViewResourceIndex]);
    }

    void IKernel.ClearTextRectangle(byte numericRowTop, byte numericColumnTop, byte numericRowBottom, byte numericColumnBottom, byte numericColor)
    {
        this.WindowManager.ClearWindow(new TextPosition(numericRowTop, numericColumnTop), new TextPosition(numericRowBottom, numericColumnBottom), this.GraphicsRenderer.CalculateTextBackground(numericColor));
        this.WindowManager.UpdateTextRegion();
    }

    void IKernel.SetUpperLeft(byte numericTop, byte numericLeft)
    {
        // Nothing to do
    }

    void IKernel.SetMenu(byte messageText)
    {
        var text = this.LogicInterpreter.CurrentLogic.GetMessage(messageText);

        if (!this.Menu.Submitted)
        {
            this.Menu.AppendParentItem(text);
        }
    }

    void IKernel.SetMenuItem(byte messageText, byte controllerNumber)
    {
        var text = this.LogicInterpreter.CurrentLogic.GetMessage(messageText);

        if (!this.Menu.Submitted)
        {
            MenuParentItem parentItem = this.Menu.Items[this.Menu.Items.Count - 1];

            parentItem.AppendItem(text, controllerNumber);
        }
    }

    void IKernel.SubmitMenu()
    {
        this.GameControl.SavedParentMenuItemIndex = 0;
        this.GameControl.SavedMenuItemIndex = 0;
        this.Menu.Submitted = true;
    }

    void IKernel.EnableItem(byte controllerNumber)
    {
        this.Menu.SetItemEnabled(controllerNumber, true);
    }

    void IKernel.DisableItem(byte controllerNumber)
    {
        this.Menu.SetItemEnabled(controllerNumber, false);
    }

    void IKernel.MenuInput()
    {
        if (this.State.Flags[Flags.Menu])
        {
            this.GameControl.MenuNextInput = true;
        }
    }

    void IKernel.ShowObjV(byte variableViewResourceIndex)
    {
        this.ShowView(this.State.Variables[variableViewResourceIndex]);
    }

    void IKernel.OpenDialogue()
    {
        this.WindowManager.MessageState.DialogueOpen = true;
    }

    void IKernel.CloseDialogue()
    {
        this.WindowManager.MessageState.DialogueOpen = false;
    }

    void IKernel.MulN(byte variableA, byte numericB)
    {
        this.State.Variables[variableA] *= numericB;
    }

    void IKernel.MulV(byte variableA, byte variableB)
    {
        this.State.Variables[variableA] *= this.State.Variables[variableB];
    }

    void IKernel.DivN(byte variableA, byte numericB)
    {
        this.State.Variables[variableA] /= numericB;
    }

    void IKernel.DivV(byte variableA, byte variableB)
    {
        this.State.Variables[variableA] /= this.State.Variables[variableB];
    }

    void IKernel.CloseWindow()
    {
        this.WindowManager.CloseWindow();
    }

    void IKernel.SetSimple(byte stringAutoSave)
    {
        this.SavedGameManager.AutoSaveName = this.State.Strings[stringAutoSave];
    }

    void IKernel.PollMouse()
    {
        var m = this.GameControl.PollMouse();

        this.State.Variables[Variables.BrianMouseButton] = (byte)m.Button;
        this.State.Variables[Variables.BrianMouseX] = (byte)m.X;
        this.State.Variables[Variables.BrianMouseY] = (byte)m.Y;
    }

    void IKernel.PushScript()
    {
        this.State.ScriptSaved = this.State.ScriptCount;
    }

    void IKernel.PopScript()
    {
        this.State.ScriptCount = this.State.ScriptSaved;
        this.ScriptManager.SetNextIndex(this.State.ScriptCount * 2);
    }

    void IKernel.HoldKey()
    {
        this.State.WalkMode = WalkMode.HoldKey;
    }

    void IKernel.SetPriBase(byte numericBase)
    {
        this.PriorityTable.SetBasePriority(numericBase);
    }

    void IKernel.DiscardSound(byte numericSoundResourceIndex)
    {
        // Nothing to do (incomplete)
    }

    void IKernel.HideMouse()
    {
        // Nothing to do (incomplete)
    }

    void IKernel.AllowMenu(byte numericEnabled)
    {
        this.State.MenuEnabled = numericEnabled != 0;
    }

    void IKernel.ShowMouse()
    {
        // Nothing to do (incomplete)
    }

    void IKernel.FenceMouse(byte numericX1, byte numericY1, byte numericX2, byte numericY2)
    {
        // Nothing to do (incomplete)
    }

    void IKernel.MousePosN(byte variableX, byte variableY)
    {
        // Nothing to do (incomplete)
    }

    void IKernel.ReleaseKey()
    {
        this.State.WalkMode = WalkMode.ReleaseKey;
    }

    void IKernel.AdjEgoMoveToXY()
    {
        // Nothing to do (incomplete)
    }
}
