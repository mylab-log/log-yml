using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MyLab.LogYml
{
    [ProviderAlias("Yaml")]
    internal class YamlLoggerProvider : ILoggerProvider
    {
        private readonly AsyncLogging _logging;

        internal YamlLoggerOptions UsedOptions {get;}

        public YamlLoggerProvider()
            : this(new YamlLoggerOptions())
        {

        }

        public YamlLoggerProvider(IOptions<YamlLoggerOptions> options)
            : this(options.Value)
        {

        }

        public YamlLoggerProvider(YamlLoggerOptions options)
        {
            UsedOptions = options;

            _logging = new AsyncLogging(new YamlLogWriter(options), options.WriteInterval, options.DisposeWaitingInterval);
            _logging.Start();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new YamlLogger(_logging)
            {
                CategoryName = categoryName
            };
        }

        public void Dispose()
        {
            _logging.Dispose();
        }
    }
}
