using System;
using FaithEngage.Core.Cards;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.DisplayUnits.Interfaces;
using System.Threading.Tasks;

namespace FaithEngage.Core.CardProcessor
{
	/// <summary>
    /// This is the high-level processor for dealing with cards. It is accessed
    /// by the app facade to push, pull, and get cards.
    /// </summary>
    public interface ICardProcessor
	{
		/// <summary>
        /// Fires when a new card is pushed out.
        /// </summary>
        event PushPullEventHandler onPushCard;
        /// <summary>
        /// Fires when a card is pulled (revoked, deleted, etc...).
        /// </summary>
		event PushPullEventHandler onPullCard;
        /// <summary>
        /// Fires when a card needs to be re-rendered.
        /// </summary>
		event PushPullEventHandler onReRenderCard;
        /// <summary>
        /// Gets the live cards for a given event.
        /// </summary>
        /// <returns>The live cards (if the event is located). Otherwise, empty array.</returns>
        /// <param name="eventId">Event identifier.</param>
		RenderableCardDTO[] GetLiveCardsByEvent (Guid eventId);
        /// <summary>
        /// Obtains the card for the identified display Unit.
        /// </summary>
        /// <returns>The card.</returns>
        /// <param name="displayUnitId">Display unit identifier.</param>
		RenderableCardDTO GetCard (Guid displayUnitId);
        /// <summary>
        /// Pushes out a card from the identified displayUnit.
        /// </summary>
        /// <param name="displayUnitId">Display unit identifier.</param>
        void PushCard (Guid displayUnitId);
        /// <summary>
        /// Pushes live a new card not already in the repository.
        /// </summary>
        /// <param name="newDto">New dto.</param>
        /// <param name="factory">Factory.</param>
		void PushNewCard (DisplayUnitDTO newDto, IDisplayUnitFactory factory);
		/// <summary>
        /// Pulls down a card that is currently live, removing it from view.
        /// </summary>
        /// <param name="displayUnitId">Display unit identifier.</param>
        void PullCard (Guid displayUnitId);
		/// <summary>
        /// Executes a card action asynchronously. 
        /// </summary>
        /// <returns>The card action task</returns>
        /// <param name="action">Action.</param>
        Task ExecuteCardActionAsync (CardAction action);
	}
}

