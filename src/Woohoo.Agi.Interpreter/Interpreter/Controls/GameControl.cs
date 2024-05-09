// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.Controls;

public class GameControl
{
    private Stack<MouseDown> mouseDownStack;

    public GameControl(AgiInterpreter interpreter)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));

        this.TraceControl = new TraceControl(interpreter);
        this.StatusLineControl = new StatusLineControl(interpreter);

        switch (interpreter.Preferences.InputMode)
        {
            case UserInputMode.Classic:
                this.InputControl = new ClassicInputControl(interpreter);
                break;
            case UserInputMode.WordList:
                this.InputControl = new WordListInputControl(interpreter);
                break;
            case UserInputMode.InputBox:
                this.InputControl = new PopupInputControl(interpreter);
                break;
        }

        this.mouseDownStack = new Stack<MouseDown>();
    }

    public MouseMode MouseMode { get; set; } = MouseMode.SierraV2;

    public int SavedParentMenuItemIndex { get; set; }

    public int SavedMenuItemIndex { get; set; }

    public bool MenuNextInput { get; set; }

    public TraceControl TraceControl { get; }

    public InputControl InputControl { get; }

    public StatusLineControl StatusLineControl { get; }

    protected AgiInterpreter Interpreter { get; }

    protected State State => this.Interpreter.State;

    protected ViewObjectManager ObjectManager => this.Interpreter.ObjectManager;

    protected IGraphicsDriver GraphicsDriver => this.Interpreter.GraphicsDriver;

    protected IInputDriver InputDriver => this.Interpreter.InputDriver;

    public bool ProcessEvent(InputEvent e)
    {
        ArgumentNullException.ThrowIfNull(e);

        if (!this.StatusLineControl.ProcessEvent(e))
        {
            if (!this.InputControl.ProcessEvent(e))
            {
                if (e.Type == InputEventType.Mouse)
                {
                    this.HandleMouseEvent(e.Data, new ScreenPoint(e.X, e.Y));

                    return true;
                }
            }
        }

        return false;
    }

    public void ShowMenu()
    {
        var control = new MenuControl(this.Interpreter)
        {
            Menu = this.Interpreter.Menu,
            CurrentMenuItemIndex = this.SavedMenuItemIndex,
            CurrentParentMenuItemIndex = this.SavedParentMenuItemIndex,
        };

        if (control.DoModal())
        {
            this.InputDriver.WriteEvent(InputEventType.Controller, this.Interpreter.Menu.Items[control.CurrentParentMenuItemIndex].Items[control.CurrentMenuItemIndex].Controller);
        }

        this.SavedMenuItemIndex = control.CurrentMenuItemIndex;
        this.SavedParentMenuItemIndex = control.CurrentParentMenuItemIndex;
        this.MenuNextInput = false;

        if (this.State.StatusVisible)
        {
            this.DisplayStatusLine();
        }
    }

    public void DisplayStatusLine()
    {
        this.StatusLineControl.Display();
    }

    public void Reset()
    {
        switch (this.MouseMode)
        {
            case MouseMode.Brian:
                this.mouseDownStack = new Stack<MouseDown>();
                break;

            case MouseMode.SierraV2:
                break;

            case MouseMode.SierraV3:
                break;
        }
    }

    public MouseDown PollMouse()
    {
        MouseDown m = this.PopMouseStack();
        if (m is null)
        {
            this.InputDriver.PollMouse(out int button, out int screenScaledX, out int screenScaledY);

            PicturePoint pt = this.GraphicsDriver.ScreenToPicturePoint(new ScreenPoint(screenScaledX, screenScaledY));

            m = new MouseDown(button, pt.X, pt.Y);
        }

        return m;
    }

    private MouseDown PopMouseStack()
    {
        MouseDown m = null;

        if (this.mouseDownStack.Count > 0)
        {
            m = this.mouseDownStack.Pop();
        }

        return m;
    }

    private void PushMouseStack(int button, int x, int y)
    {
        this.mouseDownStack.Push(new MouseDown(button, x, y));
    }

    private void HandleMouseEvent(int button, ScreenPoint scaledScreen)
    {
        PicturePoint pt = this.GraphicsDriver.ScreenToPicturePoint(scaledScreen);

        switch (this.MouseMode)
        {
            case MouseMode.Brian:
                this.PushMouseStack(button, pt.X, pt.Y);
                break;

            case MouseMode.SierraV2:
                {
                    int x = pt.X;
                    int y = pt.Y;

                    MouseArea area = this.CheckArea(ref x, ref y);

                    switch (area)
                    {
                        case MouseArea.Status:
                            // Note we don't check for Flags.Menu, so the mouse
                            // can trigger the menu even if it can't be done with keyboard
                            // Because we don't check Flags.Menu, we have to check for empty menu
                            if (this.State.MenuEnabled && this.Interpreter.Menu.Items.Count > 0)
                            {
                                this.ShowMenu();
                            }

                            break;

                        case MouseArea.Game:
                            this.ObjectManager.MoveEgo(x, y);
                            break;

                        case MouseArea.CommandEntry:
                            this.ObjectManager.MoveEgo(x, 169);
                            break;
                    }
                }

                break;

            case MouseMode.SierraV3:
                break;
        }
    }

    private MouseArea CheckArea(ref int x, ref int y)
    {
        MouseArea area = MouseArea.Unknown;

        var mouseAreaPositionList = new List<MouseAreaPosition>
        {
            new(MouseArea.Status, 0, this.State.StatusLineRow * WindowManager.CharacterHeight, 40 * WindowManager.CharacterWidth, 1 * WindowManager.CharacterHeight),
            new(MouseArea.Game, 0, this.State.WindowRowMin * WindowManager.CharacterHeight, 40 * WindowManager.CharacterWidth, 21 * WindowManager.CharacterHeight),
            new(MouseArea.CommandEntry, 0, (this.State.WindowRowMin + 21) * WindowManager.CharacterHeight, 40 * WindowManager.CharacterWidth, 10 * WindowManager.CharacterHeight),
        };

        foreach (MouseAreaPosition current in mouseAreaPositionList)
        {
            if (x >= current.X &&
                x < (current.X + current.Width) &&
                y >= current.Y &&
                y < (current.Y + current.Height))
            {
                area = current.Area;
                x -= current.X;
                y -= current.Y;
                break;
            }
        }

        return area;
    }
}
