using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Eponine_slave
{
    public class SimpleParser
    {
        public ParserMeans header;
        public int filelen;
        public int index;
        public int max;
        public string filename;
        public byte[] filebyte;
        private byte[] mbytes;
        public enum ParserMeans
        {
            heartbreak = 0,
            workrequest = 1,
        }
        public SimpleParser(byte[] bytes)
        {
            mbytes = bytes;
            filelen = 0;
            index = 0;
            max = 0;
            filename = string.Empty;
            filebyte = null;
        }
        public bool Parse()
        {
            if (mbytes == null)
                return false;
            try
            {
                MemoryStream ms = new MemoryStream(mbytes);
                BinaryReader br = new BinaryReader(ms);
                header = (ParserMeans)br.ReadInt16();
                switch (header)
                {
                    case  ParserMeans.heartbreak:
                        break;
                    case ParserMeans.workrequest:
                        filelen = br.ReadInt32();
                        index = br.ReadInt32();
                        max = br.ReadInt32();
                        byte[] bytes = br.ReadBytes(30);
                        filename = Encoding.UTF8.GetString(bytes).Replace("\0","");
                        filebyte = br.ReadBytes(filelen);
                        break;
                }
                return true;
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }
    }
}
