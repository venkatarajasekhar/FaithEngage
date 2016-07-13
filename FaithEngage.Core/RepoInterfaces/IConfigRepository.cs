using System;
namespace FaithEngage.Core.RepoInterfaces
{
	public interface IConfigRepository
	{
		string Get(string key);
		string Set(string key, string value);
	}
}

