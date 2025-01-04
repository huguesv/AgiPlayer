// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Detection;

/// <summary>
/// Game detection algorithm which uses the file names only.
/// </summary>
/// <remarks>
/// This is the least accurate game detection algorithm.
/// The platform is set to PC unless an amiga V3 map file is found, or amiga
/// pointer/busy file is found, or atari prg file is found.
/// The interpreter version is set to 3.002.149 if a game id is detected in
/// the file names or an amiga V3 map file is found, otherwise the interpreter
/// version is set to 2.936.
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

        var gameFiles = container.GetGameFiles();
        var interpreterFiles = container.GetInterpreterFiles();
        var id = container.GetGameId();

        bool inventoryFound = false;
        bool vocabularyFound = false;
        bool idVolumeFound = false;
        bool volumeFound = false;
        bool amigaMapFound = false;
        bool amigaBusyFound = false;
        bool amigaPointerFound = false;
        bool idMapFound = false;
        bool logMapFound = false;
        bool picMapFound = false;
        bool sndMapFound = false;
        bool viewMapFound = false;
        bool atariPrgFound = false;
        bool sierraStandardFound = false;

        foreach (string file in interpreterFiles)
        {
            if (Path.GetExtension(file).Equals(".prg", StringComparison.OrdinalIgnoreCase))
            {
                atariPrgFound = true;
                continue;
            }

            if (string.Equals("pointer", file, StringComparison.OrdinalIgnoreCase))
            {
                amigaPointerFound = true;
                continue;
            }

            if (string.Equals("busy", file, StringComparison.OrdinalIgnoreCase))
            {
                amigaBusyFound = true;
                continue;
            }

            if (string.Equals("sierrastandard", file, StringComparison.OrdinalIgnoreCase))
            {
                sierraStandardFound = true;
                continue;
            }
        }

        foreach (string file in gameFiles)
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

            if (file.Equals("vol.0", StringComparison.OrdinalIgnoreCase))
            {
                volumeFound = true;
                continue;
            }

            if (file.EndsWith("vol.0", StringComparison.OrdinalIgnoreCase))
            {
                idVolumeFound = true;
                continue;
            }

            if (string.Equals("dirs", file, StringComparison.OrdinalIgnoreCase))
            {
                amigaMapFound = true;
                continue;
            }

            if (string.Equals("logdir", file, StringComparison.OrdinalIgnoreCase))
            {
                logMapFound = true;
                continue;
            }

            if (string.Equals("picdir", file, StringComparison.OrdinalIgnoreCase))
            {
                picMapFound = true;
                continue;
            }

            if (string.Equals("snddir", file, StringComparison.OrdinalIgnoreCase))
            {
                sndMapFound = true;
                continue;
            }

            if (string.Equals("viewdir", file, StringComparison.OrdinalIgnoreCase))
            {
                viewMapFound = true;
                continue;
            }

            if (file.EndsWith("dir", StringComparison.OrdinalIgnoreCase))
            {
                idMapFound = true;
                continue;
            }
        }

        if (inventoryFound && vocabularyFound)
        {
            if (amigaMapFound && volumeFound)
            {
                return new GameDetectorResult(container.Name, InterpreterVersion.V3002149, Platform.Amiga, string.Empty);
            }

            if (logMapFound && picMapFound && sndMapFound && viewMapFound && volumeFound)
            {
                if (amigaPointerFound || amigaBusyFound)
                {
                    return new GameDetectorResult(container.Name, InterpreterVersion.V2936, Platform.Amiga, string.Empty);
                }
                else if (atariPrgFound)
                {
                    return new GameDetectorResult(container.Name, InterpreterVersion.V2936, Platform.AtariST, string.Empty);
                }
                else if (sierraStandardFound)
                {
                    return new GameDetectorResult(container.Name, InterpreterVersion.V2936, Platform.AppleIIgs, string.Empty);
                }
                else
                {
                    return new GameDetectorResult(container.Name, InterpreterVersion.V2936, Platform.PC, string.Empty);
                }
            }

            if (idMapFound && idVolumeFound)
            {
                return new GameDetectorResult(container.Name, InterpreterVersion.V3002149, Platform.PC, string.Empty);
            }
        }

        return null;
    }
}
