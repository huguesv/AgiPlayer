// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl2;

#if USE_SDL2
using Woohoo.Agi.Interpreter;
using static Woohoo.Agi.Player.Backend.Sdl2.NativeMethods;

internal sealed class Sdl2GraphicsDriver : IGraphicsDriver
{
    private int displayScaleX;
    private int displayScaleY;
    private IntPtr windowPtr;
    private IntPtr texturePtr;
    private IntPtr rendererPtr;
    private byte[] textureData;
    private int displayWidth;
    private int displayHeight;
    private int nativeWidth;
    private int nativeHeight;
    private int nativeScaleX;
    private int nativeScaleY;
    private GraphicsColor[] palette;

    public Sdl2GraphicsDriver()
    {
    }

    void IGraphicsDriver.Initialize()
    {
        SDL_Init(SDL_INIT_EVERYTHING);

        /*
        int joysticks = SDL_NumJoysticks();
        for (int i = 0; i < joysticks; i++)
        {
            string name = SDL_JoystickName(i);
            Console.WriteLine("Joystick {0}: {1}", i, name);
        }
        */
    }

    void IGraphicsDriver.SetCaption(string caption)
    {
        SDL_SetWindowTitle(this.windowPtr, caption);
    }

    void IGraphicsDriver.Display(int displayScaleX, int displayScaleY, int renderScaleX, int renderScaleY)
    {
        if (displayScaleX < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(displayScaleX));
        }

        if (displayScaleY < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(displayScaleY));
        }

        this.nativeWidth = 320 * renderScaleX;
        this.nativeHeight = 200 * renderScaleY;
        this.nativeScaleX = renderScaleX;
        this.nativeScaleY = renderScaleY;
        this.displayScaleX = displayScaleX;
        this.displayScaleY = displayScaleY;
        this.displayWidth = this.nativeWidth * displayScaleX;
        this.displayHeight = this.nativeHeight * displayScaleY;

        SDL_CreateWindowAndRenderer(this.displayWidth, this.displayHeight, SDL_WindowFlags.SDL_WINDOW_SHOWN, out this.windowPtr, out this.rendererPtr);

        this.texturePtr = SDL_CreateTexture(this.rendererPtr, SDL_PIXELFORMAT_ABGR8888, (int)SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, this.nativeWidth, this.nativeHeight);
        this.textureData = new byte[this.nativeWidth * this.nativeHeight * 4];

        SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, "linear");
        SDL_RenderSetLogicalSize(this.rendererPtr, this.nativeWidth, this.nativeHeight);
    }

    void IGraphicsDriver.Update(RenderRectangle rect)
    {
        SDL_RenderClear(this.rendererPtr);
        SDL_RenderCopy(this.rendererPtr, this.texturePtr, IntPtr.Zero, IntPtr.Zero);
        SDL_RenderPresent(this.rendererPtr);
    }

    void IGraphicsDriver.SetPalette(GraphicsColor[] colors)
    {
        this.palette = (GraphicsColor[])colors.Clone();
    }

    unsafe void IGraphicsDriver.Fill(RenderRectangle rect, byte color)
    {
        for (int j = 0; j < rect.Height; j++)
        {
            for (int i = 0; i < rect.Width; i++)
            {
                int textureIndex = (rect.Y + j) * this.nativeWidth * 4 + (rect.X + i) * 4;
                this.textureData[textureIndex] = this.palette[color].R;
                this.textureData[textureIndex + 1] = this.palette[color].G;
                this.textureData[textureIndex + 2] = this.palette[color].B;
                this.textureData[textureIndex + 3] = 255;
            }

            this.UpdateTexture();
        }
    }

    unsafe void IGraphicsDriver.RenderToScreen(RenderBuffer buffer, int offsetYRenderPoints, RenderPoint topLeft, RenderPoint bottomRight, bool fade)
    {
        // all these coordinates are in render buffer coordinates (320x200 or 640x400)
        byte[,] bytes = buffer.GetBuffer();

        if (!fade)
        {
            for (int j = topLeft.Y; j < bottomRight.Y; j++)
            {
                for (int i = topLeft.X; i < bottomRight.X; i++)
                {
                    byte pixel = bytes[i, j];

                    int textureIndex = (offsetYRenderPoints + j) * this.nativeWidth * 4 + i * 4;
                    this.textureData[textureIndex] = this.palette[pixel].R;
                    this.textureData[textureIndex + 1] = this.palette[pixel].G;
                    this.textureData[textureIndex + 2] = this.palette[pixel].B;
                    this.textureData[textureIndex + 3] = 255;
                }
            }

            this.UpdateTexture();
        }
        else
        {
            //const int Delay = 50;
            //byte[][] fadeBits = new byte[][]
            //{
            //    new byte[] { 0, 11, 7, 15 },
            //    new byte[] { 8, 4, 1, 12 },
            //    new byte[] { 14, 2, 9, 5 },
            //    new byte[] { 10, 6, 13, 3 },
            //};

            //for (int fadeCount = 0; fadeCount < 16; fadeCount++)
            //{
            //    for (int j = topLeft.Y; j < bottomRight.Y; j++)
            //    {
            //        int positionY = (offsetYRenderPoints + j) * this.displayScaleY;

            //        for (int i = topLeft.X; i < bottomRight.X; i++)
            //        {
            //            if (fadeBits[i % 4][j % 4] == fadeCount)
            //            {
            //                byte pixel = bytes[i, j];

            //                int positionX = i * this.displayScaleX;

            //                for (int deltaY = 0; deltaY < this.displayScaleY; deltaY++)
            //                {
            //                    for (int deltaX = 0; deltaX < this.displayScaleX; deltaX++)
            //                    {
            //                        Marshal.WriteByte(new IntPtr(surface.pixels.ToInt32() + ((positionY + deltaY) * surface.pitch) + (positionX + deltaX)), pixel);
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    var rects = new SDL_Rect[]
            //    {
            //        new SDL_Rect()
            //        {
            //            x = topLeft.X * this.displayScaleX,
            //            y = (topLeft.Y + offsetYRenderPoints) * this.displayScaleY,
            //            w = (bottomRight.X - topLeft.X) * this.displayScaleX,
            //            h = (bottomRight.Y - topLeft.Y) * this.displayScaleY,
            //        }
            //    };

            //    SDL_UpdateWindowSurfaceRects(this.windowPtr, rects, rects.Length);

            //    SDL_Delay(Delay);
            //}
        }

        //var src = new SDL_Rect()
        //{
        //    x = topLeft.X * this.displayScaleX,
        //    y = (topLeft.Y + offsetYRenderPoints) * this.displayScaleY,
        //    w = (bottomRight.X - topLeft.X) * this.displayScaleX,
        //    h = (bottomRight.Y - topLeft.Y) * this.displayScaleY,
        //};

        //SDL_RenderCopy(this.rendererPtr, this.texturePtr, ref src, ref src);
        //SDL_RenderPresent(this.rendererPtr);
    }

    void IGraphicsDriver.Shake(byte count)
    {
        //const int Offset = 3;
        //const int Delay = 100;

        //int scaledOffsetX = Offset * this.displayScaleX;
        //int scaledOffsetY = Offset * this.displayScaleY;

        //SDL_Surface surface = (SDL_Surface)Marshal.PtrToStructure(this.surfacePtr, typeof(SDL_Surface));

        //// Save screen
        //byte[] backup = new byte[surface.pitch * this.displayHeight];
        //Marshal.Copy(surface.pixels, backup, 0, surface.pitch * this.displayHeight);

        //for (byte c = 0; c < count; c++)
        //{
        //    SDL_Delay(Delay);

        //    // Draw black on left
        //    SDL_Rect leftRect = new SDL_Rect() { x = (short)0, y = (short)0, w = (short)scaledOffsetX, h = (short)this.displayHeight };
        //    SDL_FillRect(this.surfacePtr, ref leftRect, 0);

        //    // Draw black on top
        //    SDL_Rect topRect = new SDL_Rect() { x = (short)0, y = (short)0, w = (short)this.displayWidth, h = (short)scaledOffsetY };
        //    SDL_FillRect(this.surfacePtr, ref topRect, 0);

        //    // Draw screen at offset
        //    for (int j = 0; j < this.displayHeight - scaledOffsetY; j++)
        //    {
        //        int x = surface.pixels.ToInt32() + ((j + scaledOffsetY) * surface.pitch) + scaledOffsetY;

        //        int length = this.displayWidth - scaledOffsetX;
        //        IntPtr ptr = new IntPtr(x);
        //        Marshal.Copy(backup, j * surface.pitch, ptr, length);
        //    }

        //    SDL_UpdateWindowSurface(this.windowPtr);

        //    SDL_Delay(Delay);

        //    // Restore screen
        //    Marshal.Copy(backup, 0, surface.pixels, surface.pitch * this.displayHeight);

        //    SDL_UpdateWindowSurface(this.windowPtr);
        //}
    }

    unsafe void IGraphicsDriver.RenderCharacter(RenderPoint point, RenderSize size, byte flags, byte[] pixels, int pixelsHeight, int pixelsWidth)
    {
        for (int j = 0; j < pixelsHeight; j++)
        {
            for (int i = 0; i < pixelsWidth; i++)
            {
                byte pixel = pixels[(j * pixelsWidth) + i];

                int textureIndex = (point.Y + j) * this.nativeWidth * 4 + (point.X + i) * 4;
                this.textureData[textureIndex] = this.palette[pixel].R;
                this.textureData[textureIndex + 1] = this.palette[pixel].G;
                this.textureData[textureIndex + 2] = this.palette[pixel].B;
                this.textureData[textureIndex + 3] = 255;
            }
        }

        this.UpdateTexture();

        //int fontScaleX = (this.displayScaleX * size.Width) / pixelsWidth;
        //int fontScaleY = (this.displayScaleY * size.Height) / pixelsHeight;

        //SDL_Surface surface = (SDL_Surface)Marshal.PtrToStructure(this.surfacePtr, typeof(SDL_Surface));
        //for (int i = 0; i < pixelsWidth; i++)
        //{
        //    int positionX = (point.X * this.displayScaleX) + (i * fontScaleX);

        //    for (int j = 0; j < pixelsHeight; j++)
        //    {
        //        int positionY = (point.Y * this.displayScaleY) + (j * fontScaleY);

        //        byte pixel = pixels[(j * pixelsWidth) + i];

        //        for (int deltaY = 0; deltaY < fontScaleY; deltaY++)
        //        {
        //            for (int deltaX = 0; deltaX < fontScaleX; deltaX++)
        //            {
        //                Marshal.WriteByte(new IntPtr(surface.pixels.ToInt32() + ((positionY + deltaY) * surface.pitch) + (positionX + deltaX)), pixel);
        //            }
        //        }
        //    }
        //}
    }

    PicturePoint IGraphicsDriver.ScreenToPicturePoint(ScreenPoint point)
    {
        return new PicturePoint((byte)(point.X / this.displayScaleX / 2 / this.nativeScaleX), (byte)(point.Y / this.displayScaleY / this.nativeScaleY));
    }

    RenderPoint IGraphicsDriver.ScreenToRenderPoint(ScreenPoint point)
    {
        return new RenderPoint(point.X / this.displayScaleX / this.nativeScaleX, point.Y / this.displayScaleY / this.nativeScaleY);
    }

    void IGraphicsDriver.Scroll(RenderRectangle rect, int lineCountRenderPoints)
    {
        //// all these coordinates are in render buffer coordinates (320x200 or 640x400)
        //SDL_Surface surface = (SDL_Surface)Marshal.PtrToStructure(this.surfacePtr, typeof(SDL_Surface));

        //int scaledX = rect.X * this.displayScaleX;
        //int scaledY = rect.Y * this.displayScaleY;
        //int scaledWidth = rect.Width * this.displayScaleX;
        //int scaledHeight = rect.Height * this.displayScaleY;

        //int targetY = scaledY;
        //int sourceYOffset = lineCountRenderPoints * this.displayScaleY;

        //// Temp buffer
        //byte[] buffer = new byte[scaledWidth];

        //for (int j = 0; j < scaledHeight; j++)
        //{
        //    IntPtr targetPtr = new IntPtr(surface.pixels.ToInt32() + (targetY * surface.pitch) + scaledX);
        //    IntPtr sourcePtr = new IntPtr(surface.pixels.ToInt32() + ((targetY + sourceYOffset) * surface.pitch) + scaledX);

        //    Marshal.Copy(sourcePtr, buffer, 0, scaledWidth);
        //    Marshal.Copy(buffer, 0, targetPtr, scaledWidth);

        //    targetY += lineCountRenderPoints > 0 ? 1 : -1;
        //}
    }

    unsafe private void UpdateTexture()
    {
        fixed (byte* textureBuffer = this.textureData)
        {
            SDL_UpdateTexture(this.texturePtr, IntPtr.Zero, (IntPtr)textureBuffer, this.nativeWidth * 4);
        }
    }
}
#endif
