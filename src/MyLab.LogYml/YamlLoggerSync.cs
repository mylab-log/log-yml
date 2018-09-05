using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using MyLab.Logging;

namespace MyLab.LogYml
{
    class YamlLoggerSync : ILogger
    {
        private readonly List<object> _scopes = new List<object>();
        private readonly ILogMessageWriter _logMessageWriter;

        public string CategoryName { get; set; }

        public YamlLoggerSync(ILogMessageWriter logMessageWriter)
        {
            _logMessageWriter = logMessageWriter ?? throw new ArgumentNullException(nameof(logMessageWriter));
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            var msg = LogDtoConverter.Convert(logLevel, eventId, state, exception, formatter);
            var msgToWrite = new LogMessageToWrite(msg, logLevel);
            var writeTask = _logMessageWriter.WriteMessageAsync(Enumerable.Repeat(msgToWrite, 1), CancellationToken.None);
            writeTask.Wait(CancellationToken.None);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            _scopes.Add(state);
            return new ScopeRollback(_scopes, state);
        }
    }
}