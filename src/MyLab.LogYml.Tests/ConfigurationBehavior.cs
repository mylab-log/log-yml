using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace MyLab.LogYml.Tests
{
    public class ConfigurationBehavior
    {
        [Fact]
        public void ShouldGetDefaultOptionsWhenNotSpecified()
        {
            //Arrange
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddLogging(c =>
            {
                c.AddYaml();
            });

            var sc = serviceCollection.BuildServiceProvider();
            var yl = (YamlLoggerProvider)sc.GetService(typeof(ILoggerProvider));
            
            //Act
            

            //Assert
            Assert.NotNull(yl);
            Assert.True(yl.UsedOptions.IsDefault());
        }

        [Fact]
        public void ShouldSetCustomOptionsWhenSpecified()
        {
            //Arrange
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(c =>
            {
                c.AddYaml(o => o.DisposeWaitingInterval = TimeSpan.Zero);
            });

            var sc = serviceCollection.BuildServiceProvider();
            var yl = (YamlLoggerProvider)sc.GetService(typeof(ILoggerProvider));

            //Act


            //Assert
            Assert.NotNull(yl);
            Assert.Equal(TimeSpan.Zero, yl.UsedOptions.DisposeWaitingInterval);
        }
    }
}
