// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Resources;

/// <summary>
/// Represents a vocabulary resource.
/// </summary>
/// <remarks>
/// The compiled version of the vocabulary resource is stored in the words.tok file.
/// </remarks>
public class VocabularyResource
{
    /// <summary>
    /// Family identifier used when a word is not found.
    /// </summary>
    public const int NoFamily = -1;

    /// <summary>
    /// Family identifier used when a word is ignored.
    /// </summary>
    public const int Ignore = 0;

    /// <summary>
    /// Family identifier used to indicate that any word is a match (for a single said parameter).
    /// </summary>
    public const int AnyWord = 1;

    /// <summary>
    /// Family identifier used to indicate that any input is a match (for the whole input).
    /// </summary>
    public const int AnyInput = 9999;

    /// <summary>
    /// Initializes a new instance of the <see cref="VocabularyResource"/> class.
    /// </summary>
    /// <param name="families">Array of families of words.</param>
    public VocabularyResource(VocabularyWordFamily[] families)
    {
        this.Families = families ?? throw new ArgumentNullException(nameof(families));
    }

    /// <summary>
    /// Gets array of families of words.
    /// </summary>
    public VocabularyWordFamily[] Families { get; }

    /// <summary>
    /// Find the family identifier for the specified word.
    /// </summary>
    /// <param name="word">Word to look for.</param>
    /// <returns>Family identifier, or NoFamily if not found.</returns>
    public int GetWordIdentifier(string word)
    {
        ArgumentNullException.ThrowIfNull(word);

        int identifier = NoFamily;

        for (int i = 0; i < this.Families.Length; i++)
        {
            VocabularyWordFamily family = this.Families[i];
            if (family.Words.Contains(word))
            {
                identifier = family.Identifier;
                break;
            }
        }

        return identifier;
    }

    public string[] GetAllWords()
    {
        var items = new List<string>();
        foreach (var family in this.Families)
        {
            if (family.Identifier != VocabularyResource.Ignore &&
                family.Identifier != VocabularyResource.AnyWord)
            {
                foreach (string word in family.Words)
                {
                    items.Add(word);
                }
            }
        }

        items.Sort();

        return [.. items];
    }

    public string[] GetWords(int[] wordIds)
    {
        var items = new List<string>();
        foreach (var family in this.Families)
        {
            if (family.Identifier != VocabularyResource.Ignore &&
                family.Identifier != VocabularyResource.AnyWord &&
                Array.IndexOf(wordIds, family.Identifier) >= 0)
            {
                foreach (string word in family.Words)
                {
                    items.Add(word);
                }
            }
        }

        items.Sort();

        return [.. items];
    }
}
