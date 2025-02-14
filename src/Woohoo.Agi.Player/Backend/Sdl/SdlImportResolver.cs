// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl;

using System;

public static class SdlImportResolver
{
    private static readonly Lock InitializeLock = new();
    private static readonly Lock LoadLock = new();
    private static readonly Dictionary<string, nint> LoadedLibraries = [];

    private static bool initialized;

    public static void Initialize()
    {
        lock (InitializeLock)
        {
            if (initialized)
            {
                return;
            }

            NativeLibrary.SetDllImportResolver(typeof(SdlImportResolver).Assembly, (libraryName, assembly, searchPath) =>
            {
                lock (LoadLock)
                {
                    if (LoadedLibraries.TryGetValue(libraryName, out var lib))
                    {
                        return lib;
                    }

                    var loaded = libraryName switch
                    {
                        "SDL" => ResolveSdl(assembly, searchPath),
                        _ => nint.Zero,
                    };

                    LoadedLibraries.Add(libraryName, loaded);

                    return loaded;
                }
            });

            initialized = true;
        }
    }

    private static nint ResolveSdl(Assembly assembly, DllImportSearchPath? searchPath)
    {
        var archName = RuntimeInformation.OSArchitecture switch
        {
            Architecture.X64 => "x64",
            Architecture.X86 => "x86",
            _ => throw new PlatformNotSupportedException(),
        };

        var exeFolder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName
            ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            ?? Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location))
            ?? throw new InvalidOperationException("Could not determine executable folder");

        var libraryPath = Path.Combine(exeFolder, "runtimes", $"win-{archName}", "native", "SDL.dll");

        return File.Exists(libraryPath) ? NativeLibrary.Load(libraryPath, assembly, searchPath) : nint.Zero;
    }
}
