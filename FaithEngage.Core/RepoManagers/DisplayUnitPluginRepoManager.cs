using System;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;

namespace FaithEngage.Core.RepoManagers
{
	public class DisplayUnitPluginRepoManager : IDisplayUnitPluginRepoManager
	{
		public DisplayUnitPluginRepoManager (IDisplayUnitPluginRepository repo)
		{
		}


		#region IDisplayUnitPluginRepoManager implementation
		public void RegisterNew (FaithEngage.Core.PluginManagers.DisplayUnitPlugins.DisplayUnitPlugin plugin)
		{
			throw new NotImplementedException ();
		}
		public void UpdatePlugin (FaithEngage.Core.PluginManagers.DisplayUnitPlugins.DisplayUnitPlugin plugin)
		{
			throw new NotImplementedException ();
		}
		public void UninstallPlugin (Guid id)
		{
			throw new NotImplementedException ();
		}
		public System.Collections.Generic.List<FaithEngage.Core.PluginManagers.DisplayUnitPlugins.DisplayUnitPlugin> GetAll ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

