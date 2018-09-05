# MyLab.LogYml

For .NET Core 2.1+

Writes logs in files with YAML format. Integrates in .NET Core build-in logging.

Log messages writes into split files depends on log level. Filename contains following items:
* date
* type
* index

There are three log type files:
* err - for Critical and Error
* dbg - for Trace and Debug
* [empty] - fro other levels

Examples:
* `2018.09.05.yml` - info log
* `2018.09.05.err.yml` - error log
* `2018.09.05.err.1.yml` - info log when previous file rich the limit
* `2018.09.05.dbg.yml` - debug log

## Beginning

To integrate `YAML logging` you mast call `AddYaml` method for logging builder when configure services:

```C#
public class Startup
{
    // ....

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
        services.AddLogging(c => c.AddYaml());
    }

    // ....
}
```



## String

The following example shows how to interpret a string message in a log file:

```C#
_log.LogInformation("I did it");
```

File record:

```
Time: 2018-09-05T09:24:26.8884151+03:00
Level: Information
Message: I did it
```



## Object

An object also can be log message payload:

```C#
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
```

File record:

```
Id: '[100] Input request'
Time: 2018-09-05T09:47:09.1401014+03:00
Level: Information
Message:
  UserId: 12-4546
  IsVip: true
  Request:
    IP: 123.123.123.123
    Url: http://host/path?q=query
    TryCount: 10
```



## Exception

The following example shows how to interpret an error message in a log file:

```C#
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
```

File record:

```
Time: 2018-09-05T09:57:02.0674054+03:00
Level: Error
Message: A bug captured
Exception: >-
  System.Exception: I am bug

     at Demo.Example.Example3_Exception() in D:\Projects\my\mylab-log-yml\src\Demo\Example.cs:line 45


```



## MyLab.LogDsl

For more profit you can use [MyLab.LogDsl](https://github.com/ozzy-ext/mylab-log-dsl) library. The following example shows how to interpret a message which make with [MyLab.LogDsl](https://github.com/ozzy-ext/mylab-log-dsl):

```C#
_log.Dsl().Act("Input request")
    .AndFactIs("UserId", "12-4546")
    .AndFactIs("IP", "123.123.123.123")
    .AndFactIs("TryCount", "10")
    .AndFactIs("Url", "http://host/path?q=query")
    .AndFactIs("vip")
    .Write();
```

File record:

```
Id: 111576f9-ac1e-4054-bfdb-46d7c3f1c8c3
Time: 2018-09-05T11:25:10.5319396+03:00
Content: Input request
Attributes:
- Name: UserId
  Value: 12-4546
- Name: IP
  Value: 123.123.123.123
- Name: TryCount
  Value: 10
- Name: Url
  Value: http://host/path?q=query
- Name: Conditions
  Value: vip
```



## Config

There is ability to configure logger. Do it when configure service at thу application start:

```C#
serviceCollection.Configure<YamlLoggerOptions>(o =>
{
    o.BasePath = "logs";
    o.WriteInterval = TimeSpan.FromSeconds(5);
    o.DisposeWaitingInterval = TimeSpan.FromSeconds(2);
    o.FileSizeLimit = 5 * 1024 * 1024;
});
```
In example above specified the default values.

`BasePath` - related or absolute path for files
`WriteInterval` - an interval between log writing iterations
`DisposeWaitingInterval` - an interval to write left log messages when application closing
`FileSizeLimit` - a max length of log file. A file with the next index will be writes when reach this limit.



## PS

All examples above you can find in `Demo` project.
