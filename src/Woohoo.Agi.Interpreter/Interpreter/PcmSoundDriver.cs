// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    public sealed class PcmSoundDriver : ISoundDriver
    {
        public const int GenerateSilence = 0;
        public const int GenerateTone = 1;
        public const int GeneratePeriod = 2;
        public const int GenerateWhite = 3;

        private const int Mult = 111844;
        private const int AudioS16LSB = -32752;
        private const double DefaultVolume = 0x7FFF;

        /// <summary>
        /// Noise feedback for white noise mode.
        /// </summary>
        private const int FeedbackWhiteNoise = 0x12000;

        /// <summary>
        /// Noise feedback for periodic noise mode.
        /// </summary>
        private const int FeedbackPeriodicNoise = 0x08000;

        /// <summary>
        /// Noise generator start preset (for periodic noise).
        /// </summary>
        private const int NoiseGeneratorStartPreset = 0x0f35;

        private static readonly short[] VolumeTable;

        private readonly short[] zeroArray = new short[11028];
        private readonly ISoundPcmDriver pcmDriver;
        private readonly List<ToneChannel> channels;

        private AgiInterpreter interpreter;

        static PcmSoundDriver()
        {
            double v = DefaultVolume;

            VolumeTable = new short[0xf + 1];
            for (int i = 0; i < 0xf; i++)
            {
                VolumeTable[i] = (short)v;
                v /= 1.258925411794;
            }

            VolumeTable[0xf] = 0;
        }

        public PcmSoundDriver(ISoundPcmDriver pcmDriver)
        {
            this.pcmDriver = pcmDriver;
            this.channels = new List<ToneChannel>();
            for (int i = 0; i < this.zeroArray.Length; i++)
            {
                this.zeroArray[i] = 0;
            }
        }

        public void SetInterpreter(AgiInterpreter interpreter)
        {
            this.interpreter = interpreter;
            this.pcmDriver.SetInterpreter(interpreter);
        }

        int ISoundDriver.Initialize()
        {
            if (this.pcmDriver.Initialize(44100, AudioS16LSB) != 0)
            {
                return -1;
            }

            return 0;
        }

        int ISoundDriver.Open(int channel)
        {
            ToneChannel tc = new ToneChannel();

            tc.Attenuation = 0xf; // silence
            tc.AgiChannel = channel;
            tc.FreqCount = 250;
            tc.FreqCountPrev = -1;
            tc.GenType = GenerateTone;
            tc.GenTypePrev = -1;
            tc.NoteCount = 0;
            tc.Avail = 1;

            this.channels.Add(tc);

            tc.Handle = this.pcmDriver.Open(this.TonePcmCallback, tc);
            if (tc.Handle == 0)
            {
                this.channels.Remove(tc);
                return 0;
            }

            return tc.Handle;
        }

        void ISoundDriver.Close(int handle)
        {
            int index = -1;
            for (int i = 0; i < this.channels.Count; i++)
            {
                if (this.channels[i].Handle == handle)
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
            {
                this.pcmDriver.Close(handle);
                this.channels.RemoveAt(index);
            }
        }

        void ISoundDriver.Lock()
        {
            this.pcmDriver.Lock();
        }

        void ISoundDriver.Unlock()
        {
            this.pcmDriver.Unlock();
        }

        void ISoundDriver.SetState(bool playing)
        {
            this.pcmDriver.SetState(playing);
        }

        void ISoundDriver.Shutdown()
        {
            this.pcmDriver.SetState(false);
            this.channels.Clear();
            this.pcmDriver.Shutdown();
        }

        private static int SilenceFill(short[] buffer, int index, int count)
        {
            // Fill with whitespace
            for (int i = index; i < (index + count); i++)
            {
                buffer[i] = 0;
            }

            return count;
        }

        private static int SquareFill(ToneChannel ch, short[] buffer, int index, int count)
        {
            if (ch.GenType != ch.GenTypePrev)
            {
                ch.FreqCountPrev = -1;
                ch.Noise.Sign = 1;
                ch.GenTypePrev = ch.GenType;
            }

            if (ch.FreqCount != ch.FreqCountPrev)
            {
                ch.Noise.Scale = (44100 / 2) * ch.FreqCount;
                ch.Noise.Count = ch.Noise.Scale;
                ch.FreqCountPrev = ch.FreqCount;
            }

            for (int i = index; i < (index + count); i++)
            {
                buffer[i] = (ch.Noise.Sign != 0) ? VolumeTable[ch.Attenuation] : (short)(0 - VolumeTable[ch.Attenuation]);

                // get next sample
                ch.Noise.Count -= Mult;
                while (ch.Noise.Count <= 0)
                {
                    ch.Noise.Sign ^= 1;
                    ch.Noise.Count += ch.Noise.Scale;
                }
            }

            return count;
        }

        private static int NoiseFill(ToneChannel ch, short[] buffer, int index, int count)
        {
            if (ch.GenType != ch.GenTypePrev)
            {
                ch.FreqCountPrev = -1;
                ch.GenTypePrev = ch.GenType;
            }

            if (ch.FreqCount != ch.FreqCountPrev)
            {
                ch.Noise.Scale = (44100 / 2) * ch.FreqCount;
                ch.Noise.Count = ch.Noise.Scale;
                ch.FreqCountPrev = ch.FreqCount;
                ch.Noise.Feedback = ch.GenType == GenerateWhite ? FeedbackWhiteNoise : FeedbackPeriodicNoise;

                // reset noise shifter
                ch.Noise.NoiseState = NoiseGeneratorStartPreset;
                ch.Noise.Sign = (int)(ch.Noise.NoiseState & 1);
            }

            for (int i = index; i < (index + count); i++)
            {
                buffer[i] = (ch.Noise.Sign != 0) ? VolumeTable[ch.Attenuation] : (short)(0 - VolumeTable[ch.Attenuation]);

                // get next sample
                ch.Noise.Count -= Mult;
                while (ch.Noise.Count <= 0)
                {
                    if ((ch.Noise.NoiseState & 1) != 0)
                    {
                        ch.Noise.NoiseState ^= (uint)ch.Noise.Feedback;
                    }

                    ch.Noise.NoiseState >>= 1;
                    ch.Noise.Sign = (int)(ch.Noise.NoiseState & 1);
                    ch.Noise.Count += ch.Noise.Scale;
                }
            }

            return count;
        }

        private int TonePcmCallback(ToneChannel ch, short[] buffer, int count)
        {
            int result = -1;
            int index = 0;

            while (count > 0)
            {
                if (ch.NoteCount <= 0)
                {
                    // Get new tone data
                    Tone tone = new Tone();
                    tone.FrequencyCount = 0;
                    tone.Attenuation = 0x0f;
                    tone.Type = GenerateTone;

                    if (ch.Avail != 0 && this.interpreter.SoundManager.FillChannel(ch.AgiChannel, tone) == 0)
                    {
                        ch.Attenuation = tone.Attenuation;
                        ch.FreqCount = tone.FrequencyCount;
                        ch.GenType = tone.Type;

                        // setup counters 'n stuff
                        // 44100 samples per sec.. tone changes 60 times per sec
                        ch.NoteCount = 44100 / 60;

                        result = 0;
                    }
                    else
                    {
                        ch.GenType = GenerateSilence;
                        ch.NoteCount = count;
                        ch.Avail = 0;
                    }
                }

                // Write nothing
                if (ch.FreqCount == 0 || ch.Attenuation == 0x0f)
                {
                    ch.GenType = GenerateSilence;
                }

                // Find which is smaller, the buffer or the note count
                int fillSize = ch.NoteCount < count ? ch.NoteCount : count;

                switch (ch.GenType)
                {
                    case GenerateTone:
                        fillSize = PcmSoundDriver.SquareFill(ch, buffer, index, fillSize);
                        break;
                    case GeneratePeriod:
                    case GenerateWhite:
                        fillSize = PcmSoundDriver.NoiseFill(ch, buffer, index, fillSize);
                        break;
                    case GenerateSilence:
                    default:
                        fillSize = PcmSoundDriver.SilenceFill(buffer, index, fillSize);
                        break;
                }

                ch.NoteCount -= fillSize;
                count -= fillSize;
                index += fillSize;
            }

            return result;
        }
    }
}
