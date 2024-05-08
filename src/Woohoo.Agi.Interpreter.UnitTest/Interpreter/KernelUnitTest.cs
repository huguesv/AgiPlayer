// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.UnitTest;

using Woohoo.Agi.Resources;

[TestClass]
public class KernelUnitTest
{
    private const string TestNotImplemented = "Test not implemented.";

    private AgiInterpreter interpreter;
    private IKernel kernel;

    [TestInitialize]
    public void CreateKernel()
    {
        this.interpreter = new AgiInterpreter(null, null, null);
        this.interpreter.CreateState();
        this.kernel = this.interpreter;
    }

    [TestMethod]
    public void EqualN()
    {
        this.interpreter.State.Variables[23] = 0;
        this.kernel.EqualN(23, 0).Should().BeTrue();
        this.kernel.EqualN(23, 5).Should().BeFalse();
        this.interpreter.State.Variables[23] = 5;
        this.kernel.EqualN(23, 5).Should().BeTrue();
    }

    [TestMethod]
    public void EqualV()
    {
        this.interpreter.State.Variables[22] = 0;
        this.interpreter.State.Variables[23] = 0;
        this.kernel.EqualV(23, 22).Should().BeTrue();
        this.interpreter.State.Variables[23] = 5;
        this.kernel.EqualV(23, 22).Should().BeFalse();
        this.interpreter.State.Variables[22] = 5;
        this.kernel.EqualV(23, 22).Should().BeTrue();
    }

    [TestMethod]
    public void LessN()
    {
        this.interpreter.State.Variables[23] = 0;
        this.kernel.LessN(23, 5).Should().BeTrue();
        this.kernel.LessN(23, 0).Should().BeFalse();
        this.interpreter.State.Variables[23] = 5;
        this.kernel.LessN(23, 4).Should().BeFalse();
        this.kernel.LessN(23, 6).Should().BeTrue();
    }

    [TestMethod]
    public void LessV()
    {
        this.interpreter.State.Variables[22] = 0;
        this.interpreter.State.Variables[23] = 0;
        this.kernel.LessV(23, 22).Should().BeFalse();
        this.interpreter.State.Variables[23] = 5;
        this.kernel.LessV(23, 22).Should().BeFalse();
        this.interpreter.State.Variables[22] = 10;
        this.kernel.LessV(23, 22).Should().BeTrue();
    }

    [TestMethod]
    public void GreaterN()
    {
        this.interpreter.State.Variables[23] = 0;
        this.kernel.GreaterN(23, 5).Should().BeFalse();
        this.kernel.GreaterN(23, 0).Should().BeFalse();
        this.interpreter.State.Variables[23] = 5;
        this.kernel.GreaterN(23, 4).Should().BeTrue();
        this.kernel.GreaterN(23, 6).Should().BeFalse();
    }

    [TestMethod]
    public void GreaterV()
    {
        this.interpreter.State.Variables[22] = 0;
        this.interpreter.State.Variables[23] = 0;
        this.kernel.GreaterV(23, 22).Should().BeFalse();
        this.interpreter.State.Variables[23] = 5;
        this.kernel.GreaterV(23, 22).Should().BeTrue();
        this.interpreter.State.Variables[22] = 10;
        this.kernel.GreaterV(23, 22).Should().BeFalse();
    }

    [TestMethod]
    public void IsSet()
    {
        this.interpreter.State.Flags[23] = false;
        this.kernel.IsSet(23).Should().BeFalse();
        this.interpreter.State.Flags[23] = true;
        this.kernel.IsSet(23).Should().BeTrue();
    }

    [TestMethod]
    public void IsSetV()
    {
        this.interpreter.State.Variables[23] = 5;
        this.interpreter.State.Flags[5] = false;
        this.kernel.IsSetV(23).Should().BeFalse();
        this.interpreter.State.Flags[5] = true;
        this.kernel.IsSetV(23).Should().BeTrue();
    }

    [TestMethod]
    public void Has()
    {
        var items = new InventoryItem[]
        {
            new InventoryItem("key", 255),
            new InventoryItem("pen", 20),
        };
        var resource = new InventoryResource(items, 1);
        this.interpreter.ResourceManager = new ResourceManager();
        this.interpreter.ResourceManager.InventoryResource = resource;

        this.kernel.Has(0).Should().BeTrue();
        this.kernel.Has(1).Should().BeFalse();
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void PosN()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void CenterPosN()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void RightPosN()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ObjInBox()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Controller()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ObjInRoom()
    {
    }

    [TestMethod]
    public void Said()
    {
        var families = new VocabularyWordFamily[]
        {
            new VocabularyWordFamily(0, "a", "the"),
            new VocabularyWordFamily(1, "get"),
            new VocabularyWordFamily(2, "key"),
            new VocabularyWordFamily(3, "box"),
        };
        var vocabulary = new VocabularyResource(families);
        this.interpreter.ResourceManager = new ResourceManager();
        this.interpreter.ResourceManager.VocabularyResource = vocabulary;

        this.interpreter.State.Flags[Flags.SaidAccepted] = false;
        this.interpreter.ParseText("get key");
        this.kernel.Said(new int[] { 1, 2 });
        this.interpreter.State.Flags[Flags.SaidAccepted].Should().BeTrue();

        this.interpreter.State.Flags[Flags.SaidAccepted] = false;
        this.interpreter.ParseText("get the key");
        this.kernel.Said(new int[] { 1, 2 });
        this.interpreter.State.Flags[Flags.SaidAccepted].Should().BeTrue();

        this.interpreter.State.Flags[Flags.SaidAccepted] = false;
        this.interpreter.ParseText("get");
        this.kernel.Said(new int[] { 1, 2 });
        this.interpreter.State.Flags[Flags.SaidAccepted].Should().BeFalse();

        this.interpreter.State.Flags[Flags.SaidAccepted] = false;
        this.interpreter.ParseText("get key box");
        this.kernel.Said(new int[] { 1, 2 });
        this.interpreter.State.Flags[Flags.SaidAccepted].Should().BeFalse();
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void HaveKey()
    {
    }

    [TestMethod]
    public void CompareStrings()
    {
        this.interpreter.State.Strings[0] = "hello1";
        this.interpreter.State.Strings[1] = "hello2";
        this.kernel.CompareStrings(0, 1).Should().BeFalse();

        this.interpreter.State.Strings[0] = "hello";
        this.interpreter.State.Strings[1] = "hello";
        this.kernel.CompareStrings(0, 1).Should().BeTrue();

        this.interpreter.State.Strings[0] = "Hello";
        this.interpreter.State.Strings[1] = "hello";
        this.kernel.CompareStrings(0, 1).Should().BeTrue();
    }

    [TestMethod]
    public void ReturnFalse()
    {
        this.kernel.ReturnFalse();
    }

    [TestMethod]
    public void Increment()
    {
        this.interpreter.State.Variables[23] = 5;
        this.kernel.Increment(23);
        this.interpreter.State.Variables[23].Should().Be(6);

        this.interpreter.State.Variables[23] = 255;
        this.kernel.Increment(23);
        this.interpreter.State.Variables[23].Should().Be(255);
    }

    [TestMethod]
    public void Decrement()
    {
        this.interpreter.State.Variables[23] = 5;
        this.kernel.Decrement(23);
        this.interpreter.State.Variables[23].Should().Be(4);

        this.interpreter.State.Variables[23] = 0;
        this.kernel.Decrement(23);
        this.interpreter.State.Variables[23].Should().Be(0);
    }

    [TestMethod]
    public void AssignN()
    {
        this.kernel.AssignN(23, 5);
        this.interpreter.State.Variables[23].Should().Be(5);
    }

    [TestMethod]
    public void AssignV()
    {
        this.interpreter.State.Variables[23] = 5;
        this.kernel.AssignV(22, 23);
        this.interpreter.State.Variables[22].Should().Be(5);
    }

    [TestMethod]
    public void AddN()
    {
        this.interpreter.State.Variables[23] = 5;
        this.kernel.AddN(23, 7);
        this.interpreter.State.Variables[23].Should().Be(12);
    }

    [TestMethod]
    public void AddV()
    {
        this.interpreter.State.Variables[23] = 5;
        this.interpreter.State.Variables[22] = 7;
        this.kernel.AddV(23, 22);
        this.interpreter.State.Variables[23].Should().Be(12);
        this.interpreter.State.Variables[22].Should().Be(7);
    }

    [TestMethod]
    public void SubN()
    {
        this.interpreter.State.Variables[23] = 5;
        this.kernel.SubN(23, 3);
        this.interpreter.State.Variables[23].Should().Be(2);
    }

    [TestMethod]
    public void SubV()
    {
        this.interpreter.State.Variables[23] = 5;
        this.interpreter.State.Variables[22] = 3;
        this.kernel.SubV(23, 22);
        this.interpreter.State.Variables[23].Should().Be(2);
        this.interpreter.State.Variables[22].Should().Be(3);
    }

    [TestMethod]
    public void LIndirectV()
    {
        this.interpreter.State.Variables[5] = 6;
        this.interpreter.State.Variables[10] = 5;
        this.interpreter.State.Variables[15] = 70;
        this.kernel.LIndirectV(10, 15);
        this.interpreter.State.Variables[5].Should().Be(70);
    }

    [TestMethod]
    public void RIndirect()
    {
        this.interpreter.State.Variables[5] = 6;
        this.interpreter.State.Variables[10] = 5;
        this.interpreter.State.Variables[15] = 70;
        this.kernel.RIndirect(15, 10);
        this.interpreter.State.Variables[15].Should().Be(6);
    }

    [TestMethod]
    public void LIndirectN()
    {
        this.interpreter.State.Variables[5] = 6;
        this.interpreter.State.Variables[10] = 5;
        this.kernel.LIndirectN(10, 80);
        this.interpreter.State.Variables[5].Should().Be(80);
    }

    [TestMethod]
    public void Set()
    {
        this.interpreter.State.Flags[23] = false;
        this.kernel.Set(23);
        this.interpreter.State.Flags[23].Should().BeTrue();
    }

    [TestMethod]
    public void Reset()
    {
        this.interpreter.State.Flags[23] = true;
        this.kernel.Reset(23);
        this.interpreter.State.Flags[23].Should().BeFalse();
    }

    [TestMethod]
    public void Toggle()
    {
        this.interpreter.State.Flags[23] = true;
        this.kernel.Toggle(23);
        this.interpreter.State.Flags[23].Should().BeFalse();
        this.kernel.Toggle(23);
        this.interpreter.State.Flags[23].Should().BeTrue();
    }

    [TestMethod]
    public void SetV()
    {
        this.interpreter.State.Variables[5] = 23;
        this.interpreter.State.Flags[23] = false;
        this.kernel.SetV(5);
        this.interpreter.State.Flags[23].Should().BeTrue();
    }

    [TestMethod]
    public void ResetV()
    {
        this.interpreter.State.Variables[5] = 23;
        this.interpreter.State.Flags[23] = true;
        this.kernel.ResetV(5);
        this.interpreter.State.Flags[23].Should().BeFalse();
    }

    [TestMethod]
    public void ToggleV()
    {
        this.interpreter.State.Variables[5] = 23;
        this.interpreter.State.Flags[23] = true;
        this.kernel.ToggleV(5);
        this.interpreter.State.Flags[23].Should().BeFalse();
        this.kernel.ToggleV(5);
        this.interpreter.State.Flags[23].Should().BeTrue();
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void NewRoom()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void NewRoomV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void LoadLogics()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void LoadLogicsV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Call()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void CallV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void LoadPicture()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void DrawPicture()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ShowPicture()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void DiscardPicture()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void OverlayPicture()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ShowPriScreen()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void LoadView()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void LoadViewV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void DiscardView()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void AnimateObj()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void UnanimateAll()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Draw()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Erase()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Position()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void PositionV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void GetPosition()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Reposition()
    {
    }

    [TestMethod]
    public void SetView()
    {
        var resource = CreateTestViewResource(60);
        this.interpreter.ResourceManager = new ResourceManager();
        this.interpreter.ResourceManager.ViewResources.Add(resource);
        this.interpreter.ObjectTable = new ViewObjectTable(1);
        this.interpreter.ObjectManager = new ViewObjectManager(this.interpreter, null);

        this.kernel.SetView(0, 60);

        var view = this.interpreter.ObjectTable.GetAt(0);
        view.ViewResource.Should().BeSameAs(resource);
    }

    [TestMethod]
    public void SetViewV()
    {
        this.interpreter.State.Variables[25] = 60;
        var resource = CreateTestViewResource(60);
        this.interpreter.ResourceManager = new ResourceManager();
        this.interpreter.ResourceManager.ViewResources.Add(resource);
        this.interpreter.ObjectTable = new ViewObjectTable(1);
        this.interpreter.ObjectManager = new ViewObjectManager(this.interpreter, null);

        this.kernel.SetViewV(0, 25);

        var view = this.interpreter.ObjectTable.GetAt(0);
        view.ViewResource.Should().BeSameAs(resource);
    }

    [TestMethod]
    public void SetLoop()
    {
        this.interpreter.ObjectTable = new ViewObjectTable(1);
        this.interpreter.ObjectManager = new ViewObjectManager(this.interpreter, null);
        var view = this.interpreter.ObjectTable.GetAt(0);
        var resource = CreateTestViewResource(60);
        view.LoopTotal = (byte)resource.Loops.Length;
        view.ViewResource = resource;

        this.kernel.SetLoop(0, 2);

        view.LoopCur.Should().Be(2);
        view.CelTotal.Should().Be(1);
    }

    [TestMethod]
    public void SetLoopV()
    {
        this.interpreter.State.Variables[25] = 2;
        this.interpreter.ObjectTable = new ViewObjectTable(1);
        this.interpreter.ObjectManager = new ViewObjectManager(this.interpreter, null);
        var view = this.interpreter.ObjectTable.GetAt(0);
        var resource = CreateTestViewResource(60);
        view.LoopTotal = (byte)resource.Loops.Length;
        view.ViewResource = resource;

        this.kernel.SetLoopV(0, 25);

        view.LoopCur.Should().Be(2);
        view.CelTotal.Should().Be(1);
    }

    [TestMethod]
    public void FixLoop()
    {
        this.interpreter.ObjectTable = new ViewObjectTable(1);
        var view = this.interpreter.ObjectTable.GetAt(0);
        view.Flags = 0;
        this.kernel.FixLoop(0);
        (view.Flags & ViewObjectFlags.LoopFixed).Should().NotBe(0);
    }

    [TestMethod]
    public void ReleaseLoop()
    {
        this.interpreter.ObjectTable = new ViewObjectTable(1);
        var view = this.interpreter.ObjectTable.GetAt(0);
        view.Flags = ViewObjectFlags.LoopFixed;
        this.kernel.ReleaseLoop(0);
        (view.Flags & ViewObjectFlags.LoopFixed).Should().Be(0);
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetCel()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetCelV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void LastCel()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void CurrentCel()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void CurrentLoop()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void CurrentView()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void NumberOfLoops()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetPriority()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetPriorityV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ReleasePriority()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void GetPriority()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void StopUpdate()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void StartUpdate()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ForceUpdate()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void IgnoreHorizon()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ObserveHorizon()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetHorizon()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ObjectOnWater()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ObjectOnLand()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ObjectOnAnything()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void IgnoreObjects()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ObserveObjects()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Distance()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void StopCycling()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void StartCycling()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void NormalCycle()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void EndOfLoop()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ReverseCycle()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ReverseLoop()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void CycleTime()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void StopMotion()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void StartMotion()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void StepSize()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void StepTime()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void MoveObj()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void MoveObjV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void FollowEgo()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Wander()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void NormalMotion()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetDir()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void GetDir()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void IgnoreBlocks()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ObserveBlocks()
    {
    }

    [TestMethod]
    public void Block()
    {
        this.interpreter.State.BlockIsSet = false;
        this.kernel.Block(1, 2, 20, 10);
        this.interpreter.State.BlockIsSet.Should().BeTrue();
        this.interpreter.State.BlockX1.Should().Be(1);
        this.interpreter.State.BlockY1.Should().Be(2);
        this.interpreter.State.BlockX2.Should().Be(20);
        this.interpreter.State.BlockY2.Should().Be(10);
    }

    [TestMethod]
    public void Unblock()
    {
        this.interpreter.State.BlockIsSet = true;
        this.kernel.Unblock();
        this.interpreter.State.BlockIsSet.Should().BeFalse();
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Get()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void GetV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Drop()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Put()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void PutV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void GetRoomV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void LoadSound()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Sound()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void StopSound()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Print()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void PrintV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Display()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void DisplayV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ClearLines()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void TextScreen()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Graphics()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetCursorChar()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetTextAttribute()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ShakeScreen()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ConfigureScreen()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void StatusLineOn()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void StatusLineOff()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetString()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void GetString()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void WordToString()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Parse()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void GetNumber()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void PreventInput()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void AcceptInput()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetKey()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void AddToPicture()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void AddToPictureV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Status()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SaveGame()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void RestoreGame()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void InitDisk()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void RestartGame()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ShowObj()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Random()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ProgramControl()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void PlayerControl()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ObjectStatusV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Quit()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ShowMemory()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Pause()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void EchoLine()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void CancelLine()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void InitJoy()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ToggleMonitor()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Version()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ScriptSize()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetGameId()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void Log()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetScanStart()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ResetScanStart()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void RepositionTo()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void RepositionToV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void TraceOn()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void TraceInfo()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void PrintAt()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void PrintAtV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void DiscardViewV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ClearTextRectangle()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetUpperLeft()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetMenu()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetMenuItem()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SubmitMenu()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void EnableItem()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void DisableItem()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void MenuInput()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ShowObjV()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void OpenDialogue()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void CloseDialogue()
    {
    }

    [TestMethod]
    public void MulN()
    {
        this.interpreter.State.Variables[23] = 5;
        this.kernel.MulN(23, 7);
        this.interpreter.State.Variables[23].Should().Be(35);
    }

    [TestMethod]
    public void MulV()
    {
        this.interpreter.State.Variables[23] = 5;
        this.interpreter.State.Variables[22] = 7;
        this.kernel.MulV(23, 22);
        this.interpreter.State.Variables[23].Should().Be(35);
        this.interpreter.State.Variables[22].Should().Be(7);
    }

    [TestMethod]
    public void DivN()
    {
        this.interpreter.State.Variables[23] = 24;
        this.kernel.DivN(23, 6);
        this.interpreter.State.Variables[23].Should().Be(4);
    }

    [TestMethod]
    public void DivV()
    {
        this.interpreter.State.Variables[23] = 24;
        this.interpreter.State.Variables[22] = 6;
        this.kernel.DivV(23, 22);
        this.interpreter.State.Variables[23].Should().Be(4);
        this.interpreter.State.Variables[22].Should().Be(6);
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void CloseWindow()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void SetSimple()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void PollMouse()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void PushScript()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void PopScript()
    {
    }

    [TestMethod]
    public void HoldKey()
    {
        this.interpreter.State.WalkMode = WalkMode.ReleaseKey;
        this.kernel.HoldKey();
        this.interpreter.State.WalkMode.Should().Be(WalkMode.HoldKey);
    }

    [TestMethod]
    public void SetPriBase()
    {
        this.interpreter.PriorityTable = new PriorityTable();
        this.kernel.SetPriBase(6);
        this.interpreter.PriorityTable.GetPriorityAt(0).Should().Be(4);
        this.interpreter.PriorityTable.GetPriorityAt(5).Should().Be(4);
        this.interpreter.PriorityTable.GetPriorityAt(6).Should().Be(5);
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void DiscardSound()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void HideMouse()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void AllowMenu()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void ShowMouse()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void FenceMouse()
    {
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void MousePosN()
    {
    }

    [TestMethod]
    public void ReleaseKey()
    {
        this.interpreter.State.WalkMode = WalkMode.HoldKey;
        this.kernel.ReleaseKey();
        this.interpreter.State.WalkMode.Should().Be(WalkMode.ReleaseKey);
    }

    [Ignore(TestNotImplemented)]
    [TestMethod]
    public void AdjEgoMoveToXY()
    {
    }

    private static ViewResource CreateTestViewResource(byte resourceIndex)
    {
        var cels = new ViewCel[]
        {
            new ViewCel(4, 8, 0, false, 0, new byte[] { 0 }),
        };

        var loops = new ViewLoop[]
        {
            new ViewLoop(cels, -1),
            new ViewLoop(cels, -1),
            new ViewLoop(cels, -1),
        };

        return new ViewResource(resourceIndex, loops, "key", 0, 0);
    }
}
