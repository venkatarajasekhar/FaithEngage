using FaithEngage.Core.Cards;
using System.Threading.Tasks;

namespace FaithEngage.Core.ActionProcessors.Interfaces
{
	/// <summary>
	/// The card action processor will receive a card action and then ensure
	/// that it is carried out to completion on the proper display unit. This
	/// allows changes in cards to (potentially) be enacted for all viewers.
	/// </summary>
	public interface ICardActionProcessor
	{
		/// <summary>
		/// Potentially triggered when the card action has a result.
		/// </summary>
		event CardActionResultEventHandler OnCardActionResult;
		/// <summary>
		/// Executes the card action.
		/// </summary>
		/// <param name="action">Action.</param>
		void ExecuteCardAction(CardAction action);
	}
}