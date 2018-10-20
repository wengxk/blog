using System;
using System.IO;
using System.Reflection;
using log4net;

namespace App1
{
    class Program
    {        
        static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());            
            log4net.Config.XmlConfigurator.Configure(logRepository,new FileInfo("log4net.config"));
            var log = LogManager.GetLogger(logRepository.Name, "DefaultLogger");
            log.Info("This is log message from log4net.");
            Console.WriteLine("Press any key to leave!");
            Console.ReadKey();
        }
    }
}

