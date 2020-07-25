﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaBrowser.Library.Extensions;
using MediaBrowser.Library.Logging;

namespace MediaBrowser.Library.Entities
{
    public class LocalCacheFolder : IndexFolder
    {
        public virtual bool AllowRemoteChildren { get { return true; } }

        public LocalCacheFolder() : base()
        {
        }

        public LocalCacheFolder(List<BaseItem> list) : base(list)
        {
        }

        public override string DisplayMediaType
        {
            get
            {
                return string.IsNullOrEmpty(base.DisplayMediaType) ? base.DisplayMediaType : this.GetType().Name;
            }
            set
            {
                base.DisplayMediaType = value;
            }
        }

        public override BaseItem ReLoad()
        {
            return Kernel.Instance.ItemRepository.RetrieveItem(Id);
        }

        public virtual string DefaultPrimaryImagePath { get; set; }

        protected override List<BaseItem> GetCachedChildren()
        {
            List<BaseItem> items = null;
            //using (new MediaBrowser.Util.Profiler(this.Name + " child retrieval"))
            {
                //Logger.ReportInfo("Getting Children for: "+this.Name);
                var children = (Kernel.Instance.LocalRepo.RetrieveChildren(Id) ?? new List<BaseItem>()).ToList();
                var currentChildIds = children.Select(i => i.Id.ToString());
                var allChildIds = Kernel.Instance.LocalRepo.RetrieveChildList(Id);
                var neededIds = allChildIds.Except(currentChildIds);
                items = AllowRemoteChildren ? children.Concat(GetRemoteChildren(neededIds.ToArray())).ToList() : children;
            }
            return items;
        }

        /// <summary>
        /// Get the list of children locally but then the items themselves from the server
        /// </summary>
        /// <returns></returns>
        protected List<BaseItem> GetRemoteChildren(string[] ids)
        {
            if (ids.Any())
            {
                Logger.ReportVerbose("Getting children remotely for {0}", Name);
                return Kernel.Instance.MB3ApiRepository.RetrieveSpecificItems(ids).ToList();
            }
            return new List<BaseItem>();
        }

    }
}
