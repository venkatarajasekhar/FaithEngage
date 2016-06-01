using System;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.DisplayUnits;
using FakeItEasy;


namespace Dummy_PluginAssembly
{
	public class DummyPlugin : DisplayUnitPlugin
	{
		#region implemented abstract members of Plugin
		public override string PluginName {
			get {
				return "My Plugin";
			}
		}
		public override int[] PluginVersion {
			get {
				return new int[] { 0, 0, 1 };
			}
		}
		#endregion
		#region implemented abstract members of DisplayUnitPlugin
		public override System.Collections.Generic.List<string> GetAttributeNames ()
		{
			throw new NotImplementedException ();
		}
		public override Type DisplayUnitType {
			get {
				return typeof(DisplayUnit);
			}
		}
		public override FaithEngage.Core.DisplayUnitEditor.DisplayUnitEditorDefinition EditorDefinition {
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

