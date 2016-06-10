using System;
using System.Collections.Generic;
using System.Linq;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.RepoInterfaces;

namespace FaithEngage.Core.RepoManagers
{
	public class DisplayUnitsRepoManager : IDisplayUnitsRepoManager
    {
        private readonly IDisplayUnitsRepository _duRepo;
        private readonly IDisplayUnitFactory _factory;
        public DisplayUnitsRepoManager (IDisplayUnitFactory factory, IDisplayUnitsRepository repo)
        {
            _factory = factory;
            _duRepo = repo;
        }

        public DisplayUnit GetById (Guid unitId)
        {
            try {
                DisplayUnit du;
                var dto = _duRepo.GetById (unitId);
                if(dto == null) return null;
                du = _factory.ConvertFromDto(dto);
                return du;
            } catch (RepositoryException ex) {
                throw new RepositoryException ("There was a problem accessing the DisplayUnitRepository.", ex);
            }

        }
            
        public Dictionary<int, DisplayUnit> GetByEvent (Guid eventId, bool onlyPushed = false)
        {
            try {
                var returnDict = new Dictionary<int,DisplayUnit> ();
                var dict = _duRepo.GetByEvent (eventId, onlyPushed);
                if (dict == null)
                    return null;
                foreach(var key in dict.Keys)
                {
                    var du = _factory.ConvertFromDto (dict [key]);
                    if(du == null) continue;
                    returnDict.Add (key, du);
                }
                ensurePositions(returnDict);
                return returnDict;
            } catch (RepositoryException ex) {
                throw new RepositoryException ("There was a problem accessing the DisplayUnitRepository", ex);
            }

        }

        public void SaveManyToEvent (Dictionary<int, DisplayUnit> unitsAtPositions, Guid eventId)
        {
            ensurePositions (unitsAtPositions);
            var dict = (
                        from u in unitsAtPositions
                        select new
                        {
                            k = u.Key,
                            v = convertToDTO(u.Value)
                        }).ToDictionary (p => p.k, p => p.v);
            try {
                _duRepo.SaveManyToEvent (dict, eventId);
            }
            catch(InvalidIdException){
                throw;
            } 
            catch (RepositoryException ex) {
                throw new RepositoryException (
                    "There was a problem saving the display units to the event.", ex);
            }

        }

        private void ensurePositions(Dictionary<int, DisplayUnit>  dict)
        {
            foreach(var key in dict.Keys.ToArray())
            {
                dict [key].PositionInEvent = key;
            }
        }

        public Dictionary<int, DisplayUnit> GetGroup (Guid eventId, Guid groupId)
        {
            var returnDict = new Dictionary<int,DisplayUnit> ();
            try {
                var dict = _duRepo.GetGroup (eventId, groupId);
                if(dict == null) return null;
                foreach(var key in dict.Keys)
                {
                    var du = _factory.ConvertFromDto (dict [key]);
                    returnDict.Add (key, du);
                }
            } catch (InvalidIdException) {
                throw;
            } catch (RepositoryException ex) {
                throw new RepositoryException (
                    "There was a problem accessing the DisplayUnitsRepository.", ex);
            }
            ensurePositions (returnDict);
            return returnDict;
        }

        public void SaveOneToEvent (DisplayUnit unit)
        {
            var dto = convertToDTO (unit);
            try{
                _duRepo.SaveOneToEvent (dto);
            }catch(InvalidIdException){
                throw;
            }catch(RepositoryException ex){
                throw new RepositoryException ("There was a problem saving to the DisplayUnitsRepository", ex);
            }
        }

        public void SaveDtoToEvent (DisplayUnitDTO dto)
        {
            try {
                _duRepo.SaveOneToEvent(dto);
            } catch (InvalidIdException) {
                throw;
            } catch (RepositoryException ex){
                throw new RepositoryException ("There was a problem saving to the DisplayUnitsRepository", ex);
            }
        }

        public void DuplicateToEvent (DisplayUnit unit)
        {
            var dto = convertToDTO (unit.Clone ());
            try{
                _duRepo.SaveOneToEvent (dto);
            }catch(InvalidIdException){
                throw;
            }catch(RepositoryException ex){
                throw new RepositoryException ("There was a problem saving to the DisplayUnitsRepository", ex);
            }
        }

        public void Delete (Guid unitId)
        {
            try {
                _duRepo.Delete (unitId);
            } catch (InvalidIdException) {
                throw;
            } catch (RepositoryException ex) {
                throw new RepositoryException("There was a problem deleting this Id.", ex);
            }
        }

        private DisplayUnitDTO convertToDTO(DisplayUnit unit)
        {
            var dto = new DisplayUnitDTO (unit.AssociatedEvent, unit.Id);
            dto.Name = unit.Name;
            dto.DateCreated = unit.DateCreated;
            dto.Description = unit.Description;
			dto.PluginId = unit.Plugin.PluginId.Value;
            dto.PositionInEvent = unit.PositionInEvent;
            dto.Attributes = unit.GetAttributes ();
            if(unit.UnitGroup.HasValue)
            {
                dto.PositionInGroup = unit.UnitGroup.Value.Position;
                dto.GroupId = unit.UnitGroup.Value.Id;
            }
            return dto;
        }


        public DisplayUnit PushDU (Guid id)
        {
            try {
                var dto = _duRepo.PushDU(id);
                return _factory.ConvertFromDto(dto);
            } catch (Exception ex) {
                throw;
            }
        }
        public DisplayUnit PullDu (Guid id)
        {
            throw new NotImplementedException ();
        }
    }
}

