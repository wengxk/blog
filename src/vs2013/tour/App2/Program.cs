using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    class Program
    {
        static void Main(string[] args)
        {
            ITest testClass = new TestClass1();
            testClass.Test();
            Console.WriteLine("Press any key to leave.");
            Console.ReadKey();
        }
    }

}
