using System;
using System.Collections.Generic;
using System.Drawing;
using FaithEngage.Core.Cards.Interfaces;
using FaithEngage.Core.Exceptions;
using FaithEngage.Core.PluginManagers.DisplayUnitPlugins;

namespace FaithEngage.Core.DisplayUnits
{
    public class ImageUnit : FileUnit
    {
        public ImageUnit (Dictionary<string, string> attributes) : base (attributes)
        {
        }
        

        public ImageUnit (Guid id, Dictionary<string, string> attributes) : base (id, attributes)
        {
        }
        

        #region implemented abstract members of DisplayUnit

        public override Dictionary<string, string> GetAttributes ()
        {
            throw new NotImplementedException ();
        }

        public override IRenderableCard GetCard ()
        {
            throw new NotImplementedException ();
        }

        public override void SetAttributes (Dictionary<string, string> attributes)
        {
            throw new NotImplementedException ();
        }

        public override DisplayUnitPlugin Plugin {
            get {
                throw new NotImplementedException ();
            }
        }

        #endregion




        private Image _image;


        public Image Image {
            get
            {
                if(_image == null){
                    loadImage ();
                }
                return _image;                               
            }
        }

        private void loadImage(){
            try{
                this._image = Image.FromFile (FileLocation.FullName);
            }
            catch(OutOfMemoryException ex){
                throw  new NotImageException (FileLocation.FullName, "Invalid image.",ex);
            }
        } 

        protected override bool checkExtension()
        {
            switch (FileLocation.Extension.ToLower()) {
            case ".jpg":
            case ".png":
            case ".gif":
            case ".bmp":
            case ".tiff":
                return true;
            default:
                return false;
            }
        }

        ~ImageUnit()
        {
            if(_image != null)
            {
                _image.Dispose ();
            }
        }
            
    }
}

