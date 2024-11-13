// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

using Woohoo.Agi.Engine.Interpreter.Controls;
using Woohoo.Agi.Engine.Resources;
using Woohoo.Agi.Engine.Resources.Serialization;

public sealed partial class AgiInterpreter
{
    private const int StringSize = 40;
    private const int ControlMax = 50;
    private const string LogFileName = "logfile";
    private ClockState clockState;
    private byte computerType;
    private byte displayType;
    private bool[] controlState;
    private bool diskSpaceAvailable;
    private bool pictureVisible;
    private List<Blit> blitlistUpdated; // blit objects that are updated on each cycle
    private List<Blit> blitlistStatic; // blit objects that are not updated on each cycle

    public AgiInterpreter(IInputDriver inputDriver, IGraphicsDriver graphicsDriver, ISoundDriver soundDriver)
    {
        this.InputDriver = inputDriver;
        this.GraphicsDriver = graphicsDriver;
        this.SoundDriver = soundDriver;
    }

    private delegate bool BlitlistBuildPredicate(ViewObject view);

    public Preferences Preferences { get; private set; }

    public GameStartInfo GameInfo { get; private set; }

    public IInputDriver InputDriver { get; }

    public IGraphicsDriver GraphicsDriver { get; }

    public ISoundDriver SoundDriver { get; }

    public ParserResult[] ParserResults { get; private set; }

    public SavedGameManager SavedGameManager { get; private set; }

    public WindowManager WindowManager { get; set; }

    public SoundManager SoundManager { get; private set; }

    public GraphicsRenderer GraphicsRenderer { get; private set; }

    public GameControl GameControl { get; private set; }

    public State State { get; private set; }

    public ViewObjectTable ObjectTable { get; set; } // public setter only for tests

    public PriorityTable PriorityTable { get; set; } // public setter only for tests

    public ResourceLoader ResourceLoader { get; private set; }

    public ScriptManager ScriptManager { get; private set; }

    public ResourceManager ResourceManager { get; set; } // public setter only for tests

    public Random Randomizer { get; private set; }

    public Dictionary<int, int> SavedScanStarts { get; private set; }

    public LogicInterpreter LogicInterpreter { get; private set; }

    public Menu Menu { get; private set; }

    public ViewObjectManager ObjectManager { get; set; } // public setter only for tests

    private static string SavedGameFolder => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    public void Start(GameStartInfo startInfo, Preferences prefs)
    {
        this.Preferences = prefs;
        this.InitializePlayer();
        this.GameInfo = startInfo;
        this.InitializeAgi();
        this.InputDriver.InitializeDelay();
        this.ExecuteAgi();
    }

    public void Start(GameStartInfo[] startInfos, Preferences prefs)
    {
        this.Preferences = prefs;
        this.InitializePlayer();
        this.DisplayGames(startInfos);
        this.InitializeAgi();
        this.InputDriver.InitializeDelay();
        this.ExecuteAgi();
    }

    public void ShutdownText()
    {
        this.GraphicsClear();
        this.WindowManager.ShutdownText();
    }

    public void CreateState()
    {
        this.State = new State();
    }

    public void ParseText(string text)
    {
        var parser = new Parser(this.ResourceManager.VocabularyResource);

        this.ParserResults = parser.Parse(text);

        if (this.ParserResults.Length > 0)
        {
            for (int i = 0; i < this.ParserResults.Length; i++)
            {
                ParserResult result = this.ParserResults[i];
                if (result.FamilyIdentifier == VocabularyResource.NoFamily)
                {
                    this.State.Variables[Variables.BadWord] = (byte)(i + 1);
                }
            }

            this.State.Flags[Flags.PlayerCommandLine] = true;
        }
    }

    public void PollInput()
    {
        if (this.GameControl.MenuNextInput && this.State.MenuEnabled)
        {
            this.GameControl.ShowMenu();
        }

        InputEvent e = this.MapControlKey(this.InputDriver.ReadEvent());
        while (e is not null && !this.State.Flags[Flags.PlayerCommandLine])
        {
            this.ProcessEvent(e);

            e = this.MapControlKey(this.InputDriver.ReadEvent());
        }
    }

    public void ExitAgi()
    {
        this.ShutdownAgi();
        this.ShutdownPlayer();
    }

    public void ShutdownClock()
    {
        this.clockState = ClockState.TurnOff;
        this.InputDriver.ClockDenitStopThread();
    }

    public void Clock()
    {
        int timeCounter = 0;
        int tickScale = this.InputDriver.TickScale();
        int previousTick = this.InputDriver.Tick();
        while (this.clockState != ClockState.TurnOff)
        {
            int currentTick = this.InputDriver.Tick();
            this.State.Ticks += (uint)((currentTick - previousTick) / tickScale);

            switch (this.clockState)
            {
                case ClockState.Normal:
                    // it's in 1/20's of seconds
                    timeCounter += currentTick - previousTick;

                    while (timeCounter >= 20 * tickScale)
                    {
                        timeCounter -= 20 * tickScale;
                        this.State.Variables[Variables.Seconds]++;

                        if (this.State.Variables[Variables.Seconds] >= 60)
                        {
                            this.State.Variables[Variables.Seconds] = 0;
                            this.State.Variables[Variables.Minutes]++;
                        }

                        if (this.State.Variables[Variables.Minutes] >= 60)
                        {
                            this.State.Variables[Variables.Minutes] = 0;
                            this.State.Variables[Variables.Hours]++;
                        }

                        if (this.State.Variables[Variables.Hours] >= 24)
                        {
                            this.State.Variables[Variables.Hours] = 0;
                            this.State.Variables[Variables.Days]++;
                        }
                    }

                    previousTick = currentTick;
                    break;

                case ClockState.Pause:
                    previousTick = currentTick;
                    break;
            }

            this.InputDriver.Sleep(500);
        }
    }

    public void ToggleTrace()
    {
        if (this.GameControl.TraceControl.TraceState == TraceState.Uninitialized)
        {
            this.InitializeTraceWindow();
        }
        else
        {
            this.GameControl.TraceControl.Clear();
        }
    }

    public bool Prompt(string text)
    {
        var control = new PromptControl(this)
        {
            Text = text,
        };

        return control.DoModal();
    }

    private static void BlitlistFree(List<Blit> blits)
    {
        blits.Clear();
    }

    private static Blit CreateBlit(ViewObject view)
    {
        var blit = new Blit
        {
            View = view,
            X = view.X,
            Y = view.Y - view.Height + 1,
            Width = view.Width,
            Height = view.Height,
        };

        blit.CreateBuffer(view.Width * view.Height);

        view.Blit = blit;

        return blit;
    }

    private static void BlitAdd(ViewObject view, List<Blit> blits)
    {
        var blit = AgiInterpreter.CreateBlit(view);
        blits.Add(blit);
    }

    private void DisplayGames(GameStartInfo[] startInfos)
    {
        var control = new GameSelectionControl(this);
        this.GameInfo = control.DoModal(startInfos);
        if (this.GameInfo is null)
        {
            this.ShutdownPlayer();
        }
    }

    private void InitializePlayer()
    {
        this.CreateState();

        this.computerType = this.Preferences.Computer;
        this.displayType = this.Preferences.Display;
        this.controlState = new bool[ControlMax];
        this.ParserResults = [];
        this.Menu = new Menu();
        this.blitlistStatic = [];
        this.blitlistUpdated = [];
        this.SoundManager = new SoundManager(this);
        this.ScriptManager = new ScriptManager(this, new AgiError(this.ExecutionError));
        this.WindowManager = new WindowManager(this);
        this.GameControl = new GameControl(this);

        this.InitializeGraphics();
        this.InputDriver.InitializeEvents();
        this.InitializeClock();
        this.SoundManager.Initialize();

        this.WindowManager.SetTextColor(15, 0);
        this.GameControl.InputControl.RedrawInput();
    }

    private void InitializeGraphics()
    {
        this.GraphicsDriver.Initialize();
        this.GraphicsDriver.SetCaption(UserInterface.PlayerName);
        this.InitializeRenderer();
        this.WindowManager.InitializeFont();
        this.InitializeGraphicsDriver();
        this.GraphicsUpdatePalette();
        this.PriorityTable = new PriorityTable();
    }

    private void ReinitializeGraphics()
    {
        this.WindowManager.ShutdownFont();
        this.ReinitializeRenderer();
        this.WindowManager.InitializeFont();
        this.InitializeGraphicsDriver();

        this.GraphicsRenderer.TextMode = true;
        this.GraphicsUpdatePalette();
        this.WindowManager.SetTextColor(this.State.TextForeground, this.State.TextBackground);
        this.GraphicsClear();

        this.GraphicsRenderer.TextMode = false;
        this.GraphicsUpdatePalette();
        this.WindowManager.SetTextColor(this.State.TextForeground, this.State.TextBackground);
        this.GraphicsClear();
    }

    private void InitializeGraphicsDriver()
    {
        int displayScaleX = this.Preferences.DisplayScaleX;
        int displayScaleY = this.Preferences.DisplayScaleY;

        if (this.GraphicsRenderer.RenderScaleX > 1)
        {
            if ((displayScaleX % this.GraphicsRenderer.RenderScaleX) == 0)
            {
                displayScaleX /= this.GraphicsRenderer.RenderScaleX;
            }
        }

        if (this.GraphicsRenderer.RenderScaleY > 1)
        {
            if ((displayScaleY % this.GraphicsRenderer.RenderScaleY) == 0)
            {
                displayScaleY /= this.GraphicsRenderer.RenderScaleY;
            }
        }

        this.GraphicsDriver.Display(displayScaleX, displayScaleY, this.GraphicsRenderer.RenderScaleX, this.GraphicsRenderer.RenderScaleY);
    }

    private void InitializeRenderer()
    {
        this.GraphicsRenderer = new GraphicsRenderer(this.GraphicsDriver, this.State, this.Preferences);

        this.ReinitializeRenderer();
    }

    private void ReinitializeRenderer()
    {
        this.State.Variables[Variables.DisplayType] = this.displayType;

        // Only here so games that only setup ctrl-R for cga games work
        if (this.State.Variables[Variables.DisplayType] > DisplayType.Rgb)
        {
            this.State.Variables[Variables.DisplayType] = DisplayType.Cga;
        }
    }

    private void ShutdownRenderer()
    {
        // Nothing to do
        this.GraphicsRenderer = null;
    }

    private void ShutdownGraphics()
    {
        this.WindowManager.ShutdownFont();
        this.ShutdownRenderer();
    }

    private void ShutdownAgi()
    {
        this.ResourceManager = null;
    }

    private void ShutdownPlayer()
    {
        this.SoundManager.Shutdown();
        this.ShutdownClock();
        this.ShutdownGraphics();
        throw new ExitException();
    }

    private void InitializeClock()
    {
        this.State.Variables[Variables.Seconds] = 0;
        this.State.Variables[Variables.Minutes] = 0;
        this.State.Variables[Variables.Hours] = 0;
        this.State.Variables[Variables.Days] = 0;

        this.clockState = ClockState.Normal;

        this.InputDriver.ClockInitStartThread();
    }

    private void InitializeAgi()
    {
        this.GraphicsDriver.SetCaption(UserInterface.PlayerName + " - " + this.GameInfo.Name + " " + this.GameInfo.Version);

        this.GameControl.Reset();

        this.diskSpaceAvailable = true;
        this.ResourceLoader = new ResourceLoader(this.GameInfo.GameContainer, this.GameInfo.Id, this.GameInfo.Platform, this.GameInfo.Interpreter);
        this.ResourceManager = new ResourceManager();

        this.LogicInterpreter = new LogicInterpreter(this, this.GameInfo.Interpreter, this.GameControl.MouseMode, new AgiError(this.ExecutionError), new TraceFunction(this.TraceFunctionCallback), new TraceProcedure(this.TraceProcedureCallback));
        this.SavedGameManager = new SavedGameManager(this, new SavedGameXmlSerializer());
        this.Randomizer = new Random();
        this.SavedScanStarts = [];

        this.InitializeGame();
        this.LoadLogic(0, false);
        this.State.Flags[Flags.SoundOn] = true;
    }

    /// <summary>
    /// Initialize the game.
    /// </summary>
    /// <remarks>
    /// This is used at initial startup, and when a game is restarted.
    /// </remarks>
    private void InitializeGame()
    {
        this.ResourceManager.VocabularyResource = this.ResourceLoader.LoadVocabulary();
        this.ResourceManager.InventoryResource = this.ResourceLoader.LoadInventory();

        // Some games have the maximum wrong (like the thexder demo), +2 compensates for this
        this.ObjectTable = new ViewObjectTable(this.ResourceManager.InventoryResource.MaxAnimatedObjects + 2);
        this.ObjectManager = new ViewObjectManager(this, new AgiError(this.ExecutionError));

        if (this.GameInfo.Interpreter >= InterpreterVersion.V2089 && this.GameInfo.Interpreter <= InterpreterVersion.V2936)
        {
            this.ObjectManager.LoopUpdate = LoopUpdate.Four;
        }
        else if (this.GameInfo.Interpreter >= InterpreterVersion.V3002086)
        {
            this.ObjectManager.LoopUpdate = LoopUpdate.All;
        }

        // else if (version == InterpreterVersion.V3002086)
        // {
        // this.ObjectManager.LoopUpdate = LoopUpdate.Flag;
        // }
        // else if (version >= InterpreterVersion.V3002098)
        // {
        // this.ObjectManager.LoopUpdate = LoopUpdate.All;
        // }
        for (int i = 0; i < this.State.Variables.Length; i++)
        {
            this.State.Variables[i] = 0;
        }

        for (int i = 0; i < this.State.Flags.Length; i++)
        {
            this.State.Flags[i] = false;
        }

        this.ClearAllControllers();
        this.InitializeRoom();
        this.BlistsErase();

        this.State.Variables[Variables.ComputerType] = this.computerType;
        this.State.Variables[Variables.DisplayType] = this.displayType;
        this.State.Variables[Variables.InputLength] = 41;
        this.State.Variables[Variables.FreeMemory] = 10;
        this.State.Flags[Flags.NewRoom] = true;
        this.State.EgoControl = EgoControl.Player;
        this.State.BlockIsSet = false;

        if (this.computerType == ComputerType.PC)
        {
            this.State.Variables[Variables.SoundType] = SoundType.PC;
        }
        else
        {
            this.State.Variables[Variables.SoundType] = SoundType.Tandy;
            this.State.Flags[Flags.MultiChannelSound] = true;
        }
    }

    private void InitializeRoom()
    {
        this.ResourceManager.SoundResources.Clear();
        this.ResourceManager.ViewResources.Clear();
        if (this.ResourceManager.PictureResources.Count > 1)
        {
            PictureResource resource = this.ResourceManager.PictureResources[0];
            this.ResourceManager.PictureResources.Clear();
            this.ResourceManager.PictureResources.Add(resource);
        }

        if (this.ResourceManager.LogicResources.Count > 1)
        {
            LogicResource resource = this.ResourceManager.LogicResources[0];
            this.ResourceManager.LogicResources.Clear();
            this.ResourceManager.LogicResources.Add(resource);
        }
    }

    private void ExecuteAgi()
    {
        while (true)
        {
            this.ClearAllControllers();

            this.State.Flags[Flags.PlayerCommandLine] = false;
            this.State.Flags[Flags.SaidAccepted] = false;
            this.State.Variables[Variables.KeyPressed] = 0;
            this.State.Variables[Variables.BadWord] = 0;

            this.InputDriver.DoDelay();

            if (this.State.EgoControl == EgoControl.Computer)
            {
                this.State.Variables[Variables.Direction] = this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).Direction;
            }
            else
            {
                this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).Direction = this.State.Variables[Variables.Direction];
            }

            this.ObjectManager.MotionUpdateAll();

            int oldScore = this.State.Variables[Variables.Score];
            bool soundEnabled = this.State.Flags[Flags.SoundOn];

        // Save the current state of interpreter here, in case of AgiExecutionException
        // (not currently supported)
        ErrorResume:
            try
            {
                while (this.CallLogic(0))
                {
                    this.State.Variables[Variables.BadWord] = 0;
                    this.State.Variables[Variables.ObjectBorder] = BorderType.None;
                    this.State.Variables[Variables.Object] = 0;
                    this.State.Flags[Flags.PlayerCommandLine] = false;
                    oldScore = this.State.Variables[Variables.Score];
                }

                this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).Direction = this.State.Variables[Variables.Direction];

                if (oldScore != this.State.Variables[Variables.Score] ||
                    soundEnabled != this.State.Flags[Flags.SoundOn])
                {
                    this.GameControl.DisplayStatusLine();
                }

                this.State.Variables[Variables.ObjectBorder] = BorderType.None;
                this.State.Variables[Variables.Object] = 0;
                this.State.Flags[Flags.NewRoom] = false;
                this.State.Flags[Flags.Restart] = false;
                this.State.Flags[Flags.Restore] = false;

                if (this.GraphicsRenderer.GraphicsUpdateNeeded())
                {
                    this.UpdateObjectTable();
                }
            }
            catch (ExecutionException)
            {
                // Restore interpreter state here (not currently supported)
                goto ErrorResume;
            }
        }
    }

    private void TraceProcedureCallback(byte op, int currentLogicDataIndex)
    {
        if (this.GameControl.TraceControl.TraceState == TraceState.Initialized)
        {
            // TODO: this shouldn't be part of logic execution
            this.TraceProcedure(op, currentLogicDataIndex);
        }
    }

    private void TraceFunctionCallback(bool result, int operatorIndex)
    {
        if (this.GameControl.TraceControl.TraceState == TraceState.Initialized)
        {
            // TODO: this shouldn't be part of logic execution
            this.TraceFunction(result, operatorIndex);
        }
    }

    private void TraceProcedure(byte op, int operatorIndex)
    {
        var table = new LogicProcedureTable(this.GameInfo.Interpreter);
        var command = table.GetAt(op);

        this.GameControl.TraceControl.Trace(command, operatorIndex, false, 0xffff, 0);
    }

    private void TraceFunction(bool result, int operatorIndex)
    {
        byte op = this.LogicInterpreter.CurrentLogic.GetCode(operatorIndex);
        operatorIndex++;

        bool said = op == LogicFunctionCode.Said;

        var table = new LogicFunctionTable();
        var command = table.GetAt(op);

        this.GameControl.TraceControl.Trace(command, operatorIndex, said, result ? 1 : 0, 0xdc);
    }

    private bool CallLogic(byte logicResourceIndex)
    {
        bool loaded = false;

        var resource = this.ResourceManager.FindLogic(logicResourceIndex);
        if (resource is null)
        {
            resource = this.LoadLogic(logicResourceIndex, false);
            loaded = true;
        }

        if (this.GameControl.TraceControl.TraceState == TraceState.Unknown)
        {
            this.GameControl.TraceControl.TraceState = TraceState.Initialized;
        }

        if (logicResourceIndex == 0)
        {
            this.GameControl.TraceControl.Logic0Called = true;
        }

        bool restart = this.LogicInterpreter.Execute(resource);

        if (loaded && !restart)
        {
            this.BlistsErase();
            this.ResourceManager.LogicResources.Remove(resource);
            this.BlistsDraw();
        }

        return restart;
    }

    private LogicResource LoadLogic(byte resourceIndex, bool writeToScript)
    {
        var resource = this.ResourceManager.FindLogic(resourceIndex);
        if (resource is null)
        {
            this.BlistsErase();

            resource = this.ResourceLoader.LoadLogic(resourceIndex);
            if (this.Preferences.SkipStartupQuestion)
            {
                this.PatchLogic(resource);
            }

            this.ResourceManager.LogicResources.Add(resource);

            this.BlistsDraw();
        }

        if (writeToScript)
        {
            this.ScriptManager.Write(ScriptCodes.LoadLogic, resourceIndex);
        }

        return resource;
    }

    private void PatchLogic(LogicResource resource)
    {
        foreach (var patch in LogicPatches.All)
        {
            if (patch.ResourceIndex == resource.ResourceIndex &&
                Array.IndexOf(patch.GameIds, this.State.Id) >= 0)
            {
                resource.Patch(patch.Original, patch.Patched);
                break;
            }
        }
    }

    private PictureResource LoadPicture(byte pictureResourceIndex)
    {
        var resource = this.ResourceManager.FindPicture(pictureResourceIndex);
        if (resource is null)
        {
            this.BlistsErase();
            this.ScriptManager.Write(ScriptCodes.LoadPicture, pictureResourceIndex);

            resource = this.ResourceLoader.LoadPicture(pictureResourceIndex);
            if (resource is not null)
            {
                this.ResourceManager.PictureResources.Add(resource);

                this.BlistsDraw();
            }
        }

        return resource;
    }

    private SoundResource LoadSound(byte soundResourceIndex)
    {
        var resource = this.ResourceManager.FindSound(soundResourceIndex);
        if (resource is null)
        {
            this.BlistsErase();
            this.ScriptManager.Write(ScriptCodes.LoadSound, soundResourceIndex);

            resource = this.ResourceLoader.LoadSound(soundResourceIndex);
            if (resource is not null)
            {
                this.ResourceManager.SoundResources.Add(resource);

                this.BlistsDraw();
            }
        }

        return resource;
    }

    private ViewResource LoadView(byte viewResourceIndex, bool forceLoad)
    {
        var resource = this.ResourceManager.FindView(viewResourceIndex);
        if (resource is not null && !forceLoad)
        {
            return resource;
        }

        this.BlistsErase();

        if (resource is null)
        {
            this.ScriptManager.Write(ScriptCodes.LoadView, viewResourceIndex);
        }

        resource = this.ResourceLoader.LoadView(viewResourceIndex);
        if (resource is not null)
        {
            this.ResourceManager.ViewResources.Add(resource);

            this.BlistsDraw();
        }

        return resource;
    }

    private void OverlayPicture(byte pictureResourceIndex)
    {
        var resource = this.ResourceManager.FindPicture(pictureResourceIndex);
        if (resource is null)
        {
            this.ExecutionError(ErrorCodes.PictureOverlayResourceNotLoaded, pictureResourceIndex);
        }

        this.ScriptManager.Write(ScriptCodes.OverlayPicture, pictureResourceIndex);

        this.BlistsErase();
        this.GraphicsRenderer.DrawPictureToPictureBuffer(resource, true);
        this.BlistsDraw();
        this.BlistsUpdate();

        this.pictureVisible = false;
    }

    private void DrawPicture(byte pictureResourceIndex)
    {
        var resource = this.ResourceManager.FindPicture(pictureResourceIndex);
        if (resource is null)
        {
            this.ExecutionError(ErrorCodes.PictureDrawResourceNotLoaded, pictureResourceIndex);
        }

        this.ScriptManager.Write(ScriptCodes.DrawPicture, pictureResourceIndex);

        this.BlistsErase();
        this.GraphicsRenderer.DrawPictureToPictureBuffer(resource, false);
        this.BlistsDraw();

        this.pictureVisible = false;
    }

    private void DiscardPicture(byte pictureResourceIndex)
    {
        var resource = this.ResourceManager.FindPicture(pictureResourceIndex);
        if (resource is null)
        {
            this.ExecutionError(ErrorCodes.PictureDiscardPictureResourceNotLoaded, pictureResourceIndex);
        }

        this.ScriptManager.Write(ScriptCodes.DiscardPicture, pictureResourceIndex);

        this.ResourceManager.PictureResources.Remove(resource);

        this.BlistsErase();
        this.BlistsDraw();
    }

    private void DiscardView(byte viewResourceIndex)
    {
        var resource = this.ResourceManager.FindView(viewResourceIndex);
        if (resource is null)
        {
            this.ExecutionError(ErrorCodes.ViewDiscardViewResourceNotLoaded, viewResourceIndex);
        }

        this.ScriptManager.Write(ScriptCodes.DiscardView, viewResourceIndex);
        this.BlistsErase();

        int position = this.ResourceManager.ViewResources.IndexOf(resource);
        if (position >= 0)
        {
            int removeCount = this.ResourceManager.ViewResources.Count - position;
            for (int i = 0; i < removeCount; i++)
            {
                this.ResourceManager.ViewResources.RemoveAt(position);
            }
        }

        this.BlistsDraw();
    }

    private void CallRoom(byte logicResourceIndex)
    {
        this.SoundManager.StopPlaying();

        this.InputDriver.ClearEvents();

        this.ScriptManager.Clear();
        this.ScriptManager.Allow();

        for (int index = 0; index < this.ObjectTable.Length; index++)
        {
            var view = this.ObjectTable.GetAt(index);

            view.Flags &= ~(ViewObjectFlags.Animate | ViewObjectFlags.Drawn);
            view.Flags |= ViewObjectFlags.Update;
            view.ViewCel = null;
            view.ViewResource = null;
            view.Blit = null;
            view.StepTime = 1;
            view.StepCount = 1;
            view.CycleCount = 1;
            view.CycleTime = 1;
            view.StepSize = 1;
        }

        if (this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).Motion == Motion.Ego)
        {
            this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).Motion = Motion.Normal;
            this.State.Variables[Variables.Direction] = Direction.Motionless;
        }

        this.InitializeRoom();
        this.State.EgoControl = EgoControl.Player;
        this.State.BlockIsSet = false;
        this.State.Horizon = 0x24;

        this.State.Variables[Variables.PreviousRoom] = this.State.Variables[Variables.CurrentRoom];
        this.State.Variables[Variables.CurrentRoom] = logicResourceIndex;
        this.State.Variables[Variables.ObjectBorder] = BorderType.None;
        this.State.Variables[Variables.Object] = 0;
        this.State.Variables[Variables.EgoViewResource] = this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).ViewCur;
        this.State.Variables[Variables.FreeMemory] = 10;

        this.LoadLogic(logicResourceIndex, true);
        if (this.GameControl.TraceControl.TraceLogicIndex != 0)
        {
            this.LoadLogic(this.GameControl.TraceControl.TraceLogicIndex, false);
        }

        switch (this.State.Variables[Variables.Border])
        {
            case BorderType.ScreenTopEdgeOrHorizon:
                this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).Y = 0xa7;
                break;
            case BorderType.ScreenRightEdge:
                this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).X = 0;
                break;
            case BorderType.ScreenBottomEdge:
                this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).Y = 0x25;
                break;
            case BorderType.ScreenLeftEdge:
                this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).X = (byte)(0xa0 - this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).Width);
                break;
        }

        this.State.Variables[Variables.Border] = BorderType.None;
        this.State.Flags[Flags.NewRoom] = true;

        this.ClearAllControllers();
        this.GameControl.DisplayStatusLine();
        this.GameControl.InputControl.RedrawInput();
    }

    private void ClearAllControllers()
    {
        // Reset all booleans to false
        this.controlState = new bool[ControlMax];
    }

    private void ActivateController(int controller)
    {
        this.controlState[controller] = true;
    }

    private void TextScreen()
    {
        this.GameControl.InputControl.EnableInput();
        this.GraphicsRenderer.TextMode = true;
        this.GraphicsUpdatePalette();
        this.WindowManager.SetTextColor(this.State.TextForeground, this.State.TextBackground);
        this.GraphicsClear();
        this.WindowManager.ClearWindowPortion(0, 24, this.State.TextCombine);
        this.WindowManager.UpdateTextRegion();
    }

    private void GraphicsScreen()
    {
        this.GraphicsRenderer.TextMode = false;
        this.GraphicsUpdatePalette();
        this.WindowManager.SetTextColor(this.State.TextForeground, this.State.TextBackground);
        this.GraphicsClear();
        this.GraphicsRender(false, false);
        this.GameControl.DisplayStatusLine();
        this.GameControl.InputControl.RedrawInput();
    }

    private void GraphicsUpdatePalette()
    {
        var colors = this.GraphicsRenderer.GetPaletteColors();
        this.GraphicsDriver.SetPalette(colors);
    }

    private void GraphicsClear()
    {
        this.GraphicsDriver.Fill(new RenderRectangle(0, 0, this.GraphicsRenderer.RenderScaleX * 320, this.GraphicsRenderer.RenderScaleY * 200), 0);
        this.GraphicsDriver.Update(new RenderRectangle(0, 0, 0, 0));
    }

    private void GraphicsRender(bool picBuffRotate, bool fade)
    {
        if (fade)
        {
            this.GraphicsPaletteFadeOut();
        }

        this.GraphicsRenderer.RenderPictureBuffer(new PictureRectangle(0, PictureResource.Height - 1, PictureResource.Width, PictureResource.Height), picBuffRotate, fade && this.Preferences.Theme == UserInterfaceTheme.AtariST);

        if (fade)
        {
            this.GraphicsPaletteFadeIn();
        }
    }

    private void DisplayInventoryScreen()
    {
        var screen = new InventoryControl(this)
        {
            Inventory = this.ResourceManager.InventoryResource,
            SelectionEnabled = this.State.Flags[Flags.StatusSelect],
            SelectedInventoryNumber = this.State.Variables[Variables.StatusSelectedItem],
        };

        bool result = screen.DoModal();

        if (screen.SelectionEnabled)
        {
            if (result)
            {
                this.State.Variables[Variables.StatusSelectedItem] = screen.SelectedInventoryNumber;
            }
            else
            {
                this.State.Variables[Variables.StatusSelectedItem] = 0xff;
            }
        }
    }

    private bool RestartGame()
    {
        this.SoundManager.StopPlaying();
        bool inputDisabled = !this.GameControl.InputControl.InputEditEnabled;
        this.GameControl.InputControl.EnableInput();

        bool result;
        if (this.State.Flags[Flags.RestartMode])
        {
            result = true;
        }
        else
        {
            result = this.Prompt(UserInterface.RestartQuery);
        }

        if (result)
        {
            this.GameControl.InputControl.CancelInput();
            bool soundEnabled = this.State.Flags[Flags.SoundOn];
            this.InitializeGame();

            this.State.Flags[Flags.Restart] = true;
            if (soundEnabled)
            {
                this.State.Flags[Flags.SoundOn] = true;
            }

            this.State.Ticks = 0;
            if (this.GameControl.TraceControl.TraceLogicIndex != 0)
            {
                this.LoadLogic(this.GameControl.TraceControl.TraceLogicIndex, false);
            }

            this.Menu.EnableAllItems();
        }

        if (result || inputDisabled)
        {
            this.GameControl.InputControl.DisableInput();
        }

        return result;
    }

    private void WriteLogEntry(string text)
    {
        using (var writer = new StreamWriter(LogFileName, true))
        {
            writer.WriteLine();
            writer.WriteLine("Room {0}", this.State.Variables[Variables.CurrentRoom]);
            writer.WriteLine("Input line: {0}", this.GameControl.InputControl.InputPrevious);
            writer.WriteLine(text);
        }
    }

    [DoesNotReturn]
    private void ExecutionError(int a, int b)
    {
        this.SoundManager.StopPlaying();
        this.InputDriver.ClearEvents();
        this.InitializeRoom();

        this.State.Variables[Variables.Error] = (byte)a;
        this.State.Variables[Variables.Error2] = (byte)b;

        throw new ExecutionException();
    }

    private void SaveLogicScanStart()
    {
        this.SavedScanStarts.Clear();
        foreach (var logic in this.ResourceManager.LogicResources)
        {
            this.SavedScanStarts.Add(logic.ResourceIndex, logic.ScanStart);
        }
    }

    private void RestoreLogicScanStart(LogicResource resource)
    {
        if (this.SavedScanStarts.ContainsKey(resource.ResourceIndex))
        {
            resource.ScanStart = this.SavedScanStarts[resource.ResourceIndex];
        }
    }

    private void ReloadState()
    {
        this.SoundManager.StopPlaying();
        this.InitializeRoom();
        this.ScriptManager.Block();

        for (int index = 0; index < this.ObjectTable.Length; index++)
        {
            var view = this.ObjectTable.GetAt(index);

            // Hack content of some members temporarily
            // We'll restore them after we execute the scripts
            view.Number = (byte)view.X;
            view.X = view.Flags;
            if ((view.Flags & ViewObjectFlags.Animate) != 0)
            {
                view.Flags = (view.Flags & ~ViewObjectFlags.Drawn) | ViewObjectFlags.Update;
            }
        }

        this.BlistsErase();
        this.pictureVisible = false;

        this.ScriptManager.ResetIterator();
        byte[] scriptData = this.ScriptManager.IncrementIterator();
        while (scriptData is not null)
        {
            switch (scriptData[0])
            {
                case ScriptCodes.LoadLogic:
                    this.RestoreLogicScanStart(this.LoadLogic(scriptData[1], false));
                    break;
                case ScriptCodes.LoadView:
                    this.LoadView(scriptData[1], true);
                    break;
                case ScriptCodes.LoadPicture:
                    this.LoadPicture(scriptData[1]);
                    break;
                case ScriptCodes.LoadSound:
                    this.LoadSound(scriptData[1]);
                    break;
                case ScriptCodes.DrawPicture:
                    this.DrawPicture(scriptData[1]);
                    break;
                case ScriptCodes.AddToPicture:
                    {
                        scriptData = this.ScriptManager.IncrementIterator();
                        Debug.Assert(scriptData is not null, "Null script data.");
                        if (scriptData is not null)
                        {
                            byte addNum = scriptData[0];
                            byte addLoop = scriptData[1];

                            scriptData = this.ScriptManager.IncrementIterator();
                            Debug.Assert(scriptData is not null, "Null script data.");
                            if (scriptData is not null)
                            {
                                byte addCel = scriptData[0];
                                byte addX = scriptData[1];

                                scriptData = this.ScriptManager.IncrementIterator();
                                Debug.Assert(scriptData is not null, "Null script data.");
                                if (scriptData is not null)
                                {
                                    byte addY = scriptData[0];
                                    byte addPriority = scriptData[1];

                                    this.AddViewToPicture(addNum, addLoop, addCel, addX, addY, addPriority);
                                }
                            }
                        }
                    }

                    break;
                case ScriptCodes.DiscardPicture:
                    this.DiscardPicture(scriptData[1]);
                    break;
                case ScriptCodes.DiscardView:
                    this.DiscardView(scriptData[1]);
                    break;
                case ScriptCodes.OverlayPicture:
                    this.OverlayPicture(scriptData[1]);
                    break;
                default:
                    Debug.Assert(false, "Unexpected script instruction.");
                    break;
            }

            scriptData = this.ScriptManager.IncrementIterator();
        }

        this.ScriptManager.Allow();

        for (int i = 0; i < this.ObjectTable.Length; i++)
        {
            var view = this.ObjectTable.GetAt(i);

            // Fix members we temporarily hacked earlier
            int oldFlags = view.X;
            view.X = view.Number;
            view.Number = (byte)i;

            if (this.ResourceManager.FindView(view.ViewCur) is not null)
            {
                this.ObjViewSet(view, view.ViewCur);
            }

            if ((oldFlags & ViewObjectFlags.Animate) != 0)
            {
                if ((oldFlags & ViewObjectFlags.Drawn) != 0)
                {
                    this.DrawViewObject(view.Number);
                    if (view.Motion == Motion.Follow)
                    {
                        view.FollowCount = 0xff;
                    }
                }

                view.Flags = oldFlags;
            }
        }

        this.GameControl.InputControl.EnableInput();
        this.GameControl.InputControl.CancelInput();
        this.GraphicsRender(false, false);
        this.pictureVisible = true;
        this.GameControl.DisplayStatusLine();
        this.GameControl.InputControl.RedrawInput();
    }

    private GraphicsColor[] GetFadedPalette(int fadeStep, int fadeTotalCount)
    {
        var colors = this.GraphicsRenderer.GetPaletteColors();

        var fadedColors = new GraphicsColor[colors.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            fadedColors[i] = new GraphicsColor((byte)(colors[i].R * fadeStep / fadeTotalCount), (byte)(colors[i].G * fadeStep / fadeTotalCount), (byte)(colors[i].B * fadeStep / fadeTotalCount));
        }

        return fadedColors;
    }

    private void GraphicsPaletteFadeOut()
    {
        if (this.Preferences.DisplayFadeDelay > 0 && this.Preferences.DisplayFadeCount > 1)
        {
            this.GraphicsDriver.SetPalette(this.GetFadedPalette(this.Preferences.DisplayFadeCount, this.Preferences.DisplayFadeCount));
            for (int i = this.Preferences.DisplayFadeCount - 1; i >= 1; i--)
            {
                this.InputDriver.Sleep(this.Preferences.DisplayFadeDelay);
                this.GraphicsDriver.SetPalette(this.GetFadedPalette(i, this.Preferences.DisplayFadeCount));
            }
        }
    }

    private void GraphicsPaletteFadeIn()
    {
        if (this.Preferences.DisplayFadeDelay > 0 && this.Preferences.DisplayFadeCount > 1)
        {
            this.GraphicsDriver.SetPalette(this.GetFadedPalette(1, this.Preferences.DisplayFadeCount));
            for (int i = 2; i <= this.Preferences.DisplayFadeCount; i++)
            {
                this.InputDriver.Sleep(this.Preferences.DisplayFadeDelay);
                this.GraphicsDriver.SetPalette(this.GetFadedPalette(i, this.Preferences.DisplayFadeCount));
            }
        }
    }

    private void InitializeTraceWindow()
    {
        if (this.GameControl.TraceControl.TraceState == TraceState.Uninitialized && this.State.Flags[Flags.Debug])
        {
            this.GameControl.TraceControl.Show();
        }
    }

    private string PromptSaveRestoreFolder(string prompt, string currentPath)
    {
        if (this.SavedGameManager.AutoSaveName.Length > 0)
        {
            return currentPath;
        }

        var control = new SaveRestoreFolderBrowseControl(this)
        {
            Title = prompt,
            FolderPath = currentPath,
        };

        control.DoModal();

        return control.FolderPath;
    }

    private string PromptRestoreFilePath()
    {
        bool inputDisabled = !this.GameControl.InputControl.InputEditEnabled;
        this.GameControl.InputControl.EnableInput();
        this.SoundManager.StopPlaying();

        string filePath = string.Empty;

        string prompt = string.Format(CultureInfo.CurrentCulture, UserInterface.RestorePathPromptFormat, UserInterface.PathExample);
        string folderPath = this.PromptSaveRestoreFolder(prompt, SavedGameFolder);
        if (folderPath.Length > 0)
        {
            int index = this.PromptFileSlot(false, folderPath, out _);
            if (index > 0)
            {
                filePath = this.SavedGameManager.GetFilePath(index, folderPath);
            }
        }

        if (inputDisabled)
        {
            this.GameControl.InputControl.DisableInput();
        }

        return filePath;
    }

    private string PromptSaveFilePath(out string description)
    {
        bool inputDisabled = !this.GameControl.InputControl.InputEditEnabled;
        this.GameControl.InputControl.EnableInput();
        this.SoundManager.StopPlaying();

        description = string.Empty;

        string filePath = string.Empty;

        string prompt = string.Format(CultureInfo.CurrentCulture, UserInterface.SavePathPromptFormat, UserInterface.PathExample);
        string folderPath = this.PromptSaveRestoreFolder(prompt, SavedGameFolder);
        if (folderPath.Length > 0)
        {
            int index = this.PromptFileSlot(true, folderPath, out description);
            if (index > 0)
            {
                if (this.SavedGameManager.AutoSaveName.Length == 0)
                {
                    var inputBox = new InputBoxControl(this)
                    {
                        Title = UserInterface.SaveDescriptionPrompt,
                        Text = description,
                    };

                    if (inputBox.DoModal())
                    {
                        description = inputBox.Text;
                        filePath = this.SavedGameManager.GetFilePath(index, folderPath);
                    }
                }
                else
                {
                    filePath = this.SavedGameManager.GetFilePath(index, folderPath);
                }
            }
        }

        if (inputDisabled)
        {
            this.GameControl.InputControl.DisableInput();
        }

        return filePath;
    }

    private int PromptFileSlot(bool save, string folderPath, out string description)
    {
        int[] slotNumbers;
        string[] descriptions;
        int slotCount;
        int current;

        description = string.Empty;

        if (save)
        {
            this.SavedGameManager.GetSaveSlotInformation(folderPath, out slotNumbers, out descriptions, out slotCount, out current);
        }
        else
        {
            this.SavedGameManager.GetRestoreSlotInformation(folderPath, out slotNumbers, out descriptions, out slotCount, out current);
        }

        if (!save && slotCount == 0 && this.SavedGameManager.AutoSaveName.Length == 0)
        {
            string msg = string.Format(CultureInfo.CurrentCulture, UserInterface.RestoreNoGamesInFolderFormat, folderPath);
            this.Prompt(msg);
            return 0;
        }

        if (this.SavedGameManager.AutoSaveName.Length > 0 && !this.diskSpaceAvailable)
        {
            description = this.SavedGameManager.AutoSaveName;
            for (int i = 0; i < descriptions.Length; i++)
            {
                if (descriptions[i] == description)
                {
                    return slotNumbers[i];
                }
            }

            if (save)
            {
                for (int i = 0; i < descriptions.Length; i++)
                {
                    if (descriptions[i].Length == 0)
                    {
                        return slotNumbers[i];
                    }
                }
            }

            if (!save)
            {
                return 0;
            }
        }

        string title;
        if (this.SavedGameManager.AutoSaveName.Length == 0)
        {
            // Select game
            if (save)
            {
                title = UserInterface.SaveSelectSlot;
            }
            else
            {
                title = UserInterface.RestoreSelectSlot;
            }
        }
        else if (!this.diskSpaceAvailable)
        {
            // Disk full
            title = UserInterface.SaveDiskFull;
        }
        else
        {
            // Select name
            title = UserInterface.SaveRestoreSelectName;
        }

        this.diskSpaceAvailable = false;

        if (slotCount == 0)
        {
            return 0;
        }

        var control = new SaveRestoreGameBrowseControl(this)
        {
            Title = title,
        };

        control.SetSlotInformation(descriptions, slotCount);
        control.SelectedSlotIndex = current;

        if (control.DoModal())
        {
            if (this.SavedGameManager.AutoSaveName.Length == 0)
            {
                description = descriptions[control.SelectedSlotIndex];
            }
            else
            {
                if (!save)
                {
                    this.SavedGameManager.AutoSaveName = descriptions[control.SelectedSlotIndex];
                }
            }

            return slotNumbers[control.SelectedSlotIndex];
        }
        else
        {
            return 0;
        }
    }

    private void SaveGame()
    {
        string filePath = this.PromptSaveFilePath(out var description);
        if (filePath.Length > 0)
        {
            this.clockState = ClockState.Pause;

            this.SavedGameManager.SaveTo(filePath, description);
            this.Prompt(UserInterface.SaveDone);

            this.clockState = ClockState.Normal;
        }
    }

    private bool RestoreGame()
    {
        bool restored = false;

        string filePath = this.PromptRestoreFilePath();
        if (filePath.Length > 0 && File.Exists(filePath))
        {
            restored = this.RestoreGame(filePath);
        }

        return restored;
    }

    private bool RestoreGame(string filePath)
    {
        this.clockState = ClockState.Pause;

        using (var stream = new FileStream(filePath, FileMode.Open))
        {
            this.SavedGameManager.GameSerializer.LoadFrom(this, stream);
        }

        this.State.Variables[Variables.ComputerType] = this.computerType;
        this.State.Variables[Variables.DisplayType] = this.displayType;
        this.State.Variables[Variables.FreeMemory] = 10;

        if (this.computerType == ComputerType.PC)
        {
            this.State.Variables[Variables.SoundType] = SoundType.PC;
        }
        else
        {
            this.State.Variables[Variables.SoundType] = SoundType.Tandy;
            this.State.Flags[Flags.MultiChannelSound] = true;
        }

        this.ReloadState();
        this.ClearAllControllers();
        this.State.Flags[Flags.Restore] = true;
        this.Menu.EnableAllItems();

        this.clockState = ClockState.Normal;

        return true;
    }

    private void DrawViewObject(byte viewNum)
    {
        if (viewNum >= this.ObjectTable.Length)
        {
            this.ExecutionError(ErrorCodes.ObjectDrawViewObjectOutOfRange, viewNum);
        }

        var view = this.ObjectTable.GetAt(viewNum);
        if (view.ViewCel is null)
        {
            this.ExecutionError(ErrorCodes.ObjectDrawViewObjectCelNull, viewNum);
        }

        if ((view.Flags & ViewObjectFlags.Drawn) == 0)
        {
            view.Flags |= ViewObjectFlags.Update;
            this.ObjectManager.ShufflePosition(view);
            view.CelPrevWidth = view.ViewCel.Width;
            view.CelPrevHeight = view.ViewCel.Height;
            view.PreviousX = view.X;
            view.PreviousY = view.Y;

            this.BlitlistErase(this.blitlistUpdated);

            view.Flags |= ViewObjectFlags.Drawn;

            this.BuildUpdatedList();
            this.BlitlistDraw(this.blitlistUpdated);

            this.ObjCelUpdate(view);
            view.Flags &= ~ViewObjectFlags.SkipUpdate;
        }
    }

    private void EraseViewObject(byte viewNum)
    {
        if (viewNum >= this.ObjectTable.Length)
        {
            this.ExecutionError(ErrorCodes.ObjectEraseViewObjectOutOfRange, viewNum);
        }

        var view = this.ObjectTable.GetAt(viewNum);
        if ((view.Flags & ViewObjectFlags.Drawn) != 0)
        {
            bool update;

            this.BlitlistErase(this.blitlistUpdated);

            if ((view.Flags & ViewObjectFlags.Update) == 0)
            {
                update = false;
            }
            else
            {
                update = true;
            }

            if (!update)
            {
                this.BlitlistErase(this.blitlistStatic);
            }

            view.Flags &= ~ViewObjectFlags.Drawn;

            if (!update)
            {
                this.BuildStaticList();
                this.BlitlistDraw(this.blitlistStatic);
            }

            this.BuildUpdatedList();
            this.BlitlistDraw(this.blitlistUpdated);

            this.ObjCelUpdate(view);
        }
    }

    private void ObjStopUpdate(ViewObject view)
    {
        if ((view.Flags & ViewObjectFlags.Update) != 0)
        {
            this.BlistsErase();
            view.Flags &= ~ViewObjectFlags.Update;
            this.BlistsDraw();
        }
    }

    private void ObjStartUpdate(ViewObject view)
    {
        if ((view.Flags & ViewObjectFlags.Update) == 0)
        {
            this.BlistsErase();
            view.Flags |= ViewObjectFlags.Update;
            this.BlistsDraw();
        }
    }

    private void ObjViewSet(ViewObject view, byte viewResourceIndex)
    {
        var resource = this.ResourceManager.FindView(viewResourceIndex);
        if (resource is null)
        {
            this.ExecutionError(ErrorCodes.ObjectViewSetViewResourceNotLoaded, viewResourceIndex);
        }

        this.ObjectManager.SetView(view, resource);
    }

    private void ShowView(byte viewResourceIndex)
    {
        this.ScriptManager.Block();
        bool loaded = this.ResourceManager.FindView(viewResourceIndex) is not null;

        var resource = this.LoadView(viewResourceIndex, false);
        if (resource is null)
        {
            this.Prompt(UserInterface.NotNow);
        }
        else
        {
            var view = new ViewObject
            {
                CelCur = 0,
                LoopCur = 0,
            };

            this.ObjViewSet(view, viewResourceIndex);
            view.CelPrevHeight = view.ViewCel.Height;
            view.CelPrevWidth = view.ViewCel.Width;
            view.PreviousX = (PictureResource.Width - 1 - view.Width) / 2;
            view.X = view.PreviousX;
            view.PreviousY = PictureResource.Height - 1;
            view.Y = PictureResource.Height - 1;
            view.Priority = 0x0f;
            view.Flags |= ViewObjectFlags.PriorityFixed;
            view.Number = 0xff;

            var blit = AgiInterpreter.CreateBlit(view);
            this.BlitSave(blit);
            this.GraphicsRenderer.DrawViewToPictureBuffer(view);
            this.ObjCelUpdate(view);

            resource = this.ResourceManager.FindView(viewResourceIndex);
            this.Prompt(resource.Description);

            this.BlitRestore(blit);
            this.ObjCelUpdate(view);

            if (!loaded)
            {
                this.DiscardView(viewResourceIndex);
            }

            this.ScriptManager.Allow();
        }
    }

    private void ObjCelUpdate(ViewObject view)
    {
        if (!this.pictureVisible)
        {
            return;
        }

        byte prevHeight = view.CelPrevHeight;
        byte prevWidth = view.CelPrevWidth;

        view.CelPrevHeight = view.ViewCel.Height;
        view.CelPrevWidth = view.ViewCel.Width;

        int x;
        int y;
        int w;
        int h;

        int y2;
        int h1;
        int h2;

        if (view.Y < view.PreviousY)
        {
            y = view.PreviousY;
            y2 = view.Y;

            h1 = prevHeight;
            h2 = view.ViewCel.Height;
        }
        else
        {
            y = view.Y;
            y2 = view.PreviousY;
            h1 = view.ViewCel.Height;
            h2 = prevHeight;
        }

        if ((y2 - h2) > (y - h1))
        {
            h = h1;
        }
        else
        {
            h = y - y2 + h2;
        }

        int x2;
        int w1;
        int w2;

        if (view.X > view.PreviousX)
        {
            x = view.PreviousX;
            x2 = view.X;
            w1 = prevWidth;
            w2 = view.ViewCel.Width;
        }
        else
        {
            x = view.X;
            x2 = view.PreviousX;
            w1 = view.ViewCel.Width;
            w2 = prevWidth;
        }

        if ((x2 + w2) < (x + w1))
        {
            w = w1;
        }
        else
        {
            w = w2 + x2 - x;
        }

        if ((x + w) > 161)
        {
            w = 161 - x;
        }

        if ((h - y) > 1)
        {
            h = y + 1;
        }

        this.GraphicsRenderer.RenderPictureBuffer(new PictureRectangle(x, y, w, h), false, false);
    }

    private void AddViewToPicture(byte viewResourceIndex, byte loop, byte cel, byte x, byte y, byte priority)
    {
        this.ScriptManager.Write(ScriptCodes.AddToPicture, 0);
        this.ScriptManager.Write(viewResourceIndex, loop);
        this.ScriptManager.Write(cel, x);
        this.ScriptManager.Write(y, priority);

        var view = new ViewObject();
        this.ObjViewSet(view, viewResourceIndex);
        this.ObjectManager.SetViewLoop(view, loop);
        this.ObjectManager.SetViewCel(view, cel);

        view.CelPrevHeight = view.ViewCel.Height;
        view.CelPrevWidth = view.ViewCel.Width;

        view.PreviousX = x;
        view.X = x;
        view.PreviousY = y;
        view.Y = y;
        view.Flags = ViewObjectFlags.IgnoreObjects | ViewObjectFlags.IgnoreHorizon | ViewObjectFlags.PriorityFixed;
        view.Priority = 0xf;

        this.ObjectManager.ShufflePosition(view);

        if ((priority & 0xf) == 0)
        {
            view.Flags = 0;
        }

        view.Priority = priority;

        this.BlistsErase();
        this.AddViewToPictureBuffer(view);
        this.BlistsDraw();
        this.ObjCelUpdate(view);
    }

    private void AddViewToPictureBuffer(ViewObject view)
    {
        if ((view.Priority & 0x0f) == 0)
        {
            view.Priority = (byte)(view.Priority | this.PriorityTable.GetPriorityAt(view.Y));
        }

        this.GraphicsRenderer.DrawViewToPictureBuffer(view);

        if (view.Priority <= 0x3f)
        {
            // count up from current priority to find the size of the box that is at the view's feet
            // this prevents the ego from walking into a different priority or walking through the view.
            int cx = view.Y;
            byte priorityHeight = 0;
            do
            {
                priorityHeight++;
                if (cx <= 0)
                {
                    break;
                }

                cx--;
            }
            while (this.PriorityTable.GetPriorityAt(cx) == this.PriorityTable.GetPriorityAt(view.Y));

            byte height = view.ViewCel.Height;
            if (height > priorityHeight)
            {
                height = priorityHeight;
            }

            // bottom line
            for (int i = 0; i < view.ViewCel.Width; i++)
            {
                this.GraphicsRenderer.PictureBuffer.Priority[(view.Y * PictureResource.Width) + view.X + i] = (byte)(view.Priority & 0xf0);
            }

            if (height > 1)
            {
                // sides
                for (int j = 1; j < height; j++)
                {
                    this.GraphicsRenderer.PictureBuffer.Priority[((view.Y - j) * PictureResource.Width) + view.X] = (byte)(view.Priority & 0xf0);
                    this.GraphicsRenderer.PictureBuffer.Priority[((view.Y - j) * PictureResource.Width) + view.X + view.Width - 1] = (byte)(view.Priority & 0xf0);
                }

                // top line
                for (int i = 1; i < view.ViewCel.Width - 1; i++)
                {
                    this.GraphicsRenderer.PictureBuffer.Priority[((view.Y - (height - 1)) * PictureResource.Width) + view.X + i] = (byte)(view.Priority & 0xf0);
                }
            }
        }
    }

    private void UpdateObjectTable()
    {
        bool modified = this.ObjectManager.UpdateAll();

        if (modified)
        {
            this.BlitlistErase(this.blitlistUpdated);

            this.ObjectManager.StepUpdateAll();

            this.BuildUpdatedList();
            this.BlitlistDraw(this.blitlistUpdated);
            this.BlitlistUpdate(this.blitlistUpdated);

            this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).Flags &= ~(ViewObjectFlags.Land | ViewObjectFlags.Water);
        }
    }

    private void BuildUpdatedList()
    {
        this.BlitlistBuild(ViewObjectManager.IsUpdated, this.blitlistUpdated);
    }

    private void BuildStaticList()
    {
        this.BlitlistBuild(ViewObjectManager.IsStatic, this.blitlistStatic);
    }

    private void BlitlistBuild(BlitlistBuildPredicate predicate, List<Blit> blits)
    {
        var usedViews = new List<ViewObject>();
        var sortOrders = new List<int>();

        for (int index = 0; index < this.ObjectTable.Length; index++)
        {
            var view = this.ObjectTable.GetAt(index);

            if (predicate(view))
            {
                usedViews.Add(view);

                if ((view.Flags & ViewObjectFlags.PriorityFixed) != 0)
                {
                    sortOrders.Add(this.PriorityTable.CalculateSortPosition(view.Priority));
                }
                else
                {
                    sortOrders.Add(view.Y);
                }
            }
        }

        int next = 256;
        for (int i = 0; i < usedViews.Count; i++)
        {
            int currentPosition = 255;

            for (int d = 0; d < sortOrders.Count; d++)
            {
                if (sortOrders[d] < currentPosition)
                {
                    next = d;
                    currentPosition = sortOrders[d];
                }
            }

            sortOrders[next] = 255;
            Debug.Assert(next != 256, "Oops!");
            AgiInterpreter.BlitAdd(usedViews[next], blits);
        }
    }

    private void BlistsErase()
    {
        this.BlitlistErase(this.blitlistUpdated);
        this.BlitlistErase(this.blitlistStatic);
    }

    private void BlistsDraw()
    {
        this.BuildStaticList();
        this.BlitlistDraw(this.blitlistStatic);

        this.BuildUpdatedList();
        this.BlitlistDraw(this.blitlistUpdated);
    }

    private void BlistsUpdate()
    {
        this.BlitlistUpdate(this.blitlistStatic);
        this.BlitlistUpdate(this.blitlistUpdated);
    }

    private void BlitlistUpdate(List<Blit> blits)
    {
        int last = blits.Count - 1;
        for (int cur = last; cur >= 0; cur--)
        {
            var blit = blits[cur];
            var view = blit.View;

            this.ObjCelUpdate(view);
            if (view.StepCount == view.StepTime)
            {
                if ((view.X == view.PreviousX) && (view.Y == view.PreviousY))
                {
                    view.Flags |= ViewObjectFlags.MotionLess;
                }
                else
                {
                    view.PreviousX = view.X;
                    view.PreviousY = view.Y;
                    view.Flags &= ~ViewObjectFlags.MotionLess;
                }
            }
        }
    }

    private void BlitlistErase(List<Blit> blits)
    {
        int last = blits.Count - 1;
        for (int cur = last; cur >= 0; cur--)
        {
            this.BlitRestore(blits[cur]);
        }

        AgiInterpreter.BlitlistFree(blits);
    }

    private void BlitlistDraw(List<Blit> blits)
    {
        foreach (var blit in blits)
        {
            this.BlitSave(blit);
            this.GraphicsRenderer.DrawViewToPictureBuffer(blit.View);
        }
    }

    private void BlitSave(Blit blit)
    {
        for (int j = 0; j < blit.Height; j++)
        {
            int y = blit.Y + j;

            for (int i = 0; i < blit.Width; i++)
            {
                int x = blit.X + i;

                blit.PriorityBuffer[(j * blit.Width) + i] = this.GraphicsRenderer.PictureBuffer.Priority[(y * PictureResource.Width) + x];
                blit.VisualBuffer[(j * blit.Width) + i] = this.GraphicsRenderer.PictureBuffer.Visual[(y * PictureResource.Width) + x];
            }
        }
    }

    private void BlitRestore(Blit blit)
    {
        for (int j = 0; j < blit.Height; j++)
        {
            int y = blit.Y + j;

            for (int i = 0; i < blit.Width; i++)
            {
                int x = blit.X + i;

                this.GraphicsRenderer.PictureBuffer.Priority[(y * PictureResource.Width) + x] = blit.PriorityBuffer[(j * blit.Width) + i];
                this.GraphicsRenderer.PictureBuffer.Visual[(y * PictureResource.Width) + x] = blit.VisualBuffer[(j * blit.Width) + i];
            }
        }
    }

    private void ProcessEvent(InputEvent e)
    {
        switch (e.Type)
        {
            case InputEventType.Ascii:
            case InputEventType.Mouse:
                this.GameControl.ProcessEvent(e);
                break;

            case InputEventType.Direction:
                this.ProcessDirection(e.Data);
                break;

            case InputEventType.Controller:
                this.ActivateController(e.Data);
                break;

            default:
                break;
        }
    }

    private InputEvent MapControlKey(InputEvent e)
    {
        if (e is not null)
        {
            if (e.Type == InputEventType.Ascii)
            {
                for (int i = 0; i < this.State.Controls.Length; i++)
                {
                    if (this.State.Controls[i].Key == 0)
                    {
                        break;
                    }

                    if (this.State.Controls[i].Key == e.Data)
                    {
                        e.Type = InputEventType.Controller;
                        e.Data = this.State.Controls[i].Number;
                        break;
                    }
                }
            }
        }

        return e;
    }

    private void ProcessDirection(int direction)
    {
        if (direction == this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).Direction)
        {
            this.State.Variables[Variables.Direction] = Direction.Motionless;
        }
        else
        {
            this.State.Variables[Variables.Direction] = (byte)direction;
        }

        if (this.State.EgoControl == EgoControl.Player)
        {
            this.ObjectTable.GetAt(ViewObjectTable.EgoIndex).Motion = Motion.Normal;
        }
    }
}
