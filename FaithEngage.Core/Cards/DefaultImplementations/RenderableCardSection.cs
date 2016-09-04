using System;
using System.Collections.Generic;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Core.PluginManagers.Files;
using FaithEngage.Core.TemplatingService;

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

