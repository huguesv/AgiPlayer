// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

public class SoundChannel
{
    public int DataIndex { get; set; }

    public int AttenuationCopy { get; set; }

    public int Attenuation { get; set; }

    public int GenType { get; set; }

    public int ToneHandle { get; set; }

    public int FreqCount { get; set; }

    public int Avail { get; set; }

    public int DissolveCount { get; set; }

    public int Duration { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Direct access to array items.")]
    public byte[] Data { get; set; }
}
