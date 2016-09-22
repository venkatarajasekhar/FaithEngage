using System;
namespace FaithEngage.Core.Factories
{
	/// <summary>
	/// A general purpose interface. Converts an object of type Tin to Tout.
	/// </summary>
	public interface IConverterFactory<Tin,Tout>
	{
		Tout Convert(Tin source);
	}
}

