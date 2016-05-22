using System;

namespace FaithEngage.Core.Cards
{
    public class RenderableCardDTO
    {
        public RenderableCardSectionDTO[] Sections{get;set;}
        public string Title{get;set;}
        public string Description{get;set;}
        public Guid OriginatingDisplayUnit{get;set;}
        public int PositionInEvent{ get; set;}
        public Guid AssociatedEvent{get;set;}
    }
}

