using System;
using System.Collections.Generic;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.Factories;
using FaithEngage.Core.PluginManagers.Files;
using FaithEngage.Core.UserClasses;

namespace FaithEngage.Core.DisplayUnitEditor
{
	/// <summary>
	/// This is passed into the display unit editor definition's GetHtmlEditorForm() method
	/// to provide all the context necessary to generate an HTML editor for a given display unit type.
	/// </summary>
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

