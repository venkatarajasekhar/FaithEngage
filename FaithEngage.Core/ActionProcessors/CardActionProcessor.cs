using System;
using System.Linq;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Cards;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.ActionProcessors.Interfaces;
using System.Collections.Generic;

namespace FaithEngage.Core.ActionProcessors
{
	public class CardActionProcessor : ICardActionProcessor
	{
		private readonly IDisplayUnitsRepoManager _repo;
		private List<DisplayUnit> _awaitingResponse = new List<DisplayUnit> ();
		public CardActionProcessor (IDisplayUnitsRepoManager repo)
		{
			_repo = repo;
		}

		public void ExecuteCardAction(CardAction action)
		{
			DisplayUnit du = null;
			try
			{
				du = _repo.GetById(action.OriginatingDisplayUnit);
				du.OnCardActionResult += Du_OnCardActionResult;
				_awaitingResponse.Add(du);
				du.ExecuteCardAction(action);
				_repo.SaveOneToEvent(du);
			}
			catch (Exception ex)
			{
				if (du != null)
				{
					du.OnCardActionResult -= Du_OnCardActionResult;
					flushDisplayUnit(du);
				}
				throw ex;
			}
		}

		void Du_OnCardActionResult (DisplayUnit sender, CardActionResultArgs e)
		{
			flushDisplayUnit(sender);
			OnCardActionResult?.Invoke(sender, e);
		}

		void flushDisplayUnit(DisplayUnit unit)
		{
			_awaitingResponse = _awaitingResponse.Where(x => x.Id != unit.Id).ToList();
		}

		public event CardActionResultEventHandler OnCardActionResult;

	}
}

