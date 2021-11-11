// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

public class ScriptManager
{
    private bool scriptWriteAllowed;
    private int scriptNextIndex;
    private int scriptIteratorIndex;
    private int scriptMemory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScriptManager"/> class.
    /// </summary>
    /// <param name="interpreter">Interpreter.</param>
    /// <param name="error">Error handler.</param>
    public ScriptManager(AgiInterpreter interpreter, AgiError error)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
        this.Error = error ?? throw new ArgumentNullException(nameof(error));
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Direct access to array items.")]
    public byte[] ScriptData { get; private set; }

    protected AgiInterpreter Interpreter { get; }

    protected AgiError Error { get; }

    protected State State => this.Interpreter.State;

    public void SetNextIndex(int nextIndex)
    {
        this.scriptNextIndex = nextIndex;
    }

    /// <summary>
    /// Clear the script.  Usually done when a new room is loaded.
    /// </summary>
    public void Clear()
    {
        if (this.State.ScriptSize > 0 && this.ScriptData == null)
        {
            this.ScriptData = new byte[this.State.ScriptSize * 2];
        }

        this.scriptNextIndex = 0;
        this.State.ScriptCount = 0;
    }

    /// <summary>
    /// Allow writing of scripts.
    /// </summary>
    public void Allow()
    {
        this.scriptWriteAllowed = true;
    }

    /// <summary>
    /// Disallow writing of scripts.
    /// </summary>
    public void Block()
    {
        this.scriptWriteAllowed = false;
    }

    public void Write(byte script, byte resourceIndex)
    {
        if (!this.State.Flags[Flags.ScriptBlock])
        {
            if (this.scriptWriteAllowed)
            {
                if (this.scriptNextIndex >= (this.State.ScriptSize * 2))
                {
                    this.Error(ErrorCodes.ScriptFull, this.scriptMemory);
                }

                this.ScriptData[this.scriptNextIndex++] = script;
                this.ScriptData[this.scriptNextIndex++] = resourceIndex;
                this.State.ScriptCount++;
            }

            if ((this.scriptNextIndex / 2) > this.scriptMemory)
            {
                this.scriptMemory = this.scriptNextIndex / 2;
            }
        }
    }

    public void ResetIterator()
    {
        this.scriptIteratorIndex = 0;
        this.scriptNextIndex = this.State.ScriptCount * 2;
    }

    public byte[] IncrementIterator()
    {
        byte[] data = null;

        if (this.scriptIteratorIndex < this.scriptNextIndex)
        {
            data = new byte[2];
            data[0] = this.ScriptData[this.scriptIteratorIndex++];
            data[1] = this.ScriptData[this.scriptIteratorIndex++];
        }

        return data;
    }
}
