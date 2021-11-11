// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Detection
{
    using Woohoo.Agi.Interpreter;

    /// <summary>
    /// Game detection.
    /// </summary>
    public sealed class GameDetector
    {
        private IGameDetectorAlgorithm[] algorithms;

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
        public GameDetectorResult Detect(IGameContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            var result = new GameDetectorResult();

            int i = 0;
            while (!result.Detected && i < this.algorithms.Length)
            {
                result = this.algorithms[i].Detect(container);
                i++;
            }

            return result;
        }
    }
}
