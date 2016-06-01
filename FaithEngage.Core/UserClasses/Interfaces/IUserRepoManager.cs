using System;

namespace FaithEngage.Core.UserClasses.Interfaces
{
	public interface IUserRepoManager
	{
		User GetByUsername(string username);
		void Update(User user);
		Guid Save(User user);
		void Delete(string username);
	}
}

