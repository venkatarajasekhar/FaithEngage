using System;
namespace FaithEngage.Core.Factories
{
	public interface IConverterFactory<Tin,Tout>
	{
		Tout Convert(Tin source);
	}
}

