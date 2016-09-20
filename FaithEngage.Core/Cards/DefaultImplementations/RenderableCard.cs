using System;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Core.DisplayUnits;

namespace FaithEngage.Core.Cards.DefaultImplementations
{
	/// <summary>
    /// This is a default implementation of IRenderableCard. It is simple and
    /// only rerenders to itself. For simple cards, this is all that is necessary.
    /// </summary>
    public class RenderableCard : IRenderableCard
    {
        #region IRenderableCard implementation
        public IRenderableCardSection[] Sections {
            get;
            set;
        }
        public string Title {
            get;
            set;
        }
        public string Description {
            get;
            set;
        }
        public DisplayUnit OriginatingDisplayUnit {
            get;
            set;
        }
			
		virtual public IRenderableCard ReRender (CardActionResultArgs args)
		{
			return this;
		}
        #endregion
        
    }
}

