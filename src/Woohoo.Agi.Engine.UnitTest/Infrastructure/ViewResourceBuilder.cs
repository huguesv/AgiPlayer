// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.UnitTest.Infrastructure;

using System.Collections.Generic;
using Woohoo.Agi.Engine.Resources;

internal class ViewResourceBuilder
{
    private readonly List<ViewLoop> loops = [];
    private byte index;
    private string description = string.Empty;

    public ViewResourceBuilder WithIndex(byte index)
    {
        this.index = index;
        return this;
    }

    public ViewResourceBuilder WithDescription(string description)
    {
        this.description = description;
        return this;
    }

    public ViewResourceBuilder WithLoop(Action<ViewLoopBuilder> viewLoopBuilderFunc)
    {
        ViewLoopBuilder viewLoopBuilder = new();
        viewLoopBuilderFunc(viewLoopBuilder);
        this.loops.Add(viewLoopBuilder.Build());
        return this;
    }

    public ViewResource Build()
    {
        return new ViewResource(this.index, this.loops.ToArray(), this.description, 0, 0);
    }

    internal class ViewLoopBuilder
    {
        private readonly List<ViewCel> cels = [];
        private int mirrorOfIndex = -1;

        public ViewLoopBuilder WithMirrorOfIndex(int mirrorOfIndex)
        {
            this.mirrorOfIndex = mirrorOfIndex;
            return this;
        }

        public ViewLoopBuilder WithCel(Action<ViewCelBuilder> viewCelBuilderFunc)
        {
            ViewCelBuilder viewCelBuilder = new();
            viewCelBuilderFunc(viewCelBuilder);
            this.cels.Add(viewCelBuilder.Build());
            return this;
        }

        internal ViewLoop Build()
        {
            return new ViewLoop(this.cels.ToArray(), this.mirrorOfIndex);
        }
    }

    internal class ViewCelBuilder()
    {
        private byte width;
        private byte height;
        private byte transparentColor;
        private bool mirror;
        private byte mirrorLoopNumber;
        private byte[] pixels = [];

        public ViewCelBuilder WithSize(byte width, byte height)
        {
            this.width = width;
            this.height = height;
            return this;
        }

        public ViewCelBuilder WithTransparentColor(byte transparentColor)
        {
            this.transparentColor = transparentColor;
            return this;
        }

        public ViewCelBuilder WithMirror(byte mirrorLoopNumber)
        {
            this.mirror = true;
            this.mirrorLoopNumber = mirrorLoopNumber;
            return this;
        }

        public ViewCelBuilder WithPixels(byte[] pixels)
        {
            this.pixels = pixels;
            return this;
        }

        public ViewCelBuilder WithRandomPixels()
        {
            this.pixels = Random.Shared.GetItems<byte>([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15], this.width * this.height);
            return this;
        }

        internal ViewCel Build()
        {
            return new ViewCel(this.width, this.height, this.transparentColor, this.mirror, this.mirrorLoopNumber, this.pixels);
        }
    }
}
