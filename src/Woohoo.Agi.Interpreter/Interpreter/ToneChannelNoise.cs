// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    using System;

    public class ToneChannelNoise
    {
        public int Feedback { get; set; }

        [CLSCompliant(false)]
        public uint NoiseState { get; set; }

        // TODO: should this be a bool?
        public int Sign { get; set; }

        public int Scale { get; set; }

        public int Count { get; set; }
    }
}
