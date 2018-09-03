using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyLab.Logging;
using YamlDotNet.Serialization;

namespace MyLab.LogYml
{
    class YamlLogWriter : ILogMessageWriter
    {
        private readonly YamlLoggerOptions _options;
        private readonly IYamlLogWriterOpenFileStrategy _openFileStrategy;

        public YamlLogWriter(YamlLoggerOptions options, IYamlLogWriterOpenFileStrategy openFileStrategy)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _openFileStrategy = openFileStrategy ?? throw new ArgumentNullException(nameof(openFileStrategy));
        }

        public YamlLogWriter(YamlLoggerOptions options)
            :this(options, new DefaultYamlLogWriterOpenFileStrategy())
        {
        }

        public async Task WriteMessageAsync(IEnumerable<LogEntity> messages, CancellationToken cancel)
        {
            var sb = new SerializerBuilder()
                .WithEventEmitter(emitter => new NullObjectAsEmptyYamlEventEmitter(emitter));
            var s = sb.Build();

            var fileProvider = new LogFileProvider(_options, new DefaultFsTools());
            
            foreach (var group in messages.GroupBy(GetFilenameTag, m => m))
            {
                var fn = fileProvider.ProvideFilename(group.Key);
                var fp = _options.BasePath == null 
                    ? fn 
                    : Path.Combine(_options.BasePath, fn);

                try
                {
                    var dir = Path.GetDirectoryName(fp);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    using (var file = _openFileStrategy.OpenFile(fp))
                    {
                        foreach (var logEntity in group)
                        {
                            //if(cancel.IsCancellationRequested)
                            //    break;

                            var str = s.Serialize(logEntity);

                            await file.WriteLineAsync(str);
                            await file.WriteLineAsync();
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.ToString());
                }
            }
        }

        string GetFilenameTag(LogEntity logEntity)
        {
            if (logEntity.Markers.Contains(Markers.Error))
                return "error";
            if (logEntity.Markers.Contains(Markers.Debug))
                return "debug";
            return "info";
        }
    }
}