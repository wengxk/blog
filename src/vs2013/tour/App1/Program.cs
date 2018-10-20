using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    class Program
    {
        //适用于单独的配置文件设置
        private static readonly log4net.ILog _log
            = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly log4net.ILog _infoLogger = log4net.LogManager.GetLogger("logger");
        private static readonly log4net.ILog _infoLogger_1 = log4net.LogManager.GetLogger("logger.1");

        static void Main(string[] args)
        {
            //适用于内嵌在应用程序config文件中的配置
            //log4net.Config.XmlConfigurator.Configure();
            //var log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //log.Info("Hello world!");
            //_log.Info("Hello world!");

            //LogHelper.Log.Info(LogHelper.Log.Logger.Name);
            //_log.Error("An error occurred!");
            Console.WriteLine("logger prints:");
            _infoLogger.Info("Hello world!");
            Console.WriteLine("========================");
            Console.WriteLine("logger.1 prints:");
            _infoLogger_1.Info("Hello world!");
            //_infoLogger.Info(_infoLogger.Logger.Name + "," + _infoLogger.IsDebugEnabled);
            Console.WriteLine("Press any key to leave!");
            Console.ReadLine();
        }
    }

    class LogHelper
    {
        public static readonly log4net.ILog Log
               = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
