using System;
using FaithEngage.Core.Interfaces;
using Konves.Scripture.Version;
using Konves.Scripture;
using FaithEngage.Core.Exceptions;
using System.Reflection;
using FaithEngage.AppServices.Wrappers;
using System.Text;
using System.Linq;

namespace FaithEngage.AppServices.Providers
{
    public class KonvesReferenceProvider : IReferenceProvider
    {

        private static ScriptureInfo _scriptureInfo;

        private static ScriptureInfo getScriptureInfo()
        {
            if (_scriptureInfo == null) {
                if (ScriptureInfo.TryRegister ("esv", "Data/esv.xml")) {
                    _scriptureInfo = ScriptureInfo.GetInstance ("esv");
                }else{
                    throw new Exception ("Unable to obtain ScriptureInfo object."); //TODO: Consider creating custom exception
                }
            }
            return _scriptureInfo;
        }
            
        public FaithEngage.Core.Interfaces.IReference Parse (string reference)
        {
            Reference kref;

            try {
                if(Reference.TryParse(reference,getScriptureInfo(),out kref)){
                }
                else{
                    throw new ReferenceFormatException ();
                }
            } catch(ReferenceFormatException ex){
                throw new ScriptureReferenceParseException (reference: reference, message: "Reference parse failed.", innerException: ex);
            } catch (NullReferenceException ex) {
                throw new ScriptureReferenceParseException (reference: reference, message: "Reference is missing a book or chapter.", innerException: ex);
            } catch(Exception ex){
                throw new ScriptureReferenceParseException ("Failed Parse.", reference, ex);
            }

            var outRef = new KonvesReference (kref);
            return outRef;
        }

        public string GetReference (FaithEngage.Core.Interfaces.IReference reference)
        {
            if (!reference.IsParsed)
                throw new UnParsedReferenceObjectException ();
            StringBuilder sb = new StringBuilder ();
            if(reference.HasSubReferences){
                var refs = reference.GetSubReferences ();
                var lastRef = refs.Last ();

                foreach(var indRef in refs){
                    sb.Append (string.Format ("{0:BBB c:v} - {1:BBB c:v}", indRef.FirstVerse, indRef.LastVerse));
                    if(indRef.LastVerse.ToString() != lastRef.LastVerse.ToString()){
                        sb.Append ("; ");
                    }
                }
            }else{
                sb.Append (string.Format ("{0:BBB c:v} - {1:BBB c:v}",reference.FirstVerse, reference.LastVerse));
            }
            return sb.ToString ();
        }
    }
}

