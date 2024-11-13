// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// Interpreter state, this is serialized in saved games.
/// </summary>
public class State
{
    public const int DefaultStatusLineRow = 21;

    private const int VariableCount = 256;
    private const int FlagCount = 256;
    private const int StringCount = 24;
    private const int ControlCount = 50;
    private const int DefaultScriptSize = 50;
    private const int DefaultInputPosition = 23;

    /// <summary>
    /// Initializes a new instance of the <see cref="State"/> class.
    /// </summary>
    public State()
    {
        this.Id = string.Empty;
        this.Variables = new byte[VariableCount];
        this.Flags = new bool[FlagCount];
        this.Strings = new string[StringCount];
        for (int s = 0; s < this.Strings.Length; s++)
        {
            this.Strings[s] = string.Empty;
        }

        this.Controls = new ControlEntry[ControlCount];
        for (int c = 0; c < this.Controls.Length; c++)
        {
            this.Controls[c] = new ControlEntry();
        }

        this.ScriptSize = DefaultScriptSize;
        this.InputPosition = DefaultInputPosition;
        this.StatusLineRow = DefaultStatusLineRow;
        this.MenuEnabled = true;
        this.Cursor = string.Empty;
    }

    /// <summary>
    /// Gets or sets game id.
    /// </summary>
    public string Id { get; set; }

    public byte[] Variables { get; set; }

    public bool[] Flags { get; set; }

    public uint Ticks { get; set; }

    public int Horizon { get; set; }

    public int BlockX1 { get; set; }

    public int BlockY1 { get; set; }

    public int BlockX2 { get; set; }

    public int BlockY2 { get; set; }

    public EgoControl EgoControl { get; set; }

    public bool BlockIsSet { get; set; }

    public int ScriptSize { get; set; }

    public int ScriptCount { get; set; }

    public int ScriptSaved { get; set; }

    public ControlEntry[] Controls { get; set; }

    public string[] Strings { get; set; }

    public byte TextForeground { get; set; }

    public byte TextBackground { get; set; }

    public byte TextCombine { get; set; }

    public bool InputEnabled { get; set; }

    public byte InputPosition { get; set; }

    public string Cursor { get; set; }

    public byte StatusLineRow { get; set; }

    public bool StatusVisible { get; set; }

    public int WindowRowMin { get; set; }

    public int WindowRowMax { get; set; }

    public bool MenuEnabled { get; set; }

    public WalkMode WalkMode { get; set; }
}
