// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.UnitTest;

using Woohoo.Agi.Interpreter;
using Woohoo.Agi.Resources;

[TestClass]
public class ResourceManagerUnitTest
{
    [TestMethod]
    public void FindExistingLogic()
    {
        var mgr = new ResourceManager();
        mgr.LogicResources.Add(new LogicResource(44, [], []));

        var resource = mgr.FindLogic(44);
        resource.Should().NotBeNull();
    }

    [TestMethod]
    public void FindNonExistingLogic()
    {
        var mgr = new ResourceManager();
        mgr.LogicResources.Add(new LogicResource(44, [], []));

        var resource = mgr.FindLogic(45);
        resource.Should().BeNull();
    }

    [TestMethod]
    public void FindExistingPicture()
    {
        var mgr = new ResourceManager();
        mgr.PictureResources.Add(new PictureResource(54, []));

        var resource = mgr.FindPicture(54);
        resource.Should().NotBeNull();
    }

    [TestMethod]
    public void FindNonExistingPicture()
    {
        var mgr = new ResourceManager();
        mgr.PictureResources.Add(new PictureResource(54, []));

        var resource = mgr.FindPicture(55);
        resource.Should().BeNull();
    }

    [TestMethod]
    public void FindExistingSound()
    {
        var mgr = new ResourceManager();
        mgr.SoundResources.Add(new SoundResource(64, [], [], [], []));

        var resource = mgr.FindSound(64);
        resource.Should().NotBeNull();
    }

    [TestMethod]
    public void FindNonExistingSound()
    {
        var mgr = new ResourceManager();
        mgr.SoundResources.Add(new SoundResource(64, [], [], [], []));

        var resource = mgr.FindSound(65);
        resource.Should().BeNull();
    }

    [TestMethod]
    public void FindExistingView()
    {
        var mgr = new ResourceManager();
        mgr.ViewResources.Add(new ViewResource(74, [], string.Empty, 0, 0));

        var resource = mgr.FindView(74);
        resource.Should().NotBeNull();
    }

    [TestMethod]
    public void FindNonExistingView()
    {
        var mgr = new ResourceManager();
        mgr.ViewResources.Add(new ViewResource(74, [], string.Empty, 0, 0));

        var resource = mgr.FindView(75);
        resource.Should().BeNull();
    }
}
