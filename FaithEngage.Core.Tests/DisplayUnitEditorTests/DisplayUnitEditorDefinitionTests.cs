using NUnit.Framework;
using FaithEngage.Core.DisplayUnitEditor;


namespace FaithEngage.Core.DisplayUnitEditor
{
    [TestFixture]
    public class DisplayUnitEditorDefinitionTests
    {
        [Test]
        public void Ctor_EnforceSectionOrder_False(){
            var def = new DisplayUnitEditorDefinition ();
            Assert.That (def.EnforceSectionOrder == false);
        }
    }
}

