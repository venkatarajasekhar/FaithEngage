using FaithEngage.Core.Cards;
using System.Threading.Tasks;

namespace FaithEngage.Core.ActionProcessors.Interfaces
{
	public interface ICardActionProcessor
	{
		event CardActionResultEventHandler OnCardActionResult;
		void ExecuteCardAction(CardAction action);
	}
}