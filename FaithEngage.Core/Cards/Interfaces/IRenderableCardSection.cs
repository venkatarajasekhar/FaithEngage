using System;

namespace FaithEngage.Core.Cards.Interfaces
{
    /// <summary>
    /// A subsection of a renderable card. It can have subsections as well.
    /// </summary>
    public interface IRenderableCardSection
    {
        string HeadingText{get;set;}
        string HtmlContents{get;set;}
    }
}

