// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Interpreter;

using Woohoo.Agi.Engine.Interpreter;
using Woohoo.Agi.Engine.Resources;
using Woohoo.Agi.Engine.UnitTest.Infrastructure;

public class KernelUnitTest
{
    private const string TestNotImplemented = "Test not implemented.";

    [Fact]
    public void EqualN()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 0;
        kernel.EqualN(23, 0).Should().BeTrue();
        kernel.EqualN(23, 5).Should().BeFalse();
        interpreter.State.Variables[23] = 5;
        kernel.EqualN(23, 5).Should().BeTrue();
    }

    [Fact]
    public void EqualV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[22] = 0;
        interpreter.State.Variables[23] = 0;
        kernel.EqualV(23, 22).Should().BeTrue();
        interpreter.State.Variables[23] = 5;
        kernel.EqualV(23, 22).Should().BeFalse();
        interpreter.State.Variables[22] = 5;
        kernel.EqualV(23, 22).Should().BeTrue();
    }

    [Fact]
    public void LessN()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 0;
        kernel.LessN(23, 5).Should().BeTrue();
        kernel.LessN(23, 0).Should().BeFalse();
        interpreter.State.Variables[23] = 5;
        kernel.LessN(23, 4).Should().BeFalse();
        kernel.LessN(23, 6).Should().BeTrue();
    }

    [Fact]
    public void LessV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[22] = 0;
        interpreter.State.Variables[23] = 0;
        kernel.LessV(23, 22).Should().BeFalse();
        interpreter.State.Variables[23] = 5;
        kernel.LessV(23, 22).Should().BeFalse();
        interpreter.State.Variables[22] = 10;
        kernel.LessV(23, 22).Should().BeTrue();
    }

    [Fact]
    public void GreaterN()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 0;
        kernel.GreaterN(23, 5).Should().BeFalse();
        kernel.GreaterN(23, 0).Should().BeFalse();
        interpreter.State.Variables[23] = 5;
        kernel.GreaterN(23, 4).Should().BeTrue();
        kernel.GreaterN(23, 6).Should().BeFalse();
    }

    [Fact]
    public void GreaterV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[22] = 0;
        interpreter.State.Variables[23] = 0;
        kernel.GreaterV(23, 22).Should().BeFalse();
        interpreter.State.Variables[23] = 5;
        kernel.GreaterV(23, 22).Should().BeTrue();
        interpreter.State.Variables[22] = 10;
        kernel.GreaterV(23, 22).Should().BeFalse();
    }

    [Fact]
    public void IsSet()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Flags[23] = false;
        kernel.IsSet(23).Should().BeFalse();
        interpreter.State.Flags[23] = true;
        kernel.IsSet(23).Should().BeTrue();
    }

    [Fact]
    public void IsSetV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 5;
        interpreter.State.Flags[5] = false;
        kernel.IsSetV(23).Should().BeFalse();
        interpreter.State.Flags[5] = true;
        kernel.IsSetV(23).Should().BeTrue();
    }

    [Fact]
    public void Has()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        var items = new InventoryItem[]
        {
            new("key", 255),
            new("pen", 20),
        };
        var resource = new InventoryResource(items, 1);
        interpreter.ResourceManager = new ResourceManager
        {
            InventoryResource = resource,
        };

        kernel.Has(0).Should().BeTrue();
        kernel.Has(1).Should().BeFalse();
    }

    [Fact(Skip = TestNotImplemented)]
    public void PosN()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void CenterPosN()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void RightPosN()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ObjInBox()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Controller()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ObjInRoom()
    {
    }

    [Fact]
    public void Said()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        var families = new VocabularyWordFamily[]
        {
            new(0, "a", "the"),
            new(1, "get"),
            new(2, "key"),
            new(3, "box"),
        };
        var vocabulary = new VocabularyResource(families);
        interpreter.ResourceManager = new ResourceManager
        {
            VocabularyResource = vocabulary,
        };

        interpreter.State.Flags[Flags.SaidAccepted] = false;
        interpreter.ParseText("get key");
        kernel.Said([1, 2]);
        interpreter.State.Flags[Flags.SaidAccepted].Should().BeTrue();

        interpreter.State.Flags[Flags.SaidAccepted] = false;
        interpreter.ParseText("get the key");
        kernel.Said([1, 2]);
        interpreter.State.Flags[Flags.SaidAccepted].Should().BeTrue();

        interpreter.State.Flags[Flags.SaidAccepted] = false;
        interpreter.ParseText("get");
        kernel.Said([1, 2]);
        interpreter.State.Flags[Flags.SaidAccepted].Should().BeFalse();

        interpreter.State.Flags[Flags.SaidAccepted] = false;
        interpreter.ParseText("get key box");
        kernel.Said([1, 2]);
        interpreter.State.Flags[Flags.SaidAccepted].Should().BeFalse();
    }

    [Fact(Skip = TestNotImplemented)]
    public void HaveKey()
    {
    }

    [Fact]
    public void CompareStrings()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Strings[0] = "hello1";
        interpreter.State.Strings[1] = "hello2";
        kernel.CompareStrings(0, 1).Should().BeFalse();

        interpreter.State.Strings[0] = "hello";
        interpreter.State.Strings[1] = "hello";
        kernel.CompareStrings(0, 1).Should().BeTrue();

        interpreter.State.Strings[0] = "Hello";
        interpreter.State.Strings[1] = "hello";
        kernel.CompareStrings(0, 1).Should().BeTrue();
    }

    [Fact]
    public void ReturnFalse()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        kernel.ReturnFalse();
    }

    [Fact]
    public void Increment()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 5;
        kernel.Increment(23);
        interpreter.State.Variables[23].Should().Be(6);

        interpreter.State.Variables[23] = 255;
        kernel.Increment(23);
        interpreter.State.Variables[23].Should().Be(255);
    }

    [Fact]
    public void Decrement()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 5;
        kernel.Decrement(23);
        interpreter.State.Variables[23].Should().Be(4);

        interpreter.State.Variables[23] = 0;
        kernel.Decrement(23);
        interpreter.State.Variables[23].Should().Be(0);
    }

    [Fact]
    public void AssignN()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        kernel.AssignN(23, 5);
        interpreter.State.Variables[23].Should().Be(5);
    }

    [Fact]
    public void AssignV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 5;
        kernel.AssignV(22, 23);
        interpreter.State.Variables[22].Should().Be(5);
    }

    [Fact]
    public void AddN()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 5;
        kernel.AddN(23, 7);
        interpreter.State.Variables[23].Should().Be(12);
    }

    [Fact]
    public void AddV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 5;
        interpreter.State.Variables[22] = 7;
        kernel.AddV(23, 22);
        interpreter.State.Variables[23].Should().Be(12);
        interpreter.State.Variables[22].Should().Be(7);
    }

    [Fact]
    public void SubN()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 5;
        kernel.SubN(23, 3);
        interpreter.State.Variables[23].Should().Be(2);
    }

    [Fact]
    public void SubV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 5;
        interpreter.State.Variables[22] = 3;
        kernel.SubV(23, 22);
        interpreter.State.Variables[23].Should().Be(2);
        interpreter.State.Variables[22].Should().Be(3);
    }

    [Fact]
    public void LIndirectV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[5] = 6;
        interpreter.State.Variables[10] = 5;
        interpreter.State.Variables[15] = 70;
        kernel.LIndirectV(10, 15);
        interpreter.State.Variables[5].Should().Be(70);
    }

    [Fact]
    public void RIndirect()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[5] = 6;
        interpreter.State.Variables[10] = 5;
        interpreter.State.Variables[15] = 70;
        kernel.RIndirect(15, 10);
        interpreter.State.Variables[15].Should().Be(6);
    }

    [Fact]
    public void LIndirectN()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[5] = 6;
        interpreter.State.Variables[10] = 5;
        kernel.LIndirectN(10, 80);
        interpreter.State.Variables[5].Should().Be(80);
    }

    [Fact]
    public void Set()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Flags[23] = false;
        kernel.Set(23);
        interpreter.State.Flags[23].Should().BeTrue();
    }

    [Fact]
    public void Reset()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Flags[23] = true;
        kernel.Reset(23);
        interpreter.State.Flags[23].Should().BeFalse();
    }

    [Fact]
    public void Toggle()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Flags[23] = true;
        kernel.Toggle(23);
        interpreter.State.Flags[23].Should().BeFalse();
        kernel.Toggle(23);
        interpreter.State.Flags[23].Should().BeTrue();
    }

    [Fact]
    public void SetV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[5] = 23;
        interpreter.State.Flags[23] = false;
        kernel.SetV(5);
        interpreter.State.Flags[23].Should().BeTrue();
    }

    [Fact]
    public void ResetV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[5] = 23;
        interpreter.State.Flags[23] = true;
        kernel.ResetV(5);
        interpreter.State.Flags[23].Should().BeFalse();
    }

    [Fact]
    public void ToggleV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[5] = 23;
        interpreter.State.Flags[23] = true;
        kernel.ToggleV(5);
        interpreter.State.Flags[23].Should().BeFalse();
        kernel.ToggleV(5);
        interpreter.State.Flags[23].Should().BeTrue();
    }

    [Fact(Skip = TestNotImplemented)]
    public void NewRoom()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void NewRoomV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void LoadLogics()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void LoadLogicsV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Call()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void CallV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void LoadPicture()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void DrawPicture()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ShowPicture()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void DiscardPicture()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void OverlayPicture()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ShowPriScreen()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void LoadView()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void LoadViewV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void DiscardView()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void AnimateObj()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void UnanimateAll()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Draw()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Erase()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Position()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void PositionV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void GetPosition()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Reposition()
    {
    }

    [Fact]
    public void SetView()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        var resource = CreateTestViewResource(60);
        interpreter.ResourceManager = new ResourceManager();
        interpreter.ResourceManager.ViewResources.Add(resource);
        interpreter.ObjectTable = new ViewObjectTable(1);
        interpreter.ObjectManager = new ViewObjectManager(interpreter, null);

        kernel.SetView(0, 60);

        var view = interpreter.ObjectTable.GetAt(0);
        view.ViewResource.Should().BeSameAs(resource);
    }

    [Fact]
    public void SetViewV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[25] = 60;
        var resource = CreateTestViewResource(60);
        interpreter.ResourceManager = new ResourceManager();
        interpreter.ResourceManager.ViewResources.Add(resource);
        interpreter.ObjectTable = new ViewObjectTable(1);
        interpreter.ObjectManager = new ViewObjectManager(interpreter, null);

        kernel.SetViewV(0, 25);

        var view = interpreter.ObjectTable.GetAt(0);
        view.ViewResource.Should().BeSameAs(resource);
    }

    [Fact]
    public void SetLoop()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.ObjectTable = new ViewObjectTable(1);
        interpreter.ObjectManager = new ViewObjectManager(interpreter, null);
        var view = interpreter.ObjectTable.GetAt(0);
        var resource = CreateTestViewResource(60);
        view.LoopTotal = (byte)resource.Loops.Length;
        view.ViewResource = resource;

        kernel.SetLoop(0, 2);

        view.LoopCur.Should().Be(2);
        view.CelTotal.Should().Be(1);
    }

    [Fact]
    public void SetLoopV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[25] = 2;
        interpreter.ObjectTable = new ViewObjectTable(1);
        interpreter.ObjectManager = new ViewObjectManager(interpreter, null);
        var view = interpreter.ObjectTable.GetAt(0);
        var resource = CreateTestViewResource(60);
        view.LoopTotal = (byte)resource.Loops.Length;
        view.ViewResource = resource;

        kernel.SetLoopV(0, 25);

        view.LoopCur.Should().Be(2);
        view.CelTotal.Should().Be(1);
    }

    [Fact]
    public void FixLoop()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.ObjectTable = new ViewObjectTable(1);
        var view = interpreter.ObjectTable.GetAt(0);
        view.Flags = 0;
        kernel.FixLoop(0);
        (view.Flags & ViewObjectFlags.LoopFixed).Should().NotBe(0);
    }

    [Fact]
    public void ReleaseLoop()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.ObjectTable = new ViewObjectTable(1);
        var view = interpreter.ObjectTable.GetAt(0);
        view.Flags = ViewObjectFlags.LoopFixed;
        kernel.ReleaseLoop(0);
        (view.Flags & ViewObjectFlags.LoopFixed).Should().Be(0);
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetCel()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetCelV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void LastCel()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void CurrentCel()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void CurrentLoop()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void CurrentView()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void NumberOfLoops()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetPriority()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetPriorityV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ReleasePriority()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void GetPriority()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void StopUpdate()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void StartUpdate()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ForceUpdate()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void IgnoreHorizon()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ObserveHorizon()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetHorizon()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ObjectOnWater()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ObjectOnLand()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ObjectOnAnything()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void IgnoreObjects()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ObserveObjects()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Distance()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void StopCycling()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void StartCycling()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void NormalCycle()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void EndOfLoop()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ReverseCycle()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ReverseLoop()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void CycleTime()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void StopMotion()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void StartMotion()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void StepSize()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void StepTime()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void MoveObj()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void MoveObjV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void FollowEgo()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Wander()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void NormalMotion()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetDir()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void GetDir()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void IgnoreBlocks()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ObserveBlocks()
    {
    }

    [Fact]
    public void Block()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.BlockIsSet = false;
        kernel.Block(1, 2, 20, 10);
        interpreter.State.BlockIsSet.Should().BeTrue();
        interpreter.State.BlockX1.Should().Be(1);
        interpreter.State.BlockY1.Should().Be(2);
        interpreter.State.BlockX2.Should().Be(20);
        interpreter.State.BlockY2.Should().Be(10);
    }

    [Fact]
    public void Unblock()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.BlockIsSet = true;
        kernel.Unblock();
        interpreter.State.BlockIsSet.Should().BeFalse();
    }

    [Fact]
    public void Get()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        var resource = new InventoryResource([new("key", 50), new("flower", 60)], 2);
        interpreter.ResourceManager = new ResourceManager();
        interpreter.ResourceManager.InventoryResource = resource;
        interpreter.ObjectTable = new ViewObjectTable(1);
        interpreter.ObjectManager = new ViewObjectManager(interpreter, null);

        kernel.Get(1);

        resource.Items[0].Location.Should().Be(50);
        resource.Items[1].Location.Should().Be(0xff);
    }

    [Fact]
    public void GetV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        var resource = new InventoryResource([new("key", 50), new("flower", 60)], 2);
        interpreter.ResourceManager = new ResourceManager();
        interpreter.ResourceManager.InventoryResource = resource;
        interpreter.ObjectTable = new ViewObjectTable(1);
        interpreter.ObjectManager = new ViewObjectManager(interpreter, null);
        interpreter.State.Variables[5] = 1;

        kernel.GetV(5);

        resource.Items[0].Location.Should().Be(50);
        resource.Items[1].Location.Should().Be(0xff);
    }

    [Fact]
    public void Drop()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        var resource = new InventoryResource([new("key", 50), new("flower", 60)], 2);
        interpreter.ResourceManager = new ResourceManager();
        interpreter.ResourceManager.InventoryResource = resource;
        interpreter.ObjectTable = new ViewObjectTable(1);
        interpreter.ObjectManager = new ViewObjectManager(interpreter, null);

        kernel.Drop(1);

        resource.Items[0].Location.Should().Be(50);
        resource.Items[1].Location.Should().Be(0x00);
    }

    [Fact]
    public void Put()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        var resource = new InventoryResource([new("key", 50), new("flower", 60)], 2);
        interpreter.ResourceManager = new ResourceManager();
        interpreter.ResourceManager.InventoryResource = resource;
        interpreter.ObjectTable = new ViewObjectTable(1);
        interpreter.ObjectManager = new ViewObjectManager(interpreter, null);

        kernel.Put(1, 61);

        resource.Items[0].Location.Should().Be(50);
        resource.Items[1].Location.Should().Be(61);
    }

    [Fact]
    public void PutV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        var resource = new InventoryResource([new("key", 50), new("flower", 60)], 2);
        interpreter.ResourceManager = new ResourceManager();
        interpreter.ResourceManager.InventoryResource = resource;
        interpreter.ObjectTable = new ViewObjectTable(1);
        interpreter.ObjectManager = new ViewObjectManager(interpreter, null);
        interpreter.State.Variables[8] = 1;
        interpreter.State.Variables[9] = 61;

        kernel.PutV(8, 9);

        resource.Items[0].Location.Should().Be(50);
        resource.Items[1].Location.Should().Be(61);
    }

    [Fact]
    public void GetRoomV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        var resource = new InventoryResource([new("key", 50), new("flower", 60)], 2);
        interpreter.ResourceManager = new ResourceManager();
        interpreter.ResourceManager.InventoryResource = resource;
        interpreter.ObjectTable = new ViewObjectTable(1);
        interpreter.ObjectManager = new ViewObjectManager(interpreter, null);
        interpreter.State.Variables[8] = 1;
        interpreter.State.Variables[9] = 61;

        kernel.GetRoomV(8, 9);

        interpreter.State.Variables[9].Should().Be(60);
    }

    [Fact(Skip = TestNotImplemented)]
    public void LoadSound()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Sound()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void StopSound()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Print()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void PrintV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Display()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void DisplayV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ClearLines()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void TextScreen()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Graphics()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetCursorChar()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetTextAttribute()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ShakeScreen()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ConfigureScreen()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void StatusLineOn()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void StatusLineOff()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetString()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void GetString()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void WordToString()
    {
    }

    [Fact]
    public void Parse()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        var vocabulary = new VocabularyResource([new(40, "get"), new(50, "key"), new(60, "flower")]);
        interpreter.ResourceManager = new ResourceManager();
        interpreter.ResourceManager.VocabularyResource = vocabulary;
        interpreter.ObjectTable = new ViewObjectTable(1);
        interpreter.ObjectManager = new ViewObjectManager(interpreter, null);
        interpreter.State.Strings[0] = "get flower";

        kernel.Parse(0);

        interpreter.State.Flags[Flags.PlayerCommandLine].Should().Be(true);
        interpreter.State.Flags[Flags.SaidAccepted].Should().Be(false);
        interpreter.ParserResults.Should().HaveCount(2);
        interpreter.ParserResults[0].Should().BeEquivalentTo(new ParserResult("get", 40));
        interpreter.ParserResults[1].Should().BeEquivalentTo(new ParserResult("flower", 60));
    }

    [Fact(Skip = TestNotImplemented)]
    public void GetNumber()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void PreventInput()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void AcceptInput()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetKey()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void AddToPicture()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void AddToPictureV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Status()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SaveGame()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void RestoreGame()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void InitDisk()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void RestartGame()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ShowObj()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Random()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ProgramControl()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void PlayerControl()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ObjectStatusV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Quit()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ShowMemory()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Pause()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void EchoLine()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void CancelLine()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void InitJoy()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ToggleMonitor()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Version()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ScriptSize()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetGameId()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void Log()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetScanStart()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ResetScanStart()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void RepositionTo()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void RepositionToV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void TraceOn()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void TraceInfo()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void PrintAt()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void PrintAtV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void DiscardViewV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ClearTextRectangle()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetUpperLeft()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetMenu()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetMenuItem()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SubmitMenu()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void EnableItem()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void DisableItem()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void MenuInput()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ShowObjV()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void OpenDialogue()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void CloseDialogue()
    {
    }

    [Fact]
    public void MulN()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 5;
        kernel.MulN(23, 7);
        interpreter.State.Variables[23].Should().Be(35);
    }

    [Fact]
    public void MulV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 5;
        interpreter.State.Variables[22] = 7;
        kernel.MulV(23, 22);
        interpreter.State.Variables[23].Should().Be(35);
        interpreter.State.Variables[22].Should().Be(7);
    }

    [Fact]
    public void DivN()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 24;
        kernel.DivN(23, 6);
        interpreter.State.Variables[23].Should().Be(4);
    }

    [Fact]
    public void DivV()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.Variables[23] = 24;
        interpreter.State.Variables[22] = 6;
        kernel.DivV(23, 22);
        interpreter.State.Variables[23].Should().Be(4);
        interpreter.State.Variables[22].Should().Be(6);
    }

    [Fact(Skip = TestNotImplemented)]
    public void CloseWindow()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void SetSimple()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void PollMouse()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void PushScript()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void PopScript()
    {
    }

    [Fact]
    public void HoldKey()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.WalkMode = WalkMode.ReleaseKey;
        kernel.HoldKey();
        interpreter.State.WalkMode.Should().Be(WalkMode.HoldKey);
    }

    [Fact]
    public void SetPriBase()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.PriorityTable = new PriorityTable();
        kernel.SetPriBase(6);
        interpreter.PriorityTable.GetPriorityAt(0).Should().Be(4);
        interpreter.PriorityTable.GetPriorityAt(5).Should().Be(4);
        interpreter.PriorityTable.GetPriorityAt(6).Should().Be(5);
    }

    [Fact(Skip = TestNotImplemented)]
    public void DiscardSound()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void HideMouse()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void AllowMenu()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void ShowMouse()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void FenceMouse()
    {
    }

    [Fact(Skip = TestNotImplemented)]
    public void MousePosN()
    {
    }

    [Fact]
    public void ReleaseKey()
    {
        var interpreter = new InterpreterBuilder().Build();
        var kernel = (IKernel)interpreter;

        interpreter.State.WalkMode = WalkMode.HoldKey;
        kernel.ReleaseKey();
        interpreter.State.WalkMode.Should().Be(WalkMode.ReleaseKey);
    }

    [Fact(Skip = TestNotImplemented)]
    public void AdjEgoMoveToXY()
    {
    }

    private static ViewResource CreateTestViewResource(byte resourceIndex)
    {
        return new ViewResourceBuilder()
            .WithIndex(resourceIndex)
            .WithDescription("key")
            .WithLoop(l => l.WithCel(c => c.WithSize(4, 8).WithRandomPixels()))
            .WithLoop(l => l.WithCel(c => c.WithSize(4, 8).WithRandomPixels()))
            .WithLoop(l => l.WithCel(c => c.WithSize(4, 8).WithRandomPixels()))
            .Build();
    }
}
