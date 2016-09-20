using System;
using FaithEngage.Core.Cards;

namespace FaithEngage.Core.CardProcessor
{
    /// <summary>
    /// These are used by the PushPullEventHandler delegate.
    /// </summary>
    public class CardEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the id of the event to which this card belongs.
        /// </summary>
        /// <value>The event identifier.</value>
        public Guid EventId {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the id of the display unit to which this card belongs.
        /// </summary>
        /// <value>The display unit identifier.</value>
        public Guid DisplayUnitId {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the RenderableCardDTO that is the representation of this
        /// card.
        /// </summary>
        /// <value>The card.</value>
        public RenderableCardDTO card{get;set;}
    }
}

