using System;
using FaithEngage.Core.UserClasses.Interfaces;
using FaithEngage.Core.UserClasses;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.RepoInterfaces;

namespace FaithEngage.Core.RepoManagers
{
	public class UserRepoManager : IUserRepoManager
	{
		private readonly IUserRepository _repo;
		public UserRepoManager (IUserRepository repo)
		{
			_repo = repo;	
		}
			
		#region IUserRepoManager implementation
		public User GetByUsername (string username)
		{
			User user;
			try {
				user = _repo.GetByUsername(username);
			} catch (RepositoryException ex) {
				throw new RepositoryException ("There was a problem accessing the User Repository", ex);
			}
			return user;
		}
		public void Update (User user)
		{
			try {
				_repo.Update(user);
			} catch (RepositoryException ex) {
				throw new RepositoryException ("There was a problem accessing the User Repository", ex);
			}
		}
		public Guid Save (User user)
		{
			try {
				return _repo.Save(user);
			} catch (RepositoryException ex) {
				throw new RepositoryException ("There was a problem accessing the User Repository", ex);
			}
		}
		public void Delete (string username)
		{
			try {
				_repo.Delete(username);
			} catch (RepositoryException ex) {
				throw new RepositoryException ("There was a problem accessing the User Repository", ex);
			}
		}
		#endregion
	}
}

