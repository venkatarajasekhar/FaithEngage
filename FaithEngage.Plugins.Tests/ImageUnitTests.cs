using System;
using NUnit.Framework;
using FaithEngage.Core.DisplayUnits;
using System.Drawing;
using System.IO;


namespace FaithEngage.Core.Tests
{
    [TestFixture]
    public class ImageUnitTests
    {

        private string VALID_IMAGE_FILE = "TestingFiles" + Path.DirectorySeparatorChar + "cross_sunset-26.jpg";
        private string VALID_FILE_NOT_IMAGE = "TestingFiles" + Path.DirectorySeparatorChar + "levelcards.pdf";
        private string INVALID_FILE = "TestingFiles" + Path.DirectorySeparatorChar + "NotAFile.jpg";
        private string VALID_FILE_IMAGE_EXTENSION_NOT_IMAGE = "TestingFiles" + Path.DirectorySeparatorChar + "SermonWk5.jpg";


        [Test]
        [ExpectedException("FaithEngage.Core.Exceptions.InvalidFileTypeException")]
        public void Ctor_ValidFileNotImage_NotImageException()
        {
            var iu = new ImageUnit (VALID_FILE_NOT_IMAGE);
        }

        [Test]
        [ExpectedException("FaithEngage.Core.Exceptions.FileDoesNotExistException")]
        public void Ctor_InvalidFile_FileDoesNotExistException()
        {
            var iu = new ImageUnit (INVALID_FILE);
        }

        [Test]
        [ExpectedException("FaithEngage.Core.Exceptions.NotImageException")]
        public void ImageProp_ValidFileAlteredExtension_ThrowsNotImageException()
        {
            var iu = new ImageUnit (VALID_FILE_IMAGE_EXTENSION_NOT_IMAGE);
            var image = iu.Image;
        }

        [Test]
        public void Ctor_ValidImageFile_ValidImageUnit()
        {
            var iu = new ImageUnit (VALID_IMAGE_FILE);
            Assert.That (iu, Is.InstanceOf (typeof(ImageUnit)));
        }
           
    }
}

