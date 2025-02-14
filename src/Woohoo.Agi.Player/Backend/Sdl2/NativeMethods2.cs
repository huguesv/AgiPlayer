// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl2;

#if USE_SDL2

#pragma warning disable SA1120

public static partial class NativeMethods
{
    //
    // Summary:
    //     int (SDLCALL *fn)(void *)
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int ThreadDelegate();

    //
    // Summary:
    //     Create thread
    //
    // Returns:
    //     IntPtr to SDL_Thread struct
    //
    // Remarks:
    //     Binds to C-function call in SDL_thread.h: extern DECLSPEC SDL_Thread * SDLCALL
    //     SDL_CreateThread(int (SDLCALL *fn)(void *), const char *name, void *data)
    [SuppressUnmanagedCodeSecurity]
    [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SDL_CreateThread(ThreadDelegate fn, IntPtr name, object data);

    //
    // Summary:
    //     Wait for a thread to finish. The return code for the thread function is placed
    //     in the area pointed to by 'status', if 'status' is not NULL.
    //
    // Remarks:
    //     Binds to C-function call in SDL_thread.h: extern DECLSPEC void SDLCALL SDL_WaitThread(SDL_Thread
    //     *thread, int *status)
    [SuppressUnmanagedCodeSecurity]
    [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_WaitThread(IntPtr thread, out int status);
}

#pragma warning restore SA1120

#endif
