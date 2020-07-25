﻿using System.IO;

namespace MediaBrowser.Model.IO
{
    /// <summary>
    /// Interface IZipClient
    /// </summary>
    public interface IZipClient
    {
        /// <summary>
        /// Extracts all.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetPath">The target path.</param>
        /// <param name="overwriteExistingFiles">if set to <c>true</c> [overwrite existing files].</param>
        void ExtractAll(string sourceFile, string targetPath, bool overwriteExistingFiles);

        /// <summary>
        /// Extracts all.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="targetPath">The target path.</param>
        /// <param name="overwriteExistingFiles">if set to <c>true</c> [overwrite existing files].</param>
        void ExtractAll(Stream source, string targetPath, bool overwriteExistingFiles);
    }
}
