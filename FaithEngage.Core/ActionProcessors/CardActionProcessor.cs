using System;
using System.Linq;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Cards;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.ActionProcessors.Interfaces;
using System.Collections.Generic;

namespace FaithEngage.Core.ActionProcessors
{
	/// <summary>
	/// Executes card actions on display units and their responses.
	/// </summary>
	public class CardActionProcessor : ICardActionProcessor
	{
		private readonly IDisplayUnitsRepoManager _repo;
		//Keep references to all display units whose actions have been processed
		//TODO: Consider caching these so they can be cleared on a revolving basis
		private List<DisplayUnit> _awaitingResponse = new List<DisplayUnit> ();
		public CardActionProcessor (IDisplayUnitsRepoManager repo)
		{
			_repo = repo;
		}

		/// <summary>
		/// Processes the CardAction on the associated DisplayUnit.
		/// </summary>
		/// <param name="action">Action.</param>
		public void ExecuteCardAction(CardAction action)
		{
			DisplayUnit du = null;
			try
			{
				//Grab the display unit from the repo
				du = _repo.GetById(action.OriginatingDisplayUnit);
				//Subscribe to the onCardActionResult event on it
				du.OnCardActionResult += Du_OnCardActionResult;
				//Add it to the list to hold on to the reference
				_awaitingResponse.Add(du);
				//Execute the card action on the display unit
				var preHash = du.GetHashCode ();
				du.ExecuteCardAction(action);
				var postHash = du.GetHashCode ();

				if (preHash != postHash) {
					_repo.SaveOneToEvent (du);
					//TODO: Add event notification for changed DU
				}	
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

