// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

public static class UserInterface
{
    public static string PlayerName => StringUtility.ConvertSystemResourceText(PlayerResources.PlayerName);

    public static string PlayerVersion => StringUtility.ConvertSystemResourceText(PlayerResources.PlayerVersion);

    public static string KernelVersion1 => StringUtility.ConvertSystemResourceText(string.Format(CultureInfo.CurrentUICulture, PlayerResources.KernelVersion1, PlayerName, PlayerVersion));

    public static string RestartQuery => StringUtility.ConvertSystemResourceText(PlayerResources.RestartQueryPC);

    public static string SaveQuery => StringUtility.ConvertSystemResourceText(PlayerResources.SaveQuery);

    public static string SaveDone => StringUtility.ConvertSystemResourceText(PlayerResources.SaveDone);

    public static string RestoreQuery => StringUtility.ConvertSystemResourceText(PlayerResources.RestoreQuery);

    public static string RestoreNoGameAvailable => StringUtility.ConvertSystemResourceText(PlayerResources.RestoreNoGameAvailable);

    public static string HintMessageFormat => StringUtility.ConvertSystemResourceText(PlayerResources.HintMessageFormat);

    public static string HintsNotFound => StringUtility.ConvertSystemResourceText(PlayerResources.HintsNotFound);

    public static string HintsNotAvailable => StringUtility.ConvertSystemResourceText(PlayerResources.HintsNotAvailable);

    public static string ListBoxItemTooLong => StringUtility.ConvertSystemResourceText(PlayerResources.ListBoxItemTooLong);

    public static string ListBoxItemFormat => StringUtility.ConvertSystemResourceText(PlayerResources.ListBoxItemFormat);

    public static string ListBoxScrollbarUp => StringUtility.ConvertSystemResourceText(PlayerResources.ListBoxScrollbarUp);

    public static string ListBoxScrollbarUpHidden => StringUtility.ConvertSystemResourceText(PlayerResources.ListBoxScrollbarUpHidden);

    public static string ListBoxScrollbarDown => StringUtility.ConvertSystemResourceText(PlayerResources.ListBoxScrollbarDown);

    public static string ListBoxScrollbarDownHidden => StringUtility.ConvertSystemResourceText(PlayerResources.ListBoxScrollbarDownHidden);

    public static string GameSelectionHeader => StringUtility.ConvertSystemResourceText(PlayerResources.GameSelectionHeader);

    public static string GameSelectionNoGameFound => StringUtility.ConvertSystemResourceText(PlayerResources.GameSelectionNoGameFound);

    public static string QuitQuery => StringUtility.ConvertSystemResourceText(PlayerResources.QuitQueryPC);

    public static string Pause => StringUtility.ConvertSystemResourceText(PlayerResources.PausePC);

    public static string NotNow => StringUtility.ConvertSystemResourceText(PlayerResources.NotNow);

    public static string InventoryCarrying => StringUtility.ConvertSystemResourceText(PlayerResources.InventoryCarrying);

    public static string InventoryNothing => StringUtility.ConvertSystemResourceText(PlayerResources.InventoryNothing);

    public static string InventoryStatusForItems => StringUtility.ConvertSystemResourceText(PlayerResources.InventoryStatusForItemsPC);

    public static string InventoryStatusNoItems => StringUtility.ConvertSystemResourceText(PlayerResources.InventoryStatusNoItemsPC);

    public static string MessageTooVerboseFormat => StringUtility.ConvertSystemResourceText(PlayerResources.MessageTooVerboseFormat);

    public static string ScoreStatusOld => StringUtility.ConvertSystemResourceText(PlayerResources.ScoreStatusOld);

    public static string ScoreStatusNew => StringUtility.ConvertSystemResourceText(PlayerResources.ScoreStatusNew);

    public static string SoundOff => StringUtility.ConvertSystemResourceText(PlayerResources.SoundOff);

    public static string SoundOn => StringUtility.ConvertSystemResourceText(PlayerResources.SoundOn);

    public static string SoundStatusNew => StringUtility.ConvertSystemResourceText(PlayerResources.SoundStatusNew);

    public static string SoundStatusOld => StringUtility.ConvertSystemResourceText(PlayerResources.SoundStatusOld);

    public static string InputBox => StringUtility.ConvertSystemResourceText(PlayerResources.InputBox);

    public static string TypelessBox => StringUtility.ConvertSystemResourceText(PlayerResources.TypelessBox);

    public static string WordListViewAll => StringUtility.ConvertSystemResourceText(PlayerResources.WordListViewAll);

    public static string WordListViewLess => StringUtility.ConvertSystemResourceText(PlayerResources.WordListViewLess);

    public static string TraceSeparatorLine => StringUtility.ConvertSystemResourceText(PlayerResources.TraceSeparatorLine);

    public static string SavePathDoesNotExistFormat => StringUtility.ConvertSystemResourceText(PlayerResources.SavePathDoesNotExistFormat);

    public static string RestorePathPromptFormat => StringUtility.ConvertSystemResourceText(PlayerResources.RestorePathPromptFormat);

    public static string RestoreNoGamesInFolderFormat => StringUtility.ConvertSystemResourceText(PlayerResources.RestoreNoGamesInFolderFormat);

    public static string SavePathPromptFormat => StringUtility.ConvertSystemResourceText(PlayerResources.SavePathPromptFormat);

    public static string SaveDescriptionPrompt => StringUtility.ConvertSystemResourceText(PlayerResources.SaveDescriptionPrompt);

    public static string PathExample => StringUtility.ConvertSystemResourceText(PlayerResources.PathExample);

    public static string SaveSelectSlot => StringUtility.ConvertSystemResourceText(PlayerResources.SaveSelectSlot);

    public static string RestoreSelectSlot => StringUtility.ConvertSystemResourceText(PlayerResources.RestoreSelectSlot);

    public static string SaveDiskFull => StringUtility.ConvertSystemResourceText(PlayerResources.SaveDiskFull);

    public static string SaveRestoreSelectName => StringUtility.ConvertSystemResourceText(PlayerResources.SaveRestoreSelectName);

    public static string TraceProcedureNumber => StringUtility.ConvertSystemResourceText(PlayerResources.TraceProcedureNumber);

    public static string TraceProcedureText => StringUtility.ConvertSystemResourceText(PlayerResources.TraceProcedureText);

    public static string TraceFunctionNumber => StringUtility.ConvertSystemResourceText(PlayerResources.TraceFunctionNumber);

    public static string TraceFunctionResult => StringUtility.ConvertSystemResourceText(PlayerResources.TraceFunctionResult);

    public static string TraceFunctionResultTrue => StringUtility.ConvertSystemResourceText(PlayerResources.TraceFunctionResultTrue);

    public static string TraceFunctionResultFalse => StringUtility.ConvertSystemResourceText(PlayerResources.TraceFunctionResultFalse);

    public static string TraceProcedureReturn => StringUtility.ConvertSystemResourceText(PlayerResources.TraceProcedureReturn);

    public static string TraceParameterStart => StringUtility.ConvertSystemResourceText(PlayerResources.TraceParameterStart);

    public static string TraceParameterEnd => StringUtility.ConvertSystemResourceText(PlayerResources.TraceParameterEnd);

    public static string TraceParameterSeparator => StringUtility.ConvertSystemResourceText(PlayerResources.TraceParameterSeparator);

    public static string TraceParameterSigned => StringUtility.ConvertSystemResourceText(PlayerResources.TraceParameterSigned);

    public static string TraceParameterUnsigned => StringUtility.ConvertSystemResourceText(PlayerResources.TraceParameterUnsigned);

    public static string KernelVersion2(string name, string version, string id, string platform, string interpreter)
    {
        return StringUtility.ConvertSystemResourceText(string.Format(CultureInfo.CurrentUICulture, PlayerResources.KernelVersion2, name, version, id, platform, interpreter));
    }
}
