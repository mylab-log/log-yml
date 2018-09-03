using System;

namespace MyLab.LogYml.Tests
{
    class TestFsTools : IFsTools
    {
        public bool AvailableFile(string resultFilename, int optionsFileSizeLimit)
        {
            if (resultFilename == $"{DateTime.Now.Year}.{DateTime.Now.Month:D2}.{DateTime.Now.Day:D2}.foo.yml")
                return false;
            return true;
        }
    }
}