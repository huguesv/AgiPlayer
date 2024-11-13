// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Detection;

/// <summary>
/// Game information.
/// </summary>
internal class Game
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Game"/> class.
    /// </summary>
    internal Game()
    {
        this.Name = string.Empty;
        this.Platform = string.Empty;
        this.Version = string.Empty;
        this.Interpreter = string.Empty;
        this.Files = [];
    }

    /// <summary>
    /// Gets name of the game.
    /// </summary>
    internal string Name { get; init; }

    /// <summary>
    /// Gets platform.
    /// </summary>
    internal string Platform { get; init; }

    /// <summary>
    /// Gets version.
    /// </summary>
    internal string Version { get; init; }

    /// <summary>
    /// Gets version of the game interpreter.
    /// </summary>
    internal string Interpreter { get; init; }

    /// <summary>
    /// Gets game data files.
    /// </summary>
    internal GameFileCollection Files { get; }
}
