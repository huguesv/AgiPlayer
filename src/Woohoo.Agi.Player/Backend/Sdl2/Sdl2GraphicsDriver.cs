// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl2
{
#if USE_SDL2
    using Woohoo.Agi.Interpreter;
    using static Woohoo.Agi.Player.Backend.Sdl2.NativeMethods;

    internal sealed class Sdl2GraphicsDriver : IGraphicsDriver
    {
        private int displayScaleX;
        private int displayScaleY;
        private IntPtr windowPtr;
        private IntPtr surfacePtr;
        private IntPtr texturePtr;
        private IntPtr rendererPtr;
        private int displayWidth;
        private int displayHeight;
        private int bpp;
        private int nativeWidth;
        private int nativeHeight;
        private int nativeScaleX;
        private int nativeScaleY;
        //private uint palette;

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
            this.bpp = 8;

            this.windowPtr = SDL_CreateWindow("", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, this.displayWidth, this.displayHeight, SDL_WindowFlags.SDL_WINDOW_SHOWN);
            this.rendererPtr = SDL_CreateRenderer(this.windowPtr, 0, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
            this.surfacePtr = SDL_CreateRGBSurfaceWithFormat(0, this.displayWidth, this.displayHeight, this.bpp, SDL_PIXELFORMAT_INDEX8);
            //this.surfacePtr = SDL_CreateRGBSurface(0, this.displayWidth, this.displayHeight, this.bpp, 0x00FF0000, 0x0000FF00, 0x000000FF, 0xFF000000);
            this.texturePtr = SDL_CreateTextureFromSurface(this.rendererPtr, this.surfacePtr);

            SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, "linear");
            SDL_RenderSetLogicalSize(this.rendererPtr, this.displayWidth, this.displayHeight);

            SDL_Rect rect2 = new SDL_Rect() { x = 0, y = 0, w = (short)this.displayWidth, h = (short)this.displayHeight };
            SDL_SetClipRect(this.surfacePtr, ref rect2);
        }

        void IGraphicsDriver.Update(RenderRectangle rect)
        {
            var rects = new SDL_Rect[]
            {
                new SDL_Rect()
                {
                    x = rect.X * this.displayScaleX,
                    y = rect.Y * this.displayScaleY,
                    w = rect.Width * this.displayScaleX,
                    h = rect.Height * this.displayScaleY,
                }
            };

            SDL_UpdateWindowSurfaceRects(this.windowPtr, rects, rects.Length);
        }

        void IGraphicsDriver.SetPalette(GraphicsColor[] colors)
        {
            //for (int i = 0; i < colors.Length; i++)
            //{
            //    this.palette[i] = (uint)(((byte)0xff << 24) | (colors[i].R << 16) | (colors[i].G << 8) | colors[i].B);
            //}
            SDL_Color[] palette = new SDL_Color[colors.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                palette[i].r = colors[i].R;
                palette[i].g = colors[i].G;
                palette[i].b = colors[i].B;
                //palette[i].unused = 0;
            }

            SDL_Surface surface = (SDL_Surface)Marshal.PtrToStructure(this.surfacePtr, typeof(SDL_Surface));
            SDL_PixelFormat format = (SDL_PixelFormat)Marshal.PtrToStructure(surface.format, typeof(SDL_PixelFormat));
            SDL_SetPaletteColors(format.palette, palette, 0, palette.Length);

            this.texturePtr = SDL_CreateTextureFromSurface(this.rendererPtr, this.surfacePtr);
        }

        void IGraphicsDriver.Fill(RenderRectangle rect, byte color)
        {
            SDL_Rect sdlRect = new SDL_Rect() { x = (short)(rect.X * this.displayScaleX), y = (short)(rect.Y * this.displayScaleY), w = (short)(rect.Width * this.displayScaleX), h = (short)(rect.Height * this.displayScaleY) };
            SDL_FillRect(this.surfacePtr, ref sdlRect, color);
        }

        void IGraphicsDriver.RenderToScreen(RenderBuffer buffer, int offsetYRenderPoints, RenderPoint topLeft, RenderPoint bottomRight, bool fade)
        {
            // all these coordinates are in render buffer coordinates (320x200 or 640x400)
            byte[,] bytes = buffer.GetBuffer();

            SDL_Surface surface = (SDL_Surface)Marshal.PtrToStructure(this.surfacePtr, typeof(SDL_Surface));

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
                                Marshal.WriteByte(new IntPtr(surface.pixels.ToInt32() + ((positionY + deltaY) * surface.pitch) + (positionX + deltaX)), pixel);
                            }
                        }
                    }
                }

                var rects = new SDL_Rect[]
                {
                    new SDL_Rect()
                    {
                        x = topLeft.X * this.displayScaleX,
                        y = (topLeft.Y + offsetYRenderPoints) * this.displayScaleY,
                        w = (bottomRight.X - topLeft.X) * this.displayScaleX,
                        h = (bottomRight.Y - topLeft.Y) * this.displayScaleY,
                    }
                };

                SDL_UpdateWindowSurfaceRects(this.windowPtr, rects, rects.Length);
            }
            else
            {
                const int Delay = 50;
                byte[][] fadeBits = new byte[][]
                {
                    new byte[] { 0, 11, 7, 15 },
                    new byte[] { 8, 4, 1, 12 },
                    new byte[] { 14, 2, 9, 5 },
                    new byte[] { 10, 6, 13, 3 },
                };

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
                                        Marshal.WriteByte(new IntPtr(surface.pixels.ToInt32() + ((positionY + deltaY) * surface.pitch) + (positionX + deltaX)), pixel);
                                    }
                                }
                            }
                        }
                    }

                    var rects = new SDL_Rect[]
                    {
                        new SDL_Rect()
                        {
                            x = topLeft.X * this.displayScaleX,
                            y = (topLeft.Y + offsetYRenderPoints) * this.displayScaleY,
                            w = (bottomRight.X - topLeft.X) * this.displayScaleX,
                            h = (bottomRight.Y - topLeft.Y) * this.displayScaleY,
                        }
                    };

                    SDL_UpdateWindowSurfaceRects(this.windowPtr, rects, rects.Length);

                    SDL_Delay(Delay);
                }
            }

            var src = new SDL_Rect()
            {
                x = topLeft.X * this.displayScaleX,
                y = (topLeft.Y + offsetYRenderPoints) * this.displayScaleY,
                w = (bottomRight.X - topLeft.X) * this.displayScaleX,
                h = (bottomRight.Y - topLeft.Y) * this.displayScaleY,
            };

            SDL_RenderCopy(this.rendererPtr, this.texturePtr, ref src, ref src);
            SDL_RenderPresent(this.rendererPtr);
        }

        void IGraphicsDriver.Shake(byte count)
        {
            const int Offset = 3;
            const int Delay = 100;

            int scaledOffsetX = Offset * this.displayScaleX;
            int scaledOffsetY = Offset * this.displayScaleY;

            SDL_Surface surface = (SDL_Surface)Marshal.PtrToStructure(this.surfacePtr, typeof(SDL_Surface));

            // Save screen
            byte[] backup = new byte[surface.pitch * this.displayHeight];
            Marshal.Copy(surface.pixels, backup, 0, surface.pitch * this.displayHeight);

            for (byte c = 0; c < count; c++)
            {
                SDL_Delay(Delay);

                // Draw black on left
                SDL_Rect leftRect = new SDL_Rect() { x = (short)0, y = (short)0, w = (short)scaledOffsetX, h = (short)this.displayHeight };
                SDL_FillRect(this.surfacePtr, ref leftRect, 0);

                // Draw black on top
                SDL_Rect topRect = new SDL_Rect() { x = (short)0, y = (short)0, w = (short)this.displayWidth, h = (short)scaledOffsetY };
                SDL_FillRect(this.surfacePtr, ref topRect, 0);

                // Draw screen at offset
                for (int j = 0; j < this.displayHeight - scaledOffsetY; j++)
                {
                    int x = surface.pixels.ToInt32() + ((j + scaledOffsetY) * surface.pitch) + scaledOffsetY;

                    int length = this.displayWidth - scaledOffsetX;
                    IntPtr ptr = new IntPtr(x);
                    Marshal.Copy(backup, j * surface.pitch, ptr, length);
                }

                SDL_UpdateWindowSurface(this.windowPtr);

                SDL_Delay(Delay);

                // Restore screen
                Marshal.Copy(backup, 0, surface.pixels, surface.pitch * this.displayHeight);

                SDL_UpdateWindowSurface(this.windowPtr);
            }
        }

        void IGraphicsDriver.RenderCharacter(RenderPoint point, RenderSize size, byte flags, byte[] pixels, int pixelsHeight, int pixelsWidth)
        {
            int fontScaleX = (this.displayScaleX * size.Width) / pixelsWidth;
            int fontScaleY = (this.displayScaleY * size.Height) / pixelsHeight;

            SDL_Surface surface = (SDL_Surface)Marshal.PtrToStructure(this.surfacePtr, typeof(SDL_Surface));
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
                            Marshal.WriteByte(new IntPtr(surface.pixels.ToInt32() + ((positionY + deltaY) * surface.pitch) + (positionX + deltaX)), pixel);
                        }
                    }
                }
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

        void IGraphicsDriver.Scroll(RenderRectangle rect, int lineCountRenderPoints)
        {
            // all these coordinates are in render buffer coordinates (320x200 or 640x400)
            SDL_Surface surface = (SDL_Surface)Marshal.PtrToStructure(this.surfacePtr, typeof(SDL_Surface));

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
                IntPtr targetPtr = new IntPtr(surface.pixels.ToInt32() + (targetY * surface.pitch) + scaledX);
                IntPtr sourcePtr = new IntPtr(surface.pixels.ToInt32() + ((targetY + sourceYOffset) * surface.pitch) + scaledX);

                Marshal.Copy(sourcePtr, buffer, 0, scaledWidth);
                Marshal.Copy(buffer, 0, targetPtr, scaledWidth);

                targetY += lineCountRenderPoints > 0 ? 1 : -1;
            }
        }
    }
#endif
}
