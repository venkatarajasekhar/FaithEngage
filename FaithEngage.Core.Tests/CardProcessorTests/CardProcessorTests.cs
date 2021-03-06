﻿using System;
using NUnit.Framework;
using FakeItEasy;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.DisplayUnits;
using System.Linq;
using FaithEngage.Core.Cards;
using System.Collections.Generic;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Core.ActionProcessors.Interfaces;
using FaithEngage.Core.TemplatingService;
using FaithEngage.Core.PluginManagers.Files.Interfaces;

namespace FaithEngage.Core.CardProcessor
{
    [TestFixture]
    public class CardProcessorTests
    {
        private IDisplayUnitsRepoManager _mgr;
        private ICardDTOFactory _cardFactory;
		private ICardActionProcessor _cap;
        private ITemplatingService _tempService;
        private IPluginFileManager _fileMgr;
        private Guid VALID_GUID = Guid.NewGuid();
        private Guid INVALID_GUID = Guid.NewGuid ();


        [SetUp]
        public void init()
        {
            _mgr = A.Fake<IDisplayUnitsRepoManager> ();
            _cardFactory = A.Fake<ICardDTOFactory> ();
			_cap = A.Fake<ICardActionProcessor>();
            _tempService = A.Fake<ITemplatingService> ();
            _fileMgr = A.Fake<IPluginFileManager> ();
        }

        [Test]
        public void GetLiveCardsByEvent_ValidEventId_ArrayOfRenderableCardDTOs()
        {
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr );
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
            var cp = new CardProcessor (_mgr, _cardFactory, _cap,_tempService, _fileMgr);
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
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);
            A.CallTo (() => _mgr.GetByEvent (VALID_GUID, true)).Throws<InvalidIdException> ();
            var cards = cp.GetLiveCardsByEvent (VALID_GUID);
            Assert.That (cards, Is.Not.Null);
            Assert.That (cards.Length, Is.EqualTo (0));
        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void GetLiveCardsByEvent_RepoException_Throws()
        {
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);
            A.CallTo (() => _mgr.GetByEvent (VALID_GUID, true)).Throws<RepositoryException> ();
            var cards = cp.GetLiveCardsByEvent (VALID_GUID);
        }

        [Test]
        public void GetCard_ValidDuId_RenderableCardDTO()
        {
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);
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
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);

            A.CallTo (() => _mgr.GetById (INVALID_GUID)).Returns (null);

            var card = cp.GetCard (INVALID_GUID);

            Assert.That (card, Is.Null);
        }

        [Test]
        [ExpectedException]
        public void GetCard_RepoMgrThrowsException_Throws()
        {
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);
            A.CallTo (() => _mgr.GetById (VALID_GUID)).Throws<Exception> ();
            cp.GetCard (VALID_GUID);
        }

        [Test]
        public void PushCard_ValidDisplayUnitId_PushesCard()
        {
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);
            var du = A.Dummy<DisplayUnit> ();
            du.AssociatedEvent = VALID_GUID;
            A.CallTo (() => _mgr.PushDU (VALID_GUID)).Returns (du);
            var card = new RenderableCardDTO ();
            card.AssociatedEvent = VALID_GUID;
            card.OriginatingDisplayUnit = VALID_GUID;
            card.Description = "My Description";
                
            A.CallTo (() => _cardFactory.GetCard (du)).Returns (card);
            CardEventArgs args = null;
            cp.onPushCard += (x) => args = x;

            cp.PushCard (VALID_GUID);

            A.CallTo (() => _mgr.PushDU (VALID_GUID)).MustHaveHappened ();
            Assert.That (args, Is.Not.Null);
            Assert.That (args.EventId, Is.EqualTo (VALID_GUID));
            Assert.That (args.DisplayUnitId, Is.EqualTo (VALID_GUID));
            Assert.That (args.card, Is.EqualTo (card));
        }

        [Test]
        [ExpectedException(typeof(InvalidIdException))]
        public void PushCard_InvalidDisplayUnitId_ThrowsInvalidIdException()
        {
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);
            A.CallTo (() => _mgr.PushDU (INVALID_GUID)).Returns (null);

            cp.PushCard (INVALID_GUID);
        }

        [Test]
        [ExpectedException]
        public void PushCard_ExceptionThrown_NoAction()
        {
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);
            A.CallTo (() => _mgr.PushDU (VALID_GUID)).Throws<Exception>();

            CardEventArgs args = null;
            cp.onPushCard += (x) => args = x;
            cp.PushCard (VALID_GUID);

            Assert.That (args, Is.Null);
        }

        [Test]
        public void PushNewCard_ValidDTO_PushesCard()
        {
			var factory = A.Fake<IDisplayUnitFactory> ();
			var card = A.Dummy<RenderableCardDTO> ();
            A.CallTo (() => _cardFactory.GetCard (A<DisplayUnit>.Ignored)).Returns (card);
            CardEventArgs args = null;
            var dto = new DisplayUnitDTO (VALID_GUID, VALID_GUID);
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);
            cp.onPushCard += (x) => args = x;
			cp.PushNewCard (dto, factory);

            A.CallTo (() => _mgr.SaveDtoToEvent (dto)).MustHaveHappened ();
            Assert.That (args, Is.Not.Null);
            Assert.That (args.card, Is.EqualTo (card));
        }

        [Test]
        [ExpectedException(typeof(CouldNotConvertDTOException))]
        public void PushNewCard_InvalidDTO_CannotConvert_ThrowsException()
        {
            var factory = A.Fake<IDisplayUnitFactory> ();
            A.CallTo (() => factory.Convert (A<DisplayUnitDTO>.Ignored)).Returns (null);
            var dto = new DisplayUnitDTO (INVALID_GUID, INVALID_GUID);
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);
			cp.PushNewCard (dto, factory);
        }

        [Test]
        [ExpectedException]
        public void PushNewCard_EncountersException_BubblesUp()
        {
			var factory = A.Fake<IDisplayUnitFactory> ();
			A.CallTo (() => _mgr.SaveDtoToEvent (A<DisplayUnitDTO>.Ignored)).Throws<Exception> ();
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);
            cp.PushNewCard (A.Dummy<DisplayUnitDTO> (), factory);
        }

        [Test]
        public void PullCard_ValidId_PullsCard()
        {
            var du = A.Fake<DisplayUnit>();
            du.AssociatedEvent = VALID_GUID;
            A.CallTo (() => _mgr.PullDu (VALID_GUID)).Returns (du);
            CardEventArgs args = null;

			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);
            cp.onPullCard += (e) => args = e;
            cp.PullCard (VALID_GUID);

            A.CallTo (() => _mgr.PullDu (VALID_GUID)).MustHaveHappened ();
            Assert.That (args, Is.Not.Null);
            Assert.That (args.EventId, Is.EqualTo(VALID_GUID));
			Assert.That(args.DisplayUnitId, Is.Not.Null);
			Assert.That(args.DisplayUnitId, Is.Not.EqualTo(Guid.Empty));
        }

		[Test]
		[ExpectedException(typeof(InvalidIdException))]
		public void PullCard_InvalidId_ThrowsInvalidIdException()
		{
			A.CallTo (() => _mgr.PullDu (INVALID_GUID)).Returns (null);
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);
			cp.PullCard (INVALID_GUID);
		}

		[Test]
		[ExpectedException]
		public void PullCard_EncountersException_BubblesUp()
		{
			A.CallTo (() => _mgr.PullDu (VALID_GUID)).Throws<Exception> ();
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);
			cp.PullCard (VALID_GUID);
		}

		[Test]
		public void ExecuteCardActionAsync_ExcecutesCorrectly()
		{
			var cp = new CardProcessor (_mgr, _cardFactory, _cap, _tempService, _fileMgr);
			var action = new CardAction ();
			CardAction receivedAction = null;
			A.CallTo (() => _cap.ExecuteCardAction (action)).Invokes ((CardAction a) => receivedAction = a);
			var task = cp.ExecuteCardActionAsync (action);
            task.Wait ();
            A.CallTo (() => _cap.ExecuteCardAction (action)).MustHaveHappened ();
			Assert.That (receivedAction, Is.EqualTo (action));
		}

    }
}

