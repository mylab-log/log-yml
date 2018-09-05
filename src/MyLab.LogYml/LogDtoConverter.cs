using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using MyLab.Logging;

namespace MyLab.LogYml
{
    static class LogDtoConverter
    {
        public static object Convert<TState>(LogLevel level, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            if (state is LogEntity)
                return state;

            object msg = state is FormattedLogValues
                ? (object) state.ToString()
                : state;

            var lw = new LogEntityWrapper
            {
                Message = msg,
                Level = level,
                Exception = exception?.ToString()
            };

            if (eventId != default(EventId))
            {
                lw.Id = $"[{eventId.Id}] {eventId.Name}";
            }

            return lw;
        }
    }

    class LogEntityWrapper
    {
        public string Id { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public LogLevel Level { get; set; }
        public object Message { get; set; }
        public string Exception { get; set; }
    }   
}
