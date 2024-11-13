// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

public interface IGraphicsDriver
{
    void Initialize();

    void SetCaption(string caption);

    void Display(int displayScaleX, int displayScaleY, int renderScaleX, int renderScaleY);

    void Update(RenderRectangle rect);

    void SetPalette(GraphicsColor[] colors);

    void Fill(RenderRectangle rect, byte color);

    void RenderToScreen(RenderBuffer buffer, int offsetYRenderPoints, RenderPoint topLeft, RenderPoint bottomRight, bool fade);

    void Shake(byte count);

    void RenderCharacter(RenderPoint point, RenderSize size, byte flags, byte[] pixels, int pixelsHeight, int pixelsWidth);

    PicturePoint ScreenToPicturePoint(ScreenPoint point);

    RenderPoint ScreenToRenderPoint(ScreenPoint point);

    void Scroll(RenderRectangle rect, int lineCountRenderPoints);
}
