using System;
using FaithEngage.Core.Cards.Interfaces;

namespace FaithEngage.Plugins
{
    public class TextCardSection : IRenderableCardSection
    {
        #region IRenderableCardSection implementation
        public IRenderableCardSection[] Subsections {
            get;
            set;
        }
        public string HeadingText {
            get {
                throw new NotImplementedException ();
            }
            set {
                throw new NotImplementedException ();
            }
        }
        public string HtmlContents {
            get {
                throw new NotImplementedException ();
            }
            set {
                throw new NotImplementedException ();
            }
        }
        #endregion
        
    }
}

