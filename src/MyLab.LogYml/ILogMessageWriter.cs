using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyLab.Logging;

namespace MyLab.LogYml
{
    /// <summary>
    /// Determines log entity writer
    /// </summary>
    public interface ILogMessageWriter
    {
        /// <summary>
        /// Write log entity async
        /// </summary>
        Task WriteMessageAsync(IEnumerable<LogMessageToWrite> messages, CancellationToken cancel);
    }

    /// <summary>
    /// Contains data to write into log
    /// </summary>
    public class LogMessageToWrite
    {
        /// <summary>
        /// Log message object
        /// </summary>
        public object Message { get; }

        /// <summary>
        /// Log level
        /// </summary>
        public LogLevel Level { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="LogMessageToWrite"/>
        /// </summary>
        public LogMessageToWrite(object message, LogLevel level)
        {
            Message = message;
            Level = level;
        }
    }
}