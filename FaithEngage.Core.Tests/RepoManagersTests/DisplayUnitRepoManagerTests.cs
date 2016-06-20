using System;
using NUnit.Framework;
using FakeItEasy;
using FaithEngage.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.Containers;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;
using FaithEngage.Core.RepoInterfaces;
using FaithEngage.Core.Tests;

namespace FaithEngage.Core.RepoManagers
{
    [TestFixture]
    public class DisplayUnitRepoManagerTests
    {

        private Guid VALID_GUID;
        private Guid INVALID_GUID;
        private DisplayUnitDTO _dto;
        private DateTime _dt = DateTime.Now;
        private IDisplayUnitFactory _fctry;
		private DisplayUnitPlugin _plugin;
        private IDisplayUnitDtoFactory _dtoFac;
        private IDisplayUnitsRepository _repo;


        [SetUp]
        public void init()
        {
            
            VALID_GUID = Guid.NewGuid ();
            INVALID_GUID = Guid.NewGuid ();
            _fctry = A.Fake<IDisplayUnitFactory> ();
			_plugin = A.Fake<DisplayUnitPlugin> ();
			_plugin.PluginId = VALID_GUID;
            _dtoFac = A.Fake<IDisplayUnitDtoFactory> ();
            _repo = A.Fake<IDisplayUnitsRepository> ();

            _dto = new DisplayUnitDTO (VALID_GUID, VALID_GUID) {
                DateCreated = _dt,
                Description = "My description",
                Name = "My Text Unit",
                PositionInEvent = 1,
                Attributes = new Dictionary<string,string> ()
                { 
                    {"Text","This is the text for my TextUnit"} 
                }
            };
        }
            
        [Test]
        public void GetById_ValidId_DisplayUnit()
        {
            A.CallTo (() => _repo.GetById (VALID_GUID)).Returns (_dto);
            A.CallTo (() => _fctry.ConvertFromDto (_dto)).Returns (A.Fake<DisplayUnit> ());
            var mgr = new DisplayUnitsRepoManager(_fctry,_repo,_dtoFac);
            var du = mgr.GetById (VALID_GUID);

            Assert.That (du, Is.Not.Null);
        }


        [Test]
        public void GetById_InvalidId_ReturnsNull()
        {
            A.CallTo (() => _repo.GetById (INVALID_GUID)).Returns (null);
            var mgr = new DisplayUnitsRepoManager(_fctry,_repo,_dtoFac);
            var du = mgr.GetById (INVALID_GUID);
            Assert.That (du, Is.Null);
        }

        [Test]
        public void GetById_RepoThrowsException_ReturnsRepoException()
        {
            A.CallTo (() => _repo.GetById (VALID_GUID)).Throws<RepositoryException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            var e = TestHelpers.TryGetException(()=> mgr.GetById (VALID_GUID));
            Assert.That (e, Is.InstanceOf (typeof (RepositoryException)));
        }

        [Test]
        public void GetByEvent_ValidEventId_ReturnsDictOfEvents()
        {
            var dict = new Dictionary<int,DisplayUnitDTO> ();
            for(var i = 0; i < 5; i++)
            {
                dict.Add (i, new DisplayUnitDTO(VALID_GUID, VALID_GUID));
            }
            A.CallTo (() => _repo.GetByEvent (VALID_GUID, false)).Returns (dict);
            A.CallTo (() => _fctry.ConvertFromDto (null))
                .WithAnyArguments()
                .ReturnsLazily((DisplayUnitDTO d) => A.Fake<DisplayUnit>(p=> p.WithArgumentsForConstructor(new object[]{d.Attributes})));
            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            var duDict = mgr.GetByEvent (VALID_GUID, false);

            Assert.That (duDict, Is.InstanceOf (typeof(Dictionary<int,DisplayUnit>)));
            Assert.That (duDict, Is.Not.Null);
            Assert.That (duDict.Count == 5);
            foreach(var key in duDict.Keys)
            {
                Assert.That (duDict [key].PositionInEvent == key);
            }
        }

        [Test]
        public void GetByEvent_InvalidEventId_ReturnsNull()
        {
            A.CallTo (() => _repo.GetByEvent (INVALID_GUID, false)).Returns (null);

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            var duDict = mgr.GetByEvent (INVALID_GUID, false);

            Assert.That (duDict, Is.Null);

        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void GetByEvent_RepoThrowsException_ReturnsNull()
        {
            A.CallTo (() => _repo.GetByEvent (VALID_GUID, false)).Throws<RepositoryException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            var duDict = mgr.GetByEvent (VALID_GUID, false);
        }

        [Test]
        public void GetByEvent_FactoryUnableToConvertOne_ReturnsDictWithOutInvalidDisplayUnit()
        {
            var dtos = Enumerable.Repeat (_dto, 5).ToArray ();
            var dict = new Dictionary<int,DisplayUnitDTO> ();
            for(var i = 0; i < dtos.Length; i++)
            {
                dict.Add (i, dtos [i]);
            }
            A.CallTo (() => _repo.GetByEvent (VALID_GUID, false)).Returns (dict);
            A.CallTo (() => _fctry.ConvertFromDto (_dto))
                .ReturnsLazily (
                    (DisplayUnitDTO d) => A.Fake<DisplayUnit>(
                        p=> p.WithArgumentsForConstructor(
                            new object[]{d.Attributes}
                        )
                    )
                );
            A.CallTo (() => _fctry.ConvertFromDto (_dto)).Returns (null).Once();
            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            var duDict = mgr.GetByEvent (VALID_GUID, false);

            Assert.That (duDict, Is.InstanceOf (typeof(Dictionary<int,DisplayUnit>)));
            Assert.That (duDict, Is.Not.Null);
            Assert.That (duDict.Count == 4);
            Assert.That (!duDict.ContainsKey (0));
            foreach(var key in duDict.Keys)
            {
                Assert.That (duDict [key].PositionInEvent == key);
            }
        }


        [Test]
        public void SaveManyToEvent_ValidUnits_ValidEventId_NoException()
        {
            var dict = new Dictionary<string,string> () {
                {"AssociatedEvent",VALID_GUID.ToString ()}
            };
            var i = 0;
            var units = Enumerable.Repeat(0, 5).Select( u => A.Fake<DisplayUnit>(
                p=> p.WithArgumentsForConstructor(new object[]{dict})))
                .ToDictionary (p => i++, p => p);
            Dictionary<int,DisplayUnitDTO> receivedUnits = null;
            A.CallTo (() => _repo.SaveManyToEvent (A<Dictionary<int,DisplayUnitDTO>>.Ignored, VALID_GUID))
                .Invokes ((Dictionary<int,DisplayUnitDTO> p, Guid g) => receivedUnits = p);
            A.CallTo (() => _dtoFac.ConvertToDto (A<DisplayUnit>.Ignored))
             .ReturnsLazily ((DisplayUnit u) => {
                 var duDto = new DisplayUnitDTO (u.AssociatedEvent, u.Id);
                 duDto.PositionInEvent = u.PositionInEvent;
                 return duDto;
            });
        }

		[Test]
		public void SaveManyToEvent_FactoryReturnsOneNull_SavesOneFewer()
		{
			var dict = new Dictionary<string, string>() {
				{"AssociatedEvent",VALID_GUID.ToString ()}
			};
			var i = 0;
			var units = Enumerable.Repeat(0, 5).Select(u => A.Fake<DisplayUnit>(
			   p => p.WithArgumentsForConstructor(new object[] { dict })))
				.ToDictionary(p => i++, p => p);
			Dictionary<int, DisplayUnitDTO> receivedUnits = null;
			A.CallTo(() => _repo.SaveManyToEvent(A<Dictionary<int, DisplayUnitDTO>>.Ignored, VALID_GUID))
				.Invokes((Dictionary<int, DisplayUnitDTO> p, Guid g) => receivedUnits = p);
			A.CallTo(() => _dtoFac.ConvertToDto(A<DisplayUnit>.Ignored))
			 .ReturnsLazily((DisplayUnit u) =>
			 {
				 var duDto = new DisplayUnitDTO(u.AssociatedEvent, u.Id);
				 duDto.PositionInEvent = u.PositionInEvent;
				 return duDto;
			 });
			A.CallTo(() => _dtoFac.ConvertToDto(A<DisplayUnit>.Ignored)).Returns(null).Once();
			var mgr = new DisplayUnitsRepoManager(_fctry, _repo, _dtoFac);
			mgr.SaveManyToEvent(units, VALID_GUID);

			Assert.That(receivedUnits, Is.Not.Null);
			Assert.That(receivedUnits.Count == 4);
			Assert.That(receivedUnits.All(p => p.Value.AssociatedEvent == VALID_GUID));
			foreach (var key in receivedUnits.Keys.ToArray())
			{
				Assert.That(receivedUnits[key].PositionInEvent, Is.EqualTo(key));
			}
		}

        [Test]
        [ExpectedException(typeof(InvalidIdException))]
        public void SaveManyToEvent_ValidUnits_InvalidEventId_ThrowsInvalidIdException()
        {
            var dict = new Dictionary<string,string> () {
                {"AssociatedEvent",INVALID_GUID.ToString ()}
            };
            var i = 0;
            var units = Enumerable.Repeat(0, 5).Select( u => A.Fake<DisplayUnit>(
                p=> p.WithArgumentsForConstructor(new object[]{dict})))
                .ToDictionary (p => i++, p => p);
			foreach (var unit in units) 
			{
				A.CallTo (() => unit.Value.Plugin).Returns (_plugin);
			}
            A.CallTo (() => _repo.SaveManyToEvent (null, INVALID_GUID)).WithAnyArguments ().Throws<InvalidIdException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            mgr.SaveManyToEvent (units, INVALID_GUID);
        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void SaveManyToEvent_RepoThrowsException_ThrowsRepoException()
        {

            var dict = new Dictionary<string,string> () {
                {"AssociatedEvent",INVALID_GUID.ToString ()}
            };
            var i = 0;
            var units = Enumerable.Repeat(0, 5).Select( u => A.Fake<DisplayUnit>(
                p=> p.WithArgumentsForConstructor(new object[]{dict})))
                .ToDictionary (p => i++, p => p);
			foreach (var unit in units) 
			{
				A.CallTo (() => unit.Value.Plugin).Returns (_plugin);
			}

            A.CallTo (() => _repo.SaveManyToEvent (null, INVALID_GUID)).WithAnyArguments ().Throws<RepositoryException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            mgr.SaveManyToEvent (units, INVALID_GUID);
        }


        [Test]
        public void GetGroup_ValidEventId_ValidGroupId_ReturnsGroupDictionary()
        {
            var i = 0;
            var dict = Enumerable.Repeat (_dto, 5).ToDictionary (p => i++, p => p);

            A.CallTo (() => _fctry.ConvertFromDto (_dto)).ReturnsLazily ((DisplayUnitDTO d) => A.Fake<DisplayUnit> ());
            A.CallTo (() => _repo.GetGroup (VALID_GUID, VALID_GUID)).Returns (dict);

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            var du = mgr.GetGroup (VALID_GUID, VALID_GUID);

            Assert.That (du, Is.Not.Null);
            Assert.That (du.Count, Is.EqualTo (5));
            foreach(var key in du.Keys)
            {
                Assert.That (du [key].PositionInEvent == key);
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidIdException))]
        public void GetGroup_InvalidEventId_ThrowsInvalidIdException()
        {
            A.CallTo (() => _repo.GetGroup (VALID_GUID, VALID_GUID)).Throws<InvalidIdException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            var grp = mgr.GetGroup (VALID_GUID, VALID_GUID);
        }

        [Test]
        public void GetGroup_ValidEventId_InvalidGroupId_ReturnsNull()
        {
            A.CallTo (() => _repo.GetGroup (VALID_GUID, INVALID_GUID)).Returns (null);

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            var grp = mgr.GetGroup (VALID_GUID, INVALID_GUID);

            Assert.That (grp, Is.Null);
        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void GetGroup_RepoThrowsException_ThrowsRepoException()
        {
            A.CallTo (() => _repo.GetGroup (VALID_GUID, VALID_GUID)).Throws<RepositoryException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            var grp = mgr.GetGroup (VALID_GUID, VALID_GUID);
        }

        [Test]
        public void SaveOneToEvent_ValidDisplayUnit_SavesToRepo()
        {
            var unit = A.Fake<DisplayUnit> ();
            DisplayUnitDTO receivedDto = null;
            A.CallTo (() => _repo.SaveOneToEvent (A<DisplayUnitDTO>.Ignored))
                .Invokes ((DisplayUnitDTO p) => receivedDto = p);
            A.CallTo (() => _dtoFac.ConvertToDto (unit)).Returns (new DisplayUnitDTO (VALID_GUID, VALID_GUID));
            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            mgr.SaveOneToEvent (unit);

            Assert.That (receivedDto, Is.Not.Null);
        }

        [Test]
        [ExpectedException(typeof(InvalidIdException))]
        public void SaveOneToEvent_InvalidEventID_RepoThrowsInvalidIdException_ThrowsSame()
        {
            var unit = A.Fake<DisplayUnit> ();
            unit.AssociatedEvent = INVALID_GUID;
			A.CallTo (() => unit.Plugin).Returns (_plugin);
            A.CallTo (() => _repo.SaveOneToEvent (A<DisplayUnitDTO>.Ignored))
                .Throws<InvalidIdException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            mgr.SaveOneToEvent (unit);

        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void SaveOneToEvent_RepoThrowsRepoException_ThrowsRepoException()
        {
            var unit = A.Fake<DisplayUnit> ();
            unit.AssociatedEvent = VALID_GUID;
			A.CallTo (() => unit.Plugin).Returns (_plugin);
            A.CallTo (() => _repo.SaveOneToEvent (A<DisplayUnitDTO>.Ignored))
                .Throws<RepositoryException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            mgr.SaveOneToEvent (unit);
        }

        [Test]
        public void DuplicateToEvent_SavesCloneToEvent()
        {
            var unit = A.Fake<DisplayUnit> ();
            var dto = new DisplayUnitDTO (VALID_GUID, VALID_GUID);
            A.CallTo (() => unit.Clone ()).Returns (unit);
            A.CallTo (() => _dtoFac.ConvertToDto (unit)).Returns (dto);
            DisplayUnitDTO receivedUnit = null;
            A.CallTo (() => _repo.SaveOneToEvent (A<DisplayUnitDTO>.Ignored))
                .Invokes ((DisplayUnitDTO d) => receivedUnit = d);

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            mgr.DuplicateToEvent (unit);

            Assert.That (receivedUnit, Is.EqualTo(dto));
        }

        [Test]
        [ExpectedException(typeof(InvalidIdException))]
        public void DuplicateToEvent_InvalidEventId_RepoThrowsInvalidIdException_ThrowsSame()
        {
            var unit = A.Fake<DisplayUnit> ();
            unit.Description = "My Description";
            unit.Name = "My Name";
            unit.AssociatedEvent = INVALID_GUID;
			A.CallTo (() => unit.Plugin).Returns (_plugin);
            A.CallTo (() => unit.Clone ()).Returns (unit);

            A.CallTo (() => _repo.SaveOneToEvent (A<DisplayUnitDTO>.Ignored))
                .Throws<InvalidIdException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            mgr.DuplicateToEvent (unit);
        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void DuplicateToEvent_RepoThrowsRepoException_ThrowsSame()
        {
            var unit = A.Fake<DisplayUnit> ();
            unit.Description = "My Description";
            unit.Name = "My Name";
            unit.AssociatedEvent = INVALID_GUID;
			A.CallTo (() => unit.Plugin).Returns (_plugin);
			A.CallTo (() => unit.Clone ()).Returns (unit);

            A.CallTo (() => _repo.SaveOneToEvent (A<DisplayUnitDTO>.Ignored))
                .Throws<RepositoryException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            mgr.DuplicateToEvent (unit);
        }

        [Test]
        public void Delete_ValidId_SendsDeleteToRepo()
        {
            Guid receivedGuid = Guid.Empty;
            A.CallTo (() => _repo.Delete (VALID_GUID)).Invokes ((Guid g) => receivedGuid = g);
                
            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            mgr.Delete (VALID_GUID);

            Assert.That (receivedGuid, Is.EqualTo (VALID_GUID));
        }

        [Test]
        [ExpectedException(typeof(InvalidIdException))]
        public void Delete_InvalidId_RepoThrowsInvalidIdException_ThrowsSame()
        {
            A.CallTo (() => _repo.Delete (INVALID_GUID)).Throws<InvalidIdException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            mgr.Delete (INVALID_GUID);
        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void Delete_RepoThrowsException_ThrowsSame()
        {
            A.CallTo (() => _repo.Delete (INVALID_GUID)).Throws<RepositoryException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,_repo, _dtoFac);
            mgr.Delete (INVALID_GUID);
        }
    }
}

