// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player;

using Woohoo.Agi.Engine;
using Woohoo.Agi.Engine.Detection;
using Woohoo.Agi.Engine.Interpreter;

internal abstract class AgiPlayer
{
    protected AgiInterpreter Interpreter { get; set; }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <returns>Error code.</returns>
    [STAThread]
    public static int Main(string[] args)
    {
        var prefs = ReadPreferences();
        var player = CreatePlayer(prefs);
        return player.Run(args, prefs);
    }

    protected abstract void Quit();

    private static AgiPlayer CreatePlayer(Preferences prefs)
    {
        AgiPlayer player = prefs.Backend.ToUpperInvariant() switch
        {
#if USE_SDL
            "SDL" => new Backend.Sdl.SdlPlayer(),
#endif
#if USE_SDL2
            "SDL2" => new Backend.Sdl2.Sdl2Player(),
#endif
#if USE_SDL3
            "SDL3" => new Backend.Sdl3.Sdl3Player(),
#endif
#if USE_SDL
            _ => new Backend.Sdl.SdlPlayer(),
#elif USE_SDL2
            _ => new Backend.Sdl2.Sdl2Player(),
#elif USE_SDL3
            _ => new Backend.Sdl3.Sdl3Player(),
#else
#error No backend defined
#endif
        };

        return player;
    }

    private static void DisplayInfo()
    {
        Console.WriteLine("{0} v{1}", UserInterface.PlayerName, UserInterface.PlayerVersion);
        Console.WriteLine("Copyright (C) 2006-2025 Hugues Valois");
        Console.WriteLine();

        Console.WriteLine("Based upon the New Adventure Game Interpreter (NAGI)");
        Console.WriteLine("Copyright (C) 2000-2002 Nick Sonneveld & Gareth McMullin\n");

        Console.WriteLine("Based upon the Adventure Game Interpreter (AGI) v2.917 and v3.002.149");
        Console.WriteLine("Copyright (C) 1984-1988 Sierra On-Line, Inc.");
        Console.WriteLine();
    }

    private static Preferences ReadPreferences()
    {
        var preferences = new Preferences();

        var appFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var settingsFilePath = Path.Combine(appFolder, "settings.xml");
        if (File.Exists(settingsFilePath))
        {
            PreferencesSerializer.LoadFrom(preferences, settingsFilePath);
        }

        return preferences;
    }

    private int Run(string[] args, Preferences prefs)
    {
        DisplayInfo();

        string folder = Environment.CurrentDirectory;
        if (args.Length > 0)
        {
            folder = args[0];
        }

        try
        {
            // First try the specified directory and run the game if there is one
            if (this.RunGame(folder, prefs) == -1)
            {
                // Recursively try to find games and list them in game selection screen
                return this.RunGames(folder, prefs);
            }
        }
        catch (ExitException)
        {
            this.Quit();
        }
        catch (AbortException)
        {
            try
            {
                this.Interpreter.ExitAgi();
            }
            catch (ExitException)
            {
                this.Quit();
            }
        }

        return 0;
    }

    private int RunGames(string folder, Preferences prefs)
    {
        GameStartInfo[] startInfos = GameFinder.FindGames(folder, true);

        this.Interpreter.Start(startInfos, prefs);

        return 0;
    }

    private int RunGame(string folder, Preferences prefs)
    {
        GameStartInfo startInfo = GameFinder.FindGame(folder);
        if (startInfo is not null)
        {
            this.Interpreter.Start(startInfo, prefs);

            return 0;
        }

        return -1;
    }
}
