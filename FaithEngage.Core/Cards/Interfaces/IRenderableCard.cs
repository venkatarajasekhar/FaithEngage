using FaithEngage.Core.DisplayUnits;

namespace FaithEngage.Core.Cards.Interfaces
{
    /// <summary>
    /// Provides the information on what to display. Produced by a DisplayUnit.
    /// </summary>
	public interface IRenderableCard
    {
        /// <summary>
        /// An ordered array of IRenderableCardSection that contains the content of the Card.
        /// </summary>
        /// <value>The sections.</value>
		IRenderableCardSection[] Sections{get;set;}
        string Title{ get; set;}
        string Description{get;set;}
        /// <summary>
        /// Gets or sets the Display Unit that produced this card.
        /// </summary>
        /// <value>The originating display unit.</value>
		DisplayUnit OriginatingDisplayUnit{ get; set;}
		/// <summary>
		/// If the card needs to be ReRendered based upon some card action result from the
		/// display unit, this will transform the card.
		/// </summary>
		/// <returns>The render.</returns>
		/// <param name="args">Arguments.</param>
		IRenderableCard ReRender (CardActionResultArgs args);
    }
}

