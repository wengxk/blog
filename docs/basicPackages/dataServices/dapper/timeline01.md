# Dapper

## 简介

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Dapper是一个非常轻量级的ORM框架，其性能几乎接近于原生的ADO.NET的使用方式。Dapper通过扩展`IDbConnection` , 提供了诸多实用的方法。

## 使用

> 环境：vs2013 + .net4.5，安装Dapper包时注意版本选择  
> 代码：~\src\vs2013\tour\App2\App2.csproj

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Dapper提供了诸多的 `Execute` 及 `Query` 之类的方法，基本的使用方法在Dapper的Github主页都有介绍。

1. 查询与强类型的映射
2. 查询与动态类型映射
3. 类似ADO.NET的ExecuteNonQuery之类的使用
4. 实现IEnumerable的参数的自动多次执行
5. 参数化查询
6. 多查询结果的映射

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;在使用的时候要一定要注意在不同数据库之间的实际表现，例如在oracle数据库中使用Multiple Results功能时，需要使用oracle提供的RefCursor，并且需要自定义新的参数类型。

## 注意点

### Buffered vs Unbuffered readers

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;大多数情况使用默认值true即可，但若存在查询返回的数据量特别大时，根据情况需要将buffered设置为false，但是会增加连接的存在时间。

### Cached vs Uncached

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;在每次执行命令时，Dapper都会根据一定条件来判断是否需要保存该命令的相关信息，信息会保存在 ` private static readonly System.Collections.Concurrent.ConcurrentDictionary<Identity, CacheInfo> _queryCache` 对象中，且该对象不可被清除或是删除其中的键值对，因此该对象所占用的内存只会增加不会减少，所以一般在使用Dapper时要特别注意尽量不要拼凑sql，应使用参数化sql。

## 基本原理


## 参阅

- [https://github.com/StackExchange/Dapper](https://github.com/StackExchange/Dapper)
- [https://www.slideshare.net/sureshloganathan750/dapper-performance](https://www.slideshare.net/sureshloganathan750/dapper-performance)
- [https://dapper-tutorial.net/dapper](https://dapper-tutorial.net/dapper)
- [https://stackoverflow.com/questions/10454883/dapper-net-how-to-flush-concurrentdictionary](https://stackoverflow.com/questions/10454883/dapper-net-how-to-flush-concurrentdictionary)