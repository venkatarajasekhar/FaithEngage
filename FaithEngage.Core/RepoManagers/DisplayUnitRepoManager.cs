﻿using System;
using System.Collections.Generic;
using System.Linq;
using FaithEngage.Core.DisplayUnits.Interfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.DisplayUnits;
using FaithEngage.Core.RepoInterfaces;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.RepoManagers
{
	//TODO: Implement Caching for currently live events.
    public class DisplayUnitsRepoManager : IDisplayUnitsRepoManager
    {
        private readonly IDisplayUnitsRepository _duRepo;
        private readonly IDisplayUnitFactory _factory;
        private readonly IConverterFactory<DisplayUnit,DisplayUnitDTO> _dtoFac;
        public DisplayUnitsRepoManager (IDisplayUnitFactory factory, IDisplayUnitsRepository repo, IConverterFactory<DisplayUnit,DisplayUnitDTO> dtoFactory)
        {
            _factory = factory;
            _duRepo = repo;
            _dtoFac = dtoFactory;
        }

        public DisplayUnit GetById (Guid unitId)
        {
            try {
                DisplayUnit du;
                var dto = _duRepo.GetById (unitId);
                if(dto == null) return null;
				du = _factory.Convert(dto);
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
					var du = _factory.Convert (dict [key]);
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
			var dict = new Dictionary<int, DisplayUnitDTO>();
			foreach (var u in unitsAtPositions)
			{
				var dto = _dtoFac.Convert(u.Value);
				if (dto == null) continue;
				dict.Add(u.Key, dto);
			}
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
					var du = _factory.Convert (dict [key]);
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
			var dto = _dtoFac.Convert(unit);
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
			var dto = _dtoFac.Convert(unit.Clone ());
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

        public DisplayUnit PushDU (Guid id)
        {
            try {
                var dto = _duRepo.PushDU(id);
				return _factory.Convert(dto);
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

