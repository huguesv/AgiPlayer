// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    public interface IGameContainer
    {
        string Name
        {
            get;
        }

        byte[] Read(string file);

        bool Exists(string file);

        string GetGameId();

        string[] GetGameFiles();

        string[] GetFilesByExtension(string ext);
    }
}
