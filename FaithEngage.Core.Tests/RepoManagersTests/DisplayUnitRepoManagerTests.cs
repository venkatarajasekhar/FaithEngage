using System;
using NUnit.Framework;
using FakeItEasy;
using FaithEngage.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Plugins.DisplayUnits.TextUnitPlugin;
using FaithEngage.Core.Containers;
using FaithEngage.Core.DisplayUnits;

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

        [TestFixtureSetUp]
        public void init()
        {
            
            VALID_GUID = Guid.NewGuid ();
            INVALID_GUID = Guid.NewGuid ();
            _fctry = A.Fake<IDisplayUnitFactory> ();

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
            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.GetById (VALID_GUID)).Returns (_dto);
            A.CallTo (() => _fctry.ConvertFromDto (_dto)).Returns (A.Fake<DisplayUnit> ());
            var mgr = new DisplayUnitsRepoManager(_fctry,repo);
            var du = mgr.GetById (VALID_GUID);

            Assert.That (du, Is.Not.Null);
        }


        [Test]
        public void GetById_InvalidId_ReturnsNull()
        {
            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.GetById (INVALID_GUID)).Returns (null);
            var mgr = new DisplayUnitsRepoManager(_fctry,repo);
            var du = mgr.GetById (INVALID_GUID);
            Assert.That (du, Is.Null);
        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void GetById_RepoThrowsException_ReturnsRepoException()
        {
            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.GetById (VALID_GUID)).Throws<RepositoryException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            mgr.GetById (VALID_GUID);
        }

        [Test]
        public void GetByEvent_ValidEventId_ReturnsDictOfEvents()
        {
            var dict = new Dictionary<int,DisplayUnitDTO> ();
            for(var i = 0; i < 5; i++)
            {
                dict.Add (i, new DisplayUnitDTO(VALID_GUID, VALID_GUID));
            }
            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.GetByEvent (VALID_GUID, false)).Returns (dict);
            A.CallTo (() => _fctry.ConvertFromDto (null))
                .WithAnyArguments()
                .ReturnsLazily((DisplayUnitDTO d) => new TextUnit(d.Attributes));
            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
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
            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.GetByEvent (INVALID_GUID, false)).Returns (null);

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            var duDict = mgr.GetByEvent (INVALID_GUID, false);

            Assert.That (duDict, Is.Null);

        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void GetByEvent_RepoThrowsException_ReturnsNull()
        {
            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.GetByEvent (VALID_GUID, false)).Throws<RepositoryException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
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
            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.GetByEvent (VALID_GUID, false)).Returns (dict);
            A.CallTo (() => _fctry.ConvertFromDto (_dto)).ReturnsLazily ((DisplayUnitDTO d) => new TextUnit(d.Attributes));
            A.CallTo (() => _fctry.ConvertFromDto (_dto)).Returns (null).Once();
            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
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
            var unit = new TextUnit (new Dictionary<string,string> (){ {
                    "Text",
                    "My Text"
                } });
            var i = 0;
            var units = Enumerable.Repeat(0, 5).Select( u => new TextUnit(new Dictionary<string,string> (){ {"Text","My Text"} }))
                .ToDictionary (p => i++, p => (DisplayUnit)p);
            Dictionary<int,DisplayUnitDTO> receivedUnits = null;

            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.SaveManyToEvent (A<Dictionary<int,DisplayUnitDTO>>.Ignored, VALID_GUID))
                .Invokes ((Dictionary<int,DisplayUnitDTO> p, Guid g) => receivedUnits = p);

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            mgr.SaveManyToEvent (units, VALID_GUID);

            Assert.That (receivedUnits, Is.Not.Null);
            Assert.That (receivedUnits.Count == 5);
            Assert.That (receivedUnits.All (p => p.Value.Attributes ["Text"] == "My Text"));
            foreach(var key in receivedUnits.Keys)
            {
                Assert.That (receivedUnits [key].PositionInEvent == key);
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidIdException))]
        public void SaveManyToEvent_ValidUnits_InvalidEventId_ThrowsInvalidIdException()
        {
            var unit = new TextUnit (new Dictionary<string,string> (){ {
                    "text",
                    "My Text"
                } });
            var i = 0;
            var units = Enumerable.Repeat (unit, 5).ToDictionary (p => i++, p => (DisplayUnit)p);
            var repo = A.Fake<IDisplayUnitsRepository> ();

            A.CallTo (() => repo.SaveManyToEvent (null, INVALID_GUID)).WithAnyArguments ().Throws<InvalidIdException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            mgr.SaveManyToEvent (units, INVALID_GUID);
        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void SaveManyToEvent_RepoThrowsException_ThrowsRepoException()
        {
            var unit = new TextUnit (new Dictionary<string,string> (){ {
                    "Text",
                    "My Text"
                } });
            var i = 0;
            var units = Enumerable.Repeat (unit, 5).ToDictionary (p => i++, p => (DisplayUnit)p);
            var repo = A.Fake<IDisplayUnitsRepository> ();

            A.CallTo (() => repo.SaveManyToEvent (null, INVALID_GUID)).WithAnyArguments ().Throws<RepositoryException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            mgr.SaveManyToEvent (units, INVALID_GUID);
        }

        [Test]
        public void GetGroup_ValidEventId_ValidGroupId_ReturnsGroupDictionary()
        {
            var i = 0;
            var dict = Enumerable.Repeat (_dto, 5).ToDictionary (p => i++, p => p);

            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => _fctry.ConvertFromDto (_dto)).ReturnsLazily ((DisplayUnitDTO d) => A.Fake<DisplayUnit> ());
            A.CallTo (() => repo.GetGroup (VALID_GUID, VALID_GUID)).Returns (dict);

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
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
            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.GetGroup (VALID_GUID, VALID_GUID)).Throws<InvalidIdException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            var grp = mgr.GetGroup (VALID_GUID, VALID_GUID);
        }

        [Test]
        public void GetGroup_ValidEventId_InvalidGroupId_ReturnsNull()
        {
            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.GetGroup (VALID_GUID, INVALID_GUID)).Returns (null);

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            var grp = mgr.GetGroup (VALID_GUID, INVALID_GUID);

            Assert.That (grp, Is.Null);
        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void GetGroup_RepoThrowsException_ThrowsRepoException()
        {
            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.GetGroup (VALID_GUID, VALID_GUID)).Throws<RepositoryException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            var grp = mgr.GetGroup (VALID_GUID, VALID_GUID);
        }

        [Test]
        public void SaveOneToEvent_ValidDisplayUnit_SavesToRepo()
        {
            var unit = A.Fake<DisplayUnit> ();
            unit.Description = "My Description";
            unit.Name = "My Name";

            var repo = A.Fake<IDisplayUnitsRepository> ();
            DisplayUnitDTO receivedDto = null;
            A.CallTo (() => repo.SaveOneToEvent (A<DisplayUnitDTO>.Ignored))
                .Invokes ((DisplayUnitDTO p) => receivedDto = p);

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            mgr.SaveOneToEvent (unit);

            Assert.That (receivedDto, Is.Not.Null);
            Assert.That (receivedDto.Description == "My Description");
            Assert.That (receivedDto.Name == "My Name");
        }

        [Test]
        [ExpectedException(typeof(InvalidIdException))]
        public void SaveOneToEvent_InvalidEventID_RepoThrowsInvalidIdException_ThrowsSame()
        {
            var unit = A.Fake<DisplayUnit> ();

            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.SaveOneToEvent (A<DisplayUnitDTO>.Ignored))
                .Throws<InvalidIdException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            mgr.SaveOneToEvent (unit);

        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void SaveOneToEvent_RepoThrowsRepoException_ThrowsRepoException()
        {
            var unit = A.Fake<DisplayUnit> ();

            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.SaveOneToEvent (A<DisplayUnitDTO>.Ignored))
                .Throws<RepositoryException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            mgr.SaveOneToEvent (unit);
        }

        [Test]
        public void DuplicateToEvent_SavesCloneToEvent()
        {
            var unit = A.Fake<DisplayUnit> ();
            unit.Description = "My Description";
            unit.Name = "My Name";

            var repo = A.Fake<IDisplayUnitsRepository> ();
            DisplayUnitDTO receivedUnit = null;
            A.CallTo (() => repo.SaveOneToEvent (A<DisplayUnitDTO>.Ignored))
                .Invokes ((DisplayUnitDTO d) => receivedUnit = d);

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            mgr.DuplicateToEvent (unit);

            Assert.That (receivedUnit, Is.Not.Null);
        }

        [Test]
        [ExpectedException(typeof(InvalidIdException))]
        public void DuplicateToEvent_InvalidEventId_RepoThrowsInvalidIdException_ThrowsSame()
        {
            var unit = A.Fake<DisplayUnit> ();

            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.SaveOneToEvent (A<DisplayUnitDTO>.Ignored))
                .Throws<InvalidIdException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            mgr.DuplicateToEvent (unit);
        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void DuplicateToEvent_RepoThrowsRepoException_ThrowsSame()
        {
            var unit = A.Fake<DisplayUnit> ();

            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.SaveOneToEvent (A<DisplayUnitDTO>.Ignored))
                .Throws<RepositoryException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            mgr.DuplicateToEvent (unit);
        }

        [Test]
        public void Delete_ValidId_SendsDeleteToRepo()
        {
            var repo = A.Fake<IDisplayUnitsRepository> ();
            Guid receivedGuid = Guid.Empty;
            A.CallTo (() => repo.Delete (VALID_GUID)).Invokes ((Guid g) => receivedGuid = g);
                
            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            mgr.Delete (VALID_GUID);

            Assert.That (receivedGuid, Is.EqualTo (VALID_GUID));
        }

        [Test]
        [ExpectedException(typeof(InvalidIdException))]
        public void Delete_InvalidId_RepoThrowsInvalidIdException_ThrowsSame()
        {
            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.Delete (INVALID_GUID)).Throws<InvalidIdException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            mgr.Delete (INVALID_GUID);
        }

        [Test]
        [ExpectedException(typeof(RepositoryException))]
        public void Delete_RepoThrowsException_ThrowsSame()
        {
            var repo = A.Fake<IDisplayUnitsRepository> ();
            A.CallTo (() => repo.Delete (INVALID_GUID)).Throws<RepositoryException> ();

            var mgr = new DisplayUnitsRepoManager (_fctry,repo);
            mgr.Delete (INVALID_GUID);
        }



//
//        [Test]
//        public void GetAll_ReturnsPopulatedList()
//        {
//            var dtos = Enumerable.Repeat<DisplayUnitDTO> (_dto, 5).ToList ();
//
//
//            A.CallTo (() => _repo.GetAll ()).Returns (dtos);
//            var units = _mgr.GetAll ();
//
//            Assert.That (units.Count == 5);
//            Assert.That (units [0].Id, Is.EqualTo (VALID_GUID));
//            Assert.That (units, Is.Not.Null);
//        }
//
//        [Test]
//        public void GetAll_NoneInDb_ReturnsEmptyList()
//        {
//           A.CallTo (() => _repo.GetAll ()).Returns (new List<DisplayUnitDTO> ());
//            var units = _mgr.GetAll ();
//
//            Assert.That (units.Count == 0);
//            Assert.That (units, Is.Empty);
//        }
//
//        [Test]
//        [ExpectedException("FaithEngage.Core.Exceptions.CouldNotAccessRepositoryException")]
//        public void GetAll_RepositoryException_ThrowsCouldNotAccessRepositoryException()
//        {
//           A.CallTo (() => _repo.GetAll ()).Throws<Exception> ();
//
//            var units = _mgr.GetAll ();
//        }
//
//        [Test]
//        public void SaveNew_ValidBibleUnit_SavesToRepo()
//        {
//           var bu = new BibleUnit(VALID_ID, "1 John 1:9", _refProvider)
//            {
//                
//            }
//        }
//
//
//
//        [Test]
//        public void SaveNew_ValidTextUnit_SavesToRepo()
//        {
//            var tu = new TextUnit (VALID_GUID, "This is the text for my TextUnit") {
//                DateCreated = _dt,
//                Description = "My description",
//                Name = "My Text Unit"
//            };
//
//            DisplayUnitDTO dto = null;
//
//            A.CallTo (_repo).Where(call=> call.Method.Name == "SaveNew").Invokes((DisplayUnitDTO unit) => dto=unit);
//
//            _mgr.SaveNew (tu);
//            Assert.That (dto, Is.Not.Null);
//            Assert.That (dto.Id, Is.EqualTo (tu.Id));
//            Assert.That (dto.Attributes ["text"] == "This is the text for my TextUnit");
//        }

    }
}

