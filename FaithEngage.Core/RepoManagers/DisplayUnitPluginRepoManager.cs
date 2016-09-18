using System;
using System.Linq;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Interfaces;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.RepoInterfaces;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.PluginManagers.Interfaces;

namespace FaithEngage.Core.RepoManagers
{
	public class DisplayUnitPluginRepoManager : IDisplayUnitPluginRepoManager
	{
		
		private readonly IDisplayUnitPluginFactory _factory;
		private readonly IPluginRepoManager _pRepoMgr;
		private readonly IPluginRepository _repo;

		public DisplayUnitPluginRepoManager(IDisplayUnitPluginFactory factory, IPluginRepoManager pRepoMgr, IPluginRepository repo) 
		{
			_factory = factory;
			_pRepoMgr = pRepoMgr;
			_repo = repo;
		}

		public IEnumerable<DisplayUnitPlugin> GetAll ()
		{
            List<PluginDTO> dtos;
			try {
				dtos = _repo.GetAll(PluginTypeEnum.DisplayUnit);
            } catch (Exception ex) {
                throw new RepositoryException ("There was a problem obtaining plugins from the repository.", ex);
            }
			return _factory.LoadPluginsFromDtos (dtos);
		}

		IDictionary<Guid, Plugin> IPluginRepoManager.GetAllPlugins()
		{
			return _pRepoMgr.GetAllPlugins();
		}

		public DisplayUnitPlugin GetById(Guid id)
		{
            PluginDTO dto;
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

		public void RegisterNew(Plugin plugin, Guid pluginId)
		{
			_pRepoMgr.RegisterNew(plugin, pluginId);
		}

		public void UninstallPlugin(Guid id)
		{
			_pRepoMgr.UninstallPlugin(id);
		}

		public void UpdatePlugin(Plugin plugin)
		{
			_pRepoMgr.UpdatePlugin(plugin);
		}

        bool IPluginRepoManager.CheckRegistered (Guid pluginId)
        {
            return _pRepoMgr.CheckRegistered (pluginId);
        }

        bool IPluginRepoManager.CheckRegistered<TPlugin> ()
        {
            return _pRepoMgr.CheckRegistered<TPlugin> ();
        }
    }
}

