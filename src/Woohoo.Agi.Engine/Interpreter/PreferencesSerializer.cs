// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

public static class PreferencesSerializer
{
    /// <summary>
    /// Read preferences from <paramref name="filePath"/> into the instance
    /// specified in <paramref name="prefs"/>.
    /// </summary>
    /// <param name="prefs">Preferences to read into.</param>
    /// <param name="filePath">File to load from.</param>
    /// <remarks>
    /// This allows building preferences from multiple files, for example
    /// a global settings file in the install directory, with some
    /// overrides from a settings file in the user appdata folder.
    /// </remarks>
    public static void LoadFrom(Preferences prefs, string filePath)
    {
        ArgumentNullException.ThrowIfNull(prefs);
        ArgumentNullException.ThrowIfNull(filePath);

        var settings = new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
            IgnoreWhitespace = true,
        };

        var reader = XmlReader.Create(filePath, settings);

        Read(prefs, reader);
    }

    private static void Read(Preferences prefs, XmlReader reader)
    {
        reader.MoveToContent();
        reader.ReadStartElement("Settings");

        while (reader.Name != "Settings")
        {
            switch (reader.Name)
            {
                case "InputMode":
                    prefs.InputMode = (UserInputMode)Enum.Parse(typeof(UserInputMode), reader.ReadElementContentAsString(), true);
                    break;
                case "WordListIncludeSynonyms":
                    prefs.WordListIncludeSynonyms = reader.ReadElementContentAsBoolean();
                    break;
                case "WordListContextSensitive":
                    prefs.WordListContextSensitive = reader.ReadElementContentAsBoolean();
                    break;
                case "DisplayScaleX":
                    prefs.DisplayScaleX = reader.ReadElementContentAsInt();
                    break;
                case "DisplayScaleY":
                    prefs.DisplayScaleY = reader.ReadElementContentAsInt();
                    break;
                case "DisplayFadeDelay":
                    prefs.DisplayFadeDelay = reader.ReadElementContentAsInt();
                    break;
                case "DisplayFadeCount":
                    prefs.DisplayFadeCount = reader.ReadElementContentAsInt();
                    break;
                case "Theme":
                    prefs.Theme = (UserInterfaceTheme)Enum.Parse(typeof(UserInterfaceTheme), reader.ReadElementContentAsString());
                    break;
                case "SkipStartupQuestion":
                    prefs.SkipStartupQuestion = reader.ReadElementContentAsBoolean();
                    break;
                case "SoundSingleChannel":
                    prefs.SoundSingleChannel = reader.ReadElementContentAsBoolean();
                    break;
                case "SoundDissolve":
                    prefs.SoundDissolve = reader.ReadElementContentAsInt();
                    break;
                case "SoundReadVariable":
                    prefs.SoundReadVariable = reader.ReadElementContentAsBoolean();
                    break;
                case "SoundHardwareEnabled":
                    prefs.SoundHardwareEnabled = reader.ReadElementContentAsBoolean();
                    break;
                case "Computer":
                    prefs.Computer = (byte)reader.ReadElementContentAsInt();
                    break;
                case "Display":
                    prefs.Display = (byte)reader.ReadElementContentAsInt();
                    break;
                default:
                    throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Errors.SettingNotSupported, reader.Name));
            }
        }

        reader.ReadEndElement();
    }
}
