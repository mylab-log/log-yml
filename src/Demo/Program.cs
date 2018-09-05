using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyLab.LogYml;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(c => c.AddYamlSync().SetMinimumLevel(LogLevel.Debug));
            serviceCollection.Configure<YamlLoggerOptions>(o =>
            {
                o.BasePath = "logs";
                o.WriteInterval = TimeSpan.FromSeconds(5);
                o.DisposeWaitingInterval = TimeSpan.FromSeconds(2);
                o.FileSizeLimit = 5 * 1024 * 1024;
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var lf = serviceProvider.GetService<ILoggerFactory>();
            var logger = lf.CreateLogger<Example>();

            var example = new Example(logger);

            example.Example1_SimpleString();
            example.Example2_Object();
            example.Example3_Exception();
            example.Example4_Dsl();

            
        }
    }
}
