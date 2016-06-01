using System;

namespace FaithEngage.Core.UserClasses.Interfaces
{
	public interface IOrganizationRepository
	{
		Organization GetById(Guid id);
		void Update(Organization org);
		Guid Save(Organization org);
		void Delete(Guid id);
	}
}

