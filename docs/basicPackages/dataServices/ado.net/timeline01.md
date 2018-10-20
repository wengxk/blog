# ADO.NET

## 简述

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ADO.NET支撑了 .NET Framework 的数据访问服务，提供了对诸如Oracle、SqlServer、xml这样的数据源及通过OLE DB和ODBC公开的数据源的一致性访问。应用程序可以通过ADO.NET来连接这些数据源，并进行检索、更新数据源中所包含的数据。  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ADO.NET提供的各种类封装在System.Data.dll中，并与System.Xml.dll中的XML类集成。当编译使用包含System.Data命名空间的类时，需要同时引用System.Data.dll和System.Xml.dll这两个程序集。

## 两个重要组件

### .NET Framework 数据提供程序

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Data Providers即数据提供程序由各个数据源官方提供者或第三方实现，例如，针对于以Sql Server为数据源的数据提供程序（微软官方提供），以Oracle为数据源的数据提供程序（Oracle .Net组织提供）等。各个数据源提供程序会根据其数据源的一些特性进行相应的优化，但无论是何种数据提供程序，都是基于ADO.NET的顶层服务接口来实现的。

#### .NET Framework 数据提供程序的核心对象

- Connection：与数据源建立连接
- Command： 对数据源进行命令操作
- DataReader：从数据源中读取只进且只读的数据流
- DataAdapter：使用数据源填充DataSet

此外，还有以下对象：

- Transaction：提供事务支持
- CommandBuilder：生成Command
- ConnectionStringBuilder：用于操作ConnectionString
- Parameter：命令参数
- Exception：异常
- Error：数据源返回的错误信息
- ClientPermission：客户端权限

### DataSet

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;DataSet 是专门为独立于任何数据源的数据访问而设计的,可以将从数据源访问的数据装载到本地内存中进行处理而无需保持打开的连接。DataSet包含一个或多个DataTable的集合，DataTable包含数据行及数据列对象，及对DataTable框架描述的对象，例如主键、外键及其他约束等。

### DataReader与DataSet的选择

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;DataSet为我们提供了更多的针对数据的操作及特性，当我们不需要这些额外的行为时，就可以使用DataReader以或得更好的性能。

## 常规操作

对于 ADO.NET，一般性的操作无非是以下几个阶段：

- 构造Connection及Command等必须的对象
- 调用Connection.Open()方法
- 如需要，开始事务
- 执行Command
- 获得执行Command的结果，获取DataReader或者装载入DataSet
- 依据执行结果失败与否，提交或回滚事务
- 调用Connection.Close()方法及Dispose()方法

## 参阅

[https://docs.microsoft.com/zh-cn/dotnet/framework/data/adonet/](https://docs.microsoft.com/zh-cn/dotnet/framework/data/adonet/)