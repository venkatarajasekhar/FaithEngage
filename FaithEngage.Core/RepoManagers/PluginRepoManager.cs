using System;
using System.Collections.Generic;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.PluginManagers;
using FaithEngage.Core.PluginManagers.Interfaces;
using FaithEngage.Core.RepoInterfaces;

namespace FaithEngage.Core.RepoManagers
{
	public class PluginRepoManager : IPluginRepoManager<Plugin>
	{
		protected readonly IPluginRepository _repo;
		protected readonly IConverterFactory<Plugin, PluginDTO> _dtoFactory;
		public PluginRepoManager(IPluginRepository repo,IConverterFactory<Plugin, PluginDTO> dtoFactory)
		{
			_repo = repo;
			_dtoFactory = dtoFactory;
		}

		public Guid RegisterNew(Plugin plugin)
		{
			plugin.PluginId = Guid.NewGuid();
			var dto = _dtoFactory.Convert(plugin);
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

		public IEnumerable<Plugin> GetAll()
		{
			throw new NotImplementedException();
		}

		public Plugin GetById(Guid id)
		{
			throw new NotImplementedException();
		}

		public List<PluginDTO> GetAllDtos()
		{
			return _repo.GetAll();
		}
	}
}

