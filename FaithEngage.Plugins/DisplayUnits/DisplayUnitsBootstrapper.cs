using System;
using FaithEngage.Core.Bootstrappers;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace FaithEngage.CorePlugins.DisplayUnits
{
	public class DisplayUnitsBootstrapper : IBootstrapper
	{
		public BootPriority BootPriority
		{
			get
			{
				return BootPriority.Normal;
			}
		}

		public void Execute(IAppFactory factory)
		{
			registerTemplates(factory);
		}

		private void registerTemplates(IAppFactory factory)
		{
			var tempService = factory.TemplatingService;
			var curretDir = AppDomain.CurrentDomain.BaseDirectory;
			var dir = new DirectoryInfo(Path.Combine(curretDir, "Plugin Files"));
			var files = dir.EnumerateFiles("*", SearchOption.AllDirectories);
			var filesNeeded = new[]
			{
				new{fname = "TextUnitEditorTemplate.cshtml", name = "TextUnitEditor"},
				new{fname = "TextUnitCardTemplate.cshtml", name="TextUnitCard" }
			};
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

		private string getTemplateString(FileInfo file)
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
		}

		public void RegisterDependencies(IRegistrationService regService)
		{
		}
	}
}
