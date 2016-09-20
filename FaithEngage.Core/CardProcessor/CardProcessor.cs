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
    /// This is the high-level processor for dealing with cards. It is accessed
    /// by the app facade to push, pull, and get cards.
    /// </summary>
	public class CardProcessor : ICardProcessor
    {
        private readonly IDisplayUnitsRepoManager _duRepoMgr;
        private readonly ICardDTOFactory _cardFactory;
		private readonly ICardActionProcessor _cap;
        private readonly ITemplatingService _tempService;
        private readonly IPluginFileManager _plugFileManager;

        /// <summary>
        /// Fires when a new card is pushed out.
        /// </summary>
        public event PushPullEventHandler onPushCard;
        /// <summary>
        /// Fires when a card is pulled (revoked, deleted, etc...).
        /// </summary>
        public event PushPullEventHandler onPullCard;
        /// <summary>
        /// Fires when a card needs to be re-rendered.
        /// </summary>
        public event PushPullEventHandler onReRenderCard;

        public CardProcessor (IDisplayUnitsRepoManager duRepoMgr,
                              ICardDTOFactory cardFactory,
                              ICardActionProcessor cap,
                              ITemplatingService tempService,
                              IPluginFileManager plugFileMgr
                             )
        {
			//Assign dependencies
            _duRepoMgr = duRepoMgr;
			_cardFactory = cardFactory;
			_cap = cap;
            _tempService = tempService;
            _plugFileManager = plugFileMgr;
			//Subscribe to the CardActionProcessor's OnCardActionResult event.
            _cap.OnCardActionResult+= _cap_OnCardActionResult;

        }

        /// <summary>
        /// Called when the CardActionProcessor publishes it's OnCardActionResult
        /// event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
		void _cap_OnCardActionResult (DisplayUnit sender, CardActionResultArgs e)
		{
			//If there is no destination display unit set, do nothing.
            if (!e.DestinationDisplayUnit.HasValue)
				return;
			//If the sender's ID is the same as the destination displayUnit, use
            //the sender. Otherwise, obtain the destination display unit from the
            //repo. TODO: Consider caching display units associated with a given event while it is live.
            var du = (sender.Id == e.DestinationDisplayUnit) 
				? sender 
				: _duRepoMgr.GetById (e.DestinationDisplayUnit.Value);
			//If the display unit cannot be located, do nothing.
            if (du == null)
				return;
            //Get the id of the plugin of the DisplayUnit.
            var plugId = du.Plugin.PluginId;
            //Get the files associated with that plugin
            var files = _plugFileManager.GetFilesForPlugin (plugId.Value);
            //Get the card for that display unit.
            var card = du.GetCard (_tempService, files);
			//Rerender the card based upon the supplied event args
            var newCard = card.ReRender (e);
            //Check if there's actually a difference in old and new cards.
            if (card.GetHashCode () != newCard.GetHashCode ()) {
                //Convert the card to a dto
                var dto = _cardFactory.ConvertCard (newCard);
                if (dto == null)
                    return;
                //create card args with the deto
                var args = createCardEventArgs (dto);
                //Fire onReRenderCard with thos args.
                onReRenderCard (args);
            }

			
		}

        /// <summary>
        /// Gets the live cards for a given event.
        /// </summary>
        /// <returns>The live cards (if the event is located). Otherwise, empty array.</returns>
        /// <param name="eventId">Event identifier.</param>
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

        /// <summary>
        /// Obtains the card for the identified display Unit.
        /// </summary>
        /// <returns>The card.</returns>
        /// <param name="displayUnitId">Display unit identifier.</param>
        public RenderableCardDTO GetCard(Guid displayUnitId)
        {
            
                var du = _duRepoMgr.GetById (displayUnitId);
                if (du == null)
                    return null;
                return _cardFactory.GetCard (du);
        }

        /// <summary>
        /// Pushes out a card from the identified displayUnit.
        /// </summary>
        /// <param name="displayUnitId">Display unit identifier.</param>
        public void PushCard(Guid displayUnitId)
        {
            var du = _duRepoMgr.PushDU (displayUnitId);
            if (du == null)
                throw new InvalidIdException();
            var card = _cardFactory.GetCard (du);
            var args = createCardEventArgs (card);
            pushCard (args);
        }

        /// <summary>
        /// Pushes live a new card not already in the repository.
        /// </summary>
        /// <param name="newDto">New dto.</param>
        /// <param name="factory">Factory.</param>
		public void PushNewCard(DisplayUnitDTO newDto, IDisplayUnitFactory factory)
        {
            //Obtain the display unit from the factory
            var du = factory.Convert (newDto);
            if(du == null)
                throw new CouldNotConvertDTOException();
            //Save the dto to the event
            _duRepoMgr.SaveDtoToEvent (newDto);
            //Get the cardDTO from the display unit.
            var card = _cardFactory.GetCard (du);
            //create the card event args
            var args = createCardEventArgs (card);
            //Push the card out
            pushCard (args);
        }

        /// <summary>
        /// Pulls down a card that is currently live, removing it from view.
        /// </summary>
        /// <param name="displayUnitId">Display unit identifier.</param>
        public void PullCard(Guid displayUnitId)
        {
            var du = _duRepoMgr.PullDu (displayUnitId);
			if (du == null)
				throw new InvalidIdException ();
			var args = createCardEventArgs (du.AssociatedEvent, du.Id);
			pullCard (args);
        }

        /// <summary>
        /// Executes a card action asynchronously. 
        /// </summary>
        /// <returns>The card action task</returns>
        /// <param name="action">Action.</param>
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