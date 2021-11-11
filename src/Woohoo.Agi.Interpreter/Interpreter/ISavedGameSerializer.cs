// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    /// <summary>
    /// Save/restore game functionality.
    /// </summary>
    public interface ISavedGameSerializer
    {
        /// <summary>
        /// Save the game to a stream.
        /// </summary>
        /// <param name="interpreter">Interpreter to save from.</param>
        /// <param name="description">Game description.</param>
        /// <param name="stream">Stream to save to.</param>
        void SaveTo(AgiInterpreter interpreter, string description, Stream stream);

        /// <summary>
        /// Restore the game from a stream.
        /// </summary>
        /// <param name="interpreter">Interpreter to load to.</param>
        /// <param name="stream">Saved game stream to restore from.</param>
        void LoadFrom(AgiInterpreter interpreter, Stream stream);

        /// <summary>
        /// Get the game description from a saved game.
        /// </summary>
        /// <param name="stream">Saved game stream to get the description from.</param>
        /// <returns>Game description.</returns>
        string LoadDescriptionFrom(Stream stream);
    }
}
