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

> 环境：vs2013 + .net4.5  
> 代码：~\src\vs2013\tour\App1\App1.csproj

### 获取

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;在程序包管理控制台输入：
> PM> Install-Package log4net

### 配置

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;log4net支持两种方式配置：

#### 内嵌方式配置

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;在应用程序的config文件中配置，例如app.config和web.config文件，这种方式不支持动态配置。

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

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;单独创建一个配置文件，例如log4net.config文件，这种方式支持动态配置。

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

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;不同的配置方式需采用不同的方式来获取Logger对象。

#### 内嵌方式

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;首先要在应用程序启动时调用log4net.Config.XmlConfigurator.Configure()或者 log4net.Config.XmlConfigurator.Configure(ILoggerRepository) 方法。例如，在控制台程序Mian方法中，

```csharp
 static void Main(string[] args)
 {
    log4net.Config.XmlConfigurator.Configure();
    var log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    log.Info("Hello world!");
    Console.WriteLine("Press any key to leave!");
    Console.ReadLine();
 }
```

#### 单独文件方式

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;1)单独文件配置需要生成到bin目录下，如下设置：

![image lost](/docs/basicPackages/log4net/content/tl01_log4net.config.PNG)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2)告诉log4net从哪里加载配置文件信息，如下，需要在程序集信息中添加信息。

![image lost](/docs/basicPackages/log4net/content/tl01_assemblyinfo.PNG)

>如果需要支持动态配置，请将Watch设置为true.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;3)如下输出日志到控制台

```csharp
class Program
{
    private static readonly log4net.ILog _log
        = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    static void Main(string[] args)
    {
        _log.Info("Hello world!");
        Console.WriteLine("Press any key to leave!");
        Console.ReadLine();
    }
}
```

## 体系结构

log4net主要包含以下重要组件：

1. logger，提供logging service
2. appender，绑定输出日志到不同目标
3. layout，定义输出日志的参数与格式
4. filter，控制输出日志

### Logger

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Logger是命名实体，大小写敏感，遵循类似于命名空间式的命名方式层级结构。例如：Logger A是Logger A.A1的父Logger，也可以说后者是前者的子Looger。

#### root Logger

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;在Logger的层级结构中，root Logger位于最顶端,有以下特点：

- 总是存在
- 不能被命名
- 总是拥有一个分配的level，默认为Debug

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;root Logger由标签`<root>`配置，为`<log4net>`的直接子标签，不可配置多个。

#### 层级结构

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;所有命名Loogger均继承于root Logger.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;在上文中我们通过`log4net.LogManager.GetLogger()`来获取一个Logger，这个Logger的name和当前类的完全限定名一致,可以通过`_log.Info(_log.Logger.Name)`看到控制台输出结果 _ConsoleApplication1.Program_. 若我们没有显示去配置这个Logger，log4net则会自动帮我们生成一个Logger（F12查看GetLogger：This is one of the central features of log4net），而这个Logger继承于root Logger，所以我们在root里的配置能够生效于这个新建的Logger。

#### Level

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Level标识了输出日志信息的级别，例如生产环境和开发环境下的日志输出会有不同的要求，或是只需要输出错误或者失败的信息等。Logger的Level属性保存了该Logger的日志级别要求。若没有分配Level，则会从父Logger继承。以下列出了可以为Logger分配的Level，从低到高：

- ALL
- DEBUG
- INFO
- WARN
- ERROR
- FATAL
- OFF
  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;例如，在上述为root logger定义了level级别为info，则我们可以在新获取的logger输出info及info级别以上的日志信息，但是无法输出debug信息，这就是由logger的level控制的。

### Appender

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loggers的日出输出请求由其绑定的Appenders来完成，调用 `IAppender.DoAppend(LoggingEvent loggingEvent)` 方法。log4net提供了多种用途的Appenders，能够将日志输出到不同目标。关于各种appenders的说明及其用法可以从官网上获取。

#### Appender的附加行为

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Logger内部有一类型为AppenderCollection的属性Appenders，不仅是Logger本身定义的Appenders会附加到其内部属性Appenders的集合中，其父Logger所拥有的Appenders也会附加进去。例如，我们在root中定义了一个console类型的appender，我们获取另一个logger时，能够将日志输出到console，若我们还为该logger添加一个file类型的appender，则会将日志同时输出到console和file中。  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Appender的附加行为可以由Logger的属性Additivity控制，默认为true。若将其置为false，则父loggle的appenders不会参与日志输出。

```xml
<?xml version="1.0" encoding="utf-8" ?>
<log4net>
  <root >
    <level value="info" />
    <appender-ref ref="console" />
  </root>
  <appender name="console" type="log4net.Appender.ConsoleAppender">
    <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="%date %level %logger - %message%newline" />
    </layout>
  </appender>
  <appender name="console2" type="log4net.Appender.ConsoleAppender">
    <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="%date %level - %message%newline" />
    </layout>
  </appender>
  <appender name="console3" type="log4net.Appender.ConsoleAppender">
    <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="%date - %message%newline" />
    </layout>
  </appender>
  <logger name="logger">
    <level value="info"></level>
    <appender-ref ref="console2"></appender-ref>
  </logger>
  <logger name="logger.1" additivity="false">
    <level value="info"></level>
    <appender-ref ref="console3"></appender-ref>
  </logger>
</log4net>
```

```csharp
class Program
{
    private static readonly log4net.ILog _infoLogger = log4net.LogManager.GetLogger("logger");
    private static readonly log4net.ILog _infoLogger_1 = log4net.LogManager.GetLogger("logger.1");

    static void Main(string[] args)
    {
        Console.WriteLine("logger prints:");
        _infoLogger.Info("Hello world!");
        Console.WriteLine("========================");
        Console.WriteLine("logger.1 prints:");
        _infoLogger_1.Info("Hello world!");     
        Console.WriteLine("Press any key to leave!");
        Console.ReadLine();
    }
}
```

输出：

![image lost](/docs/basicPackages/log4net/content/tl01_consolelog.PNG)

### Filter

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Logger的Level属性是最基本的filter，另外AppenderSkeleton具有一个IFilter类型的属性FilterHead，所以我们能从appender上做出更多的控制。log4net提供以下类型的filter：

- log4net.Filter.DenyAllFilter
- log4net.Filter.LevelMatchFilter
- log4net.Filter.LevelRangeFilter
- log4net.Filter.LoggerMatchFilter
- log4net.Filter.PropertyFilter
- log4net.Filter.StringMatchFilter

关于各种filter的配置及使用见参阅。

### Layout

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;AppenderSkeleton还提供了类型为ILayout的属性Layout，因此我们能够控制日志的输出格式。log4net提供以下类型的layout：

- log4net.Layout.ExceptionLayout
- log4net.Layout.PatternLayout
- log4net.Layout.RawTimeStampLayout
- log4net.Layout.RawUtcTimeStampLayout
- log4net.Layout.SimpleLayout
- log4net.Layout.XmlLayout
- log4net.Layout.XmlLayoutSchemaLog4j

关于各种layout的配置及使用见参阅。

## 参阅

- [https://stackify.com/log4net-guide-dotnet-logging/](https://stackify.com/log4net-guide-dotnet-logging/)
- [https://logging.apache.org/log4net/release/features.html](https://logging.apache.org/log4net/release/features.html)

>下一篇：[.net core环境下的简单配置与使用](/docs/basicPackages/log4net/timeline02.md)