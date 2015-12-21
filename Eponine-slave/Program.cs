using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eponine_slave
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleClient simpleclient = new SimpleClient("127.0.0.1", 34718);
            Console.ReadKey();
        }
    }
}
