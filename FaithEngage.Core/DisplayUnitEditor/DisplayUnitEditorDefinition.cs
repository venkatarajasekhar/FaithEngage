using System;
using FaithEngage.Core.Cards;

namespace FaithEngage.Core.DisplayUnitEditor
{
    public class DisplayUnitEditorDefinition
    {
                /// <summary>
        /// True if card sections must be in ordered defined. False if they can be in any order.
        /// </summary>
        /// <value><c>true</c> if enforce section order; otherwise, <c>false</c>.</value>
        public bool EnforceSectionOrder {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the card section definitions. If Section order is enforced,
        /// the sections will always appear in the order of this array. Otherwise, they may
        /// be rearranged, though the default order will be as in the array.
        /// </summary>
        /// <value>The card section definitions.</value>
        public CardSectionDefinition[] CardSectionDefinitions {
            get;
            set;
        }

        public DisplayUnitEditorDefinition ()
        {
            EnforceSectionOrder = false;
        }


    }
}

