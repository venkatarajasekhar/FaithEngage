using System;
using FaithEngage.Core.Cards.Interfaces;

namespace FaithEngage.Core.Cards.DefaultImplementations
{
    public class RenderableCardSection : IRenderableCardSection
    {
        #region IRenderableCardSection implementation

        public string HeadingText {
            get;
            set;
        }
        public string HtmlContents {
            get;
            set;
        }
        #endregion
        
    }
}

