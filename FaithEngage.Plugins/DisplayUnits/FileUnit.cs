using System;
using System.IO;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.DisplayUnits;

namespace FaithEngage.Core
{
    abstract public class FileUnit : DisplayUnit
    {
        public FileUnit (System.Collections.Generic.Dictionary<string, string> attributes) : base (attributes)
        {
        }
        

        public FileUnit (Guid id, System.Collections.Generic.Dictionary<string, string> attributes) : base (id, attributes)
        {
        }
        


//        protected FileUnit (string fileLocation)
//        {
//            setFileLocation (fileLocation);
//            if(!checkExtension()) throw new InvalidFileTypeException();
//        }
//        
//
//        public FileUnit (Guid guid, string fileLocation)
//        {
//            setFileLocation (fileLocation);
//            if(!checkExtension()) throw new InvalidFileTypeException();
//        }

        public FileInfo FileLocation {
            get;
            private set;
        }
            

        protected void setFileLocation (string fileLocation)
        {
            var baseDir = Directory.GetCurrentDirectory ();
            if(File.Exists(fileLocation)){
                FileLocation = new FileInfo(fileLocation);
            }else if(File.Exists(baseDir + Path.DirectorySeparatorChar + fileLocation)){
                FileLocation = new FileInfo(baseDir + Path.DirectorySeparatorChar + fileLocation);
            }else{
                throw new FileDoesNotExistException (fileLocation);
            }
        }

        abstract protected bool checkExtension ();
        
    }
}

