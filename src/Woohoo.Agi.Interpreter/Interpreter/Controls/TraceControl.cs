// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.Controls;

using Woohoo.Agi.Resources;

public class TraceControl
{
    public const int CharacterHeight = 8;
    public const int CharacterWidth = 4;

    public TraceControl(AgiInterpreter interpreter)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
        this.TraceState = TraceState.Uninitialized;
    }

    public LogicInterpreter LogicInterpreter => this.Interpreter.LogicInterpreter;

    public ResourceManager ResourceManager => this.Interpreter.ResourceManager;

    public TraceState TraceState { get; set; }

    public byte TraceLogicIndex { get; set; }

    public bool Logic0Called { get; set; }

    public byte TraceTopGiven { get; set; }

    public byte TraceHeight { get; set; }

    public byte TraceTop { get; set; }

    public byte TraceLeft { get; set; }

    public byte TraceBottom { get; set; }

    public byte TraceRight { get; set; }

    public byte TraceWindowX { get; set; }

    public byte TraceWindowY { get; set; }

    public byte TraceWindowWidth { get; set; }

    public byte TraceWindowHeight { get; set; }

    protected AgiInterpreter Interpreter { get; }

    protected State State => this.Interpreter.State;

    protected WindowManager WindowManager => this.Interpreter.WindowManager;

    protected GraphicsRenderer GraphicsRenderer => this.Interpreter.GraphicsRenderer;

    protected IInputDriver InputDriver => this.Interpreter.InputDriver;

    public void Show()
    {
        this.TraceState = TraceState.Initialized;
        this.TraceTop = (byte)(this.State.WindowRowMin + this.TraceTopGiven + 1);
        this.TraceBottom = (byte)(this.TraceHeight + this.TraceTop - 1);
        this.TraceLeft = 2;
        this.TraceRight = (byte)(this.TraceLeft + 0x23);
        this.TraceWindowX = (byte)((this.TraceLeft * CharacterWidth) - 5);
        this.TraceWindowY = (byte)((this.TraceBottom * CharacterHeight) + 5);
        this.TraceWindowHeight = (byte)((this.TraceHeight * CharacterHeight) + 10);
        this.TraceWindowWidth = 154;
        this.WindowManager.DisplayMessageBoxWindow(new PictureRectangle(this.TraceWindowX, this.TraceWindowY, this.TraceWindowWidth, this.TraceWindowHeight), 0x0f, 0x04);
    }

    public void Clear()
    {
        if (this.TraceState != TraceState.Uninitialized)
        {
            this.TraceState = TraceState.Uninitialized;
            this.GraphicsRenderer.RenderPictureBuffer(new PictureRectangle(this.TraceWindowX, this.TraceWindowY, this.TraceWindowWidth, this.TraceWindowHeight), false, false);
        }
    }

    public void Trace(LogicCommand command, int operatorIndex, bool said, int result, int messageIndexOffset)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        this.WindowManager.PushTextPosition();
        this.WindowManager.PushTextColor();
        this.WindowManager.SetTextColor(0, 0x0f);
        this.ScrollTraceWindow();

        if (this.Logic0Called)
        {
            this.Logic0Called = false;
            this.WindowManager.PrintFormatted(UserInterface.TraceSeparatorLine);
            this.ScrollTraceWindow();
        }

        LogicResource originalLogic = this.LogicInterpreter.CurrentLogic;
        if (this.TraceLogicIndex == 0 || (this.LogicInterpreter.CurrentLogic = this.ResourceManager.FindLogic(this.TraceLogicIndex)) is null)
        {
            this.WindowManager.PrintFormatted(UserInterface.TraceProcedureNumber, originalLogic.ResourceIndex, command.Code);
        }
        else
        {
            if (command.Code == 0)
            {
                this.WindowManager.PrintFormatted(UserInterface.TraceProcedureText, originalLogic.ResourceIndex, UserInterface.TraceProcedureReturn);
            }
            else
            {
                string message = this.LogicInterpreter.CurrentLogic.GetMessage(command.Code + messageIndexOffset);
                if (message is not null)
                {
                    this.WindowManager.PrintFormatted(UserInterface.TraceProcedureText, this.LogicInterpreter.CurrentLogic.ResourceIndex, message);
                }
                else
                {
                    if (result != 0xffff)
                    {
                        this.WindowManager.PrintFormatted(UserInterface.TraceFunctionNumber, this.LogicInterpreter.CurrentLogic.ResourceIndex, command.Code);
                    }
                    else
                    {
                        this.WindowManager.PrintFormatted(UserInterface.TraceProcedureNumber, this.LogicInterpreter.CurrentLogic.ResourceIndex, command.Code);
                    }
                }
            }
        }

        this.LogicInterpreter.CurrentLogic = originalLogic;

        this.TraceParameters(command, operatorIndex, said);

        if (result != 0xffff)
        {
            this.WindowManager.GotoPosition(new TextPosition(this.TraceBottom, (byte)(this.TraceRight - 2)));
            this.WindowManager.PrintFormatted(UserInterface.TraceFunctionResult, result == 0 ? UserInterface.TraceFunctionResultFalse : UserInterface.TraceFunctionResultTrue);
        }

        this.WindowManager.UpdateTextRegion();

        this.TraceState = this.Poll();

        this.WindowManager.PopTextPosition();
        this.WindowManager.PopTextColor();
        this.WindowManager.UpdateTextRegion();
    }

    private TraceState Poll()
    {
        InputEvent e = null;

        while (this.TraceState != TraceState.Uninitialized)
        {
            e = this.InputDriver.ReadEvent();
            if (e is not null && e.Type == InputEventType.Ascii)
            {
                break;
            }
        }

        if (e is not null && e.Data == '+')
        {
            return TraceState.Unknown;
        }

        return this.TraceState;
    }

    private void TraceParameters(LogicCommand command, int operatorIndex, bool said)
    {
        int parameterCount = command.ParameterCount;

        this.WindowManager.PushTextPosition();
        if (said)
        {
            parameterCount = this.LogicInterpreter.CurrentLogic.GetCode(operatorIndex++);
        }

        this.WindowManager.DisplayCharacter(UserInterface.TraceParameterStart[0]);

        for (int param = 0; param < parameterCount; param++)
        {
            int val = this.GetTraceParameterValue(operatorIndex, param, said);

            if (said)
            {
                this.WindowManager.PrintFormatted(UserInterface.TraceParameterSigned, val);
            }
            else
            {
                this.WindowManager.PrintFormatted(UserInterface.TraceParameterUnsigned, val);
            }

            if (param < (parameterCount - 1))
            {
                this.WindowManager.DisplayCharacter(UserInterface.TraceParameterSeparator[0]);
            }
        }

        this.WindowManager.DisplayCharacter(UserInterface.TraceParameterEnd[0]);

        bool anyVariables = false;
        if (!said)
        {
            for (int param = 0; param < parameterCount; param++)
            {
                LogicArgumentType type = command.GetParameterType(param);
                if (type == LogicArgumentType.Variable)
                {
                    anyVariables = true;
                }
            }
        }

        if (anyVariables)
        {
            this.ScrollTraceWindow();
        }

        this.WindowManager.PopTextPosition();

        if (anyVariables)
        {
            this.WindowManager.DisplayCharacter(UserInterface.TraceParameterStart[0]);
            for (int param = 0; param < parameterCount; param++)
            {
                int val = this.GetTraceParameterValue(operatorIndex, param, said);

                LogicArgumentType type = command.GetParameterType(param);
                if (type == LogicArgumentType.Variable)
                {
                    this.WindowManager.PrintFormatted(UserInterface.TraceParameterSigned, this.State.Variables[val]);
                }
                else
                {
                    this.WindowManager.PrintFormatted(UserInterface.TraceParameterSigned, val);
                }

                if (param < (parameterCount - 1))
                {
                    this.WindowManager.DisplayCharacter(UserInterface.TraceParameterSeparator[0]);
                }
            }

            this.WindowManager.DisplayCharacter(UserInterface.TraceParameterEnd[0]);
        }
    }

    private int GetTraceParameterValue(int operatorIndex, int param, bool said)
    {
        int val;

        if (said)
        {
            val = this.LogicInterpreter.CurrentLogic.GetCodeLE16(operatorIndex + (param * 2));
        }
        else
        {
            val = this.LogicInterpreter.CurrentLogic.GetCode(operatorIndex + param);
        }

        return val;
    }

    private void ScrollTraceWindow()
    {
        var upperLeft = new TextPosition(this.TraceTop, this.TraceLeft);
        var lowerRight = new TextPosition(this.TraceBottom, this.TraceRight);

        this.WindowManager.ScrollWindow(upperLeft, lowerRight, 0xff);
    }
}
