// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// Priority table contains the priority for each vertical position in the picture.
/// </summary>
public class PriorityTable
{
    private readonly byte[] priorityTable;

    private bool defaultPriority;

    /// <summary>
    /// Initializes a new instance of the <see cref="PriorityTable"/> class.
    /// </summary>
    public PriorityTable()
    {
        // TODO: not sure why the size is 172 instead of 168
        this.priorityTable = new byte[172];
        this.defaultPriority = true;

        int i = 0;
        while (i < 48)
        {
            this.priorityTable[i++] = 4;
        }

        for (byte priority = 5; priority < 15; priority++)
        {
            for (int count = 0; count < 12; count++)
            {
                this.priorityTable[i++] = priority;
            }
        }
    }

    public void SetBasePriority(byte basePriority)
    {
        if (basePriority == 168)
        {
            throw new ArgumentOutOfRangeException(nameof(basePriority));
        }

        this.defaultPriority = false;

        for (int i = 0; i < 168; i++)
        {
            byte priority = 4;

            int temp = i - basePriority;
            if (temp >= 0)
            {
                priority = (byte)((temp * 10 / (168 - basePriority)) + 5);
                if (priority > 0xf)
                {
                    priority = 0xf;
                }
            }

            this.priorityTable[i] = priority;
        }
    }

    /// <summary>
    /// Get the priority at the specified vertical position.
    /// </summary>
    /// <param name="y">Vertical position in picture (0-167).</param>
    /// <returns>Priority.</returns>
    public byte GetPriorityAt(int y)
    {
        return this.priorityTable[y];
    }

    public int CalculateSortPosition(byte priority)
    {
        int sortPosition;

        if (this.defaultPriority)
        {
            sortPosition = ((priority - 5) * 12) + 0x30;
        }
        else
        {
            sortPosition = 0xa7;
            while (this.priorityTable[sortPosition] >= priority)
            {
                sortPosition--;
                if (sortPosition < 0)
                {
                    return -1;
                }
            }
        }

        return sortPosition;
    }
}
