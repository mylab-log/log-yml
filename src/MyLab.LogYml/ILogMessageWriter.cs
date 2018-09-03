using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        Task WriteMessageAsync(IEnumerable<LogEntity> messages, CancellationToken cancel);
    }
}