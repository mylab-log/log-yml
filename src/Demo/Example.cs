using System;
using Microsoft.Extensions.Logging;
using MyLab.LogDsl;

namespace Demo
{
    class Example
    {
        private readonly ILogger<Example> _log;

        public Example(ILogger<Example> log)
        {
            _log = log;
        }

        public void Example1_SimpleString()
        {
            _log.LogInformation("I did it");
        }

        public void Example2_Object()
        {
            _log.Log(LogLevel.Information, 
                new EventId(100, "Input request"), 
                new TestLogItem
                {
                    IsVip = true,
                    UserId = "12-4546",
                    Request = new TestLogItemRequestInfo
                    {
                        IP = "123.123.123.123",
                        TryCount = 10,
                        Url = "http://host/path?q=query"
                    }
                }, 
                null, 
                null);
        }

        public void Example3_Exception()
        {
            Exception captured;
            try
            {
                throw new Exception("I am bug");
            }
            catch (Exception e)
            {
                captured = e;
            }
            _log.LogError(captured, "A bug captured");
        }

        public void Example4_Dsl()
        {
            _log.Dsl().Act("Input request")
                .AndFactIs("UserId", "12-4546")
                .AndFactIs("IP", "123.123.123.123")
                .AndFactIs("TryCount", "10")
                .AndFactIs("Url", "http://host/path?q=query")
                .AndFactIs("vip")
                .Write();
        }
    }

    class TestLogItem
    {
        public string UserId { get; set; }
        public bool IsVip { get; set; }

        public TestLogItemRequestInfo Request { get; set; }
    }

    class TestLogItemRequestInfo
    {
        public string IP { get; set; }
        public string Url { get; set; }
        public int TryCount { get; set; } 
    }
}