using System;
using NUnit.Framework;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.DisplayUnits
{
	[TestFixture]
	public class DisplayUnitDTOTests
	{
		private Guid VALID_GUID = Guid.NewGuid();

		[Test]
		public void Ctor_ValidIDs_ValidDto()
		{
			var dto = new DisplayUnitDTO(VALID_GUID, VALID_GUID);

			Assert.That (dto.Id, Is.EqualTo (VALID_GUID));
			Assert.That (dto.AssociatedEvent, Is.EqualTo (VALID_GUID));
			Assert.That (dto.Attributes, Is.Not.Null);
		}

		[Test]
		[ExpectedException(typeof(EmptyGuidException))]
		public void Ctor_EmptyDisplayUnitId_ThrowsEmptyGuidException()
		{
			var dto = new DisplayUnitDTO (Guid.Empty, VALID_GUID);
		}

		[Test]
		[ExpectedException(typeof(EmptyGuidException))]
		public void Ctor_EmptyEventId_ThrowsEmptyGuidException()
		{
			var dto = new DisplayUnitDTO (VALID_GUID, Guid.Empty);
		}

		[Test]
		public void SetPositionInEvent_NonNegativeNumber_SetsValue()
		{
			var dto = new DisplayUnitDTO (VALID_GUID, VALID_GUID);
			dto.PositionInEvent = 1;
			Assert.That(dto.PositionInEvent, Is.EqualTo(1));
		}
		
		[Test]
		[ExpectedException(typeof(NegativePositionException))]
		public void SetPositionInEvent_NegativeNumber_ThrowsException()
		{
			var dto = new DisplayUnitDTO (VALID_GUID, VALID_GUID);
			dto.PositionInEvent = -1;
		}
	}
}

