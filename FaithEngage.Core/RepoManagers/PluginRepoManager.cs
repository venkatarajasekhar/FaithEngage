using System;
using System.Collections.Generic;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.PluginManagers.Interfaces;
using FaithEngage.Core.RepoInterfaces;
using System.Linq;

namespace FaithEngage.Core.RepoManagers
{
	public class PluginRepoManager : IPluginRepoManager
	{
		protected readonly IPluginRepository _repo;
		protected readonly IConverterFactory<Plugin, PluginDTO> _dtoFactory;
		IConverterFactory<PluginDTO, Plugin> _plugFac;
		public PluginRepoManager(IPluginRepository repo,IConverterFactory<Plugin, PluginDTO> dtoFactory, IConverterFactory<PluginDTO,Plugin> plugFac)
		{
			_repo = repo;
			_dtoFactory = dtoFactory;
			_plugFac = plugFac;
		}

		public void UninstallPlugin(Guid id)
		{
			if (id == Guid.Empty) throw new InvalidIdException("PluginId must not be an empty guid.");
			try
			{
				_repo.Delete(id);
			}
			catch (Exception ex)
			{
				throw new RepositoryException("There was a problem deleting the specified Id", ex);
			}
		}

		public void UpdatePlugin(Plugin plugin)
		{
			if (!plugin.PluginId.HasValue)
			{
				throw new InvalidIdException("PluginId must not be null");
			}
			var dto = _dtoFactory.Convert(plugin);
			try
			{
				_repo.Update(dto);
			}
			catch (Exception ex)
			{
				throw new RepositoryException("There was a problem updating the plugin: " + plugin.PluginName, ex);
			}
		}

        public void RegisterNew (Plugin plugin, Guid pluginId)
        {
            var dto = _dtoFactory.Convert (plugin);
            try {
                _repo.Register (dto, pluginId);
            } catch (PluginIsMissingNecessaryInfoException) {
                throw;
            } catch (PluginAlreadyRegisteredException) {
                throw;
            } catch (Exception ex) {
                throw new RepositoryException ("There was a problem registering this plugin.", ex);
            }
        }

		public IDictionary<Guid, Plugin> GetAllPlugins()
		{
			var dtos = _repo.GetAll();
			var dict = new Dictionary<Guid, Plugin>();
			foreach (var dto in dtos)
			{
				var plug = _plugFac.Convert(dto);
				dict.Add(plug.PluginId.Value, plug);
			}
			return dict;
		}

        public bool CheckRegistered (Guid pluginId)
        {
            var plug = _repo.GetById (pluginId);
            if(plug == null) return false;
            return true;
        }

        public bool CheckRegistered<TPlugin> () where TPlugin : Plugin
        {
            var plug = _repo.GetAll ().Where (p => p.FullName == typeof (TPlugin).FullName);
            if (plug.Count () > 0) return true;
            return false;
        }
    }
}

