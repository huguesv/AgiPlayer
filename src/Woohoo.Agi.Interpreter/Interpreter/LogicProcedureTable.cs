// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

public class LogicProcedureTable
{
    private LogicCommand[] procedures;

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
        List<LogicCommand> list = new List<LogicCommand>();

        list.Add(new LogicCommand(LogicProcedureCode.ReturnFalse));
        list.Add(new LogicCommand(LogicProcedureCode.Increment, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.Decrement, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.AssignN, LogicArgumentType.Variable, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.AssignV, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.AddN, LogicArgumentType.Variable, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.AddV, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.SubN, LogicArgumentType.Variable, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.SubV, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.LIndirectV, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.RIndirect, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.LIndirectN, LogicArgumentType.Variable, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.Set, LogicArgumentType.Flag));
        list.Add(new LogicCommand(LogicProcedureCode.Reset, LogicArgumentType.Flag));
        list.Add(new LogicCommand(LogicProcedureCode.Toggle, LogicArgumentType.Flag));
        list.Add(new LogicCommand(LogicProcedureCode.SetV, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.ResetV, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.ToggleV, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.NewRoom, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.NewRoomV, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.LoadLogics, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.LoadLogicsV, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.Call, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.CallV, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.LoadPicture, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.DrawPicture, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.ShowPicture));
        list.Add(new LogicCommand(LogicProcedureCode.DiscardPicture, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.OverlayPicture, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.ShowPriScreen));
        list.Add(new LogicCommand(LogicProcedureCode.LoadView, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.LoadViewV, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.DiscardView, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.AnimateObj, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.UnanimateAll));
        list.Add(new LogicCommand(LogicProcedureCode.Draw, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.Erase, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.Position, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.PositionV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.GetPosition, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.Reposition, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.SetView, LogicArgumentType.ScreenObject, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.SetViewV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.SetLoop, LogicArgumentType.ScreenObject, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.SetLoopV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.FixLoop, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.ReleaseLoop, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.SetCel, LogicArgumentType.ScreenObject, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.SetCelV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.LastCel, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.CurrentCel, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.CurrentLoop, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.CurrentView, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.NumberOfLoops, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.SetPriority, LogicArgumentType.ScreenObject, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.SetPriorityV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.ReleasePriority, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.GetPriority, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.StopUpdate, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.StartUpdate, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.ForceUpdate, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.IgnoreHorizon, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.ObserveHorizon, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.SetHorizon, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.ObjectOnWater, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.ObjectOnLand, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.ObjectOnAnything, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.IgnoreObjects, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.ObserveObjects, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.Distance, LogicArgumentType.ScreenObject, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.StopCycling, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.StartCycling, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.NormalCycle, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.EndOfLoop, LogicArgumentType.ScreenObject, LogicArgumentType.Flag));
        list.Add(new LogicCommand(LogicProcedureCode.ReverseCycle, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.ReverseLoop, LogicArgumentType.ScreenObject, LogicArgumentType.Flag));
        list.Add(new LogicCommand(LogicProcedureCode.CycleTime, LogicArgumentType.ScreenObject, LogicArgumentType.Variable)); // argument 2 is Number
        list.Add(new LogicCommand(LogicProcedureCode.StopMotion, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.StartMotion, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.StepSize, LogicArgumentType.ScreenObject, LogicArgumentType.Variable)); // argument 2 is Number
        list.Add(new LogicCommand(LogicProcedureCode.StepTime, LogicArgumentType.ScreenObject, LogicArgumentType.Variable)); // argument 2 is Number
        list.Add(new LogicCommand(LogicProcedureCode.MoveObj, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Flag));
        list.Add(new LogicCommand(LogicProcedureCode.MoveObjV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Number, LogicArgumentType.Flag)); // second to last argument is Variable?  yes, according to nic
        list.Add(new LogicCommand(LogicProcedureCode.FollowEgo, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Flag));
        list.Add(new LogicCommand(LogicProcedureCode.Wander, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.NormalMotion, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.SetDir, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.GetDir, LogicArgumentType.ScreenObject, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.IgnoreBlocks, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.ObserveBlocks, LogicArgumentType.ScreenObject));
        list.Add(new LogicCommand(LogicProcedureCode.Block, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.Unblock));
        list.Add(new LogicCommand(LogicProcedureCode.Get, LogicArgumentType.InventoryObject));
        list.Add(new LogicCommand(LogicProcedureCode.GetV, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.Drop, LogicArgumentType.InventoryObject));
        list.Add(new LogicCommand(LogicProcedureCode.Put, LogicArgumentType.InventoryObject, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.PutV, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.GetRoomV, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.LoadSound, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.Sound, LogicArgumentType.Number, LogicArgumentType.Flag));
        list.Add(new LogicCommand(LogicProcedureCode.StopSound));
        list.Add(new LogicCommand(LogicProcedureCode.Print, LogicArgumentType.Message));
        list.Add(new LogicCommand(LogicProcedureCode.PrintV, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.Display, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Message));
        list.Add(new LogicCommand(LogicProcedureCode.DisplayV, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.ClearLines, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.TextScreen));
        list.Add(new LogicCommand(LogicProcedureCode.Graphics));
        list.Add(new LogicCommand(LogicProcedureCode.SetCursorChar, LogicArgumentType.Message));
        list.Add(new LogicCommand(LogicProcedureCode.SetTextAttribute, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.ShakeScreen, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.ConfigureScreen, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.StatusLineOn));
        list.Add(new LogicCommand(LogicProcedureCode.StatusLineOff));
        list.Add(new LogicCommand(LogicProcedureCode.SetString, LogicArgumentType.String, LogicArgumentType.Message));
        list.Add(new LogicCommand(LogicProcedureCode.GetString, LogicArgumentType.String, LogicArgumentType.Message, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.WordToString, LogicArgumentType.Word, LogicArgumentType.String));
        list.Add(new LogicCommand(LogicProcedureCode.Parse, LogicArgumentType.String));
        list.Add(new LogicCommand(LogicProcedureCode.GetNumber, LogicArgumentType.Message, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.PreventInput));
        list.Add(new LogicCommand(LogicProcedureCode.AcceptInput));
        list.Add(new LogicCommand(LogicProcedureCode.SetKey, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Control));
        list.Add(new LogicCommand(LogicProcedureCode.AddToPicture, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.AddToPictureV, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.Status));
        list.Add(new LogicCommand(LogicProcedureCode.SaveGame));
        list.Add(new LogicCommand(LogicProcedureCode.RestoreGame));
        list.Add(new LogicCommand(LogicProcedureCode.InitDisk));
        list.Add(new LogicCommand(LogicProcedureCode.RestartGame));
        list.Add(new LogicCommand(LogicProcedureCode.ShowObj, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.Random, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.ProgramControl));
        list.Add(new LogicCommand(LogicProcedureCode.PlayerControl));
        list.Add(new LogicCommand(LogicProcedureCode.ObjectStatusV, LogicArgumentType.Variable));

        if (interpreter == InterpreterVersion.V2089)
        {
            // 0 argument <= 2.08
            list.Add(new LogicCommand(LogicProcedureCode.Quit));
        }
        else
        {
            // 1 argument > 2.08
            list.Add(new LogicCommand(LogicProcedureCode.Quit, LogicArgumentType.Number));
        }

        list.Add(new LogicCommand(LogicProcedureCode.ShowMemory));
        list.Add(new LogicCommand(LogicProcedureCode.Pause));
        list.Add(new LogicCommand(LogicProcedureCode.EchoLine));
        list.Add(new LogicCommand(LogicProcedureCode.CancelLine));
        list.Add(new LogicCommand(LogicProcedureCode.InitJoy));
        list.Add(new LogicCommand(LogicProcedureCode.ToggleMonitor));
        list.Add(new LogicCommand(LogicProcedureCode.Version));
        list.Add(new LogicCommand(LogicProcedureCode.ScriptSize, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.SetGameId, LogicArgumentType.Message));
        list.Add(new LogicCommand(LogicProcedureCode.Log, LogicArgumentType.Message));
        list.Add(new LogicCommand(LogicProcedureCode.SetScanStart));
        list.Add(new LogicCommand(LogicProcedureCode.ResetScanStart));
        list.Add(new LogicCommand(LogicProcedureCode.RepositionTo, LogicArgumentType.ScreenObject, LogicArgumentType.Number, LogicArgumentType.Number));
        list.Add(new LogicCommand(LogicProcedureCode.RepositionToV, LogicArgumentType.ScreenObject, LogicArgumentType.Variable, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.TraceOn));
        list.Add(new LogicCommand(LogicProcedureCode.TraceInfo, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));

        if (interpreter < InterpreterVersion.V2411)
        {
            // 3 arguments <= 2.4
            list.Add(new LogicCommand(LogicProcedureCode.PrintAt, LogicArgumentType.Message, LogicArgumentType.Number, LogicArgumentType.Number));
            list.Add(new LogicCommand(LogicProcedureCode.PrintAtV, LogicArgumentType.Message, LogicArgumentType.Variable, LogicArgumentType.Variable));
        }
        else
        {
            // 4 arguments > 2.4
            list.Add(new LogicCommand(LogicProcedureCode.PrintAt, LogicArgumentType.Message, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));
            list.Add(new LogicCommand(LogicProcedureCode.PrintAtV, LogicArgumentType.Message, LogicArgumentType.Variable, LogicArgumentType.Variable, LogicArgumentType.Variable));
        }

        list.Add(new LogicCommand(LogicProcedureCode.DiscardViewV, LogicArgumentType.Variable));
        list.Add(new LogicCommand(LogicProcedureCode.ClearTextRectangle, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));

        if (interpreter >= InterpreterVersion.V2272)
        {
            list.Add(new LogicCommand(LogicProcedureCode.SetUpperLeft, LogicArgumentType.Number, LogicArgumentType.Number)); // arguments unknown
            list.Add(new LogicCommand(LogicProcedureCode.SetMenu, LogicArgumentType.Message));
            list.Add(new LogicCommand(LogicProcedureCode.SetMenuItem, LogicArgumentType.Message, LogicArgumentType.Control));
            list.Add(new LogicCommand(LogicProcedureCode.SubmitMenu));
            list.Add(new LogicCommand(LogicProcedureCode.EnableItem, LogicArgumentType.Control));
            list.Add(new LogicCommand(LogicProcedureCode.DisableItem, LogicArgumentType.Control));
            list.Add(new LogicCommand(LogicProcedureCode.MenuInput));

            if (interpreter >= InterpreterVersion.V2411)
            {
                list.Add(new LogicCommand(LogicProcedureCode.ShowObjV, LogicArgumentType.Variable));
                list.Add(new LogicCommand(LogicProcedureCode.OpenDialogue));
                list.Add(new LogicCommand(LogicProcedureCode.CloseDialogue));
                list.Add(new LogicCommand(LogicProcedureCode.MulN, LogicArgumentType.Variable, LogicArgumentType.Number));
                list.Add(new LogicCommand(LogicProcedureCode.MulV, LogicArgumentType.Variable, LogicArgumentType.Variable));
                list.Add(new LogicCommand(LogicProcedureCode.DivN, LogicArgumentType.Variable, LogicArgumentType.Number));
                list.Add(new LogicCommand(LogicProcedureCode.DivV, LogicArgumentType.Variable, LogicArgumentType.Variable));
                list.Add(new LogicCommand(LogicProcedureCode.CloseWindow));
                list.Add(new LogicCommand(LogicProcedureCode.SetSimple, LogicArgumentType.Number));
                list.Add(new LogicCommand(LogicProcedureCode.PushScript));
                list.Add(new LogicCommand(LogicProcedureCode.PopScript));
                list.Add(new LogicCommand(LogicProcedureCode.HoldKey));

                if (interpreter >= InterpreterVersion.V2936)
                {
                    list.Add(new LogicCommand(LogicProcedureCode.SetPriBase, LogicArgumentType.Number));
                    list.Add(new LogicCommand(LogicProcedureCode.DiscardSound, LogicArgumentType.Number));

                    if (interpreter >= InterpreterVersion.V3002086)
                    {
                        if (interpreter == InterpreterVersion.V3002086)
                        {
                            // TODO: what is the parameter type?
                            // 1 argument for 3.002.08
                            list.Add(new LogicCommand(LogicProcedureCode.HideMouse));
                        }
                        else
                        {
                            // 0 argument for others
                            list.Add(new LogicCommand(LogicProcedureCode.HideMouse));
                        }

                        list.Add(new LogicCommand(LogicProcedureCode.AllowMenu, LogicArgumentType.Number));

                        if (interpreter >= InterpreterVersion.V3002098)
                        {
                            list.Add(new LogicCommand(LogicProcedureCode.ShowMouse));
                            list.Add(new LogicCommand(LogicProcedureCode.FenceMouse, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number, LogicArgumentType.Number));
                            list.Add(new LogicCommand(LogicProcedureCode.MousePosN, LogicArgumentType.Number, LogicArgumentType.Number));
                            list.Add(new LogicCommand(LogicProcedureCode.ReleaseKey));
                            list.Add(new LogicCommand(LogicProcedureCode.AdjEgoMoveToXY));
                        }
                    }
                }
            }
        }

        return list.ToArray();
    }
}
