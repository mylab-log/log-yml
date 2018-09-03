using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MyLab.Logging;

namespace MyLab.LogYml
{
    class YamlLogger : ILogger
    {
        private readonly ILogMessageQueue _logMessageQueue;
        private readonly List<object> _scopes = new List<object>();

        public string CategoryName { get; set; }

        public YamlLogger(ILogMessageQueue logMessageQueue)
        {
            _logMessageQueue = logMessageQueue;
        }

        public void Log<TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            LogEntity le = state as LogEntity;
            if(le == null)
            {
                le = new LogEntity
                {
                    EventId = eventId.Id,
                    InstanceId = Guid.NewGuid(),
                    DateTime = DateTime.Now
                };

                switch (logLevel)
                {
                    case LogLevel.Trace:
                    case LogLevel.Debug:
                        le.Markers.Add(Markers.Debug);
                        break;
                    case LogLevel.Error:
                    case LogLevel.Critical:
                        le.Markers.Add(Markers.Error);
                        break;
                    case LogLevel.Information:
                    case LogLevel.None:
                    case LogLevel.Warning:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                }
            }
            
            le.Message = string.IsNullOrWhiteSpace(eventId.Name) 
                ? le.Message = formatter(state, exception)
                : le.Message = eventId.Name + "." + formatter(state, exception);

            _logMessageQueue.Push(le);
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