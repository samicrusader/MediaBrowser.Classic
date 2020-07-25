﻿using System.Collections.Generic;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;

namespace MediaBrowser.Library.Entities
{
    public class IbnSourcedFolder : Folder
    {
        protected virtual bool ForceIbn { get { return false; } }
        public override string PrimaryImagePath
        {
            get { return !ForceIbn ? base.PrimaryImagePath ?? (base.PrimaryImagePath = GetImagePath(ImageType.Primary)) : (base.PrimaryImagePath = GetImagePath(ImageType.Primary) ?? base.PrimaryImagePath); }
            set
            {
                base.PrimaryImagePath = value;
            }
        }

        public override List<string> BackdropImagePaths
        {
            get
            {
                return !ForceIbn ? base.BackdropImagePaths != null && base.BackdropImagePaths.Count > 0 ? base.BackdropImagePaths : (base.BackdropImagePaths = GetDefaultIbnImage(ImageType.Backdrop)) : (base.BackdropImagePaths = GetDefaultIbnImage(ImageType.Backdrop));
            }
            set
            {
                base.BackdropImagePaths = value;
            }
        }

        protected List<string> GetDefaultIbnImage(ImageType type)
        {
            var path = GetImagePath(type);
            return path != null ? new List<string> {GetImagePath(type)} : new List<string>();
        } 

        public override BaseItem ReLoad()
        {
            var ret = base.ReLoad();
            ReCacheAllImages(); // we have no way to see changes
            return ret;
        }

        protected virtual string GetImagePath(ImageType imageType)
        {
            if (this.Name == null) return null;

            //Look for it on the server IBN
            return Kernel.ApiClient.GetGeneralIbnImageUrl(this.Name, new ImageOptions { ImageType = imageType });

        }
    }
}