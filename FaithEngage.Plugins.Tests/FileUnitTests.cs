using System;
using NUnit.Framework;
using System.IO;


namespace FaithEngage.Core.Tests
{
    [TestFixture]
    public class FileUnitTests
    {

        private string VALID_FILE = "TestingFiles" + Path.DirectorySeparatorChar + "levelcards.pdf";
        private string INVALID_FILE = "TestingFiles" + Path.DirectorySeparatorChar + "NotAFile.jpg";


        private class dummyFileUnit : FileUnit
        {
            public dummyFileUnit (string fileLocation) : base (fileLocation)
            {
            }
            public dummyFileUnit (Guid guid, string fileLocation) : base (guid, fileLocation)
            {
            }
            protected override bool checkExtension ()
            {
                return true;
            }
        }

        [Test]
        public void Ctor_ValidFileLocation_ValidFileUnit()
        {
            var fu = new dummyFileUnit (VALID_FILE);

            Assert.That (fu, Is.InstanceOf (typeof(FileUnit)));
            Assert.That (fu.FileLocation, Is.Not.Null);
            Assert.That (fu.FileLocation.Exists);
        }

        [Test]
        [ExpectedException("FaithEngage.Core.Exceptions.FileDoesNotExistException")]
        public void Ctor_InvalidFileLocation_ThrowsFileDoesNotExistException()
        {
            var fu = new dummyFileUnit (INVALID_FILE);

            Assert.That (fu, Is.InstanceOf (typeof(FileUnit)));
            Assert.That (fu.FileLocation, Is.Null);
            Assert.That (!fu.FileLocation.Exists);
        }

    }
}

