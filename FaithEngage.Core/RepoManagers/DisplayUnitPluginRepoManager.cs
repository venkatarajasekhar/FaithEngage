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
	public class DisplayUnitPluginRepoManager : PluginRepoManager, IDisplayUnitPluginRepoManager
	{
		
		private readonly IDisplayUnitPluginFactory _factory;

        public DisplayUnitPluginRepoManager (IDisplayUnitPluginFactory factory, IPluginRepository repo, IConverterFactory<Plugin, PluginDTO> dtoFactory) 
            : base (repo, dtoFactory)
		{
			_factory = factory;
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
	}
}

