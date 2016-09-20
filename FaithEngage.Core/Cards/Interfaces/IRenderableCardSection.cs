using System;
using FaithEngage.Core.TemplatingService;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers.Files;

namespace FaithEngage.Core.Cards.Interfaces
{
    /// <summary>
    /// A subsection of a renderable card.
    /// </summary>
    public interface IRenderableCardSection
    {
        string HeadingText{get;set;}
        string HtmlContents { get; set;}
    }
}

