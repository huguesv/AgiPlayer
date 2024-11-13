// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Detection;

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
    GameDetectorResult? IGameDetectorAlgorithm.Detect(IGameContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        var files = container.GetGameFiles();
        var id = container.GetGameId();

        bool inventoryFound = false;
        bool vocabularyFound = false;
        bool volumeFound = false;
        bool amigaMapFound = false;

        foreach (string file in files)
        {
            if (string.Equals("object", file, StringComparison.OrdinalIgnoreCase))
            {
                inventoryFound = true;
                continue;
            }

            if (string.Equals("words.tok", file, StringComparison.OrdinalIgnoreCase))
            {
                vocabularyFound = true;
                continue;
            }

            if (file.EndsWith("vol.0", StringComparison.OrdinalIgnoreCase))
            {
                volumeFound = true;
                continue;
            }

            if (string.Equals("dirs", file, StringComparison.OrdinalIgnoreCase))
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

            return new GameDetectorResult(name, interpreter, platform, version);
        }

        return null;
    }
}
