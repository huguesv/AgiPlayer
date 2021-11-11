// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter;

public class SavedGameManager
{
    public SavedGameManager(AgiInterpreter interpreter, ISavedGameSerializer gameSerializer)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
        this.GameSerializer = gameSerializer ?? throw new ArgumentNullException(nameof(gameSerializer));
        this.AutoSaveName = string.Empty;
    }

    public ISavedGameSerializer GameSerializer { get; }

    public string AutoSaveName { get; set; }

    protected AgiInterpreter Interpreter { get; }

    public void SaveTo(string filePath, string description)
    {
        if (filePath == null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        if (description == null)
        {
            throw new ArgumentNullException(nameof(description));
        }

        FileStream stream = new FileStream(filePath, FileMode.Create);
        using (stream)
        {
            this.GameSerializer.SaveTo(this.Interpreter, description, stream);
        }
    }

    public string GetGameDescription(int index, string folderPath)
    {
        if (folderPath == null)
        {
            throw new ArgumentNullException(nameof(folderPath));
        }

        return this.GetGameDescription(this.GetFilePath(index, folderPath));
    }

    public string GetGameDescription(string filePath)
    {
        if (filePath == null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        string description = string.Empty;

        if (File.Exists(filePath))
        {
            FileStream stream = new FileStream(filePath, FileMode.Open);
            using (stream)
            {
                description = this.GameSerializer.LoadDescriptionFrom(stream);
            }
        }

        return description;
    }

    public DateTime GetFileDateTime(int index, string folderPath)
    {
        if (folderPath == null)
        {
            throw new ArgumentNullException(nameof(folderPath));
        }

        string filePath = this.GetFilePath(index, folderPath);
        FileInfo info = new FileInfo(filePath);
        return info.LastWriteTime;
    }

    public string GetFilePath(int index, string folderPath)
    {
        if (folderPath == null)
        {
            throw new ArgumentNullException(nameof(folderPath));
        }

        string name = string.Format(CultureInfo.CurrentCulture, "{0}sg.{1}", this.Interpreter.State.Id.ToLower(CultureInfo.InvariantCulture), index);
        return Path.Combine(folderPath, name);
    }

    public void GetSaveSlotInformation(string folderPath, out int[] slotNumbers, out string[] descriptions, out int slotCount, out int current)
    {
        if (folderPath == null)
        {
            throw new ArgumentNullException(nameof(folderPath));
        }

        slotNumbers = new int[12];
        descriptions = new string[12];
        slotCount = 0;
        current = 0;

        var mostRecentTime = new DateTime(1970, 1, 1);
        for (int i = 0; i < descriptions.Length; i++)
        {
            descriptions[i] = this.GetGameDescription(i + 1, folderPath);
            slotNumbers[i] = i + 1;

            if (descriptions[i].Length > 0)
            {
                var time = this.GetFileDateTime(i + 1, folderPath);
                if (time.CompareTo(mostRecentTime) > 0)
                {
                    mostRecentTime = time;
                    current = i;
                }
            }
        }

        slotCount = descriptions.Length;
    }

    public void GetRestoreSlotInformation(string folderPath, out int[] slotNumbers, out string[] descriptions, out int slotCount, out int current)
    {
        if (folderPath == null)
        {
            throw new ArgumentNullException(nameof(folderPath));
        }

        slotNumbers = new int[12];
        descriptions = new string[12];
        slotCount = 0;
        current = 0;

        var mostRecentTime = new DateTime(1970, 1, 1);
        for (int i = 0; i < descriptions.Length; i++)
        {
            string desc = this.GetGameDescription(i + 1, folderPath);

            if (desc.Length > 0)
            {
                descriptions[slotCount] = desc;
                slotNumbers[slotCount] = i + 1;

                var time = this.GetFileDateTime(i + 1, folderPath);
                if (time.CompareTo(mostRecentTime) > 0)
                {
                    mostRecentTime = time;
                    current = slotCount;
                }

                slotCount++;
            }
        }
    }
}
