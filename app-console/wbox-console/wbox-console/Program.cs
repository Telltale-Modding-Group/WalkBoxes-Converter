using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using wbox_console.Utilities;
using wbox_console.Telltale;
using Newtonsoft.Json;

namespace wbox_console
{
    class Program
    {
        private static string wboxExt = ".wbox";
        private static string directory = "C:/Users/David Matos/Desktop/Desktop2022/telltale-wbox/de-wboxes";

        private static void Main(string[] args)
        {
            GetFilesInDirectory();
        }

        private static void GetFilesInDirectory()
        {
            //gather the files from the folder path into an array
            List<string> files = new List<string>(Directory.GetFiles(directory));

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow);
            Console.WriteLine("Filtering Files..."); //notify the user we are filtering the array

            //filter the array so we only get .d3dtx files
            files = IOManagement.FilterFiles(files, wboxExt);

            //if no d3dtx files were found, abort the program from going on any further (we don't have any files to convert!)
            if (files.Count < 1)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("No {0} files were found, aborting.", wboxExt);
                Console.ResetColor();
                return;
            }

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green);
            Console.WriteLine("Found {0} WalkBoxes.", files.Count.ToString()); //notify the user we found x amount of d3dtx files in the array
            Console.WriteLine("Starting...");//notify the user we are starting
            Console.ResetColor();

            //run a loop through each of the found textures and convert each one
            foreach (string file in files)
            {
                //build the path for the resulting file
                string fileName = Path.GetFileName(file); //get the file name of the file + extension

                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
                Console.WriteLine("||||||||||||||||||||||||||||||||");
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Blue);
                Console.WriteLine("Converting '{0}'...", fileName); //notify the user are converting 'x' file.
                Console.ResetColor();

                ReadWalkBox(file);

                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green);
                Console.WriteLine("Finished converting '{0}'...", fileName); //notify the user we finished converting 'x' file.
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            }
        }

        public static void ReadWalkBox(string filePath)
        {
            WalkBoxes_File walkBoxes = new WalkBoxes_File();
            walkBoxes.WalkBoxes = WalkBoxes_File.Read_WalkBox_FromTelltale(filePath);

            bool writeJSON = true;

            if(writeJSON)
            {
                string fileExt = Path.GetExtension(filePath);
                string jsonPath = filePath.Remove(filePath.Length - fileExt.Length, fileExt.Length) + ".json";

                if (File.Exists(jsonPath))
                    File.Delete(jsonPath);

                //open a stream writer to create the text file and write to it
                using (StreamWriter file = File.CreateText(jsonPath))
                {
                    //get our json seralizer
                    JsonSerializer serializer = new JsonSerializer();

                    //seralize the data and write it to the configruation file
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, walkBoxes);
                }
            }
        }

        public static void WriteJSON(string originalFilePath, WalkBoxes_File walkBoxes)
        {
            string fileExt = Path.GetExtension(originalFilePath);
            string jsonPath = originalFilePath.Remove(originalFilePath.Length - fileExt.Length, fileExt.Length) + ".json";

            if (File.Exists(jsonPath))
                File.Delete(jsonPath);

            //open a stream writer to create the text file and write to it
            using (StreamWriter file = File.CreateText(jsonPath))
            {
                //get our json seralizer
                JsonSerializer serializer = new JsonSerializer();

                //seralize the data and write it to the configruation file
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, walkBoxes);
            }
        }
    }
}
