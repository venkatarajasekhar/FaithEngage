using System;
using System.Collections.Generic;
using FaithEngage.Core.Cards;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FakeItEasy;
using NUnit.Framework;
using FaithEngage.Core.CardProcessor;

namespace FaithEngage.Core.ActionProcessors
{
	[TestFixture]
	public class CardActionProcessorTests
    {
        IDisplayUnitsRepoManager _repo;
		public Guid VALID_GUID = Guid.NewGuid();
		
        [SetUp]
		public void init()
		{
			_repo = A.Fake<IDisplayUnitsRepoManager>();
		}

		[Test]
		public void ExecuteCardAction_ValidAction_TriggersEvent_ExectutesAction()
		{
			var action = new CardAction();
			action.OriginatingDisplayUnit = VALID_GUID;
			CardAction receivedAction = null;
			DisplayUnit receivedDu = null;
			CardActionResultArgs receivedArgs = null;
			CardActionResultArgs sentArgs = new CardActionResultArgs();
			sentArgs.Responses = new Dictionary<string, string>()
			{
				{"Test1", "Hi!"}
			};
			var du = A.Fake<DisplayUnit>();
			A.CallTo(() => _repo.GetById(VALID_GUID)).Returns(du);
			A.CallTo(() => du.ExecuteCardAction(A<CardAction>.Ignored))
			 .Invokes((CardAction a) => receivedAction = a);

			var cap = new CardActionProcessor(_repo);
			cap.OnCardActionResult += (sender, e) => { receivedDu = sender; receivedArgs = e; };
			cap.ExecuteCardAction(action);
			du.OnCardActionResult += Raise.With<CardActionResultEventHandler>(du, sentArgs);
			Assert.That(receivedAction, Is.EqualTo(action));
			Assert.That(receivedDu, Is.EqualTo(du));
			Assert.That(receivedArgs, Is.EqualTo(sentArgs));
		}

        [Test]
        public void ExecuteCardAction_EncountersException_ThrowsException_NoEvent()
        {
            
        }
	}
}