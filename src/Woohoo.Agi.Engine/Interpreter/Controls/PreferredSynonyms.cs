// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter.Controls;

internal static class PreferredSynonyms
{
    // TODO: This is a temporary solution
    // Ideally we'd have a custom minimal list of words per game (probably loaded from external file).
    // Right now picking a single synonym from each family is not great,
    // because some of the synonyms are not applicable in all situations.
    // For example, in kq2, "mermaid", "woman", "old hag" and "valanice" are synonyms.
    public static string[] GetWords()
    {
        return [
            "look",
            "open",
            "get",
            "help",
            "house",
            "room",
            "car",
            "woman",
            "flower",
            "save game",
            "restore game",
            "restart game",
        ];
    }
}
