# log4net

## 简介

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;log4net库是Apache log4j框架在.NET平台的实现，能够将日志信息输出到各种目标（控制台、文件、数据库等）。

特征

- 支持多种框架
- 可输出到多种目标载体
- 层级体系
- xml配置
- 支持动态配置
- 日志上下文
- 久经验证
- 模块化可扩展性设计
- 灵活高效

## 简单使用

### 获取

在程序包管理控制台输入：
> PM> Install-Package log4net

### 配置

log4net支持两种方式配置：

#### 内嵌方式配置

在应用程序的config文件中配置，例如app.config和web.config文件，这种方式不支持动态配置。

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
    </configSections>
    <log4net>
        <appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender" >
            <layout type="log4net.Layout.PatternLayout">
                <conversionPattern value="%date [%thread] %-5level %logger [%ndc] - %message%newline" />
            </layout>
        </appender>
        <root>
            <level value="INFO" />
            <appender-ref ref="ConsoleAppender" />
        </root>
    </log4net>
</configuration>
```

#### 单独文件配置

单独创建一个配置文件，例如log4net.config文件，这种方式支持动态配置。

```xml
<?xml version="1.0" encoding="utf-8" ?>
<log4net>
    <appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender" >
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%date [%thread] %-5level %logger [%ndc] - %message%newline" />
        </layout>
    </appender>
    <root>
        <level value="INFO" />
        <appender-ref ref="ConsoleAppender" />
    </root>
</log4net>
```

### 获取Logger对象

不同的配置方式需采用不同的方式来获取Logger对象。

#### 内嵌方式

首先要在应用程序启动时调用log4net.Config.XmlConfigurator.Configure()或者 log4net.Config.XmlConfigurator.Configure(ILoggerRepository) 方法。例如，在控制台程序Mian方法中，

```csharp
 static void Main(string[] args)
 {
    log4net.Config.XmlConfigurator.Configure();
    var log = log4net.LogManager.GetLogger("ConsoleAppender");
    log.Info("Hello world!");
    Console.WriteLine("Press any key to leave!");
    Console.ReadLine();
 }
```

#### 单独文件方式

1)单独文件配置需要生成到bin目录下，如下设置：

![image lost](/docs/basicPackages/log4net/content/tl01_log4net.config.PNG)

2)告诉log4net从哪里加载配置文件信息，如下，需要在程序集信息中添加信息。

![image lost](/docs/basicPackages/log4net/content/tl01_assemblyinfo.PNG)

>如果需要支持动态配置，请将Watch设置为true.

3)如下输出日志到控制台

```csharp
class Program
{
    private static readonly log4net.ILog log
        = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //= log4net.LogManager.GetLogger("ConsoleAppender");

    static void Main(string[] args)
    {
        log.Info("Hello world!");
        Console.WriteLine("Press any key to leave!");
        Console.ReadLine();
    }
}
```

## 体系结构



## 参阅

- [https://stackify.com/log4net-guide-dotnet-logging/](https://stackify.com/log4net-guide-dotnet-logging/)
- [https://logging.apache.org/log4net/release/features.html](https://logging.apache.org/log4net/release/features.html)