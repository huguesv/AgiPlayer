// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

public enum UserInputMode
{
    /// <summary>
    /// Classic parser interface where user input is at the bottom of screen
    /// and the game does not pause when the user enters text.
    /// </summary>
    Classic,

    /// <summary>
    /// Parser interface similar to SCI, or AGI in Hercules mode.  User input is
    /// entered in an input box and the game pauses when the user enters text.
    /// </summary>
    InputBox,

    /// <summary>
    /// Same as classic interface, but with an optional modal listbox where
    /// the user can select a word instead of typing it.
    /// Press CTRL, ALT, SHIFT or CAPSLOCK to bring up the listbox.
    /// </summary>
    WordList,
}
