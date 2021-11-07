using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using wbox_console.Telltale;
using wbox_console.Utilities;

namespace wbox_console.Main
{
    public class WalkBoxes_Master
    {
        //meta header object
        public MSV6 msv6;
        public MSV5 msv5;
        public MTRE mtre;

        //main walkbox object
        public Walkboxes walkboxes;

        public WalkBoxes_Master(string filePath)
        {
            string fileMetaVersion = Read_MetaStreamKeyword(filePath);

            using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
            {
                switch (fileMetaVersion)
                {
                    case "6VSM":
                        msv6 = new(reader);
                        break;
                    case "5VSM":
                        msv5 = new(reader);
                        break;
                    case "ERTM":
                        mtre = new(reader);
                        break;
                    default:
                        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                        Console.WriteLine("'0' File version is not supported! '{1}'", filePath, fileMetaVersion);
                        Console.ResetColor();
                        return;
                }

                walkboxes = new(reader, true);
            }
        }

        public static string Read_MetaStreamKeyword(string sourceFile)
        {
            string metaStreamVersion = "";

            using (BinaryReader reader = new BinaryReader(File.OpenRead(sourceFile)))
            {
                metaStreamVersion += reader.ReadChar();
                metaStreamVersion += reader.ReadChar();
                metaStreamVersion += reader.ReadChar();
                metaStreamVersion += reader.ReadChar();
            }

            return metaStreamVersion;
        }

        public object Get_Meta_Object()
        {
            if (msv6 != null)
                return msv6;
            else if (msv5 != null)
                return msv5;
            else if (mtre != null)
                return mtre;
            else
                return null;
        }
    }
}
