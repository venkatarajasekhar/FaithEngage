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
			plugin.PluginId = Guid.NewGuid ();
			var dto = _dtoFactory.ConvertFromPlugin (plugin);
			Guid guid;
			try
			{
				guid = _repo.Register(dto);
			}
			catch (PluginIsMissingNecessaryInfoException)
			{
				throw;
			}
			catch (PluginAlreadyRegisteredException)
			{
				throw;
			}
            catch (Exception ex)
			{
				throw new RepositoryException("There was a problem registering this plugin.", ex);
			}
			return guid;

		}
		public void UpdatePlugin (DisplayUnitPlugin plugin)
		{
			if (!plugin.PluginId.HasValue) {
				throw new InvalidIdException ("PluginId must not be null");
			}
			var dto = _dtoFactory.ConvertFromPlugin (plugin);
			try {
                _repo.Update (dto);
            } catch (Exception ex) {
                throw new RepositoryException ("There was a problem updating the plugin: " + plugin.PluginName, ex);
            }
		}
		public void UninstallPlugin (Guid id)
		{
            if (id == Guid.Empty) throw new InvalidIdException ("PluginId must not be an empty guid.");
            try {
                _repo.Delete (id);             
            } catch (Exception ex) {
                throw new RepositoryException ("There was a problem deleting the specified Id", ex); 
            }
		}
		public IEnumerable<DisplayUnitPlugin> GetAll ()
		{
            List<DisplayUnitPluginDTO> dtos;
            try {
                dtos = _repo.GetAll ();
            } catch (Exception ex) {
                throw new RepositoryException ("There was a problem obtaining plugins from the repository.", ex);
            }
			return _factory.LoadPluginsFromDtos (dtos);
		}

		public DisplayUnitPlugin GetById(Guid id)
		{
            DisplayUnitPluginDTO dto;
            try {
                if (id == Guid.Empty) throw new InvalidIdException ("Empty Guids are not valid Ids.");
                dto = _repo.GetById (id);
            } catch (InvalidIdException){
                throw;
            } catch (Exception ex) {
                throw new RepositoryException ("There was a problem accessing the repository.", ex);
            }
			return _factory.LoadPluginFromDto (dto);
		}
		#endregion
	}
}

