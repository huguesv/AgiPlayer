// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable disable

namespace Woohoo.Agi.Engine.Interpreter.Controls;

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
            bool includeSynonyms = this.Interpreter.Preferences.WordListIncludeSynonyms;
            int[] applicableIds = this.Interpreter.Preferences.WordListContextSensitive
                ? this.Interpreter.GetApplicableWordFamilies()
                : [];

            while (true)
            {
                string[] words = this.ResourceManager.VocabularyResource.GetWords(includeSynonyms, PreferredSynonyms.GetWords(), applicableIds);

                var listBox = new ListBoxControl(this.Interpreter)
                {
                    Title = UserInterface.TypelessBox,
                    Width = WordSelectionListBoxWidth,
                    Height = WordSelectionListBoxHeight,
                };

                // Add a special first item used to toggle between "all" and "less" words
                string[] items = new string[words.Length + 1];
                items[0] = includeSynonyms ? UserInterface.WordListViewLess : UserInterface.WordListViewAll;
                Array.Copy(words, 0, items, 1, words.Length);

                listBox.SetItems(items);

                if (listBox.DoModal())
                {
                    if (listBox.SelectedItemIndex == 0)
                    {
                        includeSynonyms = !includeSynonyms;

                        // Update the preferences for the next invocation (only applicable for this game session)
                        this.Interpreter.Preferences.WordListIncludeSynonyms = includeSynonyms;
                    }
                    else
                    {
                        string word = words[listBox.SelectedItemIndex - 1];

                        foreach (char c in word)
                        {
                            this.AddInputCharacter(c);
                        }

                        this.AddInputCharacter(' ');

                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }
}
