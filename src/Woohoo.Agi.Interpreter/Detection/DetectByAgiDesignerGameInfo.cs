// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Detection;

using Woohoo.Agi.Interpreter;

/// <summary>
/// Game detection algorithm which uses an XML game information file
/// located in the game folder.
/// </summary>
public sealed class DetectByAgiDesignerGameInfo : IGameDetectorAlgorithm
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DetectByAgiDesignerGameInfo"/> class.
    /// </summary>
    public DetectByAgiDesignerGameInfo()
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

        const string GameInfoExtension = ".agigame";

        var files = container.GetFilesByExtension(GameInfoExtension);
        if (files.Length > 0)
        {
            try
            {
                // Example .agigame file contents:
                //
                // <?xml version="1.0" encoding="utf-8" ?>
                // <game name="The Black Cauldron"
                //  platform="PC"
                //  date="XXXX-XX-XX"
                //  version="2.00"
                //  format="FLOPPY"
                //  interpreter="2.439"
                //  language="English"/>
                var doc = new XmlDocument();
                doc.Load(files[0]);

                var node = doc.SelectSingleNode("game");
                if (node?.Attributes is not null)
                {
                    var name = node.Attributes["name"]?.Value ?? string.Empty;
                    var platform = node.Attributes["platform"]?.Value ?? string.Empty;
                    var interpreter = node.Attributes["interpreter"]?.Value ?? string.Empty;
                    var version = node.Attributes["version"]?.Value ?? string.Empty;

                    return new GameDetectorResult(name, GameInfoParser.ParseInterpreterVersion(interpreter), GameInfoParser.ParsePlatform(platform), version);
                }
            }
            catch (XmlException)
            {
            }
        }

        return null;
    }
}
