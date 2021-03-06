﻿using System;
using FaithEngage.Core.Bootstrappers;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using FaithEngage.CorePlugins.DisplayUnits.TextUnit;
using FaithEngage.Core.PluginManagers;
using System.Reflection;

namespace FaithEngage.CorePlugins.DisplayUnits
{
	public class DisplayUnitsBootstrapper : IBootstrapper
	{
		
        public DisplayUnitsBootstrapper()
        {
			var currentDir = Path.GetDirectoryName(this.GetType().Assembly.Location);
			var path = Path.Combine(currentDir, "Plugin Files");
			var pluginFilesDir = new DirectoryInfo(path);
            pluginFiles = pluginFilesDir.EnumerateFiles("*", SearchOption.AllDirectories);                                ;
        }

        protected IEnumerable<FileInfo> pluginFiles { get; set;}

        public BootPriority BootPriority
		{
			get
			{
				return BootPriority.Normal;
			}
		}

		public void Execute(IAppFactory factory)
		{
		}

		public void LoadBootstrappers(IBootList bootstrappers)
		{
            bootstrappers.Load<TextUnitBootstrapper>();
		}

		public void RegisterDependencies(IRegistrationService regService)
		{
		}
	}
}
