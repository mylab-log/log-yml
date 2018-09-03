using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyLab.Logging;

namespace MyLab.LogYml
{
    class AsyncLogging : IDisposable, ILogMessageQueue
    {
        private readonly ILogMessageWriter _logMessageWriter;
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        private readonly BlockingCollection<LogEntity> _logQueue = new BlockingCollection<LogEntity>();

        private Task _wrtTask;

        public TimeSpan WriteInterval { get; }
        
        public TimeSpan DisposeWaitingInterval { get; }

        public AsyncLogging(ILogMessageWriter logMessageWriter, TimeSpan writeInterval, TimeSpan disposeInterval)
        {
            _logMessageWriter = logMessageWriter ?? throw new ArgumentNullException(nameof(logMessageWriter));
            WriteInterval = writeInterval;
            DisposeWaitingInterval = disposeInterval;
        }

        public void Start()
        {
            _wrtTask = Task.Factory.StartNew(ProcessLogQueue, null, TaskCreationOptions.LongRunning);
        }

        private async Task ProcessLogQueue(object state)
        {
            do
            {
                var batch = new List<LogEntity>();

                while (_logQueue.TryTake(out var item))
                {
                    batch.Add(item);
                }

                if (batch.Count != 0)
                {
                    await _logMessageWriter.WriteMessageAsync(batch, _cancel.Token);
                }

                await Task.Delay(WriteInterval, _cancel.Token);
            } while (!_cancel.IsCancellationRequested);
        }

        public void Dispose()
        {
            _logQueue.CompleteAdding();
            _cancel.Cancel();

            try
            {
                _wrtTask.Wait(DisposeWaitingInterval);

                if (_logQueue.Count != 0)
                {
                    var cts = new CancellationTokenSource(DisposeWaitingInterval);
                    var finalWriteTask =_logMessageWriter.WriteMessageAsync(_logQueue, cts.Token);

                    try
                    {
                        finalWriteTask.Wait(cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        
                    }
                }
            }
            finally
            {
                _cancel.Dispose();
            }
        }

        public void Push(LogEntity message)
        {
            _logQueue.TryAdd(message);
        }
    }
}
