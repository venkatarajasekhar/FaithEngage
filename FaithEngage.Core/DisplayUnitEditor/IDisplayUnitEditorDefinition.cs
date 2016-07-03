using FaithEngage.Core.TemplatingService;

namespace FaithEngage.Core.DisplayUnitEditor
{
    public interface IDisplayUnitEditorDefinition
    {
        string GetHtmlEditorForm (ITemplatingService tService, DisplayUnitEditorContext context);
    }
}

