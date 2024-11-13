// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Resources;

/// <summary>
/// Represents an animation cel.
/// </summary>
public class ViewCel
{
    private readonly byte[] pixels;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewCel"/> class.
    /// </summary>
    /// <param name="width">Cel width in pixels.</param>
    /// <param name="height">Cel height in pixels.</param>
    /// <param name="transparentColor">Color index used for transparency (0-15).</param>
    /// <param name="mirror">Mirror.</param>
    /// <param name="mirrorLoopNumber">Mirror loop number.</param>
    /// <param name="pixels">Pixels.</param>
    /// <exception cref="ArgumentOutOfRangeException">The transparent color index is out of range.</exception>
    public ViewCel(byte width, byte height, byte transparentColor, bool mirror, byte mirrorLoopNumber, byte[] pixels)
    {
        ArgumentOutOfRangeException.ThrowIfZero(width);
        ArgumentOutOfRangeException.ThrowIfZero(height);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(transparentColor, 0x0f);
        ArgumentNullException.ThrowIfNull(pixels);

        this.Width = width;
        this.Height = height;
        this.TransparentColor = transparentColor;
        this.Mirror = mirror;
        this.MirrorLoopNumber = mirrorLoopNumber;
        this.pixels = (byte[])pixels.Clone();
    }

    /// <summary>
    /// Gets width in pixels.
    /// </summary>
    public byte Width { get; }

    /// <summary>
    /// Gets height in pixels.
    /// </summary>
    public byte Height { get; }

    /// <summary>
    /// Gets color index used for transparency (0-15).
    /// </summary>
    /// <exception cref="System.ArgumentOutOfRangeException">The transparent color index is out of range.</exception>
    public byte TransparentColor { get; }

    /// <summary>
    /// Gets a value indicating whether mirror.
    /// </summary>
    public bool Mirror { get; }

    /// <summary>
    /// Gets mirror loop number.
    /// </summary>
    public byte MirrorLoopNumber { get; }

    /// <summary>
    /// Retrieve the pixel color index at the specified point.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <returns>Color index.</returns>
    public byte GetPixel(int x, int y)
    {
        Debug.Assert(x >= 0 && x < this.Width, $"Invalid x coordinate: {x}");
        Debug.Assert(y >= 0 && y < this.Height, $"Invalid y coordinate: {y}");

        return this.pixels[(y * this.Width) + x];
    }

    /// <summary>
    /// 1-dim array of pixels, from top left to bottom right. Each pixel (byte) stores a 4-bit color textIndex.
    /// </summary>
    /// <returns>
    /// Copy of 1-dim array of pixels.
    /// </returns>
    public byte[] GetPixels()
    {
        return (byte[])this.pixels.Clone();
    }

    /// <summary>
    /// Creates and returns an horizontal mirror image.
    /// </summary>
    /// <returns>1-dim array of pixels of mirror image.</returns>
    public byte[] MirrorPixels()
    {
        byte[] mirrorPixels = new byte[this.pixels.Length];

        for (int line = 0; line < this.Height; line++)
        {
            for (int i = 0; i < this.Width; i++)
            {
                mirrorPixels[(line * this.Width) + (this.Width - i - 1)] = this.pixels[(line * this.Width) + i];
            }
        }

        return mirrorPixels;
    }
}
