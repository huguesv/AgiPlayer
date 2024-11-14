// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Interpreter;

using Woohoo.Agi.Engine.Interpreter;
using Woohoo.Agi.Engine.Resources;

public class ResourceManagerUnitTest
{
    [Fact]
    public void FindExistingLogic()
    {
        // Arrange
        var mgr = new ResourceManager();
        mgr.LogicResources.Add(new LogicResource(44, [], []));

        // Act
        var resource = mgr.FindLogic(44);

        // Assert
        resource.Should().NotBeNull();
    }

    [Fact]
    public void FindNonExistingLogic()
    {
        // Arrange
        var mgr = new ResourceManager();
        mgr.LogicResources.Add(new LogicResource(44, [], []));

        // Act
        var resource = mgr.FindLogic(45);

        // Assert
        resource.Should().BeNull();
    }

    [Fact]
    public void FindExistingPicture()
    {
        // Arrange
        var mgr = new ResourceManager();
        mgr.PictureResources.Add(new PictureResource(54, []));

        // Act
        var resource = mgr.FindPicture(54);

        // Assert
        resource.Should().NotBeNull();
    }

    [Fact]
    public void FindNonExistingPicture()
    {
        // Arrange
        var mgr = new ResourceManager();
        mgr.PictureResources.Add(new PictureResource(54, []));

        // Act
        var resource = mgr.FindPicture(55);

        // Assert
        resource.Should().BeNull();
    }

    [Fact]
    public void FindExistingSound()
    {
        // Arrange
        var mgr = new ResourceManager();
        mgr.SoundResources.Add(new SoundResource(64, [], [], [], []));

        // Act
        var resource = mgr.FindSound(64);

        // Assert
        resource.Should().NotBeNull();
    }

    [Fact]
    public void FindNonExistingSound()
    {
        // Arrange
        var mgr = new ResourceManager();
        mgr.SoundResources.Add(new SoundResource(64, [], [], [], []));

        // Act
        var resource = mgr.FindSound(65);

        // Assert
        resource.Should().BeNull();
    }

    [Fact]
    public void FindExistingView()
    {
        // Arrange
        var mgr = new ResourceManager();
        mgr.ViewResources.Add(new ViewResource(74, [], string.Empty, 0, 0));

        // Act
        var resource = mgr.FindView(74);

        // Assert
        resource.Should().NotBeNull();
    }

    [Fact]
    public void FindNonExistingView()
    {
        // Arrange
        var mgr = new ResourceManager();
        mgr.ViewResources.Add(new ViewResource(74, [], string.Empty, 0, 0));

        // Act
        var resource = mgr.FindView(75);

        // Assert
        resource.Should().BeNull();
    }
}
