// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.Controls;

public class GameSelectionControl
{
    private const byte GameSelectionListBoxHeight = 19;
    private const byte GameSelectionListBoxWidth = 34;

    public GameSelectionControl(AgiInterpreter interpreter)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
    }

    protected AgiInterpreter Interpreter { get; }

    protected WindowManager WindowManager => this.Interpreter.WindowManager;

    protected IInputDriver InputDriver => this.Interpreter.InputDriver;

    public GameStartInfo DoModal(GameStartInfo[] startInfos)
    {
        GameStartInfo gameInfo = null;

        this.WindowManager.InitializeText();

        if (this.WindowManager.MessageState.Active)
        {
            this.WindowManager.CloseWindow();
        }

        if (startInfos.Length > 0)
        {
            Array.Sort<GameStartInfo>(startInfos, new GameStartInfoComparer());

            gameInfo = this.SelectGame(startInfos);
        }
        else
        {
            this.Interpreter.Prompt(UserInterface.GameSelectionNoGameFound);
        }

        this.Interpreter.ShutdownText();

        return gameInfo;
    }

    private GameStartInfo SelectGame(GameStartInfo[] startInfos)
    {
        var items = new List<string>();

        foreach (var startInfo in startInfos)
        {
            items.Add(string.Format(CultureInfo.CurrentCulture, "{0} {1}", startInfo.Name, startInfo.Version));
        }

        var listBox = new ListBoxControl(this.Interpreter)
        {
            Title = UserInterface.GameSelectionHeader,
            Width = GameSelectionListBoxWidth,
            Height = GameSelectionListBoxHeight,
        };

        listBox.SetItems([.. items]);

        if (listBox.DoModal())
        {
            return startInfos[listBox.SelectedItemIndex];
        }

        return null;
    }
}
