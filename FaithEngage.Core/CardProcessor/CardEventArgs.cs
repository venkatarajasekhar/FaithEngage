using System;
using FaithEngage.Core.Cards;

namespace FaithEngage.Core.CardProcessor
{
    public class CardEventArgs : EventArgs
    {
        public Guid EventId {
            get;
            set;
        }
        public Guid DisplayUnitId {
            get;
            set;
        }

        public RenderableCardDTO card{get;set;}
    }
}

