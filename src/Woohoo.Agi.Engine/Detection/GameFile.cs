// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Detection;

/// <summary>
/// CRC file information.
/// </summary>
internal class GameFile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameFile"/> class.
    /// </summary>
    internal GameFile()
    {
        this.Name = string.Empty;
        this.Sha1 = string.Empty;
    }

    /// <summary>
    /// Gets or sets file name.
    /// </summary>
    internal string Name { get; set; }

    /// <summary>
    /// Gets or sets SHA1 checksum.
    /// </summary>
    internal string Sha1 { get; set; }

    /// <summary>
    /// Create instance of a <see cref="GameFile"/> from a specified
    /// container and file.
    /// </summary>
    /// <param name="container">Game container.</param>
    /// <param name="fileName">Game file.</param>
    /// <returns>Instance initialized with filename and checksum.</returns>
    internal static GameFile FromFile(IGameContainer container, string fileName)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(fileName);

        var data = container.Read(fileName);

        return new GameFile
        {
            Name = fileName,
            Sha1 = CalculateSha1(data),
        };
    }

    private static string CalculateSha1(byte[] data)
    {
        using (var algo = SHA1.Create())
        {
            algo.TransformBlock(data, 0, data.Length, null, 0);
            algo.TransformFinalBlock([], 0, 0);

            var checksum = new StringBuilder();
            foreach (byte b in algo.Hash ?? Enumerable.Empty<byte>())
            {
                checksum.Append(b.ToString("x2", CultureInfo.CurrentCulture));
            }

            return checksum.ToString();
        }
    }
}
