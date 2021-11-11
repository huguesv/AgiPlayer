// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl2;

#if USE_SDL2
using Woohoo.Agi.Interpreter;
using static Woohoo.Agi.Player.Backend.Sdl2.NativeMethods;

internal sealed class Sdl2SoundPcmDriver : ISoundPcmDriver
{
    private const int SAMPLESIZE = 11028;

    private static short[] zeroBuffer = new short[SAMPLESIZE];

    private AgiInterpreter interpreter;
    private byte handlesUsed = 0;
    private List<Sdl2SoundChannel> channels = new List<Sdl2SoundChannel>();
    private SDL_AudioCallback sdlCallback;
    private short[] buffer = new short[SAMPLESIZE];
    private short[] channelBuffer = new short[SAMPLESIZE];

    void ISoundPcmDriver.SetInterpreter(AgiInterpreter interpreter)
    {
        this.interpreter = interpreter;
    }

    int ISoundPcmDriver.Initialize(int freq, int format)
    {
        int result = 0;

        this.sdlCallback = new SDL_AudioCallback(this.SdlCallback);

        SDL_AudioSpec desired = new SDL_AudioSpec
        {
            freq = freq,
            format = (ushort)format,
            samples = SAMPLESIZE,
            channels = 1,
            userdata = IntPtr.Zero,
            callback = this.sdlCallback,
        };

        SDL_AudioSpec obtained = default(SDL_AudioSpec);

        //IntPtr desiredPtr = Marshal.AllocHGlobal(Marshal.SizeOf(desired));
        IntPtr obtainedPtr = Marshal.AllocHGlobal(Marshal.SizeOf(obtained));

        try
        {
            //Marshal.StructureToPtr(desired, desiredPtr, false);
            result = SDL_OpenAudio(ref desired, obtainedPtr);
            if (result == 0)
            {
                obtained = (SDL_AudioSpec)Marshal.PtrToStructure(obtainedPtr, typeof(SDL_AudioSpec));

                // pauses callback
                (this as ISoundPcmDriver).SetState(false);
            }
            else
            {
                Trace.WriteLine("pcthis.out_sdl_init(): unable to open audio.");
            }
        }
        finally
        {
            //Marshal.FreeHGlobal(desiredPtr);
            Marshal.FreeHGlobal(obtainedPtr);
        }

        return result;
    }

    void ISoundPcmDriver.Shutdown()
    {
        (this as ISoundPcmDriver).SetState(false);

        this.channels.Clear();

        SDL_CloseAudio();
        SDL_QuitSubSystem(SDL_INIT_AUDIO);
    }

    int ISoundPcmDriver.Open(AudioCallback callback, ToneChannel tc)
    {
        (this as ISoundPcmDriver).Lock();

        Sdl2SoundChannel channel = new Sdl2SoundChannel();
        channel.Handle = this.CreateHandle();
        if (channel.Handle == 0)
        {
            return 0;
        }

        channel.Callback = callback;
        channel.ToneChannel = tc;
        channel.Available = 1;

        this.channels.Add(channel);

        (this as ISoundPcmDriver).Unlock();

        return channel.Handle;
    }

    void ISoundPcmDriver.Close(int handle)
    {
        Sdl2SoundChannel channel = null;
        foreach (Sdl2SoundChannel current in this.channels)
        {
            if (current.Handle == handle)
            {
                channel = current;
                break;
            }
        }

        if (channel is not null)
        {
            this.FreeHandle(channel.Handle);
            this.channels.Remove(channel);
        }
    }

    void ISoundPcmDriver.SetState(bool playing)
    {
        SDL_PauseAudio(playing ? 0 : 1);
    }

    void ISoundPcmDriver.Lock()
    {
        SDL_LockAudio();
    }

    void ISoundPcmDriver.Unlock()
    {
        SDL_UnlockAudio();
    }

    private int CreateHandle()
    {
        int handle = 1;
        byte mask = 0x80;

        while (mask != 0 && (this.handlesUsed & mask) != 0)
        {
            mask >>= 1;
            handle++;
        }

        // if mask == 0 then handles_uses is untouched
        this.handlesUsed |= mask;

        if (mask == 0)
        {
            handle = 0;
        }

        return handle;
    }

    private void FreeHandle(int handle)
    {
        byte mask = 0x80;

        Debug.Assert(handle > 0, "Invalid handle.");
        Debug.Assert(handle <= 8, "Invalid handle.");

        mask >>= handle - 1;
        mask ^= 0xff;

        this.handlesUsed &= mask;
    }

    private void SdlCallback(IntPtr userdata, IntPtr stream, int len)
    {
        bool output = false;

        int numChan = this.channels.Count;
        int count = len / sizeof(short);

        Array.Copy(zeroBuffer, this.buffer, SAMPLESIZE);

        foreach (Sdl2SoundChannel sdlChannel in this.channels)
        {
            if (sdlChannel.Available != 0)
            {
                if (sdlChannel.Callback(sdlChannel.ToneChannel, this.channelBuffer, count) == 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        this.buffer[i] += (short)(this.channelBuffer[i] / numChan);
                    }

                    output = true;
                }
                else
                {
                    sdlChannel.Available = 0;
                }
            }
        }

        Marshal.Copy(this.buffer, 0, stream, count);

        if (!output)
        {
            this.interpreter.SoundManager.Done();
        }
    }
}
#endif
