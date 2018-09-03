using System.IO;

namespace MyLab.LogYml
{
    interface IFsTools
    {
        bool AvailableFile(string resultFilename, int optionsFileSizeLimit);
    }

    class DefaultFsTools : IFsTools
    {
        public bool AvailableFile(string resultFilename, int optionsFileSizeLimit)
        {
            var fi = new FileInfo(resultFilename);

            return !fi.Exists || fi.Length < optionsFileSizeLimit - 1 * 1024;
        }
    }
}