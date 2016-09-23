using System;
using Konves.Scripture;
using System.Linq;
using FaithEngage.CorePlugins.Interfaces;

namespace FaithEngage.AppServices.Wrappers
{
    public class KonvesReference : IReference
    {

        private Reference _reference;


        public bool IsParsed {
            get;
            set;
        }

        public KonvesReference (Reference reference)
        {
            if(reference.Length > 0){
                _reference = reference;
                IsParsed = true;
            }
            else{
                IsParsed = false;
            }

        }

        #region interface implementations

        public System.Collections.Generic.IEnumerable<IReference> GetSubReferences ()
        {
            if(HasSubReferences){
                return _reference.GetSubReferences ().Select(r => new KonvesReference(r));
            }
            return null;
        }

        public bool HasSubReferences {
            get {
                return _reference.HasSubReferences;
            }
        }

        public object LastVerse {
            get {
                return _reference.LastVerse;
            }
        }

        public object FirstVerse {
            get {
                return _reference.FirstVerse;
            }
        }

        public int Length{
            get{
                return _reference.Length;
            }
        }

        #endregion
    }
}

