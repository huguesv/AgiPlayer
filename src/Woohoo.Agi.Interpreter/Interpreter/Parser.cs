// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

using Woohoo.Agi.Resources;

/// <summary>
/// The parser matches the words entered by the user with family ids
/// located in a vocabulary resource.
/// </summary>
public class Parser
{
    private const string SeparatorChars = " ,.?!();:[]{}";
    private const string IllegalChars = "'`-\"";

    /// <summary>
    /// Initializes a new instance of the <see cref="Parser"/> class.
    /// </summary>
    /// <param name="vocabulary">Vocabulary resource to use to search for words.</param>
    public Parser(VocabularyResource vocabulary)
    {
        this.Vocabulary = vocabulary ?? throw new ArgumentNullException(nameof(vocabulary));
    }

    /// <summary>
    /// Gets vocabulary resource to use to search for words.
    /// </summary>
    protected VocabularyResource Vocabulary { get; }

    /// <summary>
    /// Parse the text using the parser's vocabulary.
    /// </summary>
    /// <param name="text">Text to parse.</param>
    /// <returns>Results of the parsing as (word, family identifier) pairs.</returns>
    public ParserResult[] Parse(string text)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        var results = new List<ParserResult>();

        // Clean the text from unnecessary separators
        text = Clean(text);

        int textIndex = 0;
        while (textIndex < text.Length)
        {
            int startIndex = textIndex;
            int family = this.FindLongestMatchingFamily(text, ref textIndex);
            if (family == VocabularyResource.NoFamily)
            {
                // We couldn't find a match, so we store it in the results and stop parsing
                string word = text.Substring(startIndex, textIndex - startIndex).Trim();
                results.Add(new ParserResult(word, family));
                break;
            }

            if (family != VocabularyResource.Ignore)
            {
                // We found a match, so we store it in the results
                string word = text.Substring(startIndex, textIndex - startIndex).Trim();
                results.Add(new ParserResult(word, family));
            }
        }

        return results.ToArray();
    }

    /// <summary>
    /// Clean the text to parse by removing unnecessary separators.
    /// </summary>
    /// <param name="text">Text to clean.</param>
    /// <returns>Text containing exactly one space between words.</returns>
    private static string Clean(string text)
    {
        var cleaned = new StringBuilder();

        int index = 0;
        while (index < text.Length)
        {
            if (IllegalChars.IndexOf(text[index]) >= 0 ||
                SeparatorChars.IndexOf(text[index]) >= 0)
            {
                // skip excess separators at start and inbetween words
                index++;
            }
            else
            {
                do
                {
                    if (SeparatorChars.IndexOf(text[index]) >= 0)
                    {
                        cleaned.Append(' ');
                        break;
                    }

                    if (IllegalChars.IndexOf(text[index]) < 0)
                    {
                        cleaned.Append(text[index]);
                    }

                    index++;
                }
                while (index < text.Length);
            }
        }

        return cleaned.ToString().Trim().ToLower(CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Find the family that contains the longest match.
    /// </summary>
    /// <param name="text">Text to parse.</param>
    /// <param name="index">Index of position to start the match.  On return, modified to the index of the next word.</param>
    /// <returns>Family identifier, or VocabularyResource.NoFamily if no match was found.</returns>
    private int FindLongestMatchingFamily(string text, ref int index)
    {
        // Start with the longest sentence and remove
        // a word at the end one by one until we find a
        // match or until we are down to only one word
        string longest = text.Substring(index);
        int family = this.Vocabulary.GetWordIdentifier(longest);
        while (family == VocabularyResource.NoFamily)
        {
            int lastSpace = longest.LastIndexOf(' ');
            if (lastSpace < 0)
            {
                break;
            }

            longest = longest.Substring(0, lastSpace);
            family = this.Vocabulary.GetWordIdentifier(longest);
        }

        // Increment the index to point to the word
        // after the word we just processed
        index += longest.Length;
        if (index < text.Length)
        {
            if (text[index] == ' ')
            {
                index++;
            }
        }

        return family;
    }
}
