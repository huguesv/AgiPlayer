// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter.Hints;

using System.Collections.Generic;
using System.Text;

public static class HintBookSerializer
{
    public static HintBook Deserialize(Stream stream) => Deserialize(new StreamReader(stream, Encoding.UTF8));

    public static HintBook Deserialize(TextReader reader)
    {
        // Text file format, where line starting with:
        // @ specifies optional comma-separated room numbers
        //   (defaults to all rooms)
        // $ specifies optional comma-separated flag numbers that must be set,
        //   or unset if number is prefixed with !
        //   (defaults to none)
        // % specifies optional comma-separated item numbers that must be set,
        //   or unset if number is prefixed with !
        //   (defaults to none)
        // + specifies topic title (ellided if 32 chars or more)
        // - specifies decrypted topic hint message
        // ~ specifies encrypted topic hint message
        //
        // Example:
        //
        // + topic title
        // @ 10,11,12
        // - topic hint
        // - topic hint
        // + another topic title
        // $ 75,!76
        // % 1,!2
        // ~ encrypted topic hint
        var topics = new List<Topic>();

        var title = string.Empty;
        var messages = new List<string>();
        var rooms = new List<byte>();
        var flags = new List<FlagContext>();
        var items = new List<ItemContext>();

        while (reader.ReadLine() is string line)
        {
            if (line.StartsWith('+'))
            {
                if (title.Length > 0)
                {
                    // Add previous topic
                    topics.Add(new Topic { Title = title, Messages = messages, Rooms = rooms, Flags = flags, Items = items });

                    title = string.Empty;
                    messages = [];
                    rooms = [];
                    flags = [];
                    items = [];
                }

                title = line.Substring(1).Trim();
            }
            else if (line.StartsWith('@'))
            {
                // Add rooms
                rooms.AddRange(line.Substring(1).Split(',').Select(byte.Parse));
            }
            else if (line.StartsWith('$'))
            {
                // Add flags
                flags.AddRange(line.Substring(1).Split(',').Select(ParseFlag));
            }
            else if (line.StartsWith('%'))
            {
                // Add items
                items.AddRange(line.Substring(1).Split(',').Select(ParseItem));
            }
            else if (line.StartsWith('-'))
            {
                // Add message
                messages.Add(line.Substring(1).Trim());
            }
            else if (line.StartsWith('~'))
            {
                // Add encrypted message
                messages.Add(Decrypt(line.Substring(1).Trim()));
            }
        }

        if (title.Length > 0)
        {
            // Add last topic
            topics.Add(new Topic { Title = title, Messages = messages, Rooms = rooms, Flags = flags, Items = items });
        }

        return new HintBook { Topics = topics };
    }

    private static FlagContext ParseFlag(string text)
    {
        text = text.Trim();
        if (text.StartsWith('!'))
        {
            return new FlagContext { Number = byte.Parse(text[1..]), Value = false };
        }
        else
        {
            return new FlagContext { Number = byte.Parse(text), Value = true };
        }
    }

    private static ItemContext ParseItem(string text)
    {
        text = text.Trim();
        if (text.StartsWith('!'))
        {
            return new ItemContext { Number = byte.Parse(text[1..]), Value = false };
        }
        else
        {
            return new ItemContext { Number = byte.Parse(text), Value = true };
        }
    }

    private static string Decrypt(string text)
    {
        var result = new StringBuilder(text.Length);
        foreach (char c in text)
        {
            if (c < 32)
            {
                result.Append(c);
            }
            else if (c < 80)
            {
                result.Append((char)((2 * c) - 32));
            }
            else
            {
                result.Append((char)((2 * c) - 127));
            }
        }

        return result.ToString();
    }
}
