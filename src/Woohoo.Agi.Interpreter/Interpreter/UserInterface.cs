// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Interpreter;

public static class UserInterface
{
    public static string PlayerName => StringUtility.ConvertSystemResourceText(Resources.PlayerName);

    public static string PlayerVersion => StringUtility.ConvertSystemResourceText(Resources.PlayerVersion);

    public static string KernelVersion1 => StringUtility.ConvertSystemResourceText(string.Format(CultureInfo.CurrentUICulture, Resources.KernelVersion1, PlayerName, PlayerVersion));

    public static string RestartQuery => StringUtility.ConvertSystemResourceText(Resources.RestartQueryPC);

    public static string SaveQuery => StringUtility.ConvertSystemResourceText(Resources.SaveQuery);

    public static string SaveDone => StringUtility.ConvertSystemResourceText(Resources.SaveDone);

    public static string RestoreQuery => StringUtility.ConvertSystemResourceText(Resources.RestoreQuery);

    public static string RestoreNoGameAvailable => StringUtility.ConvertSystemResourceText(Resources.RestoreNoGameAvailable);

    public static string ListBoxItemTooLong => StringUtility.ConvertSystemResourceText(Resources.ListBoxItemTooLong);

    public static string ListBoxItemFormat => StringUtility.ConvertSystemResourceText(Resources.ListBoxItemFormat);

    public static string ListBoxScrollbarUp => StringUtility.ConvertSystemResourceText(Resources.ListBoxScrollbarUp);

    public static string ListBoxScrollbarUpHidden => StringUtility.ConvertSystemResourceText(Resources.ListBoxScrollbarUpHidden);

    public static string ListBoxScrollbarDown => StringUtility.ConvertSystemResourceText(Resources.ListBoxScrollbarDown);

    public static string ListBoxScrollbarDownHidden => StringUtility.ConvertSystemResourceText(Resources.ListBoxScrollbarDownHidden);

    public static string GameSelectionHeader => StringUtility.ConvertSystemResourceText(Resources.GameSelectionHeader);

    public static string GameSelectionNoGameFound => StringUtility.ConvertSystemResourceText(Resources.GameSelectionNoGameFound);

    public static string QuitQuery => StringUtility.ConvertSystemResourceText(Resources.QuitQueryPC);

    public static string Pause => StringUtility.ConvertSystemResourceText(Resources.PausePC);

    public static string NotNow => StringUtility.ConvertSystemResourceText(Resources.NotNow);

    public static string InventoryCarrying => StringUtility.ConvertSystemResourceText(Resources.InventoryCarrying);

    public static string InventoryNothing => StringUtility.ConvertSystemResourceText(Resources.InventoryNothing);

    public static string InventoryStatusForItems => StringUtility.ConvertSystemResourceText(Resources.InventoryStatusForItemsPC);

    public static string InventoryStatusNoItems => StringUtility.ConvertSystemResourceText(Resources.InventoryStatusNoItemsPC);

    public static string MessageTooVerboseFormat => StringUtility.ConvertSystemResourceText(Resources.MessageTooVerboseFormat);

    public static string ScoreStatusOld => StringUtility.ConvertSystemResourceText(Resources.ScoreStatusOld);

    public static string ScoreStatusNew => StringUtility.ConvertSystemResourceText(Resources.ScoreStatusNew);

    public static string SoundOff => StringUtility.ConvertSystemResourceText(Resources.SoundOff);

    public static string SoundOn => StringUtility.ConvertSystemResourceText(Resources.SoundOn);

    public static string SoundStatusNew => StringUtility.ConvertSystemResourceText(Resources.SoundStatusNew);

    public static string SoundStatusOld => StringUtility.ConvertSystemResourceText(Resources.SoundStatusOld);

    public static string InputBox => StringUtility.ConvertSystemResourceText(Resources.InputBox);

    public static string TypelessBox => StringUtility.ConvertSystemResourceText(Resources.TypelessBox);

    public static string TraceSeparatorLine => StringUtility.ConvertSystemResourceText(Resources.TraceSeparatorLine);

    public static string SavePathDoesNotExistFormat => StringUtility.ConvertSystemResourceText(Resources.SavePathDoesNotExistFormat);

    public static string RestorePathPromptFormat => StringUtility.ConvertSystemResourceText(Resources.RestorePathPromptFormat);

    public static string RestoreNoGamesInFolderFormat => StringUtility.ConvertSystemResourceText(Resources.RestoreNoGamesInFolderFormat);

    public static string SavePathPromptFormat => StringUtility.ConvertSystemResourceText(Resources.SavePathPromptFormat);

    public static string SaveDescriptionPrompt => StringUtility.ConvertSystemResourceText(Resources.SaveDescriptionPrompt);

    public static string PathExample => StringUtility.ConvertSystemResourceText(Resources.PathExample);

    public static string SaveSelectSlot => StringUtility.ConvertSystemResourceText(Resources.SaveSelectSlot);

    public static string RestoreSelectSlot => StringUtility.ConvertSystemResourceText(Resources.RestoreSelectSlot);

    public static string SaveDiskFull => StringUtility.ConvertSystemResourceText(Resources.SaveDiskFull);

    public static string SaveRestoreSelectName => StringUtility.ConvertSystemResourceText(Resources.SaveRestoreSelectName);

    public static string TraceProcedureNumber => StringUtility.ConvertSystemResourceText(Resources.TraceProcedureNumber);

    public static string TraceProcedureText => StringUtility.ConvertSystemResourceText(Resources.TraceProcedureText);

    public static string TraceFunctionNumber => StringUtility.ConvertSystemResourceText(Resources.TraceFunctionNumber);

    public static string TraceFunctionResult => StringUtility.ConvertSystemResourceText(Resources.TraceFunctionResult);

    public static string TraceFunctionResultTrue => StringUtility.ConvertSystemResourceText(Resources.TraceFunctionResultTrue);

    public static string TraceFunctionResultFalse => StringUtility.ConvertSystemResourceText(Resources.TraceFunctionResultFalse);

    public static string TraceProcedureReturn => StringUtility.ConvertSystemResourceText(Resources.TraceProcedureReturn);

    public static string TraceParameterStart => StringUtility.ConvertSystemResourceText(Resources.TraceParameterStart);

    public static string TraceParameterEnd => StringUtility.ConvertSystemResourceText(Resources.TraceParameterEnd);

    public static string TraceParameterSeparator => StringUtility.ConvertSystemResourceText(Resources.TraceParameterSeparator);

    public static string TraceParameterSigned => StringUtility.ConvertSystemResourceText(Resources.TraceParameterSigned);

    public static string TraceParameterUnsigned => StringUtility.ConvertSystemResourceText(Resources.TraceParameterUnsigned);

    public static string KernelVersion2(string name, string version, string id, string platform, string interpreter)
    {
        return StringUtility.ConvertSystemResourceText(string.Format(CultureInfo.CurrentUICulture, Resources.KernelVersion2, name, version, id, platform, interpreter));
    }
}
