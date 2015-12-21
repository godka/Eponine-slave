using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using drizzle;
using System.Reflection;
using System.IO;
namespace Eponine_slave
{
    public class SimpleClient
    {
        private drizzleTCP.Client cli;
        public SimpleClient(string ip, int port)
        {
            cli = new drizzleTCP.Client(ip, port);
            cli.ClientEventArriveByte += cli_ClientEventArriveByte;
        }

        void cli_ClientEventArriveByte(drizzleTCP.Client tcp, byte[] Receive, int length)
        {
            SimpleParser parser = new SimpleParser(Receive);
            if (parser.Parse())
            {
                if (parser.header == SimpleParser.ParserMeans.heartbreak)
                {
                    Eponine_master.SimpleHeartBreakGenerator simpleheartbreakgenerator = 
                        new Eponine_master.SimpleHeartBreakGenerator(this);
                    tcp.send(simpleheartbreakgenerator.Generate(0, 0));
                    Console.WriteLine("Heart Break");
                }
                else if (parser.header == SimpleParser.ParserMeans.workrequest)
                {
                    Console.WriteLine(parser.filename);
                    string ExeName = parser.filename;
                    File.WriteAllBytes(Environment.CurrentDirectory + "/" + ExeName + ".dll", parser.filebyte);
                    Assembly ass = Assembly.LoadFile(Environment.CurrentDirectory + "/" + ExeName + ".dll");
                    Type tp = ass.GetType(ExeName + "." + ExeName);
                    Object obj = Activator.CreateInstance(tp);
                    MethodInfo meth = tp.GetMethod("MapLoop");
                    int t = Convert.ToInt32(meth.Invoke(obj, new Object[] { parser.index, parser.max }));
                    Eponine_master.SimpleReturnGenerator srg = new Eponine_master.SimpleReturnGenerator(t);
                    tcp.send(srg.Generate(0, 0));
                }
            }
            //throw new NotImplementedException();
        }


    }
}
