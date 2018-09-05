using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MyLab.LogYml
{
    /// <summary>
    /// Extension fof logging configuration
    /// </summary>
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Adds YAML logger
        /// </summary>
        public static ILoggingBuilder AddYaml(this ILoggingBuilder lBuilder)
        {
            lBuilder.Services.AddSingleton<ILoggerProvider, YamlLoggerProvider>();
            
            return lBuilder;
        }

        /// <summary>
        /// Adds YAML logger
        /// </summary>
        internal static ILoggingBuilder AddYamlSync(this ILoggingBuilder lBuilder)
        {
            lBuilder.Services.AddSingleton<ILoggerProvider, YamlLoggerProviderSync>();

            return lBuilder;
        }

        /// <summary>
        /// Adds YAML logger with options
        /// </summary>
        public static ILoggingBuilder AddYaml(this ILoggingBuilder lBuilder, Action<YamlLoggerOptions> configure)
        {
            lBuilder.AddYaml();
            lBuilder.Services.Configure(configure);

            return lBuilder;
        }
    }
}
