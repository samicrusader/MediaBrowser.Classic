﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using MediaBrowser.Library.ImageManagement;
using MediaBrowser.Library.Threading;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;

namespace MediaBrowser.Library.Entities
{
    public class LocalIbnSourcedFolder : IndexFolder
    {
        public override string PrimaryImagePath
        {
            get { return base.PrimaryImagePath ?? (base.PrimaryImagePath = GetImagePath(ImageType.Primary)); }
            set
            {
                base.PrimaryImagePath = value;
            }
        }

        public override List<string> BackdropImagePaths
        {
            get
            {
                return base.BackdropImagePaths != null && base.BackdropImagePaths.Count > 0 ? base.BackdropImagePaths : (base.BackdropImagePaths = new List<string> { GetImagePath(ImageType.Backdrop) });
            }
            set
            {
                base.BackdropImagePaths = value;
            }
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