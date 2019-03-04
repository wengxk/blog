# .NET设计准则

记录一些常用.NET框架设计准则，内容来源于相关书籍、网络、工作与学习。  
遵守这些常用的设计准则能够帮助我们在日常工作中持续提高自己的设计能力与编码素养。  
要求遵守这些准则，但在一些特殊场景并拥有足够充分的理由与自信时可以改变它们。没有最好，只有更好。

## 命名规范

遵循一套一致的命名规范有助于提高应用系统和程序框架的可用性。在进行命名时，准确易读是基本要素，同时还要避免与其他名称或是各种广泛关键字的冲突。

### 通用命名规范

#### 目标

要求：准确、易读、简洁，优先级：准确>易读>简洁。

#### 可用命名字符

要求：使用英文字母字符和数字。  
谨慎：考虑使用下划线。  
避免：使用与广泛应用的编程语言关键字冲突的标识符。

#### 缩略语与简写

不要：在标识符中使用缩略语或简写形式。  
谨慎：考虑使用常用缩略词和简写词。

### 大小写准则

#### 两种约定

为了区分标识符的各个单词，需要将每个单词的首字母大写，不要使用下划线来区分。常用标识符大小写约定有：

- PascalCasing，适用于除了参数名称之外的所有标识符
- camelCasing，仅适用于参数名称

> 在Oracle数据库中，几乎所有的元数据都是以大写形式存放的，不管是表名还是列名都是大写，而且大多项目应用也都是以下划线来区分标识中的每个单词。

#### 复合词和常用术语的大小写

| PascalCasing  | camelCasing   | Not                  |
| :------------ | :------------ | :------------------- |
| `BitFlag`     | `bitFlag`     | `Bitflag`            |
| `Callback`    | `callback`    | `CallBack`           |
| `Canceled`    | `canceled`    | `Cancelled`          |
| `DoNot`       | `doNot`       | `Don't`              |
| `Email`       | `email`       | `EMail`              |
| `Endpoint`    | `endpoint`    | `EndPoint`           |
| `FileName`    | `fileName`    | `Filename`           |
| `Gridline`    | `gridline`    | `GridLine`           |
| `Hashtable`   | `hashtable`   | `HashTable`          |
| `Id`          | `id`          | `ID`                 |
| `Indexes`     | `indexes`     | `Indices`            |
| `LogOff`      | `logOff`      | `LogOut`             |
| `LogOn`       | `logOn`       | `LogIn`              |
| `Metadata`    | `metadata`    | `MetaData, metaData` |
| `Multipanel`  | `multipanel`  | `MultiPanel`         |
| `Multiview`   | `multiview`   | `MultiView`          |
| `Namespace`   | `namespace`   | `NameSpace`          |
| `Ok`          | `ok`          | `OK`                 |
| `Pi`          | `pi`          | `PI`                 |
| `Placeholder` | `placeholder` | `PlaceHolder`        |
| `SignIn`      | `signIn`      | `SignOn`             |
| `SignOut`     | `signOut`     | `SignOff`            |
| `UserName`    | `userName`    | `Username`           |
| `WhiteSpace`  | `whiteSpace`  | `Whitespace`         |
| `Writable`    | `writable`    | `Writeable`          |

### 程序集命名

考虑：使用 `<Company>.<Component>.dll` 的模式命名程序集。

### 命名空间命名

考虑：使用 `<Company>.(<Product>|<Technology>)[.<Feature>][.<Subnamespace>]` 的模式命名。  
不要：使用与类相同的名称，避免与相关命名空间中的类型名称冲突。

### 类、结构和接口命名

要求：使用PascalCasing模式。  
要求：使用形容词短语命名接口，或偶尔用名词或名词短语命名接口。
> 接口代表一组功能方法的集合，是功能性的，而类大多表示一类对象，是名词性的。

要求：使用I作为接口的命名前缀。  
不要：给类加前缀。
考虑：以基类名作为派生类的后缀。  
要求：为泛型类参数命名时使用T或以T为前缀的描述性词汇。  

#### 常用类型的名称

| 基类型                                                                                                              | 派生的实现类型准则                                                                                                                                                                                   |
| ------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `System.Attribute`                                                                                                  | **✓ 务必**为自定义属性类的名称添加后缀 "Attribute"。                                                                                                                                                 |
| `System.Delegate`                                                                                                   | **✓ 务必**向事件中所用委托的名称中添加后缀 "EventHandler"。<br /><br /> **✓ 务必**在用作事件处理程序的委托以外的委托名称中添加后缀 "Callback"。<br /><br /> **X 不要**将后缀 "Delegate" 添加到委托。 |
| `System.EventArgs`                                                                                                  | **✓ 务必**添加后缀 "EventArgs"。                                                                                                                                                                     |
| `System.Enum`                                                                                                       | **X 不要**从此类派生；而是使用所用语言支持的关键字；例如，在 C# 中，使用关键字 `enum`。<br /><br /> **X 不要**添加后缀 "Enum" 或 "Flag"。                                                            |
| `System.Exception`                                                                                                  | **✓ 务必**添加后缀 "Exception"。                                                                                                                                                                     |
| `IDictionary` <br /> `IDictionary<TKey,TValue>`                                                                     | **✓ 务必**添加后缀 "Dictionary"。 请注意，`IDictionary` 是一种特定类型的集合，但此准则优先于后面更宽泛的集合准则。                                                                                   |
| `IEnumerable` <br /> `ICollection` <br /> `IList` <br /> `IEnumerable<T>` <br /> `ICollection<T>` <br /> `IList<T>` | **✓ 务必**添加后缀 "Collection"。                                                                                                                                                                    |
| `System.IO.Stream`                                                                                                  | **✓ 务必**添加后缀 "Stream"。                                                                                                                                                                        |
| `CodeAccessPermission IPermission`                                                                                  | **✓ 务必**添加后缀 "Permission"。                                                                                                                                                                    |

#### 枚举

要求：标志枚举使用复数形式命名，其他常规枚举请使用单数形式。  
不要：使用"Enum" 、"Flag" 或 "Flags" 作为后缀。

### 类型成员命名准则

#### 方法

要求：使用动词或动词短语。

#### 属性

要求：使用名称或形容词。  
要求：布尔类型的属性请用肯定语气形式。  
要求：集合属性请用复数形式而不是以"List" 或 "Collection" 的后缀单数形式短语来命名。

#### 事件

要求：使用动词或动词短语，并区分现在时态和过去时态。例如"Saving"和"Saved"，而不是使用"Before"或"After"为前缀。

