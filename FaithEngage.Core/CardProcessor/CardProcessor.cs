using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Cards;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.CardProcessor
{
    /// <summary>
    /// This is the high-level processor for dealing with cards. It will be accessed
    /// by the app facade to push, pull, and get cards.
    /// 
    /// Dependencies (injected through the IContainer): IDisplayUnitsRepoManager
    /// </summary>
    public class CardProcessor
    {
        private readonly IContainer _container;
        private readonly IDisplayUnitsRepoManager _duRepoMgr;
        private readonly ICardDTOFactory _cardFactory;

        public event PushPullEventHandler onPushCard;
        public event PushPullEventHandler onPullCard;

        public CardProcessor (IContainer container)
        {
            _container = container;
            _duRepoMgr = _container.Resolve<IDisplayUnitsRepoManager> ();
            _cardFactory = _container.Resolve<ICardDTOFactory> ();
        }

        public RenderableCardDTO[] GetLiveCardsByEvent(Guid eventId)
        {
            try {
                var dus = _duRepoMgr.GetByEvent (eventId, true);
                return  _cardFactory.GetCards (dus);
            } catch (InvalidIdException) {
                return new RenderableCardDTO[]{};
            } catch (Exception) {
                throw;
            }

        }

        public RenderableCardDTO GetCard(Guid displayUnitId)
        {
            try {
                var du = _duRepoMgr.GetById (displayUnitId);
                if (du == null)
                    return null;
                return _cardFactory.GetCard (du);
            } catch (Exception) {
                throw;
            }

        }

        public void PushCard(Guid displayUnitId)
        {
            var du = _duRepoMgr.PushDU (displayUnitId);
            if (du == null)
                return;
            var card = _cardFactory.GetCard (du);
            var args = createCardEventArgs (card);
			pushCard (args);
        }

        public void PushNewCard(DisplayUnitDTO newDto)
        {
            var factory = _container.Resolve<IDisplayUnitFactory> ();
            var du = factory.ConvertFromDto (newDto);
            _duRepoMgr.SaveDtoToEvent (newDto);
            var card = _cardFactory.GetCard (du);
            var args = createCardEventArgs (card);
			pushCard (args);
        }

        public void PullCard(Guid displayUnitId)
        {
            var du = _duRepoMgr.PullDu (displayUnitId);
            var args = createCardEventArgs (du.AssociatedEvent, du.Id);
			pullCard (args);
        }

        private CardEventArgs createCardEventArgs (RenderableCardDTO card){
            var args = createCardEventArgs (card.AssociatedEvent, card.OriginatingDisplayUnit);
            args.card = card;
            return args;
        }
        private CardEventArgs createCardEventArgs(Guid eventId, Guid displayUnitId)
        {
            return new CardEventArgs () {
                EventId = eventId,
                DisplayUnitId = displayUnitId
            };
        }
		private void pushCard(CardEventArgs args)
		{
			if (onPushCard != null)
			{
				onPushCard (args);
			}   
		}
		private void pullCard(CardEventArgs args)
		{
			if(onPullCard != null)
			{
				onPullCard(args);
			}	
		}

    }
}

