using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.TemplatingService;
using FaithEngage.CorePlugins.RazorTemplating;
using System.Reflection;
using System.Linq;

namespace FaithEngage.CorePlugins.RazorTemplating
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
		}

		public override void Install(IAppFactory FEFactory)
		{
		}

		public override void RegisterDependencies(IRegistrationService regService)
		{
			regService.Register<ITemplatingService, RazorTemplatingService>(LifeCycle.Singleton);
		}

		public override void Uninstall(IAppFactory FEFactory)
		{
		}
	}
}

