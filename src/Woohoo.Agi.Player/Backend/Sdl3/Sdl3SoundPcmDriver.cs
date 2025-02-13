// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl3;

#if USE_SDL3

using System;
using System.Collections.Generic;
using System.IO;
using Woohoo.Agi.Engine.Interpreter;
using static SDL3.SDL;

internal class Sdl3SoundPcmDriver : ISoundPcmDriver
{
    private const int SAMPLESIZE = 11028;

    private static readonly short[] ZeroBuffer = new short[SAMPLESIZE];

    private readonly List<Sdl3SoundChannel> channels = [];
    private readonly short[] channelBuffer = new short[SAMPLESIZE];
    private readonly short[] buffer = new short[SAMPLESIZE];

    private AgiInterpreter interpreter;
    private byte handlesUsed = 0;
    private SDL_AudioStreamCallback sdlCallback;
    private uint deviceId;
    private nint streamDevice;

    public void SetInterpreter(AgiInterpreter interpreter)
    {
        this.interpreter = interpreter;
    }

    public int Initialize(int freq, int format)
    {
        this.deviceId = SDL_AUDIO_DEVICE_DEFAULT_PLAYBACK;

        SDL_AudioSpec spec = new SDL_AudioSpec
        {
            freq = freq,
            format = SDL_AudioFormat.SDL_AUDIO_S16, // TODO: we're ignoring passed parameter here
            channels = 1,
        };

        // Hold onto a reference to the callback so it doesn't get garbage collected
        this.sdlCallback = this.SdlCallback;
        this.streamDevice = SDL_OpenAudioDeviceStream(this.deviceId, ref spec, this.sdlCallback, 0);

        return 0;
    }

    public void Shutdown()
    {
        (this as ISoundPcmDriver).SetState(false);

        this.channels.Clear();

        SDL_CloseAudioDevice(this.deviceId);
    }

    public int Open(AudioCallback callback, ToneChannel tc)
    {
        (this as ISoundPcmDriver).Lock();

        var channel = new Sdl3SoundChannel
        {
            Handle = this.CreateHandle(),
        };

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

    public void Close(int handle)
    {
        Sdl3SoundChannel channel = null;
        foreach (var current in this.channels)
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

    public void SetState(bool playing)
    {
        if (playing)
        {
            SDL_ResumeAudioStreamDevice(this.streamDevice);
        }
        else
        {
            SDL_PauseAudioStreamDevice(this.streamDevice);
        }
    }

    public void Lock()
    {
        // Not needed
    }

    public void Unlock()
    {
        // Not needed
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

    private void SdlCallback(nint userdata, nint stream, int additionalAmount, int totalAmount)
    {
        bool output = false;

        int numChan = this.channels.Count;
        int count = additionalAmount / sizeof(short);

        Array.Copy(ZeroBuffer, this.buffer, SAMPLESIZE);

        foreach (var sdlChannel in this.channels)
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

        var result = SDL_PutAudioStreamData(this.streamDevice, this.buffer, count);

        if (!output)
        {
            this.interpreter.SoundManager.Done();
        }
    }
}

#endif // USE_SDL3
