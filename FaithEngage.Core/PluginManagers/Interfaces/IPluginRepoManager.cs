using System;
using System.Collections.Generic;

namespace FaithEngage.Core.PluginManagers.Interfaces
{
	public interface IPluginRepoManager<T> where T:Plugin
	{
		Guid RegisterNew(T plugin);
		void UninstallPlugin(Guid id);
		void UpdatePlugin(T plugin);
		IEnumerable<T> GetAll();
		T GetById(Guid id);
		List<PluginDTO> GetAllDtos();
	}
}

