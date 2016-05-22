using System;
using NUnit.Framework;
using FaithEngage.Core.DisplayUnits;

namespace FaithEngage.Core.Tests
{
    [TestFixture]
    public class TextUnitTests
    {
        const string VALID_TEXT = "This is a valid text string.";

        [Test]
        public void Ctor_ValidString_ValidTextUnit()
        {
            var tu = new TextUnit (VALID_TEXT);

            Assert.That (tu, Is.InstanceOf (typeof(TextUnit)));
            Assert.That (tu.Text == VALID_TEXT);
        }
    }
}

