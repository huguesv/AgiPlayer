// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

public class LogicProcedureTable
{
    private readonly LogicCommand[] procedures;

    public LogicProcedureTable(InterpreterVersion interpreter)
    {
        this.procedures = CreateProcedures(interpreter);
    }

    public int Count => this.procedures.Length;

    public LogicCommand GetAt(int index)
    {
        return this.procedures[index];
    }

    private static LogicCommand[] CreateProcedures(InterpreterVersion interpreter)
    {
        List<LogicCommand> list =
        [
            new(LogicProcedureCode.ReturnFalse),
            new(LogicProcedureCode.Increment, LogicArgumentType.Variable),
            new(LogicProcedureCode.Decrement, LogicArgumentType.Variable),
            new(LogicProcedureCode.AssignN, LogicArgumentType.Variable, LogicArgumentType.Number),
            new(LogicProcedureCode.AssignV, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicProcedureCode.AddN, LogicArgumentType.Variable, LogicArgumentType.Number),
            new(LogicProcedureCode.AddV, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicProcedureCode.SubN, LogicArgumentType.Variable, LogicArgumentType.Number),
            new(LogicProcedureCode.SubV, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicProcedureCode.LIndirectV, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicProcedureCode.RIndirect, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicProcedureCode.LIndirectN, LogicArgumentType.Variable, LogicArgumentType.Number),
            new(LogicProcedureCode.Set, LogicArgumentType.Flag),
            new(LogicProcedureCode.Reset, LogicArgumentType.Flag),
            new(LogicProcedureCode.Toggle, LogicArgumentType.Flag),
            new(LogicProcedureCode.SetV, LogicArgumentType.Variable),
            new(LogicProcedureCode.ResetV, LogicArgumentType.Variable),
            new(LogicProcedureCode.ToggleV, LogicArgumentType.Variable),
            new(LogicProcedureCode.NewRoom, LogicArgumentType.Number),
            new(LogicProcedureCode.NewRoomV, LogicArgumentType.Variable),
            new(LogicProcedureCode.LoadLogics, LogicArgumentType.Number),
            new(LogicProcedureCode.LoadLogicsV, LogicArgumentType.Variable),
            new(LogicProcedureCode.Call, LogicArgumentType.Number),
            new(LogicProcedureCode.CallV, LogicArgumentType.Variable),
            new(LogicProcedureCode.LoadPicture, LogicArgumentType.Variable),
            new(LogicProcedureCode.DrawPicture, LogicArgumentType.Variable),
            new(LogicProcedureCode.ShowPicture),
            new(LogicProcedureCode.DiscardPicture, LogicArgumentType.Variable),
            new(LogicProcedureCode.OverlayPicture, LogicArgumentType.Variable),
            new(LogicProcedureCode.ShowPriScreen),
            new(LogicProcedureCode.LoadView, LogicArgumentType.Number),
            new(LogicProcedureCode.LoadViewV, LogicArgumentType.Variable),
            new(LogicProcedureCode.DiscardView, LogicArgumentType.Number),
            new(LogicProcedureCode.AnimateObj, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.UnanimateAll),
            new(LogicProcedureCode.Draw, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.Erase, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.Position, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number),
            new(LogicProcedureCode.PositionV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicProcedureCode.GetPosition, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicProcedureCode.Reposition, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicProcedureCode.SetView, LogicArgumentType.ScreenObject, LogicArgumentType.Number),
            new(LogicProcedureCode.SetViewV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable),
            new(LogicProcedureCode.SetLoop, LogicArgumentType.ScreenObject, LogicArgumentType.Number),
            new(LogicProcedureCode.SetLoopV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable),
            new(LogicProcedureCode.FixLoop, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.ReleaseLoop, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.SetCel, LogicArgumentType.ScreenObject, LogicArgumentType.Number),
            new(LogicProcedureCode.SetCelV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable),
            new(LogicProcedureCode.LastCel, LogicArgumentType.ScreenObject, LogicArgumentType.Variable),
            new(LogicProcedureCode.CurrentCel, LogicArgumentType.ScreenObject, LogicArgumentType.Variable),
            new(LogicProcedureCode.CurrentLoop, LogicArgumentType.ScreenObject, LogicArgumentType.Variable),
            new(LogicProcedureCode.CurrentView, LogicArgumentType.ScreenObject, LogicArgumentType.Variable),
            new(LogicProcedureCode.NumberOfLoops, LogicArgumentType.ScreenObject, LogicArgumentType.Variable),
            new(LogicProcedureCode.SetPriority, LogicArgumentType.ScreenObject, LogicArgumentType.Number),
            new(LogicProcedureCode.SetPriorityV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable),
            new(LogicProcedureCode.ReleasePriority, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.GetPriority, LogicArgumentType.ScreenObject, LogicArgumentType.Variable),
            new(LogicProcedureCode.StopUpdate, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.StartUpdate, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.ForceUpdate, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.IgnoreHorizon, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.ObserveHorizon, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.SetHorizon, LogicArgumentType.Number),
            new(LogicProcedureCode.ObjectOnWater, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.ObjectOnLand, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.ObjectOnAnything, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.IgnoreObjects, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.ObserveObjects, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.Distance, LogicArgumentType.ScreenObject, LogicArgumentType.ScreenObject, LogicArgumentType.Variable),
            new(LogicProcedureCode.StopCycling, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.StartCycling, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.NormalCycle, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.EndOfLoop, LogicArgumentType.ScreenObject, LogicArgumentType.Flag),
            new(LogicProcedureCode.ReverseCycle, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.ReverseLoop, LogicArgumentType.ScreenObject, LogicArgumentType.Flag),
            new(LogicProcedureCode.CycleTime, LogicArgumentType.ScreenObject, LogicArgumentType.Variable), // argument 2 is Number
            new(LogicProcedureCode.StopMotion, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.StartMotion, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.StepSize, LogicArgumentType.ScreenObject, LogicArgumentType.Variable), // argument 2 is Number
            new(LogicProcedureCode.StepTime, LogicArgumentType.ScreenObject, LogicArgumentType.Variable), // argument 2 is Number
            new(LogicProcedureCode.MoveObj, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Flag),
            new(LogicProcedureCode.MoveObjV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Number, LogicArgumentType.Flag), // second to last argument is Variable?  yes, according to nic
            new(LogicProcedureCode.FollowEgo, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Flag),
            new(LogicProcedureCode.Wander, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.NormalMotion, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.SetDir, LogicArgumentType.ScreenObject, LogicArgumentType.Variable),
            new(LogicProcedureCode.GetDir, LogicArgumentType.ScreenObject, LogicArgumentType.Variable),
            new(LogicProcedureCode.IgnoreBlocks, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.ObserveBlocks, LogicArgumentType.ScreenObject),
            new(LogicProcedureCode.Block, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number),
            new(LogicProcedureCode.Unblock),
            new(LogicProcedureCode.Get, LogicArgumentType.InventoryObject),
            new(LogicProcedureCode.GetV, LogicArgumentType.Variable),
            new(LogicProcedureCode.Drop, LogicArgumentType.InventoryObject),
            new(LogicProcedureCode.Put, LogicArgumentType.InventoryObject, LogicArgumentType.Variable),
            new(LogicProcedureCode.PutV, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicProcedureCode.GetRoomV, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicProcedureCode.LoadSound, LogicArgumentType.Number),
            new(LogicProcedureCode.Sound, LogicArgumentType.Number, LogicArgumentType.Flag),
            new(LogicProcedureCode.StopSound),
            new(LogicProcedureCode.Print, LogicArgumentType.Message),
            new(LogicProcedureCode.PrintV, LogicArgumentType.Variable),
            new(LogicProcedureCode.Display, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Message),
            new(LogicProcedureCode.DisplayV, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicProcedureCode.ClearLines, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number),
            new(LogicProcedureCode.TextScreen),
            new(LogicProcedureCode.Graphics),
            new(LogicProcedureCode.SetCursorChar, LogicArgumentType.Message),
            new(LogicProcedureCode.SetTextAttribute, LogicArgumentType.Number, LogicArgumentType.Number),
            new(LogicProcedureCode.ShakeScreen, LogicArgumentType.Number),
            new(LogicProcedureCode.ConfigureScreen, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number),
            new(LogicProcedureCode.StatusLineOn),
            new(LogicProcedureCode.StatusLineOff),
            new(LogicProcedureCode.SetString, LogicArgumentType.String, LogicArgumentType.Message),
            new(LogicProcedureCode.GetString, LogicArgumentType.String, LogicArgumentType.Message, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number),
            new(LogicProcedureCode.WordToString, LogicArgumentType.Word, LogicArgumentType.String),
            new(LogicProcedureCode.Parse, LogicArgumentType.String),
            new(LogicProcedureCode.GetNumber, LogicArgumentType.Message, LogicArgumentType.Variable),
            new(LogicProcedureCode.PreventInput),
            new(LogicProcedureCode.AcceptInput),
            new(LogicProcedureCode.SetKey, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Control),
            new(LogicProcedureCode.AddToPicture, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number),
            new(LogicProcedureCode.AddToPictureV, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicProcedureCode.Status),
            new(LogicProcedureCode.SaveGame),
            new(LogicProcedureCode.RestoreGame),
            new(LogicProcedureCode.InitDisk),
            new(LogicProcedureCode.RestartGame),
            new(LogicProcedureCode.ShowObj, LogicArgumentType.Number),
            new(LogicProcedureCode.Random, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Variable),
            new(LogicProcedureCode.ProgramControl),
            new(LogicProcedureCode.PlayerControl),
            new(LogicProcedureCode.ObjectStatusV, LogicArgumentType.Variable),
            interpreter == InterpreterVersion.V2089
                ? new(LogicProcedureCode.Quit)
                : new(LogicProcedureCode.Quit, LogicArgumentType.Number),
            new(LogicProcedureCode.ShowMemory),
            new(LogicProcedureCode.Pause),
            new(LogicProcedureCode.EchoLine),
            new(LogicProcedureCode.CancelLine),
            new(LogicProcedureCode.InitJoy),
            new(LogicProcedureCode.ToggleMonitor),
            new(LogicProcedureCode.Version),
            new(LogicProcedureCode.ScriptSize, LogicArgumentType.Number),
            new(LogicProcedureCode.SetGameId, LogicArgumentType.Message),
            new(LogicProcedureCode.Log, LogicArgumentType.Message),
            new(LogicProcedureCode.SetScanStart),
            new(LogicProcedureCode.ResetScanStart),
            new(LogicProcedureCode.RepositionTo, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number),
            new(LogicProcedureCode.RepositionToV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable),
            new(LogicProcedureCode.TraceOn),
            new(LogicProcedureCode.TraceInfo, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number),
        ];

        if (interpreter < InterpreterVersion.V2411)
        {
            // 3 arguments <= 2.4
            list.Add(new(LogicProcedureCode.PrintAt, LogicArgumentType.Message, LogicArgumentType.Number, LogicArgumentType.Number));
            list.Add(new(LogicProcedureCode.PrintAtV, LogicArgumentType.Message, LogicArgumentType.Variable, LogicArgumentType.Variable));
        }
        else
        {
            // 4 arguments > 2.4
            list.Add(new(LogicProcedureCode.PrintAt, LogicArgumentType.Message, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));
            list.Add(new(LogicProcedureCode.PrintAtV, LogicArgumentType.Message, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable));
        }

        list.Add(new(LogicProcedureCode.DiscardViewV, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.ClearTextRectangle, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));

        if (interpreter >= InterpreterVersion.V2272)
        {
            list.Add(new(LogicProcedureCode.SetUpperLeft, LogicArgumentType.Number, LogicArgumentType.Number)); // arguments unknown
            list.Add(new(LogicProcedureCode.SetMenu, LogicArgumentType.Message));
            list.Add(new(LogicProcedureCode.SetMenuItem, LogicArgumentType.Message, LogicArgumentType.Control));
            list.Add(new(LogicProcedureCode.SubmitMenu));
            list.Add(new(LogicProcedureCode.EnableItem, LogicArgumentType.Control));
            list.Add(new(LogicProcedureCode.DisableItem, LogicArgumentType.Control));
            list.Add(new(LogicProcedureCode.MenuInput));

            if (interpreter >= InterpreterVersion.V2411)
            {
                list.Add(new(LogicProcedureCode.ShowObjV, LogicArgumentType.Variable));
                list.Add(new(LogicProcedureCode.OpenDialogue));
                list.Add(new(LogicProcedureCode.CloseDialogue));
                list.Add(new(LogicProcedureCode.MulN, LogicArgumentType.Variable, LogicArgumentType.Number));
                list.Add(new(LogicProcedureCode.MulV, LogicArgumentType.Variable, LogicArgumentType.Variable));
                list.Add(new(LogicProcedureCode.DivN, LogicArgumentType.Variable, LogicArgumentType.Number));
                list.Add(new(LogicProcedureCode.DivV, LogicArgumentType.Variable, LogicArgumentType.Variable));
                list.Add(new(LogicProcedureCode.CloseWindow));
                list.Add(new(LogicProcedureCode.SetSimple, LogicArgumentType.Number));
                list.Add(new(LogicProcedureCode.PushScript));
                list.Add(new(LogicProcedureCode.PopScript));
                list.Add(new(LogicProcedureCode.HoldKey));

                if (interpreter >= InterpreterVersion.V2936)
                {
                    list.Add(new(LogicProcedureCode.SetPriBase, LogicArgumentType.Number));
                    list.Add(new(LogicProcedureCode.DiscardSound, LogicArgumentType.Number));

                    if (interpreter >= InterpreterVersion.V3002086)
                    {
                        if (interpreter == InterpreterVersion.V3002086)
                        {
                            // TODO: what is the parameter type?
                            // 1 argument for 3.002.08
                            list.Add(new(LogicProcedureCode.HideMouse));
                        }
                        else
                        {
                            // 0 argument for others
                            list.Add(new(LogicProcedureCode.HideMouse));
                        }

                        list.Add(new(LogicProcedureCode.AllowMenu, LogicArgumentType.Number));

                        if (interpreter >= InterpreterVersion.V3002098)
                        {
                            list.Add(new(LogicProcedureCode.ShowMouse));
                            list.Add(new(LogicProcedureCode.FenceMouse, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));
                            list.Add(new(LogicProcedureCode.MousePosN, LogicArgumentType.Number, LogicArgumentType.Number));
                            list.Add(new(LogicProcedureCode.ReleaseKey));
                            list.Add(new(LogicProcedureCode.AdjEgoMoveToXY));
                        }
                    }
                }
            }
        }

        return [.. list];
    }
}
