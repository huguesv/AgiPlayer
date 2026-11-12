// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable disable

namespace Woohoo.Agi.Engine.Interpreter;

public delegate int AudioCallback(ToneChannel ch, short[] buffer, int count);
