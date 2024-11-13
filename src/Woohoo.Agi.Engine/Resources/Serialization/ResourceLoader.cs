// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Resources.Serialization;

using Woohoo.Agi.Engine;

/// <summary>
/// Loads game resources for an AGI game.
/// </summary>
public class ResourceLoader
{
    /// <summary>
    /// Vocabulary agi file name.
    /// </summary>
    private const string VocabularyFile = "words.tok";

    /// <summary>
    /// Inventory agi file name.
    /// </summary>
    private const string InventoryFile = "object";

    private readonly IGameContainer gameContainer;
    private readonly Platform platform;
    private readonly InterpreterVersion version;
    private readonly bool gameCompression;
    private readonly bool inventoryPadded;
    private readonly IVolumeDecoder volumeDecoder;
    private readonly VolumeResourceMap resourceMap;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceLoader"/> class.
    /// </summary>
    /// <param name="gameContainer">Game container.</param>
    /// <param name="gameId">Game id.</param>
    /// <param name="platform">Game platform.</param>
    /// <param name="version">Interpreter version.</param>
    public ResourceLoader(IGameContainer gameContainer, string gameId, Platform platform, InterpreterVersion version)
    {
        ArgumentNullException.ThrowIfNull(gameContainer);
        ArgumentNullException.ThrowIfNull(gameId);

        if (!IsGameFolder(gameContainer))
        {
            throw new GameNotFoundException();
        }

        this.gameContainer = gameContainer;
        this.platform = platform;
        this.version = version;
        this.inventoryPadded = platform == Platform.Amiga;
        if (this.IsVersion2())
        {
            this.volumeDecoder = new VolumeDecoderV2();
            this.gameCompression = false;
        }
        else if (this.IsVersion3())
        {
            this.volumeDecoder = new VolumeDecoderV3(gameId, platform);
            this.gameCompression = true;
        }
        else
        {
            throw new NotSupportedException();
        }

        this.resourceMap = this.volumeDecoder.LoadResourceMap(gameContainer);
    }

    /// <summary>
    /// Determine if a game is located in the specified folder.
    /// </summary>
    /// <param name="gameContainer">Game container.</param>
    /// <returns>True if a game is found in the folder, false otherwise.</returns>
    public static bool IsGameFolder(IGameContainer gameContainer)
    {
        ArgumentNullException.ThrowIfNull(gameContainer);

        return gameContainer.Exists(VocabularyFile) && gameContainer.Exists(InventoryFile);
    }

    /// <summary>
    /// Get the game id by looking for a volume 0 file in the specified game container.
    /// </summary>
    /// <param name="gameContainer">Game container.</param>
    /// <returns>Game id.</returns>
    public static string GetGameId(IGameContainer gameContainer)
    {
        ArgumentNullException.ThrowIfNull(gameContainer);

        return gameContainer.GetGameId();
    }

    /// <summary>
    /// Load the inventory resource from the game resources.
    /// </summary>
    /// <returns>Inventory resource.</returns>
    public InventoryResource LoadInventory()
    {
        byte[] data = this.gameContainer.Read(InventoryFile);

        return InventoryDecoder.DecryptAndReadInventory(data, this.inventoryPadded);
    }

    /// <summary>
    /// Load the vocabulary resource from the game resources.
    /// </summary>
    /// <returns>Vocabulary resource.</returns>
    public VocabularyResource LoadVocabulary()
    {
        byte[] data = this.gameContainer.Read(VocabularyFile);

        return VocabularyDecoder.ReadVocabulary(data);
    }

    /// <summary>
    /// Load the logic resource from the game resources.
    /// </summary>
    /// <param name="resourceIndex">Resource index.</param>
    /// <returns>Logic resource.</returns>
    public LogicResource LoadLogic(byte resourceIndex)
    {
        if (resourceIndex < 0 || resourceIndex > 0xff)
        {
            throw new ArgumentOutOfRangeException(nameof(resourceIndex));
        }

        var entry = this.resourceMap.LogicResources.GetEntry(resourceIndex);
        if (entry is null)
        {
            throw new ArgumentOutOfRangeException(nameof(resourceIndex));
        }

        var fileName = this.volumeDecoder.GetVolumeFile(entry.Volume);
        var data = this.volumeDecoder.ExtractResource(this.gameContainer, fileName, entry, out bool wasCompressed);

        return LogicDecoder.ReadLogic(resourceIndex, data, wasCompressed, this.gameCompression);
    }

    /// <summary>
    /// Load the view resource from the game resources.
    /// </summary>
    /// <param name="resourceIndex">Resource index.</param>
    /// <returns>View resource.</returns>
    public ViewResource LoadView(byte resourceIndex)
    {
        var entry = this.resourceMap.ViewResources.GetEntry(resourceIndex);
        if (entry is null)
        {
            throw new ArgumentOutOfRangeException(nameof(resourceIndex));
        }

        var fileName = this.volumeDecoder.GetVolumeFile(entry.Volume);
        var data = this.volumeDecoder.ExtractResource(this.gameContainer, fileName, entry, out _);

        return ViewDecoder.ReadView(resourceIndex, data);
    }

    /// <summary>
    /// Load the sound resource from the game resources.
    /// </summary>
    /// <param name="resourceIndex">Resource index.</param>
    /// <returns>Sound resource.</returns>
    public SoundResource LoadSound(byte resourceIndex)
    {
        var entry = this.resourceMap.SoundResources.GetEntry(resourceIndex);
        if (entry is null)
        {
            throw new ArgumentOutOfRangeException(nameof(resourceIndex));
        }

        var fileName = this.volumeDecoder.GetVolumeFile(entry.Volume);
        var data = this.volumeDecoder.ExtractResource(this.gameContainer, fileName, entry, out _);

        if (this.platform != Platform.AppleIIgs)
        {
            return SoundDecoder.ReadSound(resourceIndex, data);
        }
        else
        {
            return new SoundResource(resourceIndex, [], [], [], []);
        }
    }

    /// <summary>
    /// Load the picture resource from the game resources.
    /// </summary>
    /// <param name="resourceIndex">Resource index.</param>
    /// <returns>Picture resource.</returns>
    public PictureResource LoadPicture(byte resourceIndex)
    {
        var entry = this.resourceMap.PictureResources.GetEntry(resourceIndex);
        if (entry is null)
        {
            throw new ArgumentOutOfRangeException(nameof(resourceIndex));
        }

        var fileName = this.volumeDecoder.GetVolumeFile(entry.Volume);
        var data = this.volumeDecoder.ExtractResource(this.gameContainer, fileName, entry, out _);

        return PictureDecoder.ReadPicture(resourceIndex, data);
    }

    /// <summary>
    /// Determine if the resource manager game resources are for an AGI version 2 game.
    /// </summary>
    /// <returns>True for version 2, false otherwise.</returns>
    private bool IsVersion2()
    {
        return this.version >= InterpreterVersion.V2089 && this.version <= InterpreterVersion.V2936;
    }

    /// <summary>
    /// Determine if the resource manager game resources are for an AGI version 3 game.
    /// </summary>
    /// <returns>True for version 3, false otherwise.</returns>
    private bool IsVersion3()
    {
        return this.version >= InterpreterVersion.V3002086 && this.version <= InterpreterVersion.V3002149;
    }
}
