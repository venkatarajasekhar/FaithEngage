using System;
using FaithEngage.Core.DisplayUnitEditor;
using FaithEngage.Core.TemplatingService;
using FakeItEasy;
using NUnit.Framework;
namespace FaithEngage.CorePlugins.DisplayUnits.TextUnit
{
    public class TextUnitEditorDefinitionTests
    {
        [Test]
        public void GetHtmlEditorForm_CompilesEditorFromTemplate()
        {
			var tService = A.Fake<ITemplatingService>();
			var context = new DisplayUnitEditorContext();
			var tuEditDef = new TextUnitEditorDefinition();

			tuEditDef.GetHtmlEditorForm(tService, context);

			A.CallTo(()=> tService.CompileHtmlFromTemplateKey("TextUnitEditor", context)).MustHaveHappened();
        }
    }
}

