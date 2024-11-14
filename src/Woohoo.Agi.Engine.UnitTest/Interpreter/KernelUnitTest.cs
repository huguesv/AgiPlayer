// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Interpreter;

using Woohoo.Agi.Engine.Interpreter;
using Woohoo.Agi.Engine.Resources;
using Woohoo.Agi.Engine.UnitTest.Infrastructure;

public class KernelUnitTest
{
    private const string TestNotImplemented = "Test not implemented.";

    [Theory]
    [InlineData(23, 0, 0, true)]
    [InlineData(23, 0, 1, false)]
    [InlineData(23, 5, 5, true)]
    [InlineData(23, 5, 0, false)]
    [InlineData(23, 255, 255, true)]
    [InlineData(23, 255, 0, false)]
    public void EqualN(byte varA, byte valA, byte numB, bool expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;

        // Act
        var result = ((IKernel)interpreter).EqualN(varA, numB);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(23, 0, 22, 0, true)]
    [InlineData(23, 5, 22, 7, false)]
    public void EqualV(byte varA, byte valA, byte varB, byte valB, bool expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;
        interpreter.State.Variables[varB] = valB;

        // Act
        var result = ((IKernel)interpreter).EqualV(varA, varB);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(23, 0, 0, false)]
    [InlineData(23, 0, 1, true)]
    [InlineData(23, 1, 0, false)]
    [InlineData(23, 1, 255, true)]
    [InlineData(23, 255, 255, false)]
    public void LessN(byte varA, byte valA, byte numB, bool expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;

        // Act
        var result = ((IKernel)interpreter).LessN(varA, numB);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(23, 0, 22, 0, false)]
    [InlineData(23, 0, 22, 1, true)]
    [InlineData(23, 1, 22, 0, false)]
    [InlineData(23, 1, 22, 255, true)]
    [InlineData(23, 255, 22, 255, false)]
    public void LessV(byte varA, byte valA, byte varB, byte valB, bool expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;
        interpreter.State.Variables[varB] = valB;

        // Act
        var result = ((IKernel)interpreter).LessV(varA, varB);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(23, 0, 0, false)]
    [InlineData(23, 0, 1, false)]
    [InlineData(23, 1, 0, true)]
    [InlineData(23, 1, 255, false)]
    [InlineData(23, 255, 255, false)]
    public void GreaterN(byte varA, byte valA, byte numB, bool expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;

        // Act
        var result = ((IKernel)interpreter).GreaterN(varA, numB);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(23, 0, 22, 0, false)]
    [InlineData(23, 0, 22, 1, false)]
    [InlineData(23, 1, 22, 0, true)]
    [InlineData(23, 1, 22, 255, false)]
    [InlineData(23, 255, 22, 255, false)]
    public void GreaterV(byte varA, byte valA, byte varB, byte valB, bool expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;
        interpreter.State.Variables[varB] = valB;

        // Act
        var result = ((IKernel)interpreter).GreaterV(varA, varB);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(12, false, false)]
    [InlineData(12, true, true)]
    public void IsSet(byte flagNum, bool flagVal, bool expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Flags[flagNum] = flagVal;

        // Act
        var result = ((IKernel)interpreter).IsSet(flagNum);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(12, 5, false, false)]
    [InlineData(12, 5, true, true)]
    public void IsSetV(byte varA, byte valA, bool flagVal, bool expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;
        interpreter.State.Flags[valA] = flagVal;

        // Act
        var result = ((IKernel)interpreter).IsSetV(varA);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    public void Has(byte inv, bool expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder()
            .WithInventory(inv => inv
                .WithItem("key", 255)
                .WithItem("pen", 20))
            .Build();

        // Act
        var result = ((IKernel)interpreter).Has(inv);

        // Assert
        result.Should().Be(expected);
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

    [Theory]
    [InlineData("get key", new int[] { 1, 2 }, true)]
    [InlineData("get the key", new int[] { 1, 2 }, true)]
    [InlineData("get", new int[] { 1, 2 }, false)]
    [InlineData("get key box", new int[] { 1, 2 }, false)]
    public void Said(string text, int[] words, bool expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder()
            .WithVocabulary(vocab => vocab
                .WithFamily(0, "a", "the")
                .WithFamily(1, "get")
                .WithFamily(2, "key")
                .WithFamily(3, "box"))
            .Build();
        interpreter.State.Flags[Flags.SaidAccepted] = false;
        interpreter.ParseText(text);

        // Act
        var result = ((IKernel)interpreter).Said(words);

        // Assert
        result.Should().Be(expected);
        interpreter.State.Flags[Flags.SaidAccepted].Should().Be(expected);
    }

    [Fact(Skip = TestNotImplemented)]
    public void HaveKey()
    {
    }

    [Theory]
    [InlineData(0, "hello1", 1, "hello2", false)]
    [InlineData(0, "hello", 1, "hello", true)]
    [InlineData(0, "Hello", 1, "hello", true)]
    public void CompareStrings(byte stringANum, string stringA, byte stringBNum, string stringB, bool expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Strings[stringANum] = stringA;
        interpreter.State.Strings[stringBNum] = stringB;

        // Act
        var result = ((IKernel)interpreter).CompareStrings(stringANum, stringBNum);

        // Assert
        result.Should().Be(expected);
    }

    [Fact(Skip = TestNotImplemented)]
    public void ReturnFalse()
    {
    }

    [Theory]
    [InlineData(23, 0, 1)]
    [InlineData(23, 5, 6)]
    [InlineData(23, 255, 255)]
    public void Increment(byte varA, byte valA, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;

        // Act
        ((IKernel)interpreter).Increment(varA);

        // Assert
        interpreter.State.Variables[varA].Should().Be(expected);
    }

    [Theory]
    [InlineData(23, 0, 0)]
    [InlineData(23, 5, 4)]
    [InlineData(23, 255, 254)]
    public void Decrement(byte varA, byte valA, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;

        // Act
        ((IKernel)interpreter).Decrement(varA);

        // Assert
        interpreter.State.Variables[varA].Should().Be(expected);
    }

    [Theory]
    [InlineData(23, 5, 5)]
    [InlineData(24, 255, 255)]
    public void AssignN(byte varA, byte valA, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();

        // Act
        ((IKernel)interpreter).AssignN(varA, valA);

        // Assert
        interpreter.State.Variables[varA].Should().Be(expected);
    }

    [Theory]
    [InlineData(22, 23, 5, 5)]
    [InlineData(32, 33, 255, 255)]
    public void AssignV(byte varA, byte varB, byte valB, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varB] = valB;

        // Act
        ((IKernel)interpreter).AssignV(varA, varB);

        // Assert
        interpreter.State.Variables[varA].Should().Be(expected);
    }

    [Theory]
    [InlineData(23, 5, 7, 12)]
    [InlineData(24, 250, 7, 1)]
    public void AddN(byte varA, byte valA, byte numB, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;

        // Act
        ((IKernel)interpreter).AddN(varA, numB);

        // Assert
        interpreter.State.Variables[varA].Should().Be(expected);
    }

    [Theory]
    [InlineData(23, 5, 22, 7, 12)]
    [InlineData(23, 250, 22, 7, 1)]
    public void AddV(byte varA, byte valA, byte varB, byte valB, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;
        interpreter.State.Variables[varB] = valB;

        // Act
        ((IKernel)interpreter).AddV(varA, varB);

        // Assert
        interpreter.State.Variables[varA].Should().Be(expected);
        interpreter.State.Variables[varB].Should().Be(valB);
    }

    [Theory]
    [InlineData(23, 5, 3, 2)]
    [InlineData(23, 5, 7, 254)]
    public void SubN(byte varA, byte valA, byte numB, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;

        // Act
        ((IKernel)interpreter).SubN(varA, numB);

        // Assert
        interpreter.State.Variables[varA].Should().Be(expected);
    }

    [Theory]
    [InlineData(23, 5, 22, 3, 2)]
    public void SubV(byte varA, byte valA, byte varB, byte valB, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;
        interpreter.State.Variables[varB] = valB;

        // Act
        ((IKernel)interpreter).SubV(varA, varB);

        // Assert
        interpreter.State.Variables[varA].Should().Be(expected);
        interpreter.State.Variables[varB].Should().Be(valB);
    }

    [Theory]
    [InlineData(10, 5, 6, 15, 70, 70)]
    public void LIndirectV(byte varA, byte valA, byte valValA, byte varB, byte valB, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[valA] = valValA;
        interpreter.State.Variables[varA] = valA;
        interpreter.State.Variables[varB] = valB;

        // Act
        ((IKernel)interpreter).LIndirectV(varA, varB);

        // Assert
        interpreter.State.Variables[valA].Should().Be(expected);
    }

    [Theory]
    [InlineData(15, 70, 10, 5, 6, 6)]
    public void RIndirect(byte varA, byte valA, byte varB, byte valB, byte valValB, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[valB] = valValB;
        interpreter.State.Variables[varB] = valB;
        interpreter.State.Variables[varA] = valA;

        // Act
        ((IKernel)interpreter).RIndirect(varA, varB);

        // Assert
        interpreter.State.Variables[varA].Should().Be(expected);
    }

    [Theory]
    [InlineData(10, 5, 6, 80, 80)]
    public void LIndirectN(byte varA, byte valA, byte valValA, byte numB, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[valA] = valValA;
        interpreter.State.Variables[varA] = valA;

        // Act
        ((IKernel)interpreter).LIndirectN(varA, numB);

        // Assert
        interpreter.State.Variables[valA].Should().Be(expected);
    }

    [Theory]
    [InlineData(23, false)]
    [InlineData(23, true)]
    public void Set(byte varA, bool valA)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Flags[varA] = valA;

        // Act
        ((IKernel)interpreter).Set(varA);

        // Assert
        interpreter.State.Flags[varA].Should().BeTrue();
    }

    [Theory]
    [InlineData(23, false)]
    [InlineData(23, true)]
    public void Reset(byte varA, bool valA)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Flags[varA] = valA;

        // Act
        ((IKernel)interpreter).Reset(varA);

        // Assert
        interpreter.State.Flags[varA].Should().BeFalse();
    }

    [Theory]
    [InlineData(23, false, true)]
    [InlineData(23, true, false)]
    public void Toggle(byte varA, bool valA, bool expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Flags[varA] = valA;

        // Act
        ((IKernel)interpreter).Toggle(varA);

        // Assert
        interpreter.State.Flags[varA].Should().Be(expected);
    }

    [Theory]
    [InlineData(5, 23, false)]
    [InlineData(5, 23, true)]
    public void SetV(byte varA, byte valA, bool valValA)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;
        interpreter.State.Flags[valA] = valValA;

        // Act
        ((IKernel)interpreter).SetV(varA);

        // Assert
        interpreter.State.Flags[valA].Should().BeTrue();
    }

    [Theory]
    [InlineData(5, 23, false)]
    [InlineData(5, 23, true)]
    public void ResetV(byte varA, byte valA, bool valValA)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;
        interpreter.State.Flags[valA] = valValA;

        // Act
        ((IKernel)interpreter).ResetV(varA);

        // Assert
        interpreter.State.Flags[valA].Should().BeFalse();
    }

    [Theory]
    [InlineData(5, 23, false, true)]
    [InlineData(5, 23, true, false)]
    public void ToggleV(byte varA, byte valA, bool valValA, bool expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;
        interpreter.State.Flags[valA] = valValA;

        // Act
        ((IKernel)interpreter).ToggleV(varA);

        // Assert
        interpreter.State.Flags[valA].Should().Be(expected);
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
        // Arrange
        var viewResource = CreateTestViewResource(60);
        var interpreter = new InterpreterBuilder()
            .WithInventory(inv => inv.WithMaxAnimatedObjects(1))
            .WithView(viewResource)
            .Build();

        // Act
        ((IKernel)interpreter).SetView(0, 60);

        // Assert
        var view = interpreter.ObjectTable.GetAt(0);
        view.ViewResource.Should().BeSameAs(viewResource);
    }

    [Fact]
    public void SetViewV()
    {
        // Arrange
        var viewResource = CreateTestViewResource(60);
        var interpreter = new InterpreterBuilder()
            .WithInventory(inv => inv.WithMaxAnimatedObjects(1))
            .WithView(viewResource)
            .Build();
        interpreter.State.Variables[25] = 60;

        // Act
        ((IKernel)interpreter).SetViewV(0, 25);

        // Assert
        var view = interpreter.ObjectTable.GetAt(0);
        view.ViewResource.Should().BeSameAs(viewResource);
    }

    [Fact]
    public void SetLoop()
    {
        // Arrange
        var viewResource = CreateTestViewResource(60);
        var interpreter = new InterpreterBuilder()
            .WithInventory(inv => inv.WithMaxAnimatedObjects(1))
            .WithView(viewResource)
            .Build();
        var view = interpreter.ObjectTable.GetAt(0);
        view.LoopTotal = (byte)viewResource.Loops.Length;
        view.ViewResource = viewResource;

        // Act
        ((IKernel)interpreter).SetLoop(0, 2);

        // Assert
        view.LoopCur.Should().Be(2);
        view.CelTotal.Should().Be(1);
    }

    [Fact]
    public void SetLoopV()
    {
        // Arrange
        var viewResource = CreateTestViewResource(60);
        var interpreter = new InterpreterBuilder()
            .WithInventory(inv => inv.WithMaxAnimatedObjects(1))
            .WithView(viewResource)
            .Build();
        var view = interpreter.ObjectTable.GetAt(0);
        view.LoopTotal = (byte)viewResource.Loops.Length;
        view.ViewResource = viewResource;
        interpreter.State.Variables[25] = 2;

        // Act
        ((IKernel)interpreter).SetLoopV(0, 25);

        // Assert
        view.LoopCur.Should().Be(2);
        view.CelTotal.Should().Be(1);
    }

    [Fact]
    public void FixLoop()
    {
        // Arrange
        var interpreter = new InterpreterBuilder()
            .WithInventory(inv => inv.WithMaxAnimatedObjects(1))
            .Build();
        var view = interpreter.ObjectTable.GetAt(0);
        view.Flags = 0;

        // Act
        ((IKernel)interpreter).FixLoop(0);

        // Assert
        (view.Flags & ViewObjectFlags.LoopFixed).Should().NotBe(0);
    }

    [Fact]
    public void ReleaseLoop()
    {
        // Arrange
        var interpreter = new InterpreterBuilder()
            .WithInventory(inv => inv.WithMaxAnimatedObjects(1))
            .Build();
        var view = interpreter.ObjectTable.GetAt(0);
        view.Flags = ViewObjectFlags.LoopFixed;

        // Act
        ((IKernel)interpreter).ReleaseLoop(0);

        // Assert
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
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.BlockIsSet = false;

        // Act
        ((IKernel)interpreter).Block(1, 2, 20, 10);

        // Assert
        interpreter.State.BlockIsSet.Should().BeTrue();
        interpreter.State.BlockX1.Should().Be(1);
        interpreter.State.BlockY1.Should().Be(2);
        interpreter.State.BlockX2.Should().Be(20);
        interpreter.State.BlockY2.Should().Be(10);
    }

    [Fact]
    public void Unblock()
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.BlockIsSet = true;

        // Act
        ((IKernel)interpreter).Unblock();

        // Assert
        interpreter.State.BlockIsSet.Should().BeFalse();
    }

    [Fact]
    public void Get()
    {
        // Arrange
        var interpreter = new InterpreterBuilder()
            .WithInventory(inv => inv
                .WithItem("key", 50)
                .WithItem("flower", 60)
                .WithMaxAnimatedObjects(2))
            .Build();

        // Act
        ((IKernel)interpreter).Get(1);

        // Assert
        interpreter.ResourceManager.InventoryResource.Items[0].Location.Should().Be(50);
        interpreter.ResourceManager.InventoryResource.Items[1].Location.Should().Be(0xff);
    }

    [Fact]
    public void GetV()
    {
        // Arrange
        var interpreter = new InterpreterBuilder()
            .WithInventory(inv => inv
                .WithItem("key", 50)
                .WithItem("flower", 60)
                .WithMaxAnimatedObjects(2))
            .Build();
        interpreter.State.Variables[5] = 1;

        // Act
        ((IKernel)interpreter).GetV(5);

        // Assert
        interpreter.ResourceManager.InventoryResource.Items[0].Location.Should().Be(50);
        interpreter.ResourceManager.InventoryResource.Items[1].Location.Should().Be(0xff);
    }

    [Fact]
    public void Drop()
    {
        // Arrange
        var interpreter = new InterpreterBuilder()
            .WithInventory(inv => inv
                .WithItem("key", 50)
                .WithItem("flower", 60)
                .WithMaxAnimatedObjects(2))
            .Build();

        // Act
        ((IKernel)interpreter).Drop(1);

        // Assert
        interpreter.ResourceManager.InventoryResource.Items[0].Location.Should().Be(50);
        interpreter.ResourceManager.InventoryResource.Items[1].Location.Should().Be(0x00);
    }

    [Fact]
    public void Put()
    {
        // Arrange
        var interpreter = new InterpreterBuilder()
            .WithInventory(inv => inv
                .WithItem("key", 50)
                .WithItem("flower", 60)
                .WithMaxAnimatedObjects(2))
            .Build();

        // Act
        ((IKernel)interpreter).Put(1, 61);

        // Assert
        interpreter.ResourceManager.InventoryResource.Items[0].Location.Should().Be(50);
        interpreter.ResourceManager.InventoryResource.Items[1].Location.Should().Be(61);
    }

    [Fact]
    public void PutV()
    {
        // Arrange
        var interpreter = new InterpreterBuilder()
            .WithInventory(inv => inv
                .WithItem("key", 50)
                .WithItem("flower", 60)
                .WithMaxAnimatedObjects(2))
            .Build();
        interpreter.State.Variables[8] = 1;
        interpreter.State.Variables[9] = 61;

        // Act
        ((IKernel)interpreter).PutV(8, 9);

        // Assert
        interpreter.ResourceManager.InventoryResource.Items[0].Location.Should().Be(50);
        interpreter.ResourceManager.InventoryResource.Items[1].Location.Should().Be(61);
    }

    [Fact]
    public void GetRoomV()
    {
        // Arrange
        var interpreter = new InterpreterBuilder()
            .WithInventory(inv => inv
                .WithItem("key", 50)
                .WithItem("flower", 60)
                .WithMaxAnimatedObjects(2))
            .Build();
        interpreter.State.Variables[8] = 1;
        interpreter.State.Variables[9] = 61;

        // Act
        ((IKernel)interpreter).GetRoomV(8, 9);

        // Assert
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
        // Arrange
        var interpreter = new InterpreterBuilder()
            .WithVocabulary(vocab => vocab
                .WithFamily(40, "get")
                .WithFamily(50, "key")
                .WithFamily(60, "flower"))
            .Build();
        interpreter.State.Strings[0] = "get flower";

        // Act
        ((IKernel)interpreter).Parse(0);

        // Assert
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

    [Theory]
    [InlineData(23, 5, 7, 35)]
    public void MulN(byte varA, byte valA, byte numB, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;

        // Act
        ((IKernel)interpreter).MulN(varA, numB);

        // Assert
        interpreter.State.Variables[varA].Should().Be(expected);
    }

    [Theory]
    [InlineData(23, 5, 22, 7, 35)]
    public void MulV(byte varA, byte valA, byte varB, byte valB, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;
        interpreter.State.Variables[varB] = valB;

        // Act
        ((IKernel)interpreter).MulV(varA, varB);

        // Assert
        interpreter.State.Variables[varA].Should().Be(expected);
        interpreter.State.Variables[varB].Should().Be(valB);
    }

    [Theory]
    [InlineData(23, 24, 6, 4)]
    public void DivN(byte varA, byte valA, byte numB, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;

        // Act
        ((IKernel)interpreter).DivN(varA, numB);

        // Assert
        interpreter.State.Variables[varA].Should().Be(expected);
    }

    [Theory]
    [InlineData(23, 24, 22, 6, 4)]
    public void DivV(byte varA, byte valA, byte varB, byte valB, byte expected)
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.Variables[varA] = valA;
        interpreter.State.Variables[varB] = valB;

        // Act
        ((IKernel)interpreter).DivV(varA, varB);

        // Assert
        interpreter.State.Variables[varA].Should().Be(expected);
        interpreter.State.Variables[varB].Should().Be(valB);
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
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.WalkMode = WalkMode.ReleaseKey;

        // Act
        ((IKernel)interpreter).HoldKey();

        // Assert
        interpreter.State.WalkMode.Should().Be(WalkMode.HoldKey);
    }

    [Fact]
    public void SetPriBase()
    {
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.PriorityTable = new PriorityTable();

        // Act
        ((IKernel)interpreter).SetPriBase(6);

        // Assert
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
        // Arrange
        var interpreter = new InterpreterBuilder().Build();
        interpreter.State.WalkMode = WalkMode.HoldKey;

        // Act
        ((IKernel)interpreter).ReleaseKey();

        // Assert
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
