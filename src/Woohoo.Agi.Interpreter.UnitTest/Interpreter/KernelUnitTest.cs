// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.UnitTest
{
    using Woohoo.Agi.Resources;

    [TestClass]
    public class KernelUnitTest
    {
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

        [TestMethod]
        public void PosN()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CenterPosN()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void RightPosN()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ObjInBox()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Controller()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ObjInRoom()
        {
            Assert.Inconclusive();
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

        [TestMethod]
        public void HaveKey()
        {
            Assert.Inconclusive();
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

        [TestMethod]
        public void NewRoom()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void NewRoomV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void LoadLogics()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void LoadLogicsV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Call()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CallV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void LoadPicture()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DrawPicture()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ShowPicture()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DiscardPicture()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void OverlayPicture()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ShowPriScreen()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void LoadView()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void LoadViewV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DiscardView()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AnimateObj()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void UnanimateAll()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Draw()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Erase()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Position()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void PositionV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void GetPosition()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Reposition()
        {
            Assert.Inconclusive();
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

        [TestMethod]
        public void SetCel()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetCelV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void LastCel()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CurrentCel()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CurrentLoop()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CurrentView()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void NumberOfLoops()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetPriority()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetPriorityV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ReleasePriority()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void GetPriority()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void StopUpdate()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void StartUpdate()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ForceUpdate()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void IgnoreHorizon()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ObserveHorizon()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetHorizon()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ObjectOnWater()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ObjectOnLand()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ObjectOnAnything()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void IgnoreObjects()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ObserveObjects()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Distance()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void StopCycling()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void StartCycling()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void NormalCycle()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void EndOfLoop()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ReverseCycle()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ReverseLoop()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CycleTime()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void StopMotion()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void StartMotion()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void StepSize()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void StepTime()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void MoveObj()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void MoveObjV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void FollowEgo()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Wander()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void NormalMotion()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetDir()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void GetDir()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void IgnoreBlocks()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ObserveBlocks()
        {
            Assert.Inconclusive();
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

        [TestMethod]
        public void Get()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void GetV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Drop()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Put()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void PutV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void GetRoomV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void LoadSound()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Sound()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void StopSound()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Print()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void PrintV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Display()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DisplayV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ClearLines()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TextScreen()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Graphics()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetCursorChar()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetTextAttribute()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ShakeScreen()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ConfigureScreen()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void StatusLineOn()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void StatusLineOff()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetString()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void GetString()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void WordToString()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Parse()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void GetNumber()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void PreventInput()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AcceptInput()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetKey()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AddToPicture()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AddToPictureV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Status()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SaveGame()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void RestoreGame()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void InitDisk()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void RestartGame()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ShowObj()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Random()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ProgramControl()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void PlayerControl()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ObjectStatusV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Quit()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ShowMemory()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Pause()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void EchoLine()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CancelLine()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void InitJoy()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ToggleMonitor()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Version()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ScriptSize()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetGameId()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Log()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetScanStart()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ResetScanStart()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void RepositionTo()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void RepositionToV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TraceOn()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TraceInfo()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void PrintAt()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void PrintAtV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DiscardViewV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ClearTextRectangle()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetUpperLeft()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetMenu()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetMenuItem()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SubmitMenu()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void EnableItem()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DisableItem()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void MenuInput()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ShowObjV()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void OpenDialogue()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CloseDialogue()
        {
            Assert.Inconclusive();
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

        [TestMethod]
        public void CloseWindow()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SetSimple()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void PollMouse()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void PushScript()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void PopScript()
        {
            Assert.Inconclusive();
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

        [TestMethod]
        public void DiscardSound()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void HideMouse()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AllowMenu()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ShowMouse()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void FenceMouse()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void MousePosN()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ReleaseKey()
        {
            this.interpreter.State.WalkMode = WalkMode.HoldKey;
            this.kernel.ReleaseKey();
            this.interpreter.State.WalkMode.Should().Be(WalkMode.ReleaseKey);
        }

        [TestMethod]
        public void AdjEgoMoveToXY()
        {
            Assert.Inconclusive();
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
}
