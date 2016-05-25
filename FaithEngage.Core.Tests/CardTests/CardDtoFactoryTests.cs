using System;
using System.Linq;
using NUnit.Framework;
using FakeItEasy;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Core.Cards;

namespace FaithEngage.Core.Tests
{
	[TestFixture]
	public class CardDtoFactoryTests
	{
		
		[Test]
		public void GetCards_ValidDict_ReturnsCardDTOs()
		{
            var card = A.Fake<IRenderableCard> ();
            card.Title = "My Title";
            card.Description = "My Description";
            var section = A.Fake<IRenderableCardSection>();
            section.HeadingText = "My Heading Text";
            section.HtmlContents = "<p>This is my heading</p>";
            var sections = Enumerable.Repeat (section, 5).ToArray ();
            A.CallTo (() => card.Sections).Returns (sections);
            var i = 0;
            var du = A.Fake<DisplayUnit> ();
            A.CallTo (() => du.GetCard ()).Returns (card);
            var dict = Enumerable.Repeat (du, 5).ToDictionary ((k) => i++, v => v);

            var fac = new CardDtoFactory ();
            var dtos = fac.GetCards (dict);

            Assert.That (dtos.Count, Is.EqualTo (5));
            Assert.That (dtos.All (p => p.Title == "My Title"));
            Assert.That (dtos.All (p => p.Sections.All (q => q.HeadingText == "My Heading Text")));
		}

		[Test]
		public void GetCards_EmptyDict_ReturnsEmptyArray()
		{
		}

		[Test]
		public void GetCards_EncountersException_SkipsCardAndContinues()
		{
		}
	}
}

