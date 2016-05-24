using System;
using NUnit.Framework;
using FaithEngage.Core.Containers;
using FakeItEasy;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.DisplayUnits;
using System.Linq;
using FaithEngage.Core.Cards;
using System.Collections.Generic;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.CardProcessor
{
    [TestFixture]
    public class CardProcessorTests
    {
        private IContainer _container;
        private IDisplayUnitsRepoManager _mgr;
        private ICardDTOFactory _cardFactory;
        private Guid VALID_GUID = Guid.NewGuid();
        private Guid INVALID_GUID = Guid.NewGuid ();


        [TestFixtureSetUp]
        public void init()
        {
            _container = A.Fake<IContainer> ();
            _mgr = A.Fake<IDisplayUnitsRepoManager> ();
            _cardFactory = A.Fake<ICardDTOFactory> ();
            A.CallTo (() => _container.Resolve<IDisplayUnitsRepoManager> ()).Returns (_mgr);
            A.CallTo (() => _container.Resolve<ICardDTOFactory> ()).Returns (_cardFactory);
        }

        [Test]
        public void GetLiveCardsByEvent_ValidEventId_ArrayOfRenderableCardDTOs()
        {
            var cp = new CardProcessor (_container);
            var i = 0;
            var dummies = Enumerable
                .Repeat (A.Dummy<DisplayUnit> (), 5)
                .ToDictionary (p => i++, p => p);
            var dummyCards = A.CollectionOfFake<RenderableCardDTO> (5).ToArray ();

            A.CallTo (() => _mgr.GetByEvent (VALID_GUID, true)).Returns (dummies);
            A.CallTo (() => _cardFactory.GetCards (dummies)).Returns (dummyCards);

            var cards = cp.GetLiveCardsByEvent (VALID_GUID);

            Assert.That (cards, Is.Not.Null);
            Assert.That(cards, Is.EqualTo(dummyCards));
                
        }

        [Test]
        public void GetLiveCardsByEvent_InvalidEventId_ReturnsEmptyArray()
        {
            var cp = new CardProcessor (_container);
            var dummies = new Dictionary<int,DisplayUnit> ();
            var emptyArray = new RenderableCardDTO[]{};
            A.CallTo (() => _mgr.GetByEvent (VALID_GUID, true)).Returns (dummies);
            A.CallTo (() => _cardFactory.GetCards (dummies)).Returns (emptyArray);

            var cards = cp.GetLiveCardsByEvent (INVALID_GUID);

            Assert.That (cards, Is.Not.Null);
            Assert.That (cards.Length, Is.EqualTo (0));
        }

        [Test]
        public void GetLiveCardsByEvent_InvalidIdException_ReturnsEmptyArray()
        {
            var cp = new CardProcessor (_container);
            A.CallTo (() => _mgr.GetByEvent (VALID_GUID, true)).Throws<InvalidIdException> ();
            var cards = cp.GetLiveCardsByEvent (VALID_GUID);
            Assert.That (cards, Is.Not.Null);
            Assert.That (cards.Length, Is.EqualTo (0));
        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void GetLiveCardsByEvent_RepoException_Throws()
        {
            var cp = new CardProcessor (_container);
            A.CallTo (() => _mgr.GetByEvent (VALID_GUID, true)).Throws<RepositoryException> ();
            var cards = cp.GetLiveCardsByEvent (VALID_GUID);
        }

        [Test]
        public void GetCard_ValidDuId_RenderableCardDTO()
        {
            var cp = new CardProcessor (_container);
            var dummy = A.Dummy<DisplayUnit> ();
            var dummyCard = A.Dummy<RenderableCardDTO> ();

            A.CallTo (() => _mgr.GetById (VALID_GUID)).Returns (dummy);
            A.CallTo (() => _cardFactory.GetCard (dummy)).Returns (dummyCard);

            var card = cp.GetCard (VALID_GUID);

            Assert.That (card, Is.Not.Null);
            Assert.That(card, Is.EqualTo(dummyCard));
        }

        [Test]
        public void GetCard_InvalidDuId_ReturnsNull()
        {
            var cp = new CardProcessor (_container);

            A.CallTo (() => _mgr.GetById (INVALID_GUID)).Returns (null);

            var card = cp.GetCard (INVALID_GUID);

            Assert.That (card, Is.Null);
        }

        [Test]
        [ExpectedException]
        public void GetCard_RepoMgrThrowsException_Throws()
        {
            var cp = new CardProcessor (_container);
            A.CallTo (() => _mgr.GetById (VALID_GUID)).Throws<Exception> ();
            cp.GetCard (VALID_GUID);
        }

    }
}

