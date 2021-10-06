using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using wbox_console.Telltale;
using wbox_console.Utilities;

namespace wbox_console.WBOX
{
    public class WalkBoxes_File
    {
        public WBOX_6VSM wbox_6vsm;
        public WBOX_5VSM wbox_5vsm;
        public WBOX_ERTM wbox_ertm;

        public static string Read_MetaStreamKeyword(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);

            uint bytePointerPosition = 0;

            string mMetaStreamVersion = ByteFunctions.ReadFixedString(fileBytes, 4, ref bytePointerPosition);

            return mMetaStreamVersion;
        }

        public void Read_WalkBox_FromTelltale(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            Console.WriteLine("File Size: '{0}'", fileBytes.Length);

            string fileMetaVersion = Read_MetaStreamKeyword(filePath);

            if(fileMetaVersion.Equals("6VSM"))
            {
                wbox_6vsm = new WBOX_6VSM(filePath);
            }
            else if (fileMetaVersion.Equals("5VSM"))
            {
                wbox_5vsm = new WBOX_5VSM(filePath);
            }
            else if(fileMetaVersion.Equals("ERTM"))
            {
                wbox_ertm = new WBOX_ERTM(filePath);
            }
            else
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("'0' File version is not supported! '{1}'", filePath, fileMetaVersion);
                Console.ResetColor();
            }
        }

        public object Get_WBOX_Object()
        {
            if (wbox_6vsm != null)
                return wbox_6vsm;
            else if (wbox_5vsm != null)
                return wbox_5vsm;
            else if (wbox_ertm != null)
                return wbox_ertm;
            else
                return null;
        }
    }
}
