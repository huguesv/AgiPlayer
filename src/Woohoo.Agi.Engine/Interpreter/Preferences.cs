// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// Interpreter preferences.
/// </summary>
public class Preferences
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Preferences"/> class.
    /// </summary>
    public Preferences()
    {
        this.InputMode = UserInputMode.Classic;
        this.DisplayScaleX = 2;
        this.DisplayScaleY = 2;
        this.DisplayFadeDelay = 25;
        this.DisplayFadeCount = 10;
        this.Theme = UserInterfaceTheme.Ega;
        this.SoundDissolve = 3;
        this.SoundReadVariable = true;
        this.SoundHardwareEnabled = true;
    }

    /// <summary>
    /// Gets or sets user input mode.
    /// </summary>
    public UserInputMode InputMode { get; set; }

    /// <summary>
    /// Gets or sets horizontal display scale.
    /// </summary>
    public int DisplayScaleX { get; set; }

    /// <summary>
    /// Gets or sets vertical display scale.
    /// </summary>
    public int DisplayScaleY { get; set; }

    /// <summary>
    /// Gets or sets fade delay between each transition.
    /// </summary>
    public int DisplayFadeDelay { get; set; }

    /// <summary>
    /// Gets or sets number of fade transitions.
    /// </summary>
    public int DisplayFadeCount { get; set; }

    /// <summary>
    /// Gets or sets theme that defines the colors and fonts used.
    /// </summary>
    public UserInterfaceTheme Theme { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether skip the question at the start of KQ4, LSL1, MH1 and GR by patching
    /// the logic byte code.
    /// </summary>
    public bool SkipStartupQuestion { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether play only the first channel.
    /// </summary>
    public bool SoundSingleChannel { get; set; }

    /// <summary>
    /// Gets or sets sound dissolve mode (2 or 3, for the agi version).
    /// </summary>
    public int SoundDissolve { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether enable usage of the sound volume state variable.
    /// </summary>
    public bool SoundReadVariable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether enable sound playback.
    /// </summary>
    public bool SoundHardwareEnabled { get; set; }

    /// <summary>
    /// Gets or sets computer hardware type.
    /// </summary>
    public byte Computer { get; set; }

    /// <summary>
    /// Gets or sets display hardware type.
    /// </summary>
    public byte Display { get; set; }
}
