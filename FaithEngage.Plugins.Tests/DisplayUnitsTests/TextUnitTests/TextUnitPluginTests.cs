using NUnit.Framework;
using FaithEngage.Plugins.DisplayUnits.TextUnitPlugin;
using FaithEngage.Core.DisplayUnitEditor;

namespace FaithEngage.Plugins.Tests
{
    [TestFixture]
    public class TextUnitPluginTests
    {
        [Test]
        public void Ctor_InitializesAllProperties()
        {
            var tup = new TextUnitPlugin ();
            var def = tup.EditorDefinition;
            var secDef = def.CardSectionDefinitions [0];
            Assert.That (tup.PluginName == "Text Unit");
            Assert.That (tup.PluginVersion, Is.Not.Null);
            Assert.That (tup.DisplayUnitType, Is.EqualTo (typeof(TextUnit)));
            Assert.That (tup.PluginId, Is.Not.Null);
            Assert.That (def.EnforceSectionOrder, Is.False);
            Assert.That (def.CardSectionDefinitions.Length == 1);
            Assert.That (secDef.EditorFieldType == EditorFieldType.HtmlTextArea);
            Assert.That (secDef.Limit, Is.EqualTo(1));
            Assert.That (secDef.Limit, Is.EqualTo (1));
            Assert.That (secDef.HasLimit);
            Assert.That (secDef.HasMinimum);
            //Assert.That (secDef.Action, Is.Null);
        }
    }
}

