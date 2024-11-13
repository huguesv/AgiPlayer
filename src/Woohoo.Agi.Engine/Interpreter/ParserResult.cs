// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// Parser result.
/// </summary>
public class ParserResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParserResult"/> class.
    /// </summary>
    /// <param name="word">Word parsed.</param>
    /// <param name="familyIdentifier">Family identifier found in the vocabulary.</param>
    public ParserResult(string word, int familyIdentifier)
    {
        this.Word = word ?? throw new ArgumentNullException(nameof(word));
        this.FamilyIdentifier = familyIdentifier;
    }

    /// <summary>
    /// Gets word parsed.
    /// </summary>
    public string Word { get; }

    /// <summary>
    /// Gets family identifier found in the vocabulary.
    /// </summary>
    public int FamilyIdentifier { get; }
}
