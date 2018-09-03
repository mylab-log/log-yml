using System;
using Xunit;

namespace MyLab.LogYml.Tests
{
    public class LogFileProviderBehavior
    {
        [Fact]
        public void ShouldProvideFileNameWithTagAtTheEnd()
        {
            //Arrange
            var p = new LogFileProvider(new YamlLoggerOptions(), new TestFsTools());

            //Act
            var filename = p.ProvideFilename("foo");

            //Assert
            Assert.EndsWith(".foo.yml", filename);
        }

        [Fact]
        public void ShouldCurrentDateInFilename()
        {
            //Arrange
            var p = new LogFileProvider(new YamlLoggerOptions(), new TestFsTools());

            //Act
            var filename = p.ProvideFilename("foo");

            //Assert
            Assert.StartsWith($"{DateTime.Now.Year}.{DateTime.Now.Month:D2}.{DateTime.Now.Day:D2}", filename);
        }

        [Fact]
        public void ShouldIncrementFileIndexIfOversize()
        {
            //Arrange
            var p = new LogFileProvider(new YamlLoggerOptions(), new TestFsTools());
            string expectedEnd = ".1.foo.yml";

            //Act
            var filename = p.ProvideFilename("foo");

            //Assert
            Assert.EndsWith(expectedEnd, filename);
        }
    }
}
