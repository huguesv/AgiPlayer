// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.UnitTest.Infrastructure;

using System.Collections.Generic;
using Woohoo.Agi.Engine.Resources;

internal class InventoryBuilder
{
    private readonly List<InventoryItem> items = [];
    private int maxAnimatedObjects;

    public InventoryBuilder WithItem(string name, byte location)
    {
        this.items.Add(new InventoryItem(name, location));
        return this;
    }

    public InventoryBuilder WithMaxAnimatedObjects(int maxAnimatedObjects)
    {
        this.maxAnimatedObjects = maxAnimatedObjects;
        return this;
    }

    public InventoryResource Build()
    {
        return new InventoryResource(this.items.ToArray(), this.maxAnimatedObjects);
    }
}
