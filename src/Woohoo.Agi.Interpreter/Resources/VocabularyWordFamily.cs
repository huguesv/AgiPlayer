// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources;

/// <summary>
/// Represents a family of words (synonyms).
/// </summary>
public class VocabularyWordFamily
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VocabularyWordFamily"/> class.
    /// </summary>
    /// <param name="identifier">Identifier for this family of words.</param>
    /// <param name="words">Array of words.</param>
    public VocabularyWordFamily(int identifier, params string[] words)
    {
        this.Words = new List<string>(words);
        this.Identifier = identifier;
    }

    /// <summary>
    /// Gets collection of words.
    /// </summary>
    public ICollection<string> Words { get; }

    /// <summary>
    /// Gets family identifier.
    /// </summary>
    public int Identifier { get; }
}
