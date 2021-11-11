// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

using Woohoo.Agi.Resources;

/// <summary>
/// Saved game serializer which uses xml text format.
/// </summary>
public sealed class SavedGameXmlSerializer : ISavedGameSerializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SavedGameXmlSerializer"/> class.
    /// </summary>
    public SavedGameXmlSerializer()
    {
    }

    /// <summary>
    /// Gets or sets interpreter whose state is to be saved/restored.
    /// </summary>
    private AgiInterpreter Interpreter { get; set; }

    void ISavedGameSerializer.SaveTo(AgiInterpreter interpreter, string description, Stream stream)
    {
        if (description is null)
        {
            throw new ArgumentNullException(nameof(description));
        }

        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));

        var xmlWriter = new XmlTextWriter(stream, Encoding.UTF8)
        {
            Formatting = Formatting.Indented,
        };

        this.SerializeGame(xmlWriter, description);

        xmlWriter.Flush();
    }

    void ISavedGameSerializer.LoadFrom(AgiInterpreter interpreter, Stream stream)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));

        var settings = new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
            IgnoreWhitespace = true,
        };

        var reader = XmlReader.Create(stream, settings);

        this.DeserializeGame(reader);
    }

    string ISavedGameSerializer.LoadDescriptionFrom(Stream stream)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        string description = string.Empty;

        try
        {
            var doc = new XmlDocument();
            doc.Load(stream);

            var gameNode = doc.SelectSingleNode("/SavedGame");

            description = gameNode.Attributes["Description"].Value;
        }
        catch (XmlException)
        {
        }

        return description;
    }

    private static byte ConvertHexToByte(char hex)
    {
        if (hex < '0' || hex > 'f')
        {
            throw new ArgumentOutOfRangeException(nameof(hex));
        }

        if (hex >= '0' && hex <= '9')
        {
            return (byte)(hex - '0');
        }

        return (byte)(hex - 'a' + 0x0a);
    }

    private static char ConvertByteToHex(byte b)
    {
        if (b > 0x0f)
        {
            throw new ArgumentOutOfRangeException(nameof(b));
        }

        if (b < 0x0a)
        {
            return (char)('0' + b);
        }

        return (char)('a' + (b - 0x0a));
    }

    private void SerializeGame(XmlWriter xmlWriter, string description)
    {
        xmlWriter.WriteStartDocument();
        xmlWriter.WriteStartElement("SavedGame");
        xmlWriter.WriteAttributeString("Description", description);

        this.SerializeState(xmlWriter);
        this.SerializeObjectTable(xmlWriter);
        this.SerializeInventory(xmlWriter);
        this.SerializeScript(xmlWriter);
        this.SerializeLogicScanStart(xmlWriter);

        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndDocument();
    }

    private void SerializeState(XmlWriter xmlWriter)
    {
        xmlWriter.WriteStartElement("State");

        xmlWriter.WriteElementString("BlockIsSet", this.Interpreter.State.BlockIsSet.ToString());
        xmlWriter.WriteElementString("BlockX1", this.Interpreter.State.BlockX1.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("BlockX2", this.Interpreter.State.BlockX2.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("BlockY1", this.Interpreter.State.BlockY1.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("BlockY2", this.Interpreter.State.BlockY2.ToString(CultureInfo.InvariantCulture));

        xmlWriter.WriteStartElement("Controls");

        for (int i = 0; i < this.Interpreter.State.Controls.Length; i++)
        {
            xmlWriter.WriteStartElement("Control");
            xmlWriter.WriteAttributeString("Index", i.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("Key", this.Interpreter.State.Controls[i].Key.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("Number", this.Interpreter.State.Controls[i].Number.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();
        }

        xmlWriter.WriteEndElement();

        xmlWriter.WriteElementString("Cursor", this.Interpreter.State.Cursor);
        xmlWriter.WriteElementString("EgoControl", this.Interpreter.State.EgoControl.ToString());

        xmlWriter.WriteStartElement("Flags");

        for (int i = 0; i < this.Interpreter.State.Flags.Length; i++)
        {
            xmlWriter.WriteStartElement("Flag");
            xmlWriter.WriteAttributeString("Index", i.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("Value", this.Interpreter.State.Flags[i].ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();
        }

        xmlWriter.WriteEndElement();

        xmlWriter.WriteElementString("Horizon", this.Interpreter.State.Horizon.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("Id", this.Interpreter.State.Id);
        xmlWriter.WriteElementString("InputEnabled", this.Interpreter.State.InputEnabled.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("InputPosition", this.Interpreter.State.InputPosition.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("MenuEnabled", this.Interpreter.State.MenuEnabled.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("ScriptCount", this.Interpreter.State.ScriptCount.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("ScriptSaved", this.Interpreter.State.ScriptSaved.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("ScriptSize", this.Interpreter.State.ScriptSize.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("StatusLineRow", this.Interpreter.State.StatusLineRow.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("StatusVisible", this.Interpreter.State.StatusVisible.ToString(CultureInfo.InvariantCulture));

        xmlWriter.WriteStartElement("Strings");

        for (int i = 0; i < this.Interpreter.State.Strings.Length; i++)
        {
            xmlWriter.WriteStartElement("String");
            xmlWriter.WriteAttributeString("Index", i.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("Value", this.Interpreter.State.Strings[i]);
            xmlWriter.WriteEndElement();
        }

        xmlWriter.WriteEndElement();

        xmlWriter.WriteElementString("TextBackground", this.Interpreter.State.TextBackground.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("TextCombine", this.Interpreter.State.TextCombine.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("TextForeground", this.Interpreter.State.TextForeground.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("Ticks", this.Interpreter.State.Ticks.ToString(CultureInfo.InvariantCulture));

        xmlWriter.WriteStartElement("Variables");

        for (int i = 0; i < this.Interpreter.State.Variables.Length; i++)
        {
            xmlWriter.WriteStartElement("Variable");
            xmlWriter.WriteAttributeString("Index", i.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("Value", this.Interpreter.State.Variables[i].ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();
        }

        xmlWriter.WriteEndElement();

        xmlWriter.WriteElementString("WalkMode", this.Interpreter.State.WalkMode.ToString());
        xmlWriter.WriteElementString("WindowRowMax", this.Interpreter.State.WindowRowMax.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteElementString("WindowRowMin", this.Interpreter.State.WindowRowMin.ToString(CultureInfo.InvariantCulture));
        xmlWriter.WriteEndElement();
    }

    private void SerializeObjectTable(XmlWriter xmlWriter)
    {
        xmlWriter.WriteStartElement("ObjectTable");

        for (int i = 0; i < this.Interpreter.ObjectTable.Length; i++)
        {
            var view = this.Interpreter.ObjectTable.GetAt(i);

            xmlWriter.WriteStartElement("Object");

            xmlWriter.WriteAttributeString("Index", i.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteElementString("CelCur", view.CelCur.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("CelPrevHeight", view.CelPrevHeight.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("CelPrevWidth", view.CelPrevWidth.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("CelTotal", view.CelTotal.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("Cycle", view.Cycle.ToString());
            xmlWriter.WriteElementString("CycleCount", view.CycleCount.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("CycleTime", view.CycleTime.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("Direction", view.Direction.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("Flags", view.Flags.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("FollowCount", view.FollowCount.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("FollowFlag", view.FollowFlag.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("FollowStepSize", view.FollowStepSize.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("Height", view.Height.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("LoopCur", view.LoopCur.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("LoopFlag", view.LoopFlag.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("LoopTotal", view.LoopTotal.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("Motion", view.Motion.ToString());
            xmlWriter.WriteElementString("MoveFlag", view.MoveFlag.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("MoveStepSize", view.MoveStepSize.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("MoveX", view.MoveX.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("MoveY", view.MoveY.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("Num", view.Number.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("PreviousX", view.PreviousX.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("PreviousY", view.PreviousY.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("Priority", view.Priority.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("StepCount", view.StepCount.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("StepSize", view.StepSize.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("StepTime", view.StepTime.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("ViewCur", view.ViewCur.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("WanderCount", view.WanderCount.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("Width", view.Width.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("X", view.X.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString("Y", view.Y.ToString(CultureInfo.InvariantCulture));

            xmlWriter.WriteEndElement();
        }

        xmlWriter.WriteEndElement();
    }

    private void SerializeInventory(XmlWriter xmlWriter)
    {
        xmlWriter.WriteStartElement("Inventory");

        for (int i = 0; i < this.Interpreter.ResourceManager.InventoryResource.Items.Length; i++)
        {
            InventoryItem item = this.Interpreter.ResourceManager.InventoryResource.Items[i];

            xmlWriter.WriteStartElement("Item");
            xmlWriter.WriteAttributeString("Index", i.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("Location", item.Location.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();
        }

        xmlWriter.WriteEndElement();
    }

    private void SerializeScript(XmlWriter xmlWriter)
    {
        var scriptDataBuilder = new StringBuilder(this.Interpreter.ScriptManager.ScriptData.Length);

        for (int i = 0; i < this.Interpreter.ScriptManager.ScriptData.Length; i++)
        {
            byte b = this.Interpreter.ScriptManager.ScriptData[i];

            char high = ConvertByteToHex((byte)(b >> 4));
            char low = ConvertByteToHex((byte)(b & 0x0f));

            scriptDataBuilder.Append(high);
            scriptDataBuilder.Append(low);
        }

        Debug.Assert(scriptDataBuilder.Length == (this.Interpreter.ScriptManager.ScriptData.Length * 2), "Incorrectly serialized script.");
        xmlWriter.WriteElementString("Script", scriptDataBuilder.ToString());
    }

    private void SerializeLogicScanStart(XmlWriter xmlWriter)
    {
        xmlWriter.WriteStartElement("LogicScanStartList");

        foreach (var logic in this.Interpreter.ResourceManager.LogicResources)
        {
            xmlWriter.WriteStartElement("LogicScanStart");
            xmlWriter.WriteAttributeString("ResourceIndex", logic.ResourceIndex.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteAttributeString("ScanStart", logic.ScanStart.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();
        }

        xmlWriter.WriteEndElement();
    }

    private void DeserializeGame(XmlReader reader)
    {
        reader.MoveToContent();
        reader.ReadStartElement("SavedGame");

        this.DeserializeState(reader);
        this.DeserializeObjectTable(reader);
        this.DeserializeInventory(reader);
        this.DeserializeScript(reader);
        this.DeserializeLogicScanStart(reader);

        reader.ReadEndElement();
    }

    private void DeserializeState(XmlReader reader)
    {
        reader.ReadStartElement("State");

        this.Interpreter.State.BlockIsSet = bool.Parse(reader.ReadElementString("BlockIsSet"));
        this.Interpreter.State.BlockX1 = int.Parse(reader.ReadElementString("BlockX1"), CultureInfo.InvariantCulture);
        this.Interpreter.State.BlockX2 = int.Parse(reader.ReadElementString("BlockX2"), CultureInfo.InvariantCulture);
        this.Interpreter.State.BlockY1 = int.Parse(reader.ReadElementString("BlockY1"), CultureInfo.InvariantCulture);
        this.Interpreter.State.BlockY2 = int.Parse(reader.ReadElementString("BlockY2"), CultureInfo.InvariantCulture);

        reader.ReadStartElement("Controls");

        while (reader.Name == "Control")
        {
            int index = int.Parse(reader.GetAttribute("Index"), CultureInfo.InvariantCulture);
            this.Interpreter.State.Controls[index].Key = int.Parse(reader.GetAttribute("Key"), CultureInfo.InvariantCulture);
            this.Interpreter.State.Controls[index].Number = int.Parse(reader.GetAttribute("Number"), CultureInfo.InvariantCulture);
            reader.Read();
        }

        reader.ReadEndElement();

        this.Interpreter.State.Cursor = reader.ReadElementString("Cursor");
        this.Interpreter.State.EgoControl = (EgoControl)Enum.Parse(typeof(EgoControl), reader.ReadElementString("EgoControl"), true);

        reader.ReadStartElement("Flags");

        while (reader.Name == "Flag")
        {
            int index = int.Parse(reader.GetAttribute("Index"), CultureInfo.InvariantCulture);
            this.Interpreter.State.Flags[index] = bool.Parse(reader.GetAttribute("Value"));
            reader.Read();
        }

        reader.ReadEndElement();

        this.Interpreter.State.Horizon = int.Parse(reader.ReadElementString("Horizon"), CultureInfo.InvariantCulture);
        this.Interpreter.State.Id = reader.ReadElementString("Id");
        this.Interpreter.State.InputEnabled = bool.Parse(reader.ReadElementString("InputEnabled"));
        this.Interpreter.State.InputPosition = byte.Parse(reader.ReadElementString("InputPosition"), NumberStyles.Integer, CultureInfo.InvariantCulture);
        this.Interpreter.State.MenuEnabled = bool.Parse(reader.ReadElementString("MenuEnabled"));
        this.Interpreter.State.ScriptCount = int.Parse(reader.ReadElementString("ScriptCount"), CultureInfo.InvariantCulture);
        this.Interpreter.State.ScriptSaved = int.Parse(reader.ReadElementString("ScriptSaved"), CultureInfo.InvariantCulture);
        this.Interpreter.State.ScriptSize = int.Parse(reader.ReadElementString("ScriptSize"), CultureInfo.InvariantCulture);
        this.Interpreter.State.StatusLineRow = byte.Parse(reader.ReadElementString("StatusLineRow"), NumberStyles.Integer, CultureInfo.InvariantCulture);
        this.Interpreter.State.StatusVisible = bool.Parse(reader.ReadElementString("StatusVisible"));

        reader.ReadStartElement("Strings");

        while (reader.Name == "String")
        {
            int index = int.Parse(reader.GetAttribute("Index"), CultureInfo.InvariantCulture);
            this.Interpreter.State.Strings[index] = reader.GetAttribute("Value");
            reader.Read();
        }

        reader.ReadEndElement();

        this.Interpreter.State.TextBackground = byte.Parse(reader.ReadElementString("TextBackground"), NumberStyles.Integer, CultureInfo.InvariantCulture);
        this.Interpreter.State.TextCombine = byte.Parse(reader.ReadElementString("TextCombine"), NumberStyles.Integer, CultureInfo.InvariantCulture);
        this.Interpreter.State.TextForeground = byte.Parse(reader.ReadElementString("TextForeground"), NumberStyles.Integer, CultureInfo.InvariantCulture);
        this.Interpreter.State.Ticks = uint.Parse(reader.ReadElementString("Ticks"), NumberStyles.Integer, CultureInfo.InvariantCulture);

        reader.ReadStartElement("Variables");

        while (reader.Name == "Variable")
        {
            int index = int.Parse(reader.GetAttribute("Index"), CultureInfo.InvariantCulture);
            this.Interpreter.State.Variables[index] = byte.Parse(reader.GetAttribute("Value"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            reader.Read();
        }

        reader.ReadEndElement();

        this.Interpreter.State.WalkMode = (WalkMode)Enum.Parse(typeof(WalkMode), reader.ReadElementString("WalkMode"), true);
        this.Interpreter.State.WindowRowMax = int.Parse(reader.ReadElementString("WindowRowMax"), CultureInfo.InvariantCulture);
        this.Interpreter.State.WindowRowMin = int.Parse(reader.ReadElementString("WindowRowMin"), CultureInfo.InvariantCulture);

        reader.ReadEndElement();
    }

    private void DeserializeObjectTable(XmlReader reader)
    {
        reader.ReadStartElement("ObjectTable");

        while (reader.Name == "Object")
        {
            byte index = byte.Parse(reader.GetAttribute("Index"), NumberStyles.Integer, CultureInfo.InvariantCulture);

            reader.ReadStartElement("Object");

            var view = this.Interpreter.ObjectTable.GetAt(index);

            view.CelCur = byte.Parse(reader.ReadElementString("CelCur"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.CelPrevHeight = byte.Parse(reader.ReadElementString("CelPrevHeight"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.CelPrevWidth = byte.Parse(reader.ReadElementString("CelPrevWidth"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.CelTotal = byte.Parse(reader.ReadElementString("CelTotal"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.Cycle = (CycleMode)Enum.Parse(typeof(CycleMode), reader.ReadElementString("Cycle"), true);
            view.CycleCount = byte.Parse(reader.ReadElementString("CycleCount"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.CycleTime = byte.Parse(reader.ReadElementString("CycleTime"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.Direction = byte.Parse(reader.ReadElementString("Direction"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.Flags = int.Parse(reader.ReadElementString("Flags"), CultureInfo.InvariantCulture);
            view.FollowCount = byte.Parse(reader.ReadElementString("FollowCount"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.FollowFlag = byte.Parse(reader.ReadElementString("FollowFlag"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.FollowStepSize = byte.Parse(reader.ReadElementString("FollowStepSize"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.Height = int.Parse(reader.ReadElementString("Height"), CultureInfo.InvariantCulture);
            view.LoopCur = byte.Parse(reader.ReadElementString("LoopCur"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.LoopFlag = byte.Parse(reader.ReadElementString("LoopFlag"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.LoopTotal = byte.Parse(reader.ReadElementString("LoopTotal"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.Motion = (Motion)Enum.Parse(typeof(Motion), reader.ReadElementString("Motion"), true);
            view.MoveFlag = byte.Parse(reader.ReadElementString("MoveFlag"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.MoveStepSize = byte.Parse(reader.ReadElementString("MoveStepSize"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.MoveX = int.Parse(reader.ReadElementString("MoveX"), CultureInfo.InvariantCulture);
            view.MoveY = int.Parse(reader.ReadElementString("MoveY"), CultureInfo.InvariantCulture);
            view.Number = byte.Parse(reader.ReadElementString("Num"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.PreviousX = int.Parse(reader.ReadElementString("PreviousX"), CultureInfo.InvariantCulture);
            view.PreviousY = int.Parse(reader.ReadElementString("PreviousY"), CultureInfo.InvariantCulture);
            view.Priority = byte.Parse(reader.ReadElementString("Priority"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.StepCount = byte.Parse(reader.ReadElementString("StepCount"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.StepSize = byte.Parse(reader.ReadElementString("StepSize"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.StepTime = byte.Parse(reader.ReadElementString("StepTime"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.ViewCur = byte.Parse(reader.ReadElementString("ViewCur"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.WanderCount = byte.Parse(reader.ReadElementString("WanderCount"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            view.Width = int.Parse(reader.ReadElementString("Width"), CultureInfo.InvariantCulture);
            view.X = int.Parse(reader.ReadElementString("X"), CultureInfo.InvariantCulture);
            view.Y = int.Parse(reader.ReadElementString("Y"), CultureInfo.InvariantCulture);

            reader.ReadEndElement();
        }

        reader.ReadEndElement();
    }

    private void DeserializeInventory(XmlReader reader)
    {
        reader.ReadStartElement("Inventory");

        while (reader.Name == "Item")
        {
            int index = int.Parse(reader.GetAttribute("Index"), CultureInfo.InvariantCulture);
            this.Interpreter.ResourceManager.InventoryResource.Items[index].Location = byte.Parse(reader.GetAttribute("Location"), NumberStyles.Integer, CultureInfo.InvariantCulture);
            reader.Read();
        }

        reader.ReadEndElement();
    }

    private void DeserializeScript(XmlReader reader)
    {
        string data = reader.ReadElementString("Script");

        Debug.Assert((data.Length % 2) == 0, "Unexpected script length.");
        for (int i = 0; i < data.Length / 2; i++)
        {
            byte high = ConvertHexToByte(data[i * 2]);
            byte low = ConvertHexToByte(data[(i * 2) + 1]);

            this.Interpreter.ScriptManager.ScriptData[i] = (byte)((high << 4) | low);
        }
    }

    private void DeserializeLogicScanStart(XmlReader reader)
    {
        this.Interpreter.SavedScanStarts.Clear();

        reader.ReadStartElement("LogicScanStartList");

        while (reader.Name == "LogicScanStart")
        {
            int resourceIndex = int.Parse(reader.GetAttribute("ResourceIndex"), CultureInfo.InvariantCulture);
            int scanStart = int.Parse(reader.GetAttribute("ScanStart"), CultureInfo.InvariantCulture);

            this.Interpreter.SavedScanStarts.Add(resourceIndex, scanStart);

            reader.Read();
        }

        reader.ReadEndElement();
    }
}
