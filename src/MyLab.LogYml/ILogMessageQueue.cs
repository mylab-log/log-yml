using MyLab.Logging;

namespace MyLab.LogYml
{
    interface ILogMessageQueue
    {
        void Push(LogMessageToWrite msgToWrite);
    }
}