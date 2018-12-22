// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    public class ToneChannel
    {
        public ToneChannel()
        {
            this.Noise = new ToneChannelNoise();
        }

        public ToneChannelNoise Noise { get; }

        public int GenTypePrev { get; set; }

        public int GenType { get; set; }

        public int Attenuation { get; set; }

        public int FreqCountPrev { get; set; }

        public int FreqCount { get; set; }

        public int NoteCount { get; set; }

        public int Avail { get; set; }

        public int AgiChannel { get; set; }

        public int Handle { get; set; }
    }
}
