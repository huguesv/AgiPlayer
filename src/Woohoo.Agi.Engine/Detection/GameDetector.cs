// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Detection;

/// <summary>
/// Game detection.
/// </summary>
public sealed class GameDetector
{
    private readonly IGameDetectorAlgorithm[] algorithms;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameDetector"/> class.
    /// </summary>
    /// <param name="algorithms">Game detection algorithms.</param>
    public GameDetector(IGameDetectorAlgorithm[] algorithms)
    {
        this.algorithms = algorithms ?? throw new ArgumentNullException(nameof(algorithms));
    }

    /// <summary>
    /// Search the specified folder for a game.
    /// </summary>
    /// <param name="container">Game container to search.</param>
    /// <returns>Game detection result.</returns>
    public GameDetectorResult? Detect(IGameContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        foreach (var detector in this.algorithms)
        {
            var result = detector.Detect(container);
            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }
}
