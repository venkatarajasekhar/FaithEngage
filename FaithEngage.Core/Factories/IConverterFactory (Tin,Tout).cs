using System;
namespace FaithEngage.Core
{
	public interface IConverterFactory<Tin,Tout>
	{
		Tout Convert(Tin source);
	}
}

