// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine;

/// <summary>
/// Game start information.
/// </summary>
public class GameStartInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameStartInfo"/> class.
    /// </summary>
    /// <param name="container">Game container.</param>
    /// <param name="id">Game id (may be empty for a version 2 game).</param>
    /// <param name="platform">Platform.</param>
    /// <param name="interpreter">Interpreter version.</param>
    /// <param name="name">Game name.</param>
    /// <param name="version">Game version.</param>
    public GameStartInfo(IGameContainer container, string id, Platform platform, InterpreterVersion interpreter, string name, string version)
    {
        this.GameContainer = container;
        this.Id = id;
        this.Platform = platform;
        this.Interpreter = interpreter;
        this.Name = name;
        this.Version = version;
    }

    /// <summary>
    /// Gets game container.
    /// </summary>
    public IGameContainer GameContainer { get; }

    /// <summary>
    /// Gets game id (may be empty for a version 2 game).
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets platform.
    /// </summary>
    public Platform Platform { get; }

    /// <summary>
    /// Gets interpreter version.
    /// </summary>
    public InterpreterVersion Interpreter { get; }

    /// <summary>
    /// Gets game name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets game version.
    /// </summary>
    public string Version { get; }
}
