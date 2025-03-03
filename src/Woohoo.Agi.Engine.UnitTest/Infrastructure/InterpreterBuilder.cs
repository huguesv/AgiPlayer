// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.UnitTest.Infrastructure;

using Woohoo.Agi.Engine.Interpreter;
using Woohoo.Agi.Engine.Resources;

internal class InterpreterBuilder
{
    private readonly List<LogicResource> logicResources = [];
    private readonly List<PictureResource> pictureResources = [];
    private readonly List<SoundResource> soundResources = [];
    private readonly List<ViewResource> viewResources = [];
    private InventoryResource? inventory;
    private VocabularyResource? vocabulary;
    private IInputDriver? inputDriver;
    private IGraphicsDriver? graphicsDriver;
    private ISoundDriver? soundDriver;

    public InterpreterBuilder WithInventory(InventoryResource inventory)
    {
        this.inventory = inventory;
        return this;
    }

    public InterpreterBuilder WithInventory(Action<InventoryBuilder> build)
    {
        var inventoryBuilder = new InventoryBuilder();
        build(inventoryBuilder);
        this.inventory = inventoryBuilder.Build();
        return this;
    }

    public InterpreterBuilder WithVocabulary(VocabularyResource vocabulary)
    {
        this.vocabulary = vocabulary;
        return this;
    }

    public InterpreterBuilder WithVocabulary(Action<VocabularyBuilder> build)
    {
        var vocabularyBuilder = new VocabularyBuilder();
        build(vocabularyBuilder);
        this.vocabulary = vocabularyBuilder.Build();
        return this;
    }

    public InterpreterBuilder WithLogic(LogicResource logic)
    {
        this.logicResources.Add(logic);
        return this;
    }

    public InterpreterBuilder WithLogic(Action<LogicResourceBuilder> build)
    {
        var logicBuilder = new LogicResourceBuilder();
        build(logicBuilder);
        this.logicResources.Add(logicBuilder.Build());
        return this;
    }

    public InterpreterBuilder WithPicture(PictureResource picture)
    {
        this.pictureResources.Add(picture);
        return this;
    }

    public InterpreterBuilder WithPicture(Action<PictureResourceBuilder> build)
    {
        var pictureBuilder = new PictureResourceBuilder();
        build(pictureBuilder);
        this.pictureResources.Add(pictureBuilder.Build());
        return this;
    }

    public InterpreterBuilder WithSound(SoundResource sound)
    {
        this.soundResources.Add(sound);
        return this;
    }

    public InterpreterBuilder WithSound(Action<SoundResourceBuilder> build)
    {
        var soundBuilder = new SoundResourceBuilder();
        build(soundBuilder);
        this.soundResources.Add(soundBuilder.Build());
        return this;
    }

    public InterpreterBuilder WithView(ViewResource view)
    {
        this.viewResources.Add(view);
        return this;
    }

    public InterpreterBuilder WithView(Action<ViewResourceBuilder> build)
    {
        var viewBuilder = new ViewResourceBuilder();
        build(viewBuilder);
        this.viewResources.Add(viewBuilder.Build());
        return this;
    }

    public InterpreterBuilder WithInputDriver(IInputDriver inputDriver)
    {
        this.inputDriver = inputDriver;
        return this;
    }

    public InterpreterBuilder WithInputDriver(Action<IInputDriver> build)
    {
        this.inputDriver = Substitute.For<IInputDriver>();
        build(this.inputDriver);
        return this;
    }

    public InterpreterBuilder WithGraphicsDriver(IGraphicsDriver graphicsDriver)
    {
        this.graphicsDriver = graphicsDriver;
        return this;
    }

    public InterpreterBuilder WithGraphicsDriver(Action<IGraphicsDriver> build)
    {
        this.graphicsDriver = Substitute.For<IGraphicsDriver>();
        build(this.graphicsDriver);
        return this;
    }

    public InterpreterBuilder WithSoundDriver(ISoundDriver soundDriver)
    {
        this.soundDriver = soundDriver;
        return this;
    }

    public InterpreterBuilder WithSoundDriver(Action<ISoundDriver> build)
    {
        this.soundDriver = Substitute.For<ISoundDriver>();
        build(this.soundDriver);
        return this;
    }

    public AgiInterpreter Build()
    {
        var result = new AgiInterpreter(this.inputDriver, this.graphicsDriver, this.soundDriver);
        result.CreateState();

        if (this.inventory is not null || this.vocabulary is not null)
        {
            result.ResourceManager = new ResourceManager()
            {
                InventoryResource = this.inventory,
                VocabularyResource = this.vocabulary,
            };

            foreach (var logic in this.logicResources)
            {
                result.ResourceManager.LogicResources.Add(logic);
            }

            foreach (var picture in this.pictureResources)
            {
                result.ResourceManager.PictureResources.Add(picture);
            }

            foreach (var sound in this.soundResources)
            {
                result.ResourceManager.SoundResources.Add(sound);
            }

            foreach (var view in this.viewResources)
            {
                result.ResourceManager.ViewResources.Add(view);
            }

            int maxAnimatedObjects = result.ResourceManager.InventoryResource?.MaxAnimatedObjects ?? 0;
            result.ObjectTable = new ViewObjectTable(maxAnimatedObjects + 2);
            result.ObjectManager = new ViewObjectManager(result, null);
        }

        return result;
    }
}
