// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Detection;

/// <summary>
/// Result from game detection.
/// </summary>
public sealed class GameDetectorResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameDetectorResult"/> class.
    /// </summary>
    public GameDetectorResult()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameDetectorResult"/> class.
    /// </summary>
    /// <param name="name">Game name.</param>
    /// <param name="interpreter">Interpreter version.</param>
    /// <param name="platform">Platform.</param>
    /// <param name="version">Game version.</param>
    public GameDetectorResult(string name, InterpreterVersion interpreter, Platform platform, string version)
    {
        this.Detected = true;
        this.Name = name;
        this.Interpreter = interpreter;
        this.Platform = platform;
        this.Version = version;
    }

    /// <summary>
    /// Gets or sets a value indicating whether game was detected or not.
    /// </summary>
    public bool Detected { get; set; }

    /// <summary>
    /// Gets game name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets distribution media.
    /// </summary>
    public string Media { get; set; }

    /// <summary>
    /// Gets interpreter version.
    /// </summary>
    public InterpreterVersion Interpreter { get; }

    /// <summary>
    /// Gets platform.
    /// </summary>
    public Platform Platform { get; }

    /// <summary>
    /// Gets game version.
    /// </summary>
    public string Version { get; }
}
