using System;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Cards;
using FaithEngage.Core.DisplayUnits;

namespace FaithEngage.Core.ActionProcessors
{
	public class CardActionProcessor
	{
		private readonly IDisplayUnitsRepoManager _repo;
		public CardActionProcessor (IDisplayUnitsRepoManager repo)
		{
			_repo = repo;
		}

		public void ExecuteCardAction(CardAction action)
		{
			var du = _repo.GetById (action.OriginatingDisplayUnit);
			du.OnCardActionResult += Du_OnCardActionResult;
			du.ExecuteCardAction (action);
		}

		void Du_OnCardActionResult (DisplayUnit sender, CardActionResultArgs e)
		{
			OnCardActionResult (sender, e);
		}

		public event CardActionResultEventHandler OnCardActionResult;

	}
}

