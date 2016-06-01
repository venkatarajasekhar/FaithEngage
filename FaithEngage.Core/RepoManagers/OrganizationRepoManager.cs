using System;
using FaithEngage.Core.UserClasses.Interfaces;
using FaithEngage.Core.UserClasses;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.RepoManagers
{
	public class OrganizationRepoManager : IOrganizationRepoManager
	{
		private readonly IOrganizationRepository _repo;
		public OrganizationRepoManager (IOrganizationRepository repo)
		{
			_repo = repo;
		}

		#region IOrganizationRepoManager implementation

		public Organization GetById (Guid id)
		{
			Organization org;
			try {
				org = _repo.GetById(id);
			} catch (RepositoryException ex) {
				throw new RepositoryException ("There was a problem accessing the Organization Repository", ex);
			}
			return org;
		}

		public void Update (Organization org)
		{
			try {
				_repo.Update(org);
			} catch (RepositoryException ex) {
				throw new RepositoryException ("There was a problem accessing the Organization Repository", ex);
			}
		}

		public Guid Save (Organization org)
		{
			try {
				return _repo.Save(org);
			} catch (RepositoryException ex) {
				throw new RepositoryException ("There was a problem accessing the Organization Repository", ex);
			}
		}

		public void Delete (Guid id)
		{
			try {
				_repo.Delete(id);
			} catch (RepositoryException ex) {
				throw new RepositoryException ("There was a problem accessing the Organization Repository", ex);
			}
		}

		#endregion
	}
}

