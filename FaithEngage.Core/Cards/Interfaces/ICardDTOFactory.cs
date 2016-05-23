using System;
using FaithEngage.Core.Cards;
using System.Collections.Generic;
using FaithEngage.Core.DisplayUnits;

namespace FaithEngage.Core
{
    public interface ICardDTOFactory
    {
        RenderableCardDTO[] GetCards(Dictionary<int,DisplayUnit> units);
        RenderableCardDTO GetCard (DisplayUnit unit);
    }
}

