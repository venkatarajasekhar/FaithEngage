using System;
using System.Linq;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.RepoInterfaces;

namespace FaithEngage.Core.RepoManagers
{
	public class DisplayUnitPluginRepoManager : IDisplayUnitPluginRepoManager
	{
		private readonly IDisplayUnitPluginRepository _repo;
		private readonly IDisplayUnitPluginFactory _factory;
		private readonly IDisplayUnitPluginDtoFactory _dtoFactory;
		public DisplayUnitPluginRepoManager (IDisplayUnitPluginRepository repo, IDisplayUnitPluginFactory factory, IDisplayUnitPluginDtoFactory dtoFactory)
		{
			_repo = repo;
			_factory = factory;
			_dtoFactory = dtoFactory;
		}


		#region IDisplayUnitPluginRepoManager implementation
		public Guid RegisterNew (DisplayUnitPlugin plugin)
		{
			var id = Guid.NewGuid ();
			var dto = _dtoFactory.ConvertFromPlugin (plugin);
			return _repo.Register (dto);
		}
		public void UpdatePlugin (DisplayUnitPlugin plugin)
		{
			if (!plugin.PluginId.HasValue) {
				throw new InvalidIdException ("PluginId must not be null");
			}
			var dto = _dtoFactory.ConvertFromPlugin (plugin);
			_repo.Update (dto);
		}
		public void UninstallPlugin (Guid id)
		{
			_repo.Delete (id);
		}
		public IEnumerable<DisplayUnitPlugin> GetAll ()
		{
			var dtos = _repo.GetAll ();
			return _factory.LoadPluginsFromDtos (dtos);
		}

		public DisplayUnitPlugin GetById(Guid id)
		{
			var dto = _repo.GetById (id);
			return _factory.LoadPluginFromDto (dto);
		}
		#endregion
	}
}

