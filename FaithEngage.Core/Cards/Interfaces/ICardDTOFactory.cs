using System;
using FaithEngage.Core.Cards;
using System.Collections.Generic;
using FaithEngage.Core.DisplayUnits;

namespace FaithEngage.Core.Cards.Interfaces
{
    /// <summary>
    /// Obtains RenderableCardDTOs from varous sources.
    /// </summary>
    public interface ICardDTOFactory
    {
        /// <summary>
        /// Obtains an array of RenderableCrdDTOs from a dictionary of display
        /// units, ordered by the integer key used.
        /// </summary>
        /// <returns>The cards.</returns>
        /// <param name="units">A dictionary of DisplayUnits with integer keys indicating
        /// their positions in the event.</param>
        RenderableCardDTO[] GetCards(Dictionary<int,DisplayUnit> units);
        /// <summary>
        /// Obtains a single card from a DisplayUnit.
        /// </summary>
        /// <returns>The card.</returns>
        /// <param name="unit">Unit.</param>
        RenderableCardDTO GetCard (DisplayUnit unit);
		/// <summary>
        /// Converts an IRenderableCard to a RenderableCardDTO.
        /// </summary>
        /// <returns>The card.</returns>
        /// <param name="card">Card.</param>
        RenderableCardDTO ConvertCard(IRenderableCard card);
    }
}

