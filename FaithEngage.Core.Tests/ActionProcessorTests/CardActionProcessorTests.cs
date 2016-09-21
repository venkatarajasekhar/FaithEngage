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
using FaithEngage.Core.PluginManagers.Files;
using FaithEngage.Core.TemplatingService;

namespace FaithEngage.Core.ActionProcessors
{
	[TestFixture]
	public class CardActionProcessorTests
    {
        IDisplayUnitsRepoManager _repo;
		public Guid VALID_GUID = Guid.NewGuid();

		class dummyDu : DisplayUnit
		{
			CardActionProcessorTests _outer;

			public dummyDu(CardActionProcessorTests outer) : base(outer.VALID_GUID,new Dictionary<string, string>())
			{
				_outer = outer;
			}

			public override DisplayUnitPlugin Plugin {
				get {
					throw new NotImplementedException ();
				}
			}

			public override Dictionary<string, string> GetAttributes ()
			{
				throw new NotImplementedException ();
			}

			public override IRenderableCard GetCard (ITemplatingService service, IDictionary<Guid, PluginFileInfo> files)
			{
				throw new NotImplementedException ();
			}

			public override void SetAttributes (Dictionary<string, string> attributes)
			{
			}
			public override void ExecuteCardAction (CardAction Action)
			{
				this.Name = Action.Parameters ["name"];
				OnCardActionResult (
					this,
					new CardActionResultArgs {
						DestinationDisplayUnit = this.Id,
						Responses = new Dictionary<string, string> ()
						{
							{"Test1", "Hi!"}

						}
					}
				);
			}
			public override event CardActionResultEventHandler OnCardActionResult;

		}



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
			Exception thrownEx = null;
			var action = new CardAction();
			action.Parameters ["name"] = "Blue!";
			A.CallTo(() => _repo.SaveOneToEvent(A<DisplayUnit>.Ignored)).Throws<Exception>();
			var du = new dummyDu (this);
			A.CallTo (() => _repo.GetById (A<Guid>.Ignored)).Returns (du);
			DisplayUnit receivedDu = null;
			CardActionResultArgs receivedArgs = null;
			CardActionResultArgs sentArgs = new CardActionResultArgs();
			sentArgs.Responses = new Dictionary<string, string>()
			{
				{"Test1", "Hi!"}
			};
			sentArgs.DestinationDisplayUnit = du.Id;

			var cap = new CardActionProcessor(_repo);
			cap.OnCardActionResult += (sender, e) => { receivedDu = sender; receivedArgs = e; };
			try
			{
				cap.ExecuteCardAction(action);
			}
			catch (Exception ex)
			{
				thrownEx = ex;
			}

			Assert.That(thrownEx, Is.Not.Null);
        }
	}
}