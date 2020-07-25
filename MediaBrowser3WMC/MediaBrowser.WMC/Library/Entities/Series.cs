﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaBrowser.Library.Interfaces;
using MediaBrowser.Library.Persistance;
using MediaBrowser.Library.Localization;

namespace MediaBrowser.Library.Entities {
    public class Series : Folder, IContainer, IDetailLoad {

        private readonly object _detailLock = new object();

        [Persist]
        public string MpaaRating { get; set; }

        [Persist]
        private List<Actor> _actors;

        [Persist]
        public List<Actor> Actors
        {
            get
            {
                if (_actors == null && !FullDetailsLoaded)
                {
                    LoadFullDetails();
                }
                return _actors;
            }

            set { _actors = value; }
        }

        private List<string> _directors;

        [Persist]
        public List<string> Directors
        {
            get
            {
                if (_directors == null && !FullDetailsLoaded)
                {
                    LoadFullDetails();
                }
                return _directors;
            }

            set { _directors = value; }
        }

        private List<string> _genres;

        [Persist]
        public List<string> Genres
        {
            get
            {
                if (_genres == null && !FullDetailsLoaded)
                {
                    LoadFullDetails();
                }
                return _genres;
            }

            set { _genres = value; }
        }

        private List<string> _studios;

        [Persist]
        public virtual List<string> Studios
        {
            get
            {
                if (_studios == null && !FullDetailsLoaded)
                {
                    LoadFullDetails();
                }
                return _studios;
            }

            set { _studios = value; }
        }

        [Persist]
        public int? RunningTime { get; set; }

        [Persist]
        public string Status { get; set; }

        [Persist]
        public string TVDBSeriesId { get; set; }

        [Persist]
        public string AspectRatio { get; set; }

        [Persist]
        public string AirDay { get; set; }

        [Persist]
        public string AirTime { get; set; }

        //no persist so we don't muck the cache - this isn't presently used as 'series' don't have a single year
        // but we need it to be compatable with index creation
        public int? ProductionYear { get; set; }

        //doesn't make sense to index a series...
        private Dictionary<string, string> ourIndexOptions = new Dictionary<string, string>() {
            {LocalizedStrings.Instance.GetString("NoneDispPref"), ""}
        };

        public override Dictionary<string, string> IndexByOptions
        {
            get
            {
                return ourIndexOptions;
            }
            set
            {
                ourIndexOptions = value;
            }
        }

        public override string OfficialRating
        {
            get
            {
                return MpaaRating ?? "None";
            }
            set { MpaaRating = value; }
        }

        public override List<ThemeItem> ThemeSongs
        {
            get
            {
                if (base.ThemeSongs == null) LoadThemes();
                return base.ThemeSongs;
            }
            set
            {
                base.ThemeSongs = value;
            }
        }

        public override List<ThemeItem> ThemeVideos
        {
            get
            {
                if (base.ThemeVideos == null) LoadThemes();
                return base.ThemeVideos;
            }
            set
            {
                base.ThemeVideos = value;
            }
        }

        public void LoadFullDetails()
        {
            lock (_detailLock)
            {
                if (FullDetailsLoaded) return;

                var temp = Kernel.Instance.MB3ApiRepository.RetrieveItem(this.Id) as Series;
                if (temp != null)
                {
                    temp.FullDetailsLoaded = true;
                    Actors = temp.Actors;
                    Directors = temp.Directors;
                    Genres = temp.Genres;
                    Studios = temp.Studios;
                    PlaybackAllowed = temp.PlaybackAllowed;
                    CanDelete = temp.CanDelete;
                }

                FullDetailsLoaded = true;
            }
        }

        //used as a valid blank item so MCML won't blow chow
        public static Series BlankSeries = new Series() { 
            Name = "Unknown", 
            Studios = new List<string>(), 
            Genres = new List<string>(), 
            Directors = new List<string>(),
            Actors = new List<Actor>(),
            ImdbRating = 0,
            Status = "Unknown",
            RunningTime = 0,
            TVDBSeriesId = "",
            ProductionYear = 1950,
            AspectRatio = "",
            MpaaRating = "" };

        public override Series OurSeries
        {
            get
            {
                return this;
            }
        }
    }
}
