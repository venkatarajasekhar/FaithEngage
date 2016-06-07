using System;
using FaithEngage.Core.Cards;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.DisplayUnits.Interfaces;
using System.Threading.Tasks;

namespace FaithEngage.Core.CardProcessor
{
	public interface ICardProcessor
	{
		event PushPullEventHandler onPushCard;
		event PushPullEventHandler onPullCard;
		event PushPullEventHandler onReRenderCard;

		RenderableCardDTO[] GetLiveCardsByEvent (Guid eventId);

		RenderableCardDTO GetCard (Guid displayUnitId);

		void PushCard (Guid displayUnitId);

		void PushNewCard (DisplayUnitDTO newDto, IDisplayUnitFactory factory);
		void PullCard (Guid displayUnitId);
		void ExecuteCardActionAsync (CardAction action);
	}
}

