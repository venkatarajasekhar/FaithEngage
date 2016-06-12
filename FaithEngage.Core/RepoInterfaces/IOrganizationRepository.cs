using System;
using FaithEngage.Core.UserClasses;

namespace FaithEngage.Core.RepoInterfaces
{
	public interface IOrganizationRepository
	{
		Organization GetById(Guid id);
		void Update(Organization org);
		Guid Save(Organization org);
		void Delete(Guid id);
	}
}

