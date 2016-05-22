using System;
using FaithEngage.Core.Cards.Interfaces;
using System.Collections.Generic;

namespace FaithEngage.Core.DisplayUnitEditor
{
    public class CardSectionDefinition
    {
        public CardSectionDefinition (string sectionName, EditorFieldType fieldType, int max = 0, int minimum = 0)
        {
            SectionName = sectionName;
            EditorFieldType = fieldType;
            Limit = max;
            NumberRequired = minimum;
            HasLimit = (Limit > 0) ? true : false;
            HasMinimum = (NumberRequired > 0) ? true : false;

        }

        public EditorFieldType EditorFieldType { get; set;}

        public bool HasLimit { get; set;}

        public bool HasMinimum { get; set;}

        public int Limit{ get; set;}

        public string SectionName{get;set;}
        public int NumberRequired{ get; set;}


        //public ICardEditorAction Action{ get; set;}

        /// <summary>
        /// Gets or sets the options. These are used differently, depending upon the
        /// field type. Ex. The different check boxes available, or radio buttons.
        /// This is not used by every field type.
        /// </summary>
        /// <value>The options.</value>
        public string[] Options { get; set;}
    }
}

