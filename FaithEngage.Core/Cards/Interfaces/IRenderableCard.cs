using FaithEngage.Core.DisplayUnits;

namespace FaithEngage.Core.Cards.Interfaces
{
    public interface IRenderableCard
    {
        IRenderableCardSection[] Sections{get;set;}
        string Title{ get; set;}
        string Description{get;set;}
        DisplayUnit OriginatingDisplayUnit{ get; set;}
		IRenderableCard ReRender (CardActionResultArgs args);
    }
}

