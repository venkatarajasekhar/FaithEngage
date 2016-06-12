using System;
using FaithEngage.Core.UserClasses;

namespace FaithEngage.Core.RepoInterfaces
{
	public interface IUserRepository
	{
		User GetByUsername(string username);
		void Update(User user);
		Guid Save(User user);
		void Delete(string username);
	}
}

