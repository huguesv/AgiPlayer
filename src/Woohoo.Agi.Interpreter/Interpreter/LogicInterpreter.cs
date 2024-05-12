// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

using Woohoo.Agi.Resources;

/// <summary>
/// Error handler.
/// </summary>
/// <param name="a">Error code.</param>
/// <param name="b">Additional error code.</param>
public delegate void AgiError(int a, int b);

/// <summary>
/// Function tracing.
/// </summary>
/// <param name="result">Function result.</param>
/// <param name="opIndex">Byte code index.</param>
public delegate void TraceFunction(bool result, int opIndex);

/// <summary>
/// Procedure tracing.
/// </summary>
/// <param name="op">Procedure code.</param>
/// <param name="opIndex">Byte code index.</param>
public delegate void TraceProcedure(byte op, int opIndex);

internal delegate bool FunctionDelegate();

internal delegate void ProcedureDelegate();

/// <summary>
/// Logic byte code interpreter.
/// </summary>
public class LogicInterpreter
{
    private readonly InterpreterVersion interpreter;
    private readonly MouseMode mouseMode;
    private readonly LogicFunctionTable functionTable;
    private readonly FunctionDelegate[] functions;
    private readonly ProcedureDelegate[] procedures;

    public LogicInterpreter(IKernel kernel, InterpreterVersion interpreter, MouseMode mouseMode, AgiError error, TraceFunction traceFunction, TraceProcedure traceProcedure)
    {
        this.Kernel = kernel;
        this.interpreter = interpreter;
        this.mouseMode = mouseMode;
        this.Error = error;
        this.TraceFunction = traceFunction;
        this.TraceProcedure = traceProcedure;
        this.functionTable = new LogicFunctionTable();

        this.functions =
        [
            this.LogicReturnFalse,
            this.LogicEqualN,
            this.LogicEqualV,
            this.LogicLessN,
            this.LogicLessV,
            this.LogicGreaterN,
            this.LogicGreaterV,
            this.LogicIsSet,
            this.LogicIsSetV,
            this.LogicHas,
            this.LogicObjInRoom,
            this.LogicPosN,
            this.LogicController,
            this.LogicHaveKey,
            this.LogicSaid,
            this.LogicCompareStrings,
            this.LogicObjInBox,
            this.LogicCenterPosN,
            this.LogicRightPosN,
            this.LogicReturnFalse,
        ];

        this.procedures =
        [
            this.LogicNoOp,
            this.LogicIncrement,
            this.LogicDecrement,
            this.LogicAssignN,
            this.LogicAssignV,
            this.LogicAddN,
            this.LogicAddV,
            this.LogicSubN,
            this.LogicSubV,
            this.LogicLIndirectV,
            this.LogicRIndirect,
            this.LogicLIndirectN,
            this.LogicSet,
            this.LogicReset,
            this.LogicToggle,
            this.LogicSetV,
            this.LogicResetV,
            this.LogicToggleV,
            this.LogicNewRoom,
            this.LogicNewRoomV,
            this.LogicLoadLogics,
            this.LogicLoadLogicsV,
            this.LogicCall,
            this.LogicCallV,
            this.LogicLoadPicture,
            this.LogicDrawPicture,
            this.LogicShowPicture,
            this.LogicDiscardPicture,
            this.LogicOverlayPicture,
            this.LogicShowPriScreen,
            this.LogicLoadView,
            this.LogicLoadViewV,
            this.LogicDiscardView,
            this.LogicAnimateObj,
            this.LogicUnanimateAll,
            this.LogicDraw,
            this.LogicErase,
            this.LogicPosition,
            this.LogicPositionV,
            this.LogicGetPosition,
            this.LogicReposition,
            this.LogicSetView,
            this.LogicSetViewV,
            this.LogicSetLoop,
            this.LogicSetLoopV,
            this.LogicFixLoop,
            this.LogicReleaseLoop,
            this.LogicSetCel,
            this.LogicSetCelV,
            this.LogicLastCel,
            this.LogicCurrentCel,
            this.LogicCurrentLoop,
            this.LogicCurrentView,
            this.LogicNumberOfLoops,
            this.LogicSetPriority,
            this.LogicSetPriorityV,
            this.LogicReleasePriority,
            this.LogicGetPriority,
            this.LogicStopUpdate,
            this.LogicStartUpdate,
            this.LogicForceUpdate,
            this.LogicIgnoreHorizon,
            this.LogicObserveHorizon,
            this.LogicSetHorizon,
            this.LogicObjectOnWater,
            this.LogicObjectOnLand,
            this.LogicObjectOnAnything,
            this.LogicIgnoreObjects,
            this.LogicObserveObjects,
            this.LogicDistance,
            this.LogicStopCycling,
            this.LogicStartCycling,
            this.LogicNormalCycle,
            this.LogicEndOfLoop,
            this.LogicReverseCycle,
            this.LogicReverseLoop,
            this.LogicCycleTime,
            this.LogicStopMotion,
            this.LogicStartMotion,
            this.LogicStepSize,
            this.LogicStepTime,
            this.LogicMoveObj,
            this.LogicMoveObjV,
            this.LogicFollowEgo,
            this.LogicWander,
            this.LogicNormalMotion,
            this.LogicSetDir,
            this.LogicGetDir,
            this.LogicIgnoreBlocks,
            this.LogicObserveBlocks,
            this.LogicBlock,
            this.LogicUnblock,
            this.LogicGet,
            this.LogicGetV,
            this.LogicDrop,
            this.LogicPut,
            this.LogicPutV,
            this.LogicGetRoomV,
            this.LogicLoadSound,
            this.LogicSound,
            this.LogicStopSound,
            this.LogicPrint,
            this.LogicPrintV,
            this.LogicDisplay,
            this.LogicDisplayV,
            this.LogicClearLines,
            this.LogicTextScreen,
            this.LogicGraphics,
            this.LogicSetCursorChar,
            this.LogicSetTextAttribute,
            this.LogicShakeScreen,
            this.LogicConfigureScreen,
            this.LogicStatusLineOn,
            this.LogicStatusLineOff,
            this.LogicSetString,
            this.LogicGetString,
            this.LogicWordToString,
            this.LogicParse,
            this.LogicGetNumber,
            this.LogicPreventInput,
            this.LogicAcceptInput,
            this.LogicSetKey,
            this.LogicAddToPicture,
            this.LogicAddToPictureV,
            this.LogicStatus,
            this.LogicSaveGame,
            this.LogicRestoreGame,
            this.LogicInitDisk,
            this.LogicRestartGame,
            this.LogicShowObj,
            this.LogicRandom,
            this.LogicProgramControl,
            this.LogicPlayerControl,
            this.LogicObjectStatusV,
            this.LogicQuit,
            this.LogicShowMemory,
            this.LogicPause,
            this.LogicEchoLine,
            this.LogicCancelLine,
            this.LogicInitJoy,
            this.LogicToggleMonitor,
            this.LogicVersion,
            this.LogicScriptSize,
            this.LogicSetGameId,
            this.LogicLog,
            this.LogicSetScanStart,
            this.LogicResetScanStart,
            this.LogicRepositionTo,
            this.LogicRepositionToV,
            this.LogicTraceOn,
            this.LogicTraceInfo,
            this.LogicPrintAt,
            this.LogicPrintAtV,
            this.LogicDiscardViewV,
            this.LogicClearTextRectangle,
            this.LogicSetUpperLeft,
            this.LogicSetMenu,
            this.LogicSetMenuItem,
            this.LogicSubmitMenu,
            this.LogicEnableItem,
            this.LogicDisableItem,
            this.LogicMenuInput,
            this.LogicShowObjV,
            this.LogicOpenDialogue,
            this.LogicCloseDialogue,
            this.LogicMulN,
            this.LogicMulV,
            this.LogicDivN,
            this.LogicDivV,
            this.LogicCloseWindow,
            this.LogicSetSimple,
            this.LogicPushScript,
            this.LogicPopScript,
            this.LogicHoldKey,
            this.LogicSetPriBase,
            this.LogicDiscardSound,
            this.LogicHideMouse,
            this.LogicAllowMenu,
            this.LogicShowMouse,
            this.LogicFenceMouse,
            this.LogicMousePosN,
            this.LogicReleaseKey,
            this.LogicAdjEgoMoveToXY,
        ];
    }

    /// <summary>
    /// Gets or sets currently executing logic resource.
    /// </summary>
    public LogicResource CurrentLogic { get; set; }

    /// <summary>
    /// Gets or sets index in byte code of currently executing logic resource.
    /// </summary>
    public int CurrentLogicDataIndex { get; set; }

    /// <summary>
    /// Gets kernel implementation.
    /// </summary>
    protected IKernel Kernel { get; }

    /// <summary>
    /// Gets error handler.
    /// </summary>
    protected AgiError Error { get; }

    /// <summary>
    /// Gets function tracing.
    /// </summary>
    protected TraceFunction TraceFunction { get; }

    /// <summary>
    /// Gets procedure tracing.
    /// </summary>
    protected TraceProcedure TraceProcedure { get; }

    /// <summary>
    /// Execute the specified logic resource.
    /// </summary>
    /// <param name="resource">Logic to execute.</param>
    /// <returns>true if logic execution must be restarted, false if not.</returns>
    public bool Execute(LogicResource resource)
    {
        var previousLogic = this.CurrentLogic;
        this.CurrentLogic = resource ?? throw new ArgumentNullException(nameof(resource));
        this.CurrentLogicDataIndex = resource.ScanStart;

        byte op = resource.GetCode(this.CurrentLogicDataIndex++);
        while (op != 0)
        {
            if (op == LogicControlCode.If)
            {
                this.ExecuteIf();
                op = resource.GetCode(this.CurrentLogicDataIndex++);
            }
            else if (op == LogicControlCode.Else)
            {
                this.CurrentLogicDataIndex += resource.GetCodeLE16(this.CurrentLogicDataIndex) + 2;
                op = resource.GetCode(this.CurrentLogicDataIndex++);
            }
            else
            {
                op = this.ExecuteProcedure(op);
                if (this.CurrentLogicDataIndex == -1)
                {
                    break;
                }
            }
        }

        this.CurrentLogic = previousLogic;

        return this.CurrentLogicDataIndex == -1;
    }

    private byte ExecuteProcedure(byte op)
    {
        while (op < LogicControlCode.Or && op != 0)
        {
            if (op > LogicProcedureCode.Last)
            {
                // TODO: this shouldn't be part of logic execution
                this.Error(0x10, op);
            }

            this.TraceProcedure(op, this.CurrentLogicDataIndex);

            if (op < this.procedures.Length)
            {
                this.procedures[op]();
            }

            if (this.CurrentLogicDataIndex == -1)
            {
                break;
            }

            op = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        }

        return op;
    }

    private bool ExecuteFunction(byte op)
    {
        int operatorIndex = this.CurrentLogicDataIndex - 1;
        if (op > LogicFunctionCode.Last)
        {
            this.Error(0xf, op);
        }

        bool result = false;

        if (op < this.functions.Length)
        {
            result = this.functions[op]();
        }

        this.TraceFunction(result, operatorIndex);

        return result;
    }

    private void ExecuteIf()
    {
        byte notMode = 0;
        byte orMode = 0;

        while (true)
        {
            byte op = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
            if (op == LogicControlCode.Or)
            {
                if (orMode != 0)
                {
                    this.SkipFalseAnd();
                    break;
                }

                orMode++;
            }
            else if (op == LogicControlCode.If)
            {
                // skip the word offset
                this.CurrentLogicDataIndex += 2;
                break;
            }
            else if (op == LogicControlCode.Not)
            {
                notMode = (byte)(notMode ^ 1);
            }
            else
            {
                // this is how the original code works
                // but doesn't look for 0xFE so it will cause
                // an agi_error here
                byte result = this.ExecuteFunction(op) ? (byte)1 : (byte)0;
                result = (byte)(result ^ notMode);
                notMode = 0;
                if (result != 0)
                {
                    if (orMode != 0)
                    {
                        orMode = 0;
                        this.SkipTrueOr();
                    }
                }
                else if (orMode == 0)
                {
                    this.SkipFalseAnd();
                    break;
                }
            }
        }
    }

    private void SkipTrueOr()
    {
        byte op = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        while (op != LogicControlCode.Or)
        {
            if (op <= LogicControlCode.Or)
            {
                this.SkipFunction(op);
            }

            op = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        }
    }

    private void SkipFalseAnd()
    {
        byte op = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        while (op != LogicControlCode.If)
        {
            if (op < LogicControlCode.Or)
            {
                this.SkipFunction(op);
            }

            op = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        }

        this.CurrentLogicDataIndex += this.CurrentLogic.GetCodeLE16(this.CurrentLogicDataIndex) + 2;
    }

    private void SkipFunction(byte op)
    {
        if (op == LogicFunctionCode.Said)
        {
            int paramCount = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
            this.CurrentLogicDataIndex += paramCount * 2;
        }
        else
        {
            LogicCommand cmd = this.functionTable.GetAt(op);
            for (int param = 0; param < cmd.ParameterCount; param++)
            {
                this.CurrentLogicDataIndex += LogicCommandTable.GetArgumentTypeLength(cmd.GetParameterType(param));
            }
        }
    }

    private bool LogicReturnFalse()
    {
        return false;
    }

    private bool LogicEqualN()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.EqualN(a, b);
    }

    private bool LogicEqualV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.EqualV(a, b);
    }

    private bool LogicLessN()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.LessN(a, b);
    }

    private bool LogicLessV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.LessV(a, b);
    }

    private bool LogicGreaterN()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.GreaterN(a, b);
    }

    private bool LogicGreaterV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.GreaterV(a, b);
    }

    private bool LogicIsSet()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.IsSet(a);
    }

    private bool LogicIsSetV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.IsSetV(a);
    }

    private bool LogicHas()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.Has(a);
    }

    private bool LogicPosN()
    {
        byte v = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte c = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte d = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.PosN(v, a, b, c, d);
    }

    private bool LogicCenterPosN()
    {
        byte v = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte c = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte d = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.CenterPosN(v, a, b, c, d);
    }

    private bool LogicRightPosN()
    {
        byte v = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte c = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte d = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.RightPosN(v, a, b, c, d);
    }

    private bool LogicObjInBox()
    {
        byte v = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte c = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte d = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.ObjInBox(v, a, b, c, d);
    }

    private bool LogicController()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.Controller(a);
    }

    private bool LogicObjInRoom()
    {
        byte o = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.ObjInRoom(o, a);
    }

    private bool LogicSaid()
    {
        byte count = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        int[] ids = new int[count];
        for (int i = 0; i < count; i++)
        {
            ids[i] = this.CurrentLogic.GetCodeLE16(this.CurrentLogicDataIndex);
            this.CurrentLogicDataIndex += 2;
        }

        return this.Kernel.Said(ids);
    }

    private bool LogicHaveKey()
    {
        return this.Kernel.HaveKey();
    }

    private bool LogicCompareStrings()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        return this.Kernel.CompareStrings(a, b);
    }

    private void LogicNoOp()
    {
    }

    private void LogicIncrement()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Increment(a);
    }

    private void LogicDecrement()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Decrement(a);
    }

    private void LogicAssignN()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.AssignN(a, b);
    }

    private void LogicAssignV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.AssignV(a, b);
    }

    private void LogicAddN()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.AddN(a, b);
    }

    private void LogicAddV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.AddV(a, b);
    }

    private void LogicSubN()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SubN(a, b);
    }

    private void LogicSubV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SubV(a, b);
    }

    private void LogicLIndirectV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.LIndirectV(a, b);
    }

    private void LogicRIndirect()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.RIndirect(a, b);
    }

    private void LogicLIndirectN()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.LIndirectN(a, b);
    }

    private void LogicSet()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Set(a);
    }

    private void LogicReset()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Reset(a);
    }

    private void LogicToggle()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Toggle(a);
    }

    private void LogicSetV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetV(a);
    }

    private void LogicResetV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ResetV(a);
    }

    private void LogicToggleV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ToggleV(a);
    }

    private void LogicNewRoom()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.NewRoom(a);

        this.CurrentLogicDataIndex = -1;
    }

    private void LogicNewRoomV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.NewRoomV(a);

        this.CurrentLogicDataIndex = -1;
    }

    private void LogicLoadLogics()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.LoadLogics(a);
    }

    private void LogicLoadLogicsV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.LoadLogicsV(a);
    }

    private void LogicCall()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Call(a);
    }

    private void LogicCallV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.CallV(a);
    }

    private void LogicLoadPicture()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.LoadPicture(a);
    }

    private void LogicDrawPicture()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.DrawPicture(a);
    }

    private void LogicShowPicture()
    {
        this.Kernel.ShowPicture();
    }

    private void LogicDiscardPicture()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.DiscardPicture(a);
    }

    private void LogicOverlayPicture()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.OverlayPicture(a);
    }

    private void LogicShowPriScreen()
    {
        this.Kernel.ShowPriScreen();
    }

    private void LogicLoadView()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.LoadView(a);
    }

    private void LogicLoadViewV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.LoadViewV(a);
    }

    private void LogicDiscardView()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.DiscardView(a);
    }

    private void LogicAnimateObj()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.AnimateObj(a);
    }

    private void LogicUnanimateAll()
    {
        this.Kernel.UnanimateAll();
    }

    private void LogicDraw()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Draw(a);
    }

    private void LogicErase()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Erase(a);
    }

    private void LogicPosition()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte c = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Position(a, b, c);
    }

    private void LogicPositionV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte c = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.PositionV(a, b, c);
    }

    private void LogicGetPosition()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte c = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.GetPosition(a, b, c);
    }

    private void LogicReposition()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte varX = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte varY = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Reposition(a, varX, varY);
    }

    private void LogicSetView()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetView(a, b);
    }

    private void LogicSetViewV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetViewV(a, b);
    }

    private void LogicSetLoop()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetLoop(a, b);
    }

    private void LogicSetLoopV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetLoopV(a, b);
    }

    private void LogicFixLoop()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.FixLoop(a);
    }

    private void LogicReleaseLoop()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ReleaseLoop(a);
    }

    private void LogicSetCel()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetCel(a, b);
    }

    private void LogicSetCelV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetCelV(a, b);
    }

    private void LogicLastCel()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.LastCel(a, b);
    }

    private void LogicCurrentCel()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.CurrentCel(a, b);
    }

    private void LogicCurrentLoop()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.CurrentLoop(a, b);
    }

    private void LogicCurrentView()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.CurrentView(a, b);
    }

    private void LogicNumberOfLoops()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.NumberOfLoops(a, b);
    }

    private void LogicSetPriority()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetPriority(a, b);
    }

    private void LogicSetPriorityV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetPriorityV(a, b);
    }

    private void LogicReleasePriority()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ReleasePriority(a);
    }

    private void LogicGetPriority()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.GetPriority(a, b);
    }

    private void LogicStopUpdate()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.StopUpdate(a);
    }

    private void LogicStartUpdate()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.StartUpdate(a);
    }

    private void LogicForceUpdate()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ForceUpdate(a);
    }

    private void LogicIgnoreHorizon()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.IgnoreHorizon(a);
    }

    private void LogicObserveHorizon()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ObserveHorizon(a);
    }

    private void LogicSetHorizon()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetHorizon(a);
    }

    private void LogicObjectOnWater()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ObjectOnWater(a);
    }

    private void LogicObjectOnLand()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ObjectOnLand(a);
    }

    private void LogicObjectOnAnything()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ObjectOnAnything(a);
    }

    private void LogicIgnoreObjects()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.IgnoreObjects(a);
    }

    private void LogicObserveObjects()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ObserveObjects(a);
    }

    private void LogicDistance()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte v = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Distance(a, b, v);
    }

    private void LogicStopCycling()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.StopCycling(a);
    }

    private void LogicStartCycling()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.StartCycling(a);
    }

    private void LogicNormalCycle()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.NormalCycle(a);
    }

    private void LogicEndOfLoop()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.EndOfLoop(a, b);
    }

    private void LogicReverseCycle()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ReverseCycle(a);
    }

    private void LogicReverseLoop()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ReverseLoop(a, b);
    }

    private void LogicCycleTime()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.CycleTime(a, b);
    }

    private void LogicStopMotion()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.StopMotion(a);
    }

    private void LogicStartMotion()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.StartMotion(a);
    }

    private void LogicStepSize()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.StepSize(a, b);
    }

    private void LogicStepTime()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.StepTime(a, b);
    }

    private void LogicMoveObj()
    {
        byte v = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte x = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte y = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte stepSize = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte moveFlag = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.MoveObj(v, x, y, stepSize, moveFlag);
    }

    private void LogicMoveObjV()
    {
        byte v = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte x = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte y = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte stepSize = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte moveFlag = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.MoveObjV(v, x, y, stepSize, moveFlag);
    }

    private void LogicFollowEgo()
    {
        byte v = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte stepSize = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte followFlag = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.FollowEgo(v, stepSize, followFlag);
    }

    private void LogicWander()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Wander(a);
    }

    private void LogicNormalMotion()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.NormalMotion(a);
    }

    private void LogicSetDir()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetDir(a, b);
    }

    private void LogicGetDir()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.GetDir(a, b);
    }

    private void LogicIgnoreBlocks()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.IgnoreBlocks(a);
    }

    private void LogicObserveBlocks()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ObserveBlocks(a);
    }

    private void LogicBlock()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte c = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte d = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Block(a, b, c, d);
    }

    private void LogicUnblock()
    {
        this.Kernel.Unblock();
    }

    private void LogicGet()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Get(a);
    }

    private void LogicGetV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.GetV(a);
    }

    private void LogicDrop()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Drop(a);
    }

    private void LogicPut()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Put(a, b);
    }

    private void LogicPutV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.PutV(a, b);
    }

    private void LogicGetRoomV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.GetRoomV(a, b);
    }

    private void LogicLoadSound()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.LoadSound(a);
    }

    private void LogicSound()
    {
        byte index = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte flag = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Sound(index, flag);
    }

    private void LogicStopSound()
    {
        this.Kernel.StopSound();
    }

    private void LogicPrint()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Print(a);
    }

    private void LogicPrintV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.PrintV(a);
    }

    private void LogicDisplay()
    {
        byte row = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte column = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte msg = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Display(row, column, msg);
    }

    private void LogicDisplayV()
    {
        byte row = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte column = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte msg = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.DisplayV(row, column, msg);
    }

    private void LogicClearLines()
    {
        byte upper = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte lower = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte attr = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ClearLines(upper, lower, attr);
    }

    private void LogicTextScreen()
    {
        this.Kernel.TextScreen();
    }

    private void LogicGraphics()
    {
        this.Kernel.Graphics();
    }

    private void LogicSetCursorChar()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetCursorChar(a);
    }

    private void LogicSetTextAttribute()
    {
        byte fg = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte bg = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetTextAttribute(fg, bg);
    }

    private void LogicShakeScreen()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ShakeScreen(a);
    }

    private void LogicConfigureScreen()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte c = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ConfigureScreen(a, b, c);
    }

    private void LogicStatusLineOn()
    {
        this.Kernel.StatusLineOn();
    }

    private void LogicStatusLineOff()
    {
        this.Kernel.StatusLineOff();
    }

    private void LogicSetString()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetString(a, b);
    }

    private void LogicGetString()
    {
        byte s = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte m = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte row = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte column = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte l = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.GetString(s, m, row, column, l);
    }

    private void LogicWordToString()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.WordToString(a, b);
    }

    private void LogicParse()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Parse(a);
    }

    private void LogicGetNumber()
    {
        byte m = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte v = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.GetNumber(m, v);
    }

    private void LogicPreventInput()
    {
        this.Kernel.PreventInput();
    }

    private void LogicAcceptInput()
    {
        this.Kernel.AcceptInput();
    }

    private void LogicSetKey()
    {
        int key = this.CurrentLogic.GetCodeLE16(this.CurrentLogicDataIndex);
        this.CurrentLogicDataIndex += 2;
        byte num = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetKey(key, num);
    }

    private void LogicAddToPicture()
    {
        byte num = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte loop = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte cel = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte x = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte y = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte pri1 = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte pri2 = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.AddToPicture(num, loop, cel, x, y, pri1, pri2);
    }

    private void LogicAddToPictureV()
    {
        byte num = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte loop = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte cel = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte x = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte y = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte pri1 = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte pri2 = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.AddToPictureV(num, loop, cel, x, y, pri1, pri2);
    }

    private void LogicStatus()
    {
        this.Kernel.Status();
    }

    private void LogicSaveGame()
    {
        this.Kernel.SaveGame();
    }

    private void LogicRestoreGame()
    {
        this.Kernel.RestoreGame();
    }

    private void LogicInitDisk()
    {
        this.Kernel.InitDisk();
    }

    private void LogicRestartGame()
    {
        this.Kernel.RestartGame();
    }

    private void LogicShowObj()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ShowObj(a);
    }

    private void LogicRandom()
    {
        byte min = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte max = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte var = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Random(min, max, var);
    }

    private void LogicProgramControl()
    {
        this.Kernel.ProgramControl();
    }

    private void LogicPlayerControl()
    {
        this.Kernel.PlayerControl();
    }

    private void LogicObjectStatusV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ObjectStatusV(a);
    }

    private void LogicQuit()
    {
        byte immediate = 1;

        if (this.interpreter > InterpreterVersion.V2089)
        {
            immediate = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        }

        this.Kernel.Quit(immediate);
    }

    private void LogicShowMemory()
    {
        this.Kernel.ShowMemory();
    }

    private void LogicPause()
    {
        this.Kernel.Pause();
    }

    private void LogicEchoLine()
    {
        this.Kernel.EchoLine();
    }

    private void LogicCancelLine()
    {
        this.Kernel.CancelLine();
    }

    private void LogicInitJoy()
    {
        this.Kernel.InitJoy();
    }

    private void LogicToggleMonitor()
    {
        this.Kernel.ToggleMonitor();
    }

    private void LogicVersion()
    {
        this.Kernel.Version();
    }

    private void LogicScriptSize()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ScriptSize(a);
    }

    private void LogicSetGameId()
    {
        byte msg = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetGameId(msg);
    }

    private void LogicLog()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.Log(a);
    }

    private void LogicSetScanStart()
    {
        this.Kernel.SetScanStart();
    }

    private void LogicResetScanStart()
    {
        this.Kernel.ResetScanStart();
    }

    private void LogicRepositionTo()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte x = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte y = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.RepositionTo(a, x, y);
    }

    private void LogicRepositionToV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte x = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte y = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.RepositionToV(a, x, y);
    }

    private void LogicTraceOn()
    {
        this.Kernel.TraceOn();
    }

    private void LogicTraceInfo()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte c = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.TraceInfo(a, b, c);
    }

    private void LogicPrintAt()
    {
        Debug.Assert(this.interpreter >= InterpreterVersion.V2411, "print.at does not take 4 params on this interpreter.");

        byte msg = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte row = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte column = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte width = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.PrintAt(msg, row, column, width);
    }

    private void LogicPrintAtV()
    {
        Debug.Assert(this.interpreter >= InterpreterVersion.V2411, "print.at.v does not take 4 params on this interpreter.");

        byte msg = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte row = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte column = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte width = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.PrintAtV(msg, row, column, width);
    }

    private void LogicDiscardViewV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.DiscardViewV(a);
    }

    private void LogicClearTextRectangle()
    {
        byte upperRow = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte upperColum = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte lowerRow = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte lowerColumn = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte attr = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ClearTextRectangle(upperRow, upperColum, lowerRow, lowerColumn, attr);
    }

    private void LogicSetUpperLeft()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetUpperLeft(a, b);
    }

    private void LogicSetMenu()
    {
        byte msg = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetMenu(msg);
    }

    private void LogicSetMenuItem()
    {
        byte msg = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte controller = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetMenuItem(msg, controller);
    }

    private void LogicSubmitMenu()
    {
        this.Kernel.SubmitMenu();
    }

    private void LogicEnableItem()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.EnableItem(a);
    }

    private void LogicDisableItem()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.DisableItem(a);
    }

    private void LogicMenuInput()
    {
        this.Kernel.MenuInput();
    }

    private void LogicShowObjV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.ShowObjV(a);
    }

    private void LogicOpenDialogue()
    {
        this.Kernel.OpenDialogue();
    }

    private void LogicCloseDialogue()
    {
        this.Kernel.CloseDialogue();
    }

    private void LogicMulN()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.MulN(a, b);
    }

    private void LogicMulV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.MulV(a, b);
    }

    private void LogicDivN()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.DivN(a, b);
    }

    private void LogicDivV()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.DivV(a, b);
    }

    private void LogicCloseWindow()
    {
        this.Kernel.CloseWindow();
    }

    private void LogicSetSimple()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetSimple(a);
    }

    private void LogicPushScript()
    {
        if (this.mouseMode == MouseMode.Brian)
        {
            this.Kernel.PollMouse();
        }
        else
        {
            this.Kernel.PushScript();
        }
    }

    private void LogicPopScript()
    {
        this.Kernel.PopScript();
    }

    private void LogicHoldKey()
    {
        this.Kernel.HoldKey();
    }

    private void LogicSetPriBase()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.SetPriBase(a);
    }

    private void LogicDiscardSound()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.DiscardSound(a);
    }

    private void LogicHideMouse()
    {
        this.Kernel.HideMouse();
    }

    private void LogicAllowMenu()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.AllowMenu(a);
    }

    private void LogicShowMouse()
    {
        this.Kernel.ShowMouse();
    }

    private void LogicFenceMouse()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte c = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte d = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.FenceMouse(a, b, c, d);
    }

    private void LogicMousePosN()
    {
        byte a = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);
        byte b = this.CurrentLogic.GetCode(this.CurrentLogicDataIndex++);

        this.Kernel.MousePosN(a, b);
    }

    private void LogicReleaseKey()
    {
        this.Kernel.ReleaseKey();
    }

    private void LogicAdjEgoMoveToXY()
    {
        this.Kernel.AdjEgoMoveToXY();
    }
}
