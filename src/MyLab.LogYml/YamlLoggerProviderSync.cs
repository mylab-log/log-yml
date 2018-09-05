using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MyLab.LogYml
{
    internal class YamlLoggerProviderSync : ILoggerProvider
    {
        internal YamlLoggerOptions UsedOptions { get; }

        public YamlLoggerProviderSync()
            : this(new YamlLoggerOptions())
        {

        }

        public YamlLoggerProviderSync(IOptions<YamlLoggerOptions> options)
            : this(options.Value)
        {

        }

        public YamlLoggerProviderSync(YamlLoggerOptions options)
        {
            UsedOptions = options;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new YamlLoggerSync(new YamlLogWriter(UsedOptions))
            {
                CategoryName = categoryName
            };
        }

        public void Dispose()
        {
        }
    }
}