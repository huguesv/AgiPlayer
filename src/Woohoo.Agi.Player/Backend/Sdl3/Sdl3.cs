// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#define USE_ALLOC_FREE_STREAMDATA_API

#pragma warning disable SA1310 // Field names should not contain underscore

namespace SDL3;

#if USE_SDL3

#if !USE_ALLOC_FREE_STREAMDATA_API
using System.Runtime.InteropServices;
#endif

public static unsafe partial class SDL
{
    public const uint SDL_AUDIO_DEVICE_DEFAULT_PLAYBACK = 0xFFFFFFFFu;

    public static nint SDL_CreateTextureAsNint(IntPtr renderer, SDL_PixelFormat format, SDL_TextureAccess access, int w, int h)
    {
        unsafe
        {
            return (nint)SDL_CreateTexture(renderer, format, access, w, h);
        }
    }

    public static SDL.SDLBool SDL_PutAudioStreamData(nint stream, float[] data, int count)
    {
#if USE_ALLOC_FREE_STREAMDATA_API
        unsafe
        {
            fixed (float* p = data)
            {
                return SDL.SDL_PutAudioStreamData(stream, (nint)p, count * sizeof(float));
            }
        }
#else
        GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        try
        {
            nint ptr = handle.AddrOfPinnedObject();
            return SDL.SDL_PutAudioStreamData(stream, ptr, count * sizeof(float));
        }
        finally
        {
            handle.Free();
        }
#endif
    }

    public static SDL.SDLBool SDL_PutAudioStreamData(nint stream, double[] data, int count)
    {
#if USE_ALLOC_FREE_STREAMDATA_API
        unsafe
        {
            fixed (double* p = data)
            {
                return SDL.SDL_PutAudioStreamData(stream, (nint)p, count * sizeof(double));
            }
        }
#else
        GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        try
        {
            nint ptr = handle.AddrOfPinnedObject();
            return SDL.SDL_PutAudioStreamData(stream, ptr, count * sizeof(double));
        }
        finally
        {
            handle.Free();
        }
#endif
    }

    public static SDL.SDLBool SDL_PutAudioStreamData(nint stream, int[] data, int count)
    {
#if USE_ALLOC_FREE_STREAMDATA_API
        unsafe
        {
            fixed (int* p = data)
            {
                return SDL.SDL_PutAudioStreamData(stream, (nint)p, count * sizeof(int));
            }
        }
#else
        GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        try
        {
            nint ptr = handle.AddrOfPinnedObject();
            return SDL.SDL_PutAudioStreamData(stream, ptr, count * sizeof(int));
        }
        finally
        {
            handle.Free();
        }
#endif
    }

    public static SDL.SDLBool SDL_PutAudioStreamData(nint stream, short[] data, int count)
    {
#if USE_ALLOC_FREE_STREAMDATA_API
        unsafe
        {
            fixed (short* p = data)
            {
                return SDL.SDL_PutAudioStreamData(stream, (nint)p, count * sizeof(short));
            }
        }
#else
        GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        try
        {
            nint ptr = handle.AddrOfPinnedObject();
            return SDL.SDL_PutAudioStreamData(stream, ptr, count * sizeof(short));
        }
        finally
        {
            handle.Free();
        }
#endif
    }

    public static uint[] SDL_GetAudioPlaybackDevicesAsArray()
    {
        uint[] devices = [];
        unsafe
        {
            nint count = SDL.SDL_GetAudioPlaybackDevices(out int playbackCount);
            if (count == 0)
            {
                return devices;
            }

            devices = new uint[playbackCount];
            uint* ptr = (uint*)new IntPtr(count).ToPointer();
            for (int i = 0; i < playbackCount; i++)
            {
                devices[i] = ptr[i];
            }

            SDL.SDL_free(count);
        }

        return devices;
    }
}

#pragma warning restore SA1310 // Field names should not contain underscore

#endif // USE_SDL_3
