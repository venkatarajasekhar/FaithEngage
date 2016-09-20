using System;
using System.Collections.Generic;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.Factories;
using FaithEngage.Core.PluginManagers.Files;
using FaithEngage.Core.UserClasses;

namespace FaithEngage.Core.DisplayUnitEditor
{
	public class DisplayUnitEditorContext
	{
		public DisplayUnit DisplayUnit
		{
			get;
			set;
		}

		public User CurrentUser
		{
			get;
			set;
		}

		public Organization CurrentOrg { get; set; }

		public IDictionary<Guid, PluginFileInfo> PluginFiles {get;set;}

		public IAppFactory AppFactory { get; set; }
    }
}

