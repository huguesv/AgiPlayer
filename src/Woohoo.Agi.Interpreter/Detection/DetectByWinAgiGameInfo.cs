// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Detection;

using Woohoo.Agi.Interpreter;

/// <summary>
/// Game detection algorithm which uses a text file
/// created by WinAGI.
/// </summary>
public sealed class DetectByWinAgiGameInfo : IGameDetectorAlgorithm
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DetectByWinAgiGameInfo"/> class.
    /// </summary>
    public DetectByWinAgiGameInfo()
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

        const string GameInfoExtension = ".wag";

        var files = container.GetFilesByExtension(GameInfoExtension);
        if (files.Length > 0)
        {
            try
            {
                var id = new StringBuilder();
                var description = new StringBuilder();
                var interpreter = new StringBuilder();
                var version = new StringBuilder();

                // Last 16 bytes are not stored as properties, it's the
                // WinAGI version string. Ex: "PMWINAGI v1.0"
                var data = container.Read(files[0]);
                var index = 0;
                while (index < (data.Length - 16))
                {
                    byte code = data[index++];
                    index++; // type - not used
                    index++; // num - not used
                    int size = data[index++] + (data[index++] * 256);

                    switch (code)
                    {
                        case 129:
                            // game description
                            for (int i = 0; i < size; i++)
                            {
                                description.Append((char)data[index + i]);
                            }

                            break;

                        case 131:
                            // game id
                            for (int i = 0; i < size; i++)
                            {
                                id.Append((char)data[index + i]);
                            }

                            break;

                        case 132:
                            // interpreter
                            for (int i = 0; i < size; i++)
                            {
                                interpreter.Append((char)data[index + i]);
                            }

                            break;

                        case 134:
                            // game version
                            for (int i = 0; i < size; i++)
                            {
                                version.Append((char)data[index + i]);
                            }

                            break;
                    }

                    index += size;
                }

                var name = description.Length > 0 ? description.ToString() : id.ToString();
                var platform = Platform.PC;

                return new GameDetectorResult(name, GameInfoParser.ParseInterpreterVersion(interpreter.ToString()), platform, version.ToString());
            }
            catch (IOException)
            {
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        return null;
    }
}
