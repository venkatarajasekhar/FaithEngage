using System;
using System.Collections.Generic;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FakeItEasy;
using NUnit.Framework;
namespace FaithEngage.Core.DisplayUnits.Factories
{
	[TestFixture]
	public class DisplayUnitDtoFactoryTests
	{
		private DisplayUnitDtoFactory _fac;
		private DisplayUnit _du;
		private DisplayUnitPlugin _plugin;
		private const string VALID_STRING = "VALID STRING";
		private Guid VALID_GUID = Guid.NewGuid();

		[SetUp]
		public void setup()
		{
			_fac = new DisplayUnitDtoFactory();
			_du = A.Fake<DisplayUnit>(
				x=> x.WithArgumentsForConstructor(
					new object[] { VALID_GUID, new Dictionary<string, string>(){}}
				)
			);
			_plugin = A.Fake<DisplayUnitPlugin>();
			A.CallTo(() => _du.Plugin).Returns(_plugin);
			A.CallTo(() => _du.GetAttributes())
			 .Returns(new Dictionary<string, string>()
			 {
				{"First","First"},
				{"Second","Second"}
			});
		}

		[Test]
		public void ConvertToDTO_LoadedValidDU_ValidDto()
		{
			_du.AssociatedEvent = VALID_GUID;
			_du.DateCreated = DateTime.Now.Date;
			_du.Description = VALID_STRING;
			_du.Name = VALID_STRING;
			_plugin.PluginId = VALID_GUID;
			_du.PositionInEvent = 5;
			_du.UnitGroup = new DisplayUnitGrouping(3, VALID_GUID);

            var dto = _fac.Convert(_du);

			Assert.That(dto, Is.Not.Null);
			Assert.That(dto.AssociatedEvent, Is.EqualTo(VALID_GUID));
			Assert.That(dto.Attributes["First"], Is.EqualTo("First"));
			Assert.That(dto.DateCreated, Is.EqualTo(DateTime.Now.Date));
			Assert.That(dto.Description, Is.EqualTo(VALID_STRING));
			Assert.That(dto.GroupId, Is.EqualTo(VALID_GUID));
			Assert.That(dto.Id, Is.EqualTo(VALID_GUID));
			Assert.That(dto.Name, Is.EqualTo(VALID_STRING));
			Assert.That(dto.PluginId, Is.EqualTo(VALID_GUID));
			Assert.That(dto.PositionInEvent, Is.EqualTo(5));
			Assert.That(dto.PositionInGroup, Is.EqualTo(3));
		}

		[Test]
		public void ConvertToDTO_EmptyDU_ReturnsNull()
		{
            var dto = _fac.Convert(_du);
			Assert.That(dto, Is.Null);
		}


	}
}

