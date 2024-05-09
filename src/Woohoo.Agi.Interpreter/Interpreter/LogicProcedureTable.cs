// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

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
        var list = new List<LogicCommand>();

        list.Add(new(LogicProcedureCode.ReturnFalse));
        list.Add(new(LogicProcedureCode.Increment, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.Decrement, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.AssignN, LogicArgumentType.Variable, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.AssignV, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.AddN, LogicArgumentType.Variable, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.AddV, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.SubN, LogicArgumentType.Variable, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.SubV, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.LIndirectV, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.RIndirect, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.LIndirectN, LogicArgumentType.Variable, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.Set, LogicArgumentType.Flag));
        list.Add(new(LogicProcedureCode.Reset, LogicArgumentType.Flag));
        list.Add(new(LogicProcedureCode.Toggle, LogicArgumentType.Flag));
        list.Add(new(LogicProcedureCode.SetV, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.ResetV, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.ToggleV, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.NewRoom, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.NewRoomV, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.LoadLogics, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.LoadLogicsV, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.Call, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.CallV, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.LoadPicture, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.DrawPicture, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.ShowPicture));
        list.Add(new(LogicProcedureCode.DiscardPicture, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.OverlayPicture, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.ShowPriScreen));
        list.Add(new(LogicProcedureCode.LoadView, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.LoadViewV, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.DiscardView, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.AnimateObj, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.UnanimateAll));
        list.Add(new(LogicProcedureCode.Draw, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.Erase, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.Position, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.PositionV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.GetPosition, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.Reposition, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.SetView, LogicArgumentType.ScreenObject, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.SetViewV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.SetLoop, LogicArgumentType.ScreenObject, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.SetLoopV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.FixLoop, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.ReleaseLoop, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.SetCel, LogicArgumentType.ScreenObject, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.SetCelV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.LastCel, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.CurrentCel, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.CurrentLoop, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.CurrentView, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.NumberOfLoops, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.SetPriority, LogicArgumentType.ScreenObject, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.SetPriorityV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.ReleasePriority, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.GetPriority, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.StopUpdate, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.StartUpdate, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.ForceUpdate, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.IgnoreHorizon, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.ObserveHorizon, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.SetHorizon, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.ObjectOnWater, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.ObjectOnLand, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.ObjectOnAnything, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.IgnoreObjects, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.ObserveObjects, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.Distance, LogicArgumentType.ScreenObject, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.StopCycling, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.StartCycling, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.NormalCycle, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.EndOfLoop, LogicArgumentType.ScreenObject, LogicArgumentType.Flag));
        list.Add(new(LogicProcedureCode.ReverseCycle, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.ReverseLoop, LogicArgumentType.ScreenObject, LogicArgumentType.Flag));
        list.Add(new(LogicProcedureCode.CycleTime, LogicArgumentType.ScreenObject, LogicArgumentType.Variable)); // argument 2 is Number
        list.Add(new(LogicProcedureCode.StopMotion, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.StartMotion, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.StepSize, LogicArgumentType.ScreenObject, LogicArgumentType.Variable)); // argument 2 is Number
        list.Add(new(LogicProcedureCode.StepTime, LogicArgumentType.ScreenObject, LogicArgumentType.Variable)); // argument 2 is Number
        list.Add(new(LogicProcedureCode.MoveObj, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Flag));
        list.Add(new(LogicProcedureCode.MoveObjV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Number, LogicArgumentType.Flag)); // second to last argument is Variable?  yes, according to nic
        list.Add(new(LogicProcedureCode.FollowEgo, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Flag));
        list.Add(new(LogicProcedureCode.Wander, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.NormalMotion, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.SetDir, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.GetDir, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.IgnoreBlocks, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.ObserveBlocks, LogicArgumentType.ScreenObject));
        list.Add(new(LogicProcedureCode.Block, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.Unblock));
        list.Add(new(LogicProcedureCode.Get, LogicArgumentType.InventoryObject));
        list.Add(new(LogicProcedureCode.GetV, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.Drop, LogicArgumentType.InventoryObject));
        list.Add(new(LogicProcedureCode.Put, LogicArgumentType.InventoryObject, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.PutV, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.GetRoomV, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.LoadSound, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.Sound, LogicArgumentType.Number, LogicArgumentType.Flag));
        list.Add(new(LogicProcedureCode.StopSound));
        list.Add(new(LogicProcedureCode.Print, LogicArgumentType.Message));
        list.Add(new(LogicProcedureCode.PrintV, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.Display, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Message));
        list.Add(new(LogicProcedureCode.DisplayV, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.ClearLines, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.TextScreen));
        list.Add(new(LogicProcedureCode.Graphics));
        list.Add(new(LogicProcedureCode.SetCursorChar, LogicArgumentType.Message));
        list.Add(new(LogicProcedureCode.SetTextAttribute, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.ShakeScreen, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.ConfigureScreen, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.StatusLineOn));
        list.Add(new(LogicProcedureCode.StatusLineOff));
        list.Add(new(LogicProcedureCode.SetString, LogicArgumentType.String, LogicArgumentType.Message));
        list.Add(new(LogicProcedureCode.GetString, LogicArgumentType.String, LogicArgumentType.Message, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.WordToString, LogicArgumentType.Word, LogicArgumentType.String));
        list.Add(new(LogicProcedureCode.Parse, LogicArgumentType.String));
        list.Add(new(LogicProcedureCode.GetNumber, LogicArgumentType.Message, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.PreventInput));
        list.Add(new(LogicProcedureCode.AcceptInput));
        list.Add(new(LogicProcedureCode.SetKey, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Control));
        list.Add(new(LogicProcedureCode.AddToPicture, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.AddToPictureV, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.Status));
        list.Add(new(LogicProcedureCode.SaveGame));
        list.Add(new(LogicProcedureCode.RestoreGame));
        list.Add(new(LogicProcedureCode.InitDisk));
        list.Add(new(LogicProcedureCode.RestartGame));
        list.Add(new(LogicProcedureCode.ShowObj, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.Random, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.ProgramControl));
        list.Add(new(LogicProcedureCode.PlayerControl));
        list.Add(new(LogicProcedureCode.ObjectStatusV, LogicArgumentType.Variable));

        if (interpreter == InterpreterVersion.V2089)
        {
            // 0 argument <= 2.08
            list.Add(new(LogicProcedureCode.Quit));
        }
        else
        {
            // 1 argument > 2.08
            list.Add(new(LogicProcedureCode.Quit, LogicArgumentType.Number));
        }

        list.Add(new(LogicProcedureCode.ShowMemory));
        list.Add(new(LogicProcedureCode.Pause));
        list.Add(new(LogicProcedureCode.EchoLine));
        list.Add(new(LogicProcedureCode.CancelLine));
        list.Add(new(LogicProcedureCode.InitJoy));
        list.Add(new(LogicProcedureCode.ToggleMonitor));
        list.Add(new(LogicProcedureCode.Version));
        list.Add(new(LogicProcedureCode.ScriptSize, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.SetGameId, LogicArgumentType.Message));
        list.Add(new(LogicProcedureCode.Log, LogicArgumentType.Message));
        list.Add(new(LogicProcedureCode.SetScanStart));
        list.Add(new(LogicProcedureCode.ResetScanStart));
        list.Add(new(LogicProcedureCode.RepositionTo, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new(LogicProcedureCode.RepositionToV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new(LogicProcedureCode.TraceOn));
        list.Add(new(LogicProcedureCode.TraceInfo, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));

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
