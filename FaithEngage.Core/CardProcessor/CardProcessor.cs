using System;
using FaithEngage.Core.Cards;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Core.ActionProcessors.Interfaces;
using System.Threading.Tasks;
using FaithEngage.Core.TemplatingService;
using FaithEngage.Core.PluginManagers.Files.Interfaces;
using System.Linq;

namespace FaithEngage.Core.CardProcessor
{
    /// <summary>
    /// This is the high-level processor for dealing with cards. It will be accessed
    /// by the app facade to push, pull, and get cards.
    /// 
    /// Dependencies (injected through the IContainer): IDisplayUnitsRepoManager
    /// </summary>
	public class CardProcessor : ICardProcessor
    {
        private readonly IDisplayUnitsRepoManager _duRepoMgr;
        private readonly ICardDTOFactory _cardFactory;
		private readonly ICardActionProcessor _cap;
        private readonly ITemplatingService _tempService;
        private readonly IPluginFileManager _plugFileManager;

        public event PushPullEventHandler onPushCard;
        public event PushPullEventHandler onPullCard;
        public event PushPullEventHandler onReRenderCard;

        public CardProcessor (IDisplayUnitsRepoManager duRepoMgr,
                              ICardDTOFactory cardFactory,
                              ICardActionProcessor cap,
                              ITemplatingService tempService,
                              IPluginFileManager plugFileMgr
                             )
        {
			_duRepoMgr = duRepoMgr;
			_cardFactory = cardFactory;
			_cap = cap;
			_cap.OnCardActionResult+= _cap_OnCardActionResult;
            _tempService = tempService;
            _plugFileManager = plugFileMgr;
        }

		void _cap_OnCardActionResult (DisplayUnit sender, CardActionResultArgs e)
		{
			if (!e.DestinationDisplayUnit.HasValue)
				return;
			var du = (sender.Id == e.DestinationDisplayUnit) 
				? sender 
				: _duRepoMgr.GetById (e.DestinationDisplayUnit.Value);
			if (du == null)
				return;
            var plugId = du.Plugin.PluginId;
            var files = _plugFileManager.GetFilesForPlugin (plugId.Value);
            var card = du.GetCard (_tempService, files);
			var newCard = card.ReRender (e);
			var dto = _cardFactory.ConvertCard (newCard);
			if (dto == null)
				return;
			var args = createCardEventArgs (dto);
			onReRenderCard (args);
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
            
                var du = _duRepoMgr.GetById (displayUnitId);
                if (du == null)
                    return null;
                return _cardFactory.GetCard (du);
        }

        public void PushCard(Guid displayUnitId)
        {
            var du = _duRepoMgr.PushDU (displayUnitId);
            if (du == null)
                throw new InvalidIdException();
            var card = _cardFactory.GetCard (du);
            var args = createCardEventArgs (card);
            pushCard (args);
        }

		public void PushNewCard(DisplayUnitDTO newDto, IDisplayUnitFactory factory)
        {
            var du = factory.Convert (newDto);
            if(du == null)
                throw new CouldNotConvertDTOException();
            _duRepoMgr.SaveDtoToEvent (newDto);
            var card = _cardFactory.GetCard (du);
            var args = createCardEventArgs (card);
            pushCard (args);
        }

        public void PullCard(Guid displayUnitId)
        {
            var du = _duRepoMgr.PullDu (displayUnitId);
			if (du == null)
				throw new InvalidIdException ();
			var args = createCardEventArgs (du.AssociatedEvent, du.Id);
			pullCard (args);
        }

		public async Task ExecuteCardActionAsync(CardAction action)
		{
			await Task.Run(()=>_cap.ExecuteCardAction (action));
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

