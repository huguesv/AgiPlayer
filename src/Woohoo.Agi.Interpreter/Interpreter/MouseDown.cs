// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    public class MouseDown
    {
        public MouseDown(int button, int x, int y)
        {
            this.Button = button;
            this.X = x;
            this.Y = y;
        }

        public int Button { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
