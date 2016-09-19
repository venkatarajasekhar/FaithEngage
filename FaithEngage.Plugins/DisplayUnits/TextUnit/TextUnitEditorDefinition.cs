using System;
using FaithEngage.Core.DisplayUnitEditor;
using FaithEngage.Core.TemplatingService;
using System.Linq;

namespace FaithEngage.CorePlugins.DisplayUnits.TextUnit
{
    public class TextUnitEditorDefinition : IDisplayUnitEditorDefinition
    {
        public string GetHtmlEditorForm (ITemplatingService tService, DisplayUnitEditorContext context)
        {
            var editor = tService.CompileHtmlFromTemplateKey ("TextUnitEditor", context);
            return editor;
        }
    }
}

