using System.IO;

namespace MyLab.LogYml
{
    interface IYamlLogWriterOpenFileStrategy
    {
        StreamWriter OpenFile(string filename);
    }

    class DefaultYamlLogWriterOpenFileStrategy : IYamlLogWriterOpenFileStrategy
    {
        public StreamWriter OpenFile(string filename)
        {
            return File.AppendText(filename);
        }
    }
}