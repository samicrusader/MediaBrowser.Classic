﻿using System;
using System.IO;

namespace MediaBrowser.ApiInteraction
{
    /// <summary>
    /// Interface IHttpClient
    /// </summary>
    public interface IHttpClient : IDisposable
    {
        /// <summary>
        /// Sets the authorization header that should be supplied on every request
        /// </summary>
        /// <param name="header"></param>
        void SetAuthorizationHeader(string header);

        /// <summary>
        /// Sets the authorization token that should be supplied on every request
        /// </summary>
        /// <param name="token"></param>
        void SetAuthorizationToken(string token);

        int Timeout { get; set; }

        /// <summary>
        /// Gets the stream async.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Task{Stream}.</returns>
        Stream Get(string url);

        /// <summary>
        /// Deletes the async.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Task.</returns>
        void Delete(string url);
        
        /// <summary>
        /// Posts the async.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="postContent">Content of the post.</param>
        /// <returns>string (the result).</returns>
        string Post(string url, string contentType, string postContent);
    }
}
