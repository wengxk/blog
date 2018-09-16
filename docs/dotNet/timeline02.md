# 对象创建和垃圾回收

c#区分值类型和引用类型，两种类型有着不同的表现行为。  

1. 值类型，按值操作，直接分配在线程栈内，不受GC控制。
2. 引用类型，分配在托管堆内，变量存储的是对实际数据的引用，由GC控制回收。  

## 值类型和引用类型

值类型包括：数值类型、bool类型、enum类型、struct类型。  
引用类型包括：声明为class、interface、delegate、event、array的类型。

### 自定义struct类型

&nbsp;&nbsp;&nbsp;&nbsp;不同于引用类型，自定义结构类型，不可为其创建无参构造函数，因为每个值类型均隐式包含公共默认构造函数。可以对结构类型声明参数化构造函数，以设置其初始值，但仅在需要除默认值以外的值时才需要这样做。

```csharp
struct SampleStruct
{
    public int x;
    public int y;

    public SampleStruct()
    {
    }

    public SampleStruct(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
```

如上代码编译会提示错误：Structs cannot contain explicit parameterless constructors.

## 托管堆

&nbsp;&nbsp;&nbsp;&nbsp;进程初始化时CLR会从虚拟内存划出一片区域作为托管堆，同时CLR会维护一个指针，称之为NextObjPtr。该指针指向下一个对象的分配地址，最开始的时候该指针指向的地址称之为“基地址”。所有对象都会被分配在托管堆内。当托管堆内的区域被非垃圾对象填满时，CLR会为该托管堆分配更多的区域。

### new字符创建对象过程  

1. 计算该类型及其所有基类型所定义的实例字段的字节大小，同时包括额外开销成员（称为overhead成员，包括 _类型对象指针 type object pointer_ 和 _同步索引块 sync block index_ ）。
2. 计算需要字节大小后，在托管堆分配相应的内存。
3. 初始化对象的“类型对象指针”和“同步索引块”。
4. 调用该类型指定的构造函数来初始化实例数据。

## 垃圾回收

&nbsp;&nbsp;&nbsp;&nbsp;实际上，托管堆的大小总是有限的，当程序无法为new操作分配足够的内存时，CLR就会执行垃圾回收。

### 垃圾回收的过程

&nbsp;&nbsp;&nbsp;&nbsp;不同语言框架可能会采用不同的回收算法，CLR采用的是“引用跟踪算法”（referencing tracking algorithm）。  
&nbsp;&nbsp;&nbsp;&nbsp;当对象被分配在托管堆内时，保存对该对象的引用的变量被称为“根”（root），所有引用类型的变量都被称为根。当托管堆内的对象有对应的根并且应用程序还可以通过根来访问该对象时，则称为该对象为“reachable”，如果对象没有对应的根或是根为null或是代码已无法通过该根来访问对象，则该对象称为“unreachable”。  
&nbsp;&nbsp;&nbsp;&nbsp;垃圾回收的过程实际上就是回收这些unreachable的对象。另外，GC所处理的过程不仅于此，当GC释放这些unreachable对象占用的内存时，还会进行内存的合并碎片化处理，使那些幸存下来的对象能够彼此相邻，占用连续的地址空间，提高将来再次访问对象的性能及内存总体利用率。

> 注意：引用类型的根包括类的静态变量、实例变量、还有方法的各种传参及局部变量等。对于那些被static变量引用的对象始终都会被标记为reachable状态，不会进行内存回收，直到整个AppDomain被卸载为止。

### 基于代的回收过程

&nbsp;&nbsp;&nbsp;&nbsp;实际上那些unreachable的对象并不是在每次垃圾回收时都会被回收掉，如果每次垃圾回收都会对整个托管堆进行处理，那么这样的做法无疑是昂贵的。基于代的GC实际以三个假设为前提：

1. 对象越新，生命周期越短。
2. 对象越老，生命周期越长。
3. 回收托管堆的一部分速度要快于回收整个堆。

> 历史上很多的科学进步都是建立在假设之上，然后这些假设又最终被科学所验证。

#### 对象幸存与代的提升

&nbsp;&nbsp;&nbsp;&nbsp;刚创建的对象被称为第0代对象，而不是根据某种条件被定义分配到特定的代。刚开始所有的对象都是第0代对象，在第一次垃圾回收时有些对象会被回收，而另外有些对象会幸存下来，这时就会进行代的提升，从第0代提升到第1代。同理，在第1代回收时幸存下来的对象会被提升到第2代。  
&nbsp;&nbsp;&nbsp;&nbsp;每次垃圾回收只会发生在特定的代。托管堆上的对象共分为三代。通过不断的回收过程，很明显不同代的对象生命周期不一样：

1. 第0代。最年轻的代，大多存储的是临时变量，生命周期短。垃圾回收最常发生于此代中。
2. 第1代。多存储的是介于短生命周期和长生命周期的对象。
3. 第2代。多存储的是长生命周期对象。

![generationbased_gc](/docs/dotNet/content/tl02_generationbased_gc.png)

>没有第3代对象，System.GC.MaxGeneration被定义为2

#### 暂时代和暂时段

&nbsp;&nbsp;&nbsp;&nbsp;第0代和第1代又被称为暂时代（ephemeral generation），暂时代对象被分配在暂时段（ephemeral segment）。不用工作环境下的暂时段的大小有所不同。  

![image lose](/docs/dotNet/content/tl02_ephemeral_segment.png)

&nbsp;&nbsp;&nbsp;&nbsp;以上，通过代和暂时段的处理，GC回收避免了每次都要作用于整个托管堆，而是只处理堆的一部分，提高了垃圾回收的速度。

> 关于垃圾回收更多的内容，见参阅。

## 非托管资源的处理

&nbsp;&nbsp;&nbsp;&nbsp;虽然GC能够帮我们很好地处理对象的内存控制，但对于那些非托管对象，GC无法主动进行处理，这时，就需要我们主动告知GC来释放这些对象的内存。非托管对象主要指封装了系统资源的对象，如文件和各种网络连接等。

### 终结

&nbsp;&nbsp;&nbsp;&nbsp;好在CLR为我们提供了终结（finalization）的机制，它允许我们在终结器里定义一些代码，在对象被视为垃圾并且在GC回收内存之前调用执行。

#### 终结器

&nbsp;&nbsp;&nbsp;&nbsp;以下简单定义一个终结器：

```csharp
class  ClientManager
{
    ~ClientManager()
    {
         System.Diagnostics.Trace.WriteLine("Destructor called from class ClientManager");
    }
}
```

> 备注  
> - 终结器仅适用于类，不可为struct定义终结器
> - 一个类只有一个终结器
> - 终结器不能继承和重载
> - 无法显示调用终结器，他们会由CLR适时得调用
> - 终结器没有任何修饰符和参数
> - 终结器会隐式调用基类的Finalize方法

#### 终结器队列

终结器是如何工作的呢？如下是终结机制工作的简要说明：

1. 在new创建对象时，CLR检测到该对象类型定义了Finalize方法，则会在调用该实例构造器前将该对象的指针加入到终结列表（Finalization list）里；
2. 在第0代回收时，CLR不会将终结列表里对应的对象视为垃圾（即使是unreachable的对象数据），不会回收这些对象的内存；
3. 终结器线程(finalizer threads)在某个时间点调用Freachable queue里引用的对象的终结方法，执行之后会标记对象为垃圾；
4. 第1代回收时，CLR检测到这些垃圾对象，回收内存。

![finalization](/docs/dotNet/content/tl02_finalization.png)

> 从以上我们可以看到，可终结对象经历了两次回收才最终释放所占用的内存。因为这些对象发生了代的提升，在实际工作中，有可能经历多次回收才最终释放内存。

### Dispose模式

&nbsp;&nbsp;&nbsp;&nbsp;终结器可以帮助我们处理非托管对象，但是其内存可能会占用较长时间才会被最终回收。所以我们应实现Dispose模式，显示处理非托管资源。

```csharp
class FileManager : IDisposable
{
    bool _disposed = false;
    StreamReader _reader;

    // constructors & properties & functions
    // ...

    public FileManager(string path)
    {
        _reader = new StreamReader(path);
    }

    public void Dispose()
   {
       // dispose unmanaged resources
      Dispose(true);
       // Suppress finalization
      GC.SuppressFinalize(this);
   }

    protected virtual void Dispose(bool disposing)
   {
      if (_disposed)
         return;
      if (disposing) {
         _reader.Dispose();
         // free any other managed objects here.
         // ...
      }
      disposed = true;
   }
}
```

### 终结器和Dispose模式的使用

```csharp
class FileManager : IDisposable
{
    bool _disposed = false;
    StreamReader _reader;

    // constructors & properties & functions
    // ...

    public FileManager(string path)
    {
        _reader = new StreamReader(path);
    }

    public void Dispose()
   {
       // dispose unmanaged resources
      Dispose(true);
       // Suppress finalization
      GC.SuppressFinalize(this);
   }

    protected virtual void Dispose(bool disposing)
   {
      if (_disposed)
         return;
      if (disposing) {
         _reader.Dispose();
         // to do: free any other managed objects here
      }
       // to do: free any other unmanaged objects here

      disposed = true;
   }

   ~FileManager()
   {
       Dispose(false);
   }
}
```

## 内存诊断

见参阅。

## 参阅

* [https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/new-operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/new-operator)
* [https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/memory-management-and-gc](https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/memory-management-and-gc)
* [https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/destructors](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/destructors)