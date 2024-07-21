// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl2;

#if USE_SDL2
using Woohoo.Agi.Interpreter;
using static Woohoo.Agi.Player.Backend.Sdl2.NativeMethods;

internal class Sdl2Player : AgiPlayer
{

    /// <summary>
    /// Initializes a new instance of the <see cref="Sdl2Player"/> class.
    /// </summary>
    public Sdl2Player()
    {
        var inputDriver = new Sdl2InputDriver();
        var graphicsDriver = new Sdl2GraphicsDriver();
        var soundDriver = new PcmSoundDriver(new Sdl2SoundPcmDriver());

        this.interpreter = new AgiInterpreter(inputDriver, graphicsDriver, soundDriver);

        inputDriver.SetInterpreter(this.interpreter);
        soundDriver.SetInterpreter(this.interpreter);
    }

    protected override void Quit()
    {
        SDL_Quit();
    }
}
#endif
