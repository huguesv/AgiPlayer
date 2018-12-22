// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources.Serialization.UnitTest
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class XorTransformUnitTest
    {
        // The key is the string "Avis Durgan"
        private readonly byte[] key = { 0x41, 0x76, 0x69, 0x73, 0x20, 0x44, 0x75, 0x72, 0x67, 0x61, 0x6e };

        private byte[] unencrypted00 = new byte[] { 0, 0, 5, 1, 2, 3, 1, 2, 3, 5, 0, 0 };
        private byte[] encrypted00 = new byte[] { 0x41, 0x76, 0x6c, 0x72, 0x22, 0x47, 0x74, 0x70, 0x64, 0x64, 0x6e, 0x41 };

        [TestMethod]
        public void DecryptNull()
        {
            var transform = new XorTransform(this.key);

            Action act = () => transform.TransformFinalBlock(null, 0, 0);

            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Decrypt00()
        {
            this.Decrypt(this.encrypted00, this.unencrypted00);
        }

        private void Decrypt(byte[] encrypted, byte[] expected)
        {
            var transform = new XorTransform(this.key);
            var unencrypted = transform.TransformFinalBlock(encrypted, 0, encrypted.Length);
            unencrypted.Should().BeEquivalentTo(expected);
        }
    }
}
