// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.UnitTest
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Woohoo.Agi.Interpreter;

    [TestClass]
    public class RandomUnitTest
    {
        [TestMethod]
        public void ConsecutiveAreNotIdentical()
        {
            var rnd = new Random();

            byte val = 0;
            for (int i = 0; i < 100; i++)
            {
                byte newVal = rnd.Next();
                if (i > 0)
                {
                    newVal.Should().NotBe(val);
                }

                val = newVal;
            }
        }
    }
}
