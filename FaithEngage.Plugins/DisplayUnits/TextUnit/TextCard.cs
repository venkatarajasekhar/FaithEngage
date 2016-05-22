using System;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Core.DisplayUnits;

namespace FaithEngage.Plugins.DisplayUnits.TextUnitPlugin
{
    public class TextCard : IRenderableCard
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
        #endregion
    }
}

