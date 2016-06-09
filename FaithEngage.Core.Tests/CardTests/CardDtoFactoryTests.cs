using System;
using System.Linq;
using NUnit.Framework;
using FakeItEasy;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Core.Cards;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

namespace FaithEngage.Core.Tests
{
	[TestFixture]
	public class CardDtoFactoryTests
	{
        private IRenderableCard card;
        private IRenderableCardSection section;
        private IRenderableCardSection[] sections;
        private DisplayUnit du;

        [SetUp]
        public void Init()
        {
            card = A.Fake<IRenderableCard> ();
            card.Title = "My Title";
            card.Description = "My Description";
            section = A.Fake<IRenderableCardSection>();
            section.HeadingText = "My Heading Text";
            section.HtmlContents = "<p>This is my heading</p>";
            sections = Enumerable.Repeat (section, 5).ToArray ();
            A.CallTo (() => card.Sections).Returns (sections);
            du = A.Fake<DisplayUnit> ();
            card.OriginatingDisplayUnit = du;
            A.CallTo (() => du.GetCard ()).Returns (card);


        }
           
        [Test]
		public void GetCards_ValidDict_ReturnsCardDTOs()
        {
            var i = 0;
            var dict = Enumerable.Repeat (du, 5).ToDictionary ((k) => i++, v => v);

            var fac = new CardDtoFactory ();
            var dtos = fac.GetCards (dict);

			Assert.That (dtos.Length == 5);
            Assert.That (dtos.All (p => p.Title == "My Title"));
            Assert.That (dtos.All (p => p.Sections.All (q => q.HeadingText == "My Heading Text")));
            Assert.That (dtos.All (p => p.Sections.All (q => q.HtmlContents == "<p>This is my heading</p>")));
            Assert.That (dtos.All (p => p.OriginatingDisplayUnit == du.Id));
		}

		[Test]
		public void GetCards_EmptyDict_ReturnsEmptyArray()
		{
            var dict = new Dictionary<int,DisplayUnit> ();
            var fac = new CardDtoFactory ();
            var cards = fac.GetCards (dict);

            Assert.That (cards, Is.Not.Null);
			Assert.That (cards.Length == 0);
		}

		[Test]
		public void GetCards_EncountersException_SkipsCardAndContinues()
        {
            var i = 0;
            A.CallTo (() => du.GetCard ()).Throws<Exception> ().Once ();
            var dict = Enumerable.Repeat (du, 5).ToDictionary ((k) => i++, v => v);

            var fac = new CardDtoFactory ();
            var dtos = fac.GetCards (dict);

            Assert.That (dtos, Is.Not.Null);
			Assert.That (dtos.Length, Is.EqualTo (4));
		}

        [Test]
        public void GetCard_ValidDU_ReturnsDto()
        {
            var fac = new CardDtoFactory ();
            var dto = fac.GetCard(du);

            Assert.That (dto, Is.Not.Null);
            Assert.That (dto.Description == "My Description");
			Assert.That (dto.OriginatingDisplayUnit, Is.EqualTo (du.Id));
			Assert.That (dto.Sections.All (q => q.HeadingText == "My Heading Text"));
			Assert.That (dto.Sections.All (q => q.HtmlContents == "<p>This is my heading</p>"));	
        }

        [Test]
		public void GetCard_EncountersException_ReturnsNull()
        {
			A.CallTo (() => du.GetCard()).Throws<Exception> ().Once();
			var fac = new CardDtoFactory ();
			var dto = fac.GetCard (du);

			Assert.That (dto, Is.Null);
        }

		[Test]
		public void ConvertCard_ValidCard_ValidDto()
		{
			var card = A.Fake<IRenderableCard>();
			card.Sections = A.CollectionOfFake<IRenderableCardSection>(5).ToArray();
			for (var i = 0; i < card.Sections.Length;i++)
			{
				card.Sections[i].HeadingText = "Heading";
				card.Sections[i].HtmlContents = "<p>Contents</p>";
			}
			card.Description = "Description";
			card.OriginatingDisplayUnit = du;
			card.Title = "Title";
			du.PositionInEvent = 1;
			var id = Guid.NewGuid();
			du.AssociatedEvent = id;

			var fac = new CardDtoFactory();
			var dto = fac.ConvertCard(card);

			Assert.That(dto.AssociatedEvent, Is.EqualTo(id));
			Assert.That(dto.Description, Is.EqualTo(card.Description));
			Assert.That(dto.OriginatingDisplayUnit, Is.EqualTo(du.Id));
			Assert.That(dto.Sections.All(p => p.HeadingText == "Heading"));
			Assert.That(dto.Sections.All(p => p.HtmlContents == "<p>Contents</p>"));
			Assert.That(dto.Title, Is.EqualTo(card.Title));
			Assert.That(dto.PositionInEvent, Is.EqualTo(du.PositionInEvent));
		}

		[Test]
		public void ConvertCard_InvalidCard_ReturnsNull()
		{
			var card = A.Fake<IRenderableCard>();
			card.Sections = A.CollectionOfFake<IRenderableCardSection>(5).ToArray();
			card.OriginatingDisplayUnit = du;
			A.CallTo(() => card.Title).Throws<Exception>();

			var fac = new CardDtoFactory();
			var dto = fac.ConvertCard(card);

			Assert.That(dto, Is.Null);
		}
	}
}

