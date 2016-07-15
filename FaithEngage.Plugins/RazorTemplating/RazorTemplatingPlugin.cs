using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.TemplatingService;
using FaithEngage.Plugins.RazorTemplating;

namespace FaithEngage.Plugins
{
	public class RazorTemplatingPlugin : Plugin
	{
		string _pluginName = "Razor Templating Service Plugin";
		public override string PluginName
		{
			get
			{
				return _pluginName;
			}
		}

		int[] _pluginVersion = new int[] { 1, 0, 0 };
		public override int[] PluginVersion
		{
			get
			{
				return _pluginVersion;
			}
		}

		public override void Initialize(IAppFactory FEFactory)
		{
			throw new NotImplementedException();
		}

		public override void Install(IAppFactory container)
		{
			throw new NotImplementedException();
		}

		public override void RegisterDependencies(IRegistrationService container)
		{
			container.RegisterDependency<ITemplatingService, RazorTemplatingService>(LifeCycle.Singleton);
		}

		public override void Uninstall(IAppFactory container)
		{
			throw new NotImplementedException();
		}
	}
}

