using System;
using System.Linq;
using FaithEngage.Core.Cards;
using System.Collections.Generic;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.Cards.Interfaces;
using System.IO;
using System.CodeDom;

namespace FaithEngage.Core.Cards
{
    public class CardDtoFactory : ICardDTOFactory
    {
        public RenderableCardDTO[] GetCards(Dictionary<int,DisplayUnit> units)
        {
            return units
                .OrderBy (p => p.Key)
                .Select (v => convert(v.Value.GetCard ()))
                .ToArray();
        }

        public RenderableCardDTO GetCard(DisplayUnit unit)
        {
            return convert (unit.GetCard());
        }

        private RenderableCardDTO convert(IRenderableCard card)
        {
            var dto = new RenderableCardDTO ();
            dto.Title = card.Title;
            dto.Description = card.Description;
            dto.OriginatingDisplayUnit = card.OriginatingDisplayUnit.Id;
            dto.PositionInEvent = card.OriginatingDisplayUnit.PositionInEvent;
            dto.AssociatedEvent = card.OriginatingDisplayUnit.AssociatedEvent;
            var sections = new List<RenderableCardSectionDTO> ();
            foreach(var sec in card.Sections)
            {
                var secDto = new RenderableCardSectionDTO ();
                secDto.HeadingText = sec.HeadingText;
                secDto.HtmlContents = sec.HtmlContents;
                sections.Add (secDto);
            }
            dto.Sections = sections.ToArray ();
            return dto;
        }
    }
}

