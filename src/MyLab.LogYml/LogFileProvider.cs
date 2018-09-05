using System;

namespace MyLab.LogYml
{
    class LogFileProvider
    {
        private readonly YamlLoggerOptions _options;
        private readonly IFsTools _fsTools;

        public LogFileProvider(YamlLoggerOptions options, IFsTools fsTools)
        {
            _options = options;
            _fsTools = fsTools;
        }
        
        public string ProvideFilename(string tag)
        {
            string dateName = $"{DateTime.Now.Year}.{DateTime.Now.Month:D2}.{DateTime.Now.Day:D2}";
            string nameEnd = string.IsNullOrWhiteSpace(tag) ? "yml" : $"{tag}.yml";
            string resultFilename;
            int index = 0;
            
            do
            {
                resultFilename = dateName +
                     (index == 0 ? "." : $".{index}.") +
                     nameEnd;
                index++;

            } while (!_fsTools.AvailableFile(resultFilename, _options.FileSizeLimit));

            return resultFilename;
        }
    }
}