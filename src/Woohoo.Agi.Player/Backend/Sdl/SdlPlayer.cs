// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl;

#if USE_SDL
using Woohoo.Agi.Interpreter;
using static Woohoo.Agi.Player.Backend.Sdl.NativeMethods;

internal class SdlPlayer : AgiPlayer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SdlPlayer"/> class.
    /// </summary>
    public SdlPlayer()
    {
        var inputDriver = new SdlInputDriver();
        var graphicsDriver = new SdlGraphicsDriver();
        var soundDriver = new PcmSoundDriver(new SdlSoundPcmDriver());

        this.Interpreter = new AgiInterpreter(inputDriver, graphicsDriver, soundDriver);

        inputDriver.SetInterpreter(this.Interpreter);
        soundDriver.SetInterpreter(this.Interpreter);
    }

    protected override void Quit()
    {
        SDL_Quit();
    }
}
#endif
