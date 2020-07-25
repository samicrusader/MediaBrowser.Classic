﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.IO;
using MediaBrowser.LibraryManagement;
using MediaBrowser.Library.Localization;
using MediaBrowser.Library.Configuration;

namespace MediaBrowser.Library
{
    public class Ratings
    {
        private static Dictionary<string, int> _ratings;
        private static readonly Dictionary<int, string> ratingsStrings = new Dictionary<int, string>();
        private static ConfigData _config;
        private static ConfigData Config
        {
            get { return _config ?? (_config = Kernel.Instance.ConfigData); }
        }

        public Ratings(bool blockUnrated)
        {
            if (_ratings == null) Initialize(blockUnrated);
        }

        public Ratings()
        {
            if (_ratings == null) Initialize(false);
        }

        /// <summary>
        /// Use this constructor when calling before kernel is initialized
        /// </summary>
        /// <param name="cfg"></param>
        public Ratings(ConfigData cfg)
        {
            _config = cfg;
            if (_ratings== null) Initialize(false);
        }

        public void Initialize(bool blockUnrated)
        {
            if (_ratings != null) return;  //already intitialized

            //We get our ratings from the server
            _ratings = new Dictionary<string, int>();
            try
            {
                foreach (var rating in Kernel.ApiClient.GetParentalRatings())
                {
                    _ratings.Add(rating.Name, rating.Value);
                }
            }
            catch (Exception e)
            {
                Logging.Logger.ReportException("Error retrieving ratings from server",e);
                return;
            }

            try
            {
                _ratings.Add("", blockUnrated ? 1000 : 0);
            }
            catch (Exception e)
            {
                Logging.Logger.ReportException("Error adding blank value to ratings", e);
            }
            //and rating reverse lookup dictionary (non-redundant ones)
            ratingsStrings.Clear();
            int lastLevel = -10;
            ratingsStrings.Add(-1,LocalizedStrings.Instance.GetString("Any"));
            foreach (var pair in _ratings.OrderBy(p => p.Value))
            {
                if (pair.Value > lastLevel)
                {
                    lastLevel = pair.Value;
                    try
                    {
                        ratingsStrings.Add(pair.Value, pair.Key);
                    }
                    catch (Exception e)
                    {
                        Logging.Logger.ReportException("Error adding "+pair.Value+" to ratings strings", e);
                    }
                }
            }

        }

        public void SwitchUnrated(bool block)
        {
            _ratings.Remove("");
            if (block)
            {
                _ratings.Add("", 1000);
            }
            else
            {
                _ratings.Add("", 0);
            }
        }
        public static int Level(string ratingStr)
        {
            if (ratingStr != null && _ratings.ContainsKey(ratingStr))
                return _ratings[ratingStr];
            else
            {
                string stripped = stripCountry(ratingStr);
                if (ratingStr != null && _ratings.ContainsKey(stripped))
                    return _ratings[stripped];
                else
                    return _ratings[""]; //return "unknown" level
            }
        }

        private static string stripCountry(string rating)
        {
            int start = rating.IndexOf('-');
            return start > 0 ? rating.Substring(start + 1) : rating;
        }

        public static string ToString(int level)
        {
            //return the closest one
            while (level > 0) 
            {
                if (ratingsStrings.ContainsKey(level))
                    return ratingsStrings[level];
                else 
                    level--;
            }
            return ratingsStrings.Values.FirstOrDefault(); //default to first one
        }
        public List<string> ToStrings()
        {
            //return the whole list of ratings strings
            return ratingsStrings.Values.ToList();
        }

        public List<int> ToValues()
        {
            //return the whole list of ratings values
            return ratingsStrings.Keys.ToList();
        }

        public Microsoft.MediaCenter.UI.Image RatingImage(string rating)
        {
            return Helper.GetMediaInfoImage("Rated_" + rating);
        }


    }
}
