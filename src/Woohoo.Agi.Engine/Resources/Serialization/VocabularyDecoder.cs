// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Resources.Serialization;

using Woohoo.Agi.Engine.Resources;

/// <summary>
/// Vocabulary resource decoder.
/// </summary>
public static class VocabularyDecoder
{
    /// <summary>
    /// Decode the vocabulary resource from byte array.
    /// </summary>
    /// <param name="data">Vocabulary data.</param>
    /// <returns>Vocabulary resource.</returns>
    public static VocabularyResource ReadVocabulary(byte[] data)
    {
        var families = new List<VocabularyWordFamily>();

        // First 52 bytes are 26 2-byte absolute offsets to data for each letter a-z.
        // Most Sierra games will have the data for 'a' starting at offset 52,
        // but some fan games may have some garbage data, with 'a' starting after that garbage.
        // One such example is kq1redux.
        int offset = 0;
        for (int letter = 0; letter < 26; letter++)
        {
            int letterStartOffset = (data[offset] * 0x100) + data[offset + 1];
            offset += 2;

            if (letterStartOffset == 0)
            {
                continue;
            }

            ReadLetter(data, letterStartOffset, families);
        }

        return new VocabularyResource([.. families]);
    }

    private static void ReadLetter(byte[] data, int offset, List<VocabularyWordFamily> families)
    {
        var currentWord = new StringBuilder();
        while (offset < data.Length - 1)
        {
            string previousWord = currentWord.ToString();
            currentWord = new StringBuilder();

            // [0] = Number of characters to include from start of previous word
            int includeNumChars = data[offset];
            offset += 1;

            // Copy the characters from the previous word
            if (includeNumChars > 0)
            {
                if (previousWord.Length < includeNumChars)
                {
                    throw new VocabularyInvalidRepeatCountException();
                }

                currentWord.Append(previousWord[..includeNumChars]);
            }
            else
            {
                if (data[offset] == 0x00)
                {
                    break;
                }
            }

            // Last character in the word will be 0x80 more than the others
            while (data[offset] < 0x80)
            {
                // Decode the character (XOR with 0x7f)
                currentWord.Append((char)(data[offset] ^ 0x7f));
                offset += 1;
            }

            // Decode the last character, after substracting the extra 0x80 (XOR with 0x7f)
            currentWord.Append((char)((data[offset] - 0x80) ^ 0x7f));
            offset += 1;

            if (previousWord.Length > 0 && currentWord[0] > previousWord[0])
            {
                // We've reached the data for the next letter, so stop now
                break;
            }

            // Read the group number
            int group = (data[offset] * 0x100) + data[offset + 1];
            offset += 2;

            // Add to resource, in the correct word group
            VocabularyWordFamily? wordGroup = null;
            foreach (VocabularyWordFamily fam in families)
            {
                if (fam.Identifier == group)
                {
                    wordGroup = fam;
                    break;
                }
            }

            if (wordGroup is null)
            {
                wordGroup = new VocabularyWordFamily(group);
                families.Add(wordGroup);
            }

            wordGroup.Words.Add(currentWord.ToString());
        }
    }
}
