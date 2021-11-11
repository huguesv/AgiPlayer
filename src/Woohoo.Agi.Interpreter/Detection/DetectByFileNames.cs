// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Detection
{
    using Woohoo.Agi.Interpreter;

    /// <summary>
    /// Game detection algorithm which uses the file names only.
    /// </summary>
    /// <remarks>
    /// This is the least accurate game detection algorithm.
    /// The platform is set to PC unless an amiga V3 map file is found.
    /// The interpreter version is set to 3.002.149 if a game id is detected in
    /// the file names, otherwise the interpreter version is set to 2.936.
    /// </remarks>
    public sealed class DetectByFileNames : IGameDetectorAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DetectByFileNames"/> class.
        /// </summary>
        public DetectByFileNames()
        {
        }

        /// <summary>
        /// Detect a game in the specified folder.
        /// </summary>
        /// <param name="container">Game container.</param>
        /// <returns>Detection result.</returns>
        GameDetectorResult IGameDetectorAlgorithm.Detect(IGameContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            var result = new GameDetectorResult();

            var files = container.GetGameFiles();
            var id = container.GetGameId();

            bool inventoryFound = false;
            bool vocabularyFound = false;
            bool volumeFound = false;
            bool amigaMapFound = false;

            foreach (string file in files)
            {
                if (string.Compare("object", file, true, CultureInfo.InvariantCulture) == 0)
                {
                    inventoryFound = true;
                    continue;
                }

                if (string.Compare("words.tok", file, true, CultureInfo.InvariantCulture) == 0)
                {
                    vocabularyFound = true;
                    continue;
                }

                if (file.ToLower(CultureInfo.InvariantCulture).EndsWith("vol.0"))
                {
                    volumeFound = true;
                    continue;
                }

                if (string.Compare("dirs", file, true, CultureInfo.InvariantCulture) == 0)
                {
                    amigaMapFound = true;
                    continue;
                }
            }

            if (inventoryFound && vocabularyFound && volumeFound)
            {
                var name = container.Name;
                var platform = amigaMapFound ? Platform.Amiga : Platform.PC;
                var interpreter = id.Length > 0 ? InterpreterVersion.V3002149 : InterpreterVersion.V2936;
                var version = string.Empty;

                result = new GameDetectorResult(name, interpreter, platform, version);
            }

            return result;
        }
    }
}
