# log4net

## .NET CORE环境下的使用

> 环境：vs2017 + .netcore 2.1  
> 代码：~\src\vs2017\tour\tour.sln

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;因为 .net core项目下没有AssemblyInfo这个文件，所以不能以之前文章中的方式来配置，需要以下述方式配置，记住配置文件需要设置成生成到bin目录下。

```csharp
class Program
{
    static void Main(string[] args)
    {
        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        var log = LogManager.GetLogger(logRepository.Name, "DefaultLogger");
        log.Info("This is log message from log4net.");
        Console.WriteLine("Press any key to leave!");
        Console.ReadKey();
    }
}
```