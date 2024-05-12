// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

/// <summary>
/// Random number generator.
/// </summary>
public class Random
{
    private const double TandyClockPerSec = 20.0; // For reference, 18.2 for IBM

    private ushort agiRandomSeed;

    /// <summary>
    /// Initializes a new instance of the <see cref="Random"/> class.
    /// </summary>
    public Random()
    {
    }

    /// <summary>
    /// Get a random number.
    /// </summary>
    /// <returns>Random number (0-255).</returns>
    public byte Next()
    {
        // If the seed was never initialized, or has become 0, we get a new seed
        if (this.agiRandomSeed == 0)
        {
            this.agiRandomSeed = (ushort)(DateTime.Today.Ticks * TandyClockPerSec);
        }

        this.agiRandomSeed = (ushort)((0x7c4d * this.agiRandomSeed) + 1);

        byte rnd = (byte)(this.agiRandomSeed ^ (this.agiRandomSeed >> 8));

        return rnd;
    }
}
