using System;
using FaithEngage.Core.Bootstrappers;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using FaithEngage.CorePlugins.DisplayUnits.TextUnit;

namespace FaithEngage.CorePlugins.DisplayUnits
{
	public class DisplayUnitsBootstrapper : IBootstrapper
	{
		protected class templateDef
        {
            public string fname
            {
                get;
                set;
            }

            public string name
            {
                get;
                set;
            }
        }

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

        protected void registerTemplates(IAppFactory factory, templateDef[] filesNeeded)
		{
			var tempService = factory.TemplatingService;
			var curretDir = AppDomain.CurrentDomain.BaseDirectory;
			var dir = new DirectoryInfo(Path.Combine(curretDir, "Plugin Files"));
			var files = dir.EnumerateFiles("*", SearchOption.AllDirectories);
			
			var filesObtained =
				from f in files
				from fn in filesNeeded
				where f.FullName.Contains(fn.fname)
				select new
				{
					fileInfo = f,
					key = fn.name
				};

			foreach (var file in filesObtained)
			{
				try
				{
					var tempString = getTemplateString(file.fileInfo);
					tempService.RegisterTemplate(tempString, file.key);
				}
				catch (Exception ex)
				{
					continue;
				}
			}

		}

        protected string getTemplateString(FileInfo file)
		{
			string templateString = null;
			using (var reader = file.OpenText())
			{
				templateString = reader.ReadToEnd();
			}
			return templateString;
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
