using System;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.DisplayUnitEditor;

namespace FaithEngage.Plugins.DisplayUnits.TextUnitPlugin
{
    public class TextUnitPlugin : DisplayUnitPlugin
    {
        public TextUnitPlugin ()
        {
            var def = new DisplayUnitEditorDefinition ();
            def.EnforceSectionOrder = false;
            var secDef = new CardSectionDefinition ("Text", EditorFieldType.HtmlTextArea,1, 1);
            secDef.EditorFieldType = EditorFieldType.HtmlTextArea;
            secDef.Limit = 1;
            secDef.NumberRequired = 1;
            var secs = new CardSectionDefinition[] { secDef };
            def.CardSectionDefinitions = secs;
            EditorDefinition = def;
        }

        #region implemented abstract members of Plugin
        public override string PluginName {
            get {
                return "Text Unit";
            }
        }
        public override int[] PluginVersion {
            get {
                return new int[]{0,0,1};
            }
        }
        #endregion
        #region implemented abstract members of DisplayUnitPlugin
        public override List<string> GetAttributeNames ()
        {
            return new List<string> (){ "Text" };
        }

		private Type _duType = typeof(TextUnit);
        public override Type DisplayUnitType {
			get{ return _duType; }
        }
            
        public override DisplayUnitEditorDefinition EditorDefinition {
            get;
            set;
        }

        #endregion



    }
}

