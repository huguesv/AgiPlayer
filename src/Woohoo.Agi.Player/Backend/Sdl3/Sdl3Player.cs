// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl3;

#if USE_SDL3

using Woohoo.Agi.Engine;
using Woohoo.Agi.Engine.Interpreter;
using static SDL3.SDL;

internal class Sdl3Player : AgiPlayer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Sdl3Player"/> class.
    /// </summary>
    public Sdl3Player()
    {
        var inputDriver = new Sdl3InputDriver();
        var graphicsDriver = new Sdl3GraphicsDriver();
        var soundDriver = new PcmSoundDriver(new Sdl3SoundPcmDriver());

        this.Interpreter = new AgiInterpreter(inputDriver, graphicsDriver, soundDriver);

        inputDriver.SetInterpreter(this.Interpreter);
        soundDriver.SetInterpreter(this.Interpreter);
    }

    protected override void Quit()
    {
        SDL_Quit();
    }
}

#endif // USE_SDL3
