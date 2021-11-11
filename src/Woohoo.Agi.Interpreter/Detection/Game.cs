// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Detection;

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
        this.Files = new GameFileCollection();
    }

    /// <summary>
    /// Gets or sets name of the game.
    /// </summary>
    internal string Name { get; set; }

    /// <summary>
    /// Gets or sets platform.
    /// </summary>
    internal string Platform { get; set; }

    /// <summary>
    /// Gets or sets version.
    /// </summary>
    internal string Version { get; set; }

    /// <summary>
    /// Gets or sets version of the game interpreter.
    /// </summary>
    internal string Interpreter { get; set; }

    /// <summary>
    /// Gets game data files.
    /// </summary>
    internal GameFileCollection Files { get; }
}
