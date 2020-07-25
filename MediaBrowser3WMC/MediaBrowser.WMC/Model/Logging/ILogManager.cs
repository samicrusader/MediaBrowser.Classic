﻿using System;

namespace MediaBrowser.Model.Logging
{
    /// <summary>
    /// Interface ILogManager
    /// </summary>
    public interface ILogManager
    {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ILogger.</returns>
        ILogger GetLogger(string name);

        /// <summary>
        /// Reloads the logger.
        /// </summary>
        void ReloadLogger(LogSeverity severity);

        /// <summary>
        /// Gets the log file path.
        /// </summary>
        /// <value>The log file path.</value>
        string LogFilePath { get; }

        /// <summary>
        /// Occurs when [logger loaded].
        /// </summary>
        event EventHandler LoggerLoaded;
    }
}
