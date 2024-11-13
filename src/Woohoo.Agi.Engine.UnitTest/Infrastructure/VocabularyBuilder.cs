// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Infrastructure;

using System.Collections.Generic;
using Woohoo.Agi.Engine.Resources;

internal class VocabularyBuilder
{
    private readonly List<VocabularyWordFamily> families = [];

    public VocabularyBuilder WithFamily(int family, params string[] words)
    {
        this.families.Add(new VocabularyWordFamily(family, words));
        return this;
    }

    public VocabularyResource Build()
    {
        return new VocabularyResource(this.families.ToArray());
    }
}
