﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaBrowser.Library.Extensions;
using MediaBrowser.Library.Persistance;
using MediaBrowser.Model.Querying;

namespace MediaBrowser.Library.Entities
{
    public class FavoritesTypeFolder : ApiSourcedFolder<ItemQuery>
    {
        protected List<BaseItem> OurChildren;
        protected string[] OurTypes = new string[] {};
        protected string DisplayType;

        public FavoritesTypeFolder(string[] types, string display)
        {
            OurTypes = types;
            DisplayType = display;
            Id = (GetType().FullName + string.Concat(OurTypes)).GetMD5();
        }

        public override string Name
        {
            get { return Localization.LocalizedStrings.Instance.GetString("Favorite") + " " + DisplayType; }
            set
            {
                base.Name = value;
            }
        }

        public void Clear()
        {
            RetrieveChildren();
            OnChildrenChanged(null);
        }

        public override string DefaultPrimaryImagePath
        {
            get
            {
                return "resx://MediaBrowser/MediaBrowser.Resources/Fav" + DisplayType;
            }
        }

        public override bool IsFavorite
        {
            get
            {
                return false;
            }
            set
            {
                // do nothing
            }
        }

        public override BaseItem ReLoad()
        {
            return new FavoritesTypeFolder(OurTypes, DisplayType);
        }

        protected override List<BaseItem> GetCachedChildren()
        {
            var ret = Kernel.Instance.MB3ApiRepository.RetrieveItems(new ItemQuery
                                                                                                        {
                                                                                                            UserId = Kernel.CurrentUser.Id.ToString(),
                                                                                                            IncludeItemTypes = OurTypes,
                                                                                                            Recursive = true,
                                                                                                            Fields = MB3ApiRepository.StandardFields,
                                                                                                            Filters = new[] {ItemFilter.IsFavorite,}
                                                                                                        }).ToList();
            ApiRecursiveItemCount = ret.Count;
            return ret;
        }

        //protected override List<BaseItem> ActualChildren
        //{
        //    get { return OurChildren ?? (OurChildren = GetChildren()); }
        //}
    }
}
