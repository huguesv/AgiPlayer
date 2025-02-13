// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl3;

#if USE_SDL3

using System;
using System.Drawing;
using System.Linq;
using Woohoo.Agi.Engine.Interpreter;
using static SDL3.SDL;

internal class Sdl3GraphicsDriver : IGraphicsDriver
{
    private nint window;
    private nint windowSurface;
    private nint imageSurface;
    private nint imagePalette;

    private int displayScaleX;
    private int displayScaleY;
    private int displayWidth;
    private int displayHeight;
    private int nativeWidth;
    private int nativeHeight;
    private int nativeScaleX;
    private int nativeScaleY;

    unsafe void IGraphicsDriver.Display(int displayScaleX, int displayScaleY, int renderScaleX, int renderScaleY)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(displayScaleX, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(displayScaleY, 1);

        this.nativeWidth = 320 * renderScaleX;
        this.nativeHeight = 200 * renderScaleY;
        this.nativeScaleX = renderScaleX;
        this.nativeScaleY = renderScaleY;
        this.displayScaleX = displayScaleX;
        this.displayScaleY = displayScaleY;
        this.displayWidth = this.nativeWidth * displayScaleX;
        this.displayHeight = this.nativeHeight * displayScaleY;

        SDL_SetWindowSize(this.window, this.displayWidth, this.displayHeight);
        SDL_SetWindowPosition(this.window, 100, 100);
        this.windowSurface = (nint)SDL_GetWindowSurface(this.window);
        this.imageSurface = (nint)SDL_CreateSurface(this.displayWidth, this.displayHeight, SDL_PixelFormat.SDL_PIXELFORMAT_INDEX8);
        this.imagePalette = (nint)SDL_CreateSurfacePalette(this.imageSurface);

        SDL_LockSurface(this.imageSurface);
        try
        {
            SDL_Surface* imageSurfacePtr = (SDL_Surface*)this.imageSurface;
            byte* imageSurfacePixels = (byte*)imageSurfacePtr->pixels;

            for (int i = 0; i < this.displayWidth; i++)
            {
                for (int j = 0; j < this.displayHeight; j++)
                {
                    int destinationOffset = (j * imageSurfacePtr->pitch) + i;
                    imageSurfacePixels[destinationOffset] = 0;
                }
            }
        }
        finally
        {
            SDL_UnlockSurface(this.imageSurface);
        }

        this.UpdateRect(0, 0, this.displayWidth, this.displayHeight);
    }

    unsafe void IGraphicsDriver.Fill(RenderRectangle rect, byte color)
    {
        SDL_Rect sdlRect = new SDL_Rect
        {
            x = (short)(rect.X * this.displayScaleX),
            y = (short)(rect.Y * this.displayScaleY),
            w = (short)(rect.Width * this.displayScaleX),
            h = (short)(rect.Height * this.displayScaleY),
        };

        SDL_Rect* prect = &sdlRect;

        SDL_FillSurfaceRect((nint)this.imageSurface, (nint)prect, color);
    }

    void IGraphicsDriver.Initialize()
    {
        _ = SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO | SDL_InitFlags.SDL_INIT_EVENTS | SDL_InitFlags.SDL_INIT_AUDIO);
        this.window = SDL_CreateWindow("AgiPlayer - SDL3", 320, 200, 0);
    }

    unsafe void IGraphicsDriver.RenderCharacter(RenderPoint point, RenderSize size, byte flags, byte[] pixels, int pixelsHeight, int pixelsWidth)
    {
        int fontScaleX = (this.displayScaleX * size.Width) / pixelsWidth;
        int fontScaleY = (this.displayScaleY * size.Height) / pixelsHeight;

        SDL_LockSurface(this.imageSurface);
        try
        {
            SDL_Surface* imageSurfacePtr = (SDL_Surface*)this.imageSurface;
            byte* imageSurfacePixels = (byte*)imageSurfacePtr->pixels;

            for (int i = 0; i < pixelsWidth; i++)
            {
                int positionX = (point.X * this.displayScaleX) + (i * fontScaleX);

                for (int j = 0; j < pixelsHeight; j++)
                {
                    int positionY = (point.Y * this.displayScaleY) + (j * fontScaleY);

                    byte pixel = pixels[(j * pixelsWidth) + i];

                    for (int deltaY = 0; deltaY < fontScaleY; deltaY++)
                    {
                        for (int deltaX = 0; deltaX < fontScaleX; deltaX++)
                        {
                            int destinationOffset = ((positionY + deltaY) * imageSurfacePtr->pitch) + positionX + deltaX;
                            imageSurfacePixels[destinationOffset] = pixel;
                        }
                    }
                }
            }
        }
        finally
        {
            SDL_UnlockSurface(this.imageSurface);
        }
    }

    unsafe void IGraphicsDriver.RenderToScreen(RenderBuffer buffer, int offsetYRenderPoints, RenderPoint topLeft, RenderPoint bottomRight, bool fade)
    {
        // all these coordinates are in render buffer coordinates (320x200 or 640x400)
        byte[,] bytes = buffer.GetBuffer();

        SDL_LockSurface(this.imageSurface);
        try
        {
            SDL_Surface* imageSurfacePtr = (SDL_Surface*)this.imageSurface;
            byte* imageSurfacePixels = (byte*)imageSurfacePtr->pixels;

            if (!fade)
            {
                for (int j = topLeft.Y; j < bottomRight.Y; j++)
                {
                    int positionY = (offsetYRenderPoints + j) * this.displayScaleY;

                    for (int i = topLeft.X; i < bottomRight.X; i++)
                    {
                        byte pixel = bytes[i, j];

                        int positionX = i * this.displayScaleX;

                        for (int deltaY = 0; deltaY < this.displayScaleY; deltaY++)
                        {
                            for (int deltaX = 0; deltaX < this.displayScaleX; deltaX++)
                            {
                                int destinationOffset = ((positionY + deltaY) * imageSurfacePtr->pitch) + positionX + deltaX;
                                imageSurfacePixels[destinationOffset] = pixel;
                            }
                        }
                    }
                }

                this.UpdateRect(topLeft.X * this.displayScaleX, (topLeft.Y + offsetYRenderPoints) * this.displayScaleY, (bottomRight.X - topLeft.X) * this.displayScaleX, (bottomRight.Y - topLeft.Y) * this.displayScaleY);
            }
            else
            {
                const int Delay = 50;
                byte[][] fadeBits =
                [
                    [0, 11, 7, 15],
                    [8, 4, 1, 12],
                    [14, 2, 9, 5],
                    [10, 6, 13, 3],
                ];

                for (int fadeCount = 0; fadeCount < 16; fadeCount++)
                {
                    for (int j = topLeft.Y; j < bottomRight.Y; j++)
                    {
                        int positionY = (offsetYRenderPoints + j) * this.displayScaleY;

                        for (int i = topLeft.X; i < bottomRight.X; i++)
                        {
                            if (fadeBits[i % 4][j % 4] == fadeCount)
                            {
                                byte pixel = bytes[i, j];

                                int positionX = i * this.displayScaleX;

                                for (int deltaY = 0; deltaY < this.displayScaleY; deltaY++)
                                {
                                    for (int deltaX = 0; deltaX < this.displayScaleX; deltaX++)
                                    {
                                        int destinationOffset = ((positionY + deltaY) * imageSurfacePtr->pitch) + positionX + deltaX;
                                        imageSurfacePixels[destinationOffset] = pixel;
                                    }
                                }
                            }
                        }
                    }

                    this.UpdateRect(topLeft.X * this.displayScaleX, (topLeft.Y + offsetYRenderPoints) * this.displayScaleY, (bottomRight.X - topLeft.X) * this.displayScaleX, (bottomRight.Y - topLeft.Y) * this.displayScaleY);
                    SDL_Delay(Delay);
                }
            }
        }
        finally
        {
            SDL_UnlockSurface(this.imageSurface);
        }
    }

    PicturePoint IGraphicsDriver.ScreenToPicturePoint(ScreenPoint point)
    {
        return new PicturePoint((byte)(point.X / this.displayScaleX / 2 / this.nativeScaleX), (byte)(point.Y / this.displayScaleY / this.nativeScaleY));
    }

    RenderPoint IGraphicsDriver.ScreenToRenderPoint(ScreenPoint point)
    {
        return new RenderPoint(point.X / this.displayScaleX / this.nativeScaleX, point.Y / this.displayScaleY / this.nativeScaleY);
    }

    unsafe void IGraphicsDriver.Scroll(RenderRectangle rect, int lineCountRenderPoints)
    {
        SDL_LockSurface(this.imageSurface);
        try
        {
            SDL_Surface* imageSurfacePtr = (SDL_Surface*)this.imageSurface;
            byte* imageSurfacePixels = (byte*)imageSurfacePtr->pixels;

            // all these coordinates are in render buffer coordinates (320x200 or 640x400)
            int scaledX = rect.X * this.displayScaleX;
            int scaledY = rect.Y * this.displayScaleY;
            int scaledWidth = rect.Width * this.displayScaleX;
            int scaledHeight = rect.Height * this.displayScaleY;

            int targetY = scaledY;
            int sourceYOffset = lineCountRenderPoints * this.displayScaleY;

            // Temp buffer
            byte[] buffer = new byte[scaledWidth];

            for (int j = 0; j < scaledHeight; j++)
            {
                int targetOffset = (targetY * imageSurfacePtr->pitch) + scaledX;
                int sourceOffset = ((targetY + sourceYOffset) * imageSurfacePtr->pitch) + scaledX;

                for (int i = 0; i < scaledWidth; i++)
                {
                    buffer[i] = imageSurfacePixels[sourceOffset + i];
                }

                for (int i = 0; i < scaledWidth; i++)
                {
                    imageSurfacePixels[targetOffset + i] = buffer[i];
                }

                targetY += lineCountRenderPoints > 0 ? 1 : -1;
            }
        }
        finally
        {
            SDL_UnlockSurface(this.imageSurface);
        }
    }

    void IGraphicsDriver.SetCaption(string caption)
    {
        _ = SDL_SetWindowTitle(this.window, caption);
    }

    void IGraphicsDriver.SetPalette(GraphicsColor[] colors)
    {
        SDL_Color[] sdlColors = colors.Select(c => new SDL_Color { r = c.R, g = c.G, b = c.B, a = 255 }).ToArray();
        _ = SDL_SetPaletteColors(this.imagePalette, sdlColors, 0, sdlColors.Length);

        this.UpdateRect(0, 0, this.displayWidth, this.displayHeight);
    }

    void IGraphicsDriver.Shake(byte count)
    {
        // TODO: Implement this
    }

    unsafe void IGraphicsDriver.Update(RenderRectangle rect)
    {
        // TODO: figure out why respecting rect doesn't work
#if false
        this.UpdateRect(0, 0, this.displayWidth, this.displayHeight);
#endif
#if true
        this.UpdateRect(rect.X * this.displayScaleX, rect.Y * this.displayScaleY, rect.Width * this.displayScaleX, rect.Height * this.displayScaleY);
#endif
    }

    private unsafe void UpdateRect(int x, int y, int width, int height)
    {
        Debug.Assert(width > 0, "Width should be > 0");
        Debug.Assert(height > 0, "Height should be > 0");
        Debug.Assert(x >= 0, "X should be >= 0");
        Debug.Assert(y >= 0, "Y should be >= 0");

        SDL_Rect sdlRect = new SDL_Rect
        {
            x = x,
            y = y,
            w = width,
            h = height,
        };

        SDL_Rect* prect = &sdlRect;
        _ = SDL_BlitSurface((nint)this.imageSurface, (nint)prect, (nint)this.windowSurface, (nint)prect);

        SDL_UpdateWindowSurfaceRects(this.window, [sdlRect], 1);
    }
}

#endif // USE_SDL_3
