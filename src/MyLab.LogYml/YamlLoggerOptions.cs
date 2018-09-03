using System;

namespace MyLab.LogYml
{
    /// <summary>
    /// Contains yaml log files writing options
    /// </summary>
    public class YamlLoggerOptions
    {
        public static readonly TimeSpan DefaultWriteInterval = TimeSpan.FromSeconds(5); 
        public static readonly TimeSpan DefaultDisposeWaitingInterval = TimeSpan.FromSeconds(2);
        public static readonly int DefaultFileSizeLimit = 5 * 1024 * 1024;
        public static readonly string DefaultBasePath = "logs";
        //public static readonly int DefaultMaxRetainedFiles = 100;

        /// <summary>
        /// Writing interval
        /// </summary>
        public TimeSpan WriteInterval { get; set; } = DefaultWriteInterval;

        /// <summary>
        /// Dispose writing interval
        /// </summary>
        public TimeSpan DisposeWaitingInterval { get; set; } = DefaultDisposeWaitingInterval;

        /// <summary>
        /// Defines max available file size
        /// </summary>
        public int FileSizeLimit { get; set; } = DefaultFileSizeLimit;

        /// <summary>
        /// The base log file path
        /// </summary>
        public string BasePath { get; set; } = DefaultBasePath;

        ///// <summary>
        ///// Defines how many files number may be contains in log folder
        ///// </summary>
        //public int MaxRetainedFiles { get; set; } = DefaultMaxRetainedFiles;

        /// <summary>
        /// Gets true if all properties are equals to default values
        /// </summary>
        public bool IsDefault()
        {
            return BasePath == DefaultBasePath &&
                   WriteInterval == DefaultWriteInterval &&
                   DisposeWaitingInterval == DefaultDisposeWaitingInterval &&
                   FileSizeLimit == DefaultFileSizeLimit/* &&
                   MaxRetainedFiles == DefaultMaxRetainedFiles*/;
        }
    }
}