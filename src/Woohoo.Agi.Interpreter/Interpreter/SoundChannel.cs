// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

public class SoundChannel
{
    public SoundChannel(byte[] data, int dataIndex, int duration, int dissolveCount, int avail, int freqCount, int toneHandle)
    {
        this.Data = data;
        this.DataIndex = dataIndex;
        this.Duration = duration;
        this.DissolveCount = dissolveCount;
        this.Avail = avail;
        this.FreqCount = freqCount;
        this.ToneHandle = toneHandle;
    }

    public int DataIndex { get; set; }

    public int AttenuationCopy { get; set; }

    public int Attenuation { get; set; }

    public int GenType { get; set; }

    public int FreqCount { get; set; }

    public int Avail { get; set; }

    public int DissolveCount { get; set; }

    public int Duration { get; set; }

    public int ToneHandle { get; }

    public byte[] Data { get; }
}
