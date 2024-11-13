// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// Helper class to serialize agi strings into binary format.
/// </summary>
public static class StringUtility
{
    private const string HexConversionTable = "0123456789ABCDEF";

    /// <summary>
    /// Reverse a string.
    /// </summary>
    /// <param name="text">Text to reverse.</param>
    /// <returns>Reversed text.</returns>
    public static string Reverse(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        StringBuilder reversed = new StringBuilder();

        for (int i = text.Length - 1; i >= 0; i--)
        {
            reversed.Append(text[i]);
        }

        return reversed.ToString();
    }

    /// <summary>
    /// Format a decimal value.
    /// </summary>
    /// <param name="val">Value to convert to a string.</param>
    /// <returns>Decimal value text.</returns>
    public static string NumberToString(int val)
    {
        return val.ToString(CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Format an hexadecimal value.
    /// </summary>
    /// <param name="val">Value to convert to a string.</param>
    /// <returns>Hexadecimal value text.</returns>
    public static string NumberToHexString(int val)
    {
        StringBuilder text = new StringBuilder();

        do
        {
            int remain = val & 0x0f;
            val >>= 4;

            text.Append(HexConversionTable[remain]);
        }
        while (val != 0);

        return Reverse(text.ToString());
    }

    /// <summary>
    /// Add zeros to the beginning of the specified text.
    /// </summary>
    /// <param name="text">Original text to pad.</param>
    /// <param name="paddingSize">Total length of the desired final text.</param>
    /// <returns>Text padded with zeros.</returns>
    public static string PadWithZeros(string text, int paddingSize)
    {
        ArgumentNullException.ThrowIfNull(text);

        StringBuilder builder = new StringBuilder();
        builder.Append('0', paddingSize - text.Length);
        builder.Append(text);

        return builder.ToString();
    }

    /// <summary>
    /// Parse the decimal value contained in the text.
    /// </summary>
    /// <param name="text">Text to parse.</param>
    /// <returns>Value.</returns>
    public static byte ParseNumber(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        text = text.Trim();

        byte num = 0;

        foreach (char c in text)
        {
            if (c >= '0' && c <= '9')
            {
                num = (byte)((num * 10) + (c - '0'));
            }
            else
            {
                break;
            }
        }

        return num;
    }

    /// <summary>
    /// Parse the decimal value contained in the text.
    /// </summary>
    /// <param name="text">Text to parse.</param>
    /// <param name="index">Index to start parsing.  The index is incremented to the position where parsing stopped.</param>
    /// <returns>Value.</returns>
    public static int ParseNumber(string text, ref int index)
    {
        ArgumentNullException.ThrowIfNull(text);

        int val = 0;

        while (index < text.Length && text[index] >= '0' && text[index] <= '9')
        {
            val = (val * 10) + (text[index] - '0');
            index++;
        }

        return val;
    }

    /// <summary>
    /// Convert a .NET resource string for use in the player.
    /// </summary>
    /// <param name="text">Text to convert.</param>
    /// <returns>Converted text.</returns>
    public static string ConvertSystemResourceText(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return text.Replace("\\n", "\n");
    }
}
