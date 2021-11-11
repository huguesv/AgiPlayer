// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.Controls;

public class WordListInputControl : ClassicInputControl
{
    private const byte WordSelectionListBoxHeight = 19;
    private const byte WordSelectionListBoxWidth = 18;

    public WordListInputControl(AgiInterpreter interpreter)
        : base(interpreter)
    {
    }

    protected override void ProcessKey(InputEvent e)
    {
        if (e.Data == 0)
        {
            this.InputTypelessPoll(e.Data);
        }
        else
        {
            this.InputPollKeyPressed(e.Data);
        }
    }

    private void InputTypelessPoll(int key)
    {
        this.State.Variables[Variables.KeyPressed] = (byte)key;
        if (this.State.InputEnabled)
        {
            string[] words = this.ResourceManager.VocabularyResource.GetAllWords();

            var listBox = new ListBoxControl(this.Interpreter)
            {
                Title = UserInterface.TypelessBox,
                Width = WordSelectionListBoxWidth,
                Height = WordSelectionListBoxHeight,
            };

            listBox.SetItems(words);

            if (listBox.DoModal())
            {
                string word = words[listBox.SelectedItemIndex];

                foreach (char c in word)
                {
                    this.AddInputCharacter(c);
                }

                this.AddInputCharacter(' ');
            }
        }
    }
}
