// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

using Woohoo.Agi.Resources;

public class SoundManager
{
    private readonly short[] dissolveDataV2 = new short[]
    {
        -2, -3, -2, -1, 0x00, 0x00, 0x01, 0x01, 0x01,
        0x01, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02,
        0x02, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03,
        0x04, 0x04, 0x04, 0x04, 0x05, 0x05, 0x05, 0x05,
        0x06, 0x06, 0x06, 0x06, 0x06, 0x07, 0x07, 0x07,
        0x07, 0x08, 0x08, 0x08, 0x08, 0x09, 0x09, 0x09,
        0x09, 0x0A, 0x0A, 0x0A, 0x0A, 0x0B, 0x0B, 0x0B,
        0x0B, 0x0B, 0x0B, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C,
        0x0C, 0x0D, -100,
    };

    private readonly short[] dissolveDataV3 = new short[]
    {
        -2, -3, -2, -1,
        0, 0, 0, 0, 0,
        1, 1, 1, 1, 1, 1,
        2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
        3, 3, 3, 3, 3, 3, 3, 3,
        4, 4, 4, 4, 4,
        5, 5, 5, 5, 5,
        6, 6, 6, 6, 6,
        7, 7, 7, 7,
        8, 8, 8, 8,
        9, 9, 9, 9,
        0xA, 0x0A, 0x0A, 0xA,
        0x0B, 0x0B, 0x0B, 0x0B, 0x0B, 0x0B,
        0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C,
        0x0D,
        -100,
    };

    private SoundChannel[] soundChannels;
    private bool soundPlaying;

    public SoundManager(AgiInterpreter interpreter)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
        this.soundChannels = Array.Empty<SoundChannel>();
    }

    public byte SoundFlag { get; set; }

    protected AgiInterpreter Interpreter { get; }

    protected State State => this.Interpreter.State;

    protected Preferences Preferences => this.Interpreter.Preferences;

    protected ISoundDriver SoundDriver => this.Interpreter.SoundDriver;

    public void StopPlaying()
    {
        if (this.soundPlaying)
        {
            this.Stop();
        }
    }

    public void Initialize()
    {
        if (this.Preferences.SoundHardwareEnabled)
        {
            if (this.SoundDriver.Initialize() != 0)
            {
                this.Preferences.SoundHardwareEnabled = false;
            }
            else
            {
                this.SoundDriver.SetState(false);
            }
        }
    }

    public void Shutdown()
    {
        if (this.Preferences.SoundHardwareEnabled)
        {
            this.Stop();
            this.SoundDriver.Shutdown();
        }
    }

    public void Play(SoundResource resource)
    {
        if (resource is null)
        {
            throw new ArgumentNullException("resource");
        }

        // Playing sounds on AppleIIgs is not supported
        if (this.Preferences.SoundHardwareEnabled && this.Interpreter.GameInfo.Platform != Platform.AppleIIgs)
        {
            this.soundChannels = this.Preferences.SoundSingleChannel ? new SoundChannel[1] : new SoundChannel[4];

            for (int i = 0; i < this.soundChannels.Length; i++)
            {
                var channel = new SoundChannel(
                    data: resource.GetChannelData(i),
                    dataIndex: 0,
                    duration: 0,
                    dissolveCount: 0xffff,
                    avail: 0xffff,
                    freqCount: 0,
                    toneHandle: this.SoundDriver.Open(i));

                if (channel.ToneHandle == 0)
                {
                    Trace.WriteLine("sndgen_play: error opening tone channel.");

                    this.Shutdown();
                    this.Preferences.SoundHardwareEnabled = false;
                    return;
                }

                this.soundChannels[i] = channel;
            }

            this.soundPlaying = true;

            this.SoundDriver.SetState(true);
        }
        else
        {
            this.State.Flags[this.SoundFlag] = true;
        }
    }

    public void Done()
    {
        this.soundPlaying = false;
        this.SoundDriver.SetState(false);
        if (this.soundChannels is not null)
        {
            for (int i = 0; i < this.soundChannels.Length; i++)
            {
                if (this.soundChannels[i].ToneHandle != 0)
                {
                    this.SoundDriver.Close(this.soundChannels[i].ToneHandle);
                }
            }
        }

        this.State.Flags[this.SoundFlag] = true;
    }

    internal int FillChannel(int channel, Tone tone)
    {
        if (!this.State.Flags[Flags.SoundOn])
        {
            return -1;
        }

        var soundChannel = this.soundChannels[channel];
        if (soundChannel.Avail == 0)
        {
            return -1;
        }

        byte[] soundData = soundChannel.Data;

        while (soundChannel.Duration == 0 && soundChannel.Duration != 0xffff)
        {
            int offset = soundChannel.DataIndex;

            if (offset >= (soundData.Length - 2))
            {
                soundChannel.Duration = 0xffff;
                break;
            }

            soundChannel.Duration = (soundData[offset + 1] * 0x100) + soundData[offset];
            if (soundChannel.Duration != 0 && soundChannel.Duration != 0xffff)
            {
                // Only tone channels dissolve
                if (channel != 3 && this.Preferences.SoundDissolve != 0)
                {
                    soundChannel.DissolveCount = 0;
                }

                // Volume
                soundChannel.Attenuation = soundData[offset + 4] & 0x0f;

                // Frequency
                if (channel < 3)
                {
                    soundChannel.FreqCount = (ushort)(soundData[offset + 2] & 0x3f);
                    soundChannel.FreqCount <<= 4;
                    soundChannel.FreqCount |= (byte)(soundData[offset + 3] & 0x0f);
                    soundChannel.GenType = PcmSoundDriver.GenerateTone;
                }
                else
                {
                    byte noiseType = (byte)(soundData[offset + 3] & 0x04);
                    soundChannel.GenType = noiseType != 0 ? PcmSoundDriver.GenerateWhite : PcmSoundDriver.GeneratePeriod;

                    int noiseFreq = (byte)(soundData[offset + 3] & 0x03);
                    switch (noiseFreq)
                    {
                        case 0:
                            soundChannel.FreqCount = 32;
                            break;
                        case 1:
                            soundChannel.FreqCount = 64;
                            break;
                        case 2:
                            soundChannel.FreqCount = 128;
                            break;
                        case 3:
                            soundChannel.FreqCount = this.soundChannels[2].FreqCount * 2;
                            break;
                    }
                }
            }

            soundChannel.DataIndex += 5;
        }

        if (soundChannel.Duration != 0xffff)
        {
            tone.FrequencyCount = soundChannel.FreqCount;
            tone.Attenuation = this.CalculateVolume(soundChannel);
            tone.Type = soundChannel.GenType;
            soundChannel.Duration--;
        }
        else
        {
            soundChannel.Avail = 0;
            soundChannel.Attenuation = 0x0f; // silent
            soundChannel.AttenuationCopy = 0x0f;
            return -1;
        }

        return 0;
    }

    private void Stop()
    {
        if (this.Preferences.SoundHardwareEnabled)
        {
            this.SoundDriver.Lock();
            this.Done();
            this.SoundDriver.Unlock();
        }
    }

    private int CalculateVolume(SoundChannel soundChannel)
    {
        short[] dissolveData;

        switch (this.Preferences.SoundDissolve)
        {
            case 2:
                dissolveData = this.dissolveDataV2;
                break;
            case 3:
            default:
                dissolveData = this.dissolveDataV3;
                break;
        }

        int volume = soundChannel.Attenuation;
        if (volume != 0x0f)
        {
            if (soundChannel.DissolveCount != 0xffff)
            {
                int val = dissolveData[soundChannel.DissolveCount];
                if (val == -100)
                {
                    // end of list
                    soundChannel.DissolveCount = 0xffff;
                    soundChannel.Attenuation = soundChannel.AttenuationCopy;
                    volume = soundChannel.Attenuation;
                }
                else
                {
                    soundChannel.DissolveCount++;

                    volume += val;
                    if (volume < 0)
                    {
                        volume = 0;
                    }

                    if (volume > 0x0f)
                    {
                        volume = 0x0f;
                    }

                    soundChannel.AttenuationCopy = volume;

                    volume &= 0x0f;
                    if (this.Preferences.SoundReadVariable)
                    {
                        volume += this.State.Variables[Variables.SoundVolume];
                    }

                    if (volume > 0x0f)
                    {
                        volume = 0x0f;
                    }
                }
            }

            if (volume < 8)
            {
                volume += 2;
            }
        }

        return volume;
    }
}
