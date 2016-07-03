using System;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.DisplayUnitEditor;

namespace FaithEngage.Plugins.DisplayUnits.BibleUnitPlugin
{
    public class BibleUnitPlugin : DisplayUnitPlugin
    {
        #region implemented abstract members of Plugin
        public override string PluginName {
            get {
                return "Bible Unit Plugin";
            }
        }
        public override int[] PluginVersion {
            get {
                return new int[]{0,0,1};
            }
        }
        #endregion

        #region implemented abstract members of DisplayUnitPlugin
        public override System.Collections.Generic.List<string> GetAttributeNames ()
        {
            return new List<string> (){ "reference" };
        }

		private Type _duType = typeof(BibleUnit);
        public override Type DisplayUnitType {
			get{ return _duType; }
        }

        public override IDisplayUnitEditorDefinition EditorDefinition {
            get {
                throw new NotImplementedException ();
            }
            set {
                throw new NotImplementedException ();
            }
        }
            

        #endregion
        
    }
}

