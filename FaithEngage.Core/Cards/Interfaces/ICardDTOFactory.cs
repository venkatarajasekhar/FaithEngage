using System;
using FaithEngage.Core.Cards;
using System.Collections.Generic;
using FaithEngage.Core.DisplayUnits;

namespace FaithEngage.Core.Cards.Interfaces
{
    public interface ICardDTOFactory
    {
        RenderableCardDTO[] GetCards(Dictionary<int,DisplayUnit> units);
        RenderableCardDTO GetCard (DisplayUnit unit);
		RenderableCardDTO ConvertCard(IRenderableCard card);
    }
}

