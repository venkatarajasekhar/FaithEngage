using System;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.DisplayUnits;
using FakeItEasy;
using FaithEngage.Core.Containers;
using FaithEngage.Core.DisplayUnitEditor;
using FaithEngage.Core.Factories;

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

        public override void Initialize (IAppFactory container)
        {
        }

        public override void RegisterDependencies (IRegistrationService container)
        {
        }

        public override void Install (IAppFactory container)
        {
            
        }

        public override void Uninstall (IAppFactory container)
        {
            
        }

        public override Type DisplayUnitType {
			get {
				return typeof(DisplayUnit);
			}
		}
		public override IDisplayUnitEditorDefinition EditorDefinition {
			get {
				throw new NotImplementedException ();
			}
		}
		#endregion
		
	}
}

