// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player
{
    using System;
    using System.IO;
    using System.Reflection;
    using Woohoo.Agi.Interpreter;
    using Woohoo.Agi.Resources.Serialization;

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
#if USE_SDL
            return new Backend.Sdl.SdlPlayer().Run(args);
#elif USE_SDL2
            return new Backend.Sdl2.Sdl2Player().Run(args);
#else
#error No backend defined
            return 0;
#endif
        }

        protected abstract void Quit();

        public int Run(string[] args)
        {
            this.DisplayInfo();

            string folder = Environment.CurrentDirectory;
            if (args.Length > 0)
            {
                folder = args[0];
            }

            try
            {
                // First try the specified directory and run the game if there is one
                if (this.RunGame(folder) == -1)
                {
                    // Recursively try to find games and list them in game selection screen
                    return this.RunGames(folder);
                }
            }
            catch (ExitException)
            {
                Quit();
            }
            catch (AbortException)
            {
                try
                {
                    this.Interpreter.ExitAgi();
                }
                catch (ExitException)
                {
                    Quit();
                }
            }

            return 0;
        }

        private void DisplayInfo()
        {
            Console.WriteLine("{0} v{1}", UserInterface.PlayerName, UserInterface.PlayerVersion);
            Console.WriteLine("Copyright (C) 2006-2018 Hugues Valois");
            Console.WriteLine();

            Console.WriteLine("Based upon the New Adventure Game Interpreter (NAGI)");
            Console.WriteLine("Copyright (C) 2000-2002 Nick Sonneveld & Gareth McMullin\n");

            Console.WriteLine("Based upon the Adventure Game Interpreter (AGI) v2.917 and v3.002.149");
            Console.WriteLine("Copyright (C) 1984-1988 Sierra On-Line, Inc.");
            Console.WriteLine();
        }

        private Preferences ReadPreferences()
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

        private int RunGames(string folder)
        {
            GameStartInfo[] startInfos = GameFinder.FindGames(folder, true);

            this.Interpreter.Start(startInfos, this.ReadPreferences());

            return 0;
        }

        private int RunGame(string folder)
        {
            if (ResourceLoader.IsGameFolder(new GameFolder(folder)))
            {
                GameStartInfo startInfo = GameFinder.FindGame(folder);
                if (startInfo != null)
                {
                    this.Interpreter.Start(startInfo, this.ReadPreferences());

                    return 0;
                }
            }

            return -1;
        }
    }
}
