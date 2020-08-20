using System;
using System.Collections.Generic;
using System.IO;

namespace HACK_Assembler
{
    class Program
    {
        // Attributes
        private static Dictionary<string, short> ramTabel = null;
        private static short ramStartIndex = 15;
        private static string inputFilePath = null;
        private static string outputFileCommentPath = "./tempComment.txt";
        private static string outputFileLablePath = "./tempLable.txt";
        private static string outputFileTranslateAPath = "./tempTranslateA.txt";

        // Propperties
        public static Dictionary<string, short> RamTable
        {
            get
            {
                return ramTabel;
            }
            set
            {
                ramTabel = value;
            }
        }
        public static short RamStartIndex { get => ramStartIndex; set => ramStartIndex = value; }
        public static string InputFilePath { get => inputFilePath; set => inputFilePath = value; }
        public static string OutputFileCommentPath { get => outputFileCommentPath; set => outputFileCommentPath = value; }
        public static string OutputFileLablePath { get => outputFileLablePath; set => outputFileLablePath = value; }
        public static string OutputFileTranslateAPath { get => outputFileTranslateAPath; set => outputFileTranslateAPath = value; }


        // Main loop
        static void Main(string[] args)
        {
            RamTable = SymbolTable();
            // user input
            while (InputFilePath == null)
            {
                StartMenu();
            }

            //remove comments
            RemoveComments(new StreamReader(InputFilePath), new StreamWriter(OutputFileCommentPath, false));

            // lokking for lables
            LookingForLables(new StreamReader(OutputFileCommentPath), new StreamWriter(OutputFileLablePath, false));

            // @ Translation
            TranslateA(new StreamReader(OutputFileLablePath), new StreamWriter(OutputFileTranslateAPath, false));


            EndMenu();

            Console.ReadLine();
        }

        private static void EndMenu()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Path to Your Output File :");
                Console.ForegroundColor = ConsoleColor.White;

                // file path
                string filePath = Console.ReadLine();

                // normalise the string input
                filePath = filePath.Trim();
                filePath = filePath.ToLower();

                File.Copy(OutputFileTranslateAPath, filePath);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Done!!!!!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void TranslateA(StreamReader reader, StreamWriter writer)
        {
            try
            {
                string line;
                short lineNum = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    // check comment
                    if (line.Contains("@"))
                    {
                        line = line.Replace("@", "");
                        line = line.Trim();

                        short tempNumber;
                        if (short.TryParse(line, out tempNumber))
                        {
                            line = $"@{tempNumber}";
                        }
                        else
                        {
                            line = $"@{RamTableSearch(line)}";
                        }
                    }


                    if (!String.IsNullOrEmpty(line))
                    {
                        writer.WriteLine(line);
                        lineNum++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Sorry file could not be read");
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
                writer.Close();
            }
        }

        static void StartMenu()
        {
            bool validFile = false;

            Console.WriteLine("You are about to enroll on a great journey from your assembler code to binay");

            do
            {
                // user information
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Path to Your File :");
                Console.ForegroundColor = ConsoleColor.White;

                // file path
                string filePath = Console.ReadLine();

                // normalise the string input
                filePath = filePath.Trim();
                filePath = filePath.ToLower();

                // file check
                if (ValidFile(filePath))
                {
                    // end loop
                    validFile = true;
                    InputFilePath = filePath;
                }

            } while (!validFile);
        }

        private static void RemoveComments(StreamReader reader, StreamWriter writer)
        {
            try
            {
                string line;
                short lineNum = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    // check comment
                    if (line.Contains("//"))
                    {
                        line = RemoveComments(line);
                    }


                    if (!String.IsNullOrEmpty(line))
                    {
                        line = line.Trim();
                        writer.WriteLine(line);
                        lineNum++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Sorry file could not be read");
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
                writer.Close();
            }
        }

        private static void LookingForLables(StreamReader reader, StreamWriter writer)
        {
            try
            {
                string line;
                short lineNum = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    // check comment
                    if (line.Contains("//"))
                    {
                        line = RemoveComments(line);
                    }

                    if (!String.IsNullOrEmpty(line))
                    {
                        // check lable
                        if (line.Contains('(') && line.Contains(')'))
                        {
                            Console.WriteLine($"    Lable at {lineNum}");
                            line = line.Replace("(", "");
                            line = line.Replace(")", "");

                            if (lineNum == 0)
                            {
                                ramTabel.Add(line, (short)(lineNum));
                            }
                            else
                            {
                                ramTabel.Add(line, (short)(lineNum - 1));
                                lineNum--;
                            }

                            line = "";
                        }

                        if (!String.IsNullOrEmpty(line))
                        {
                            line = line.Trim();
                            writer.WriteLine(line);
                            lineNum++;
                        }

                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Sorry file could not be read");
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
                writer.Close();
            }
        }



        static bool ValidFile(string path)
        {
            if (path.Contains(".asm") && File.Exists(path))
            {
                return true;
            }
            return false;
        }



        private static Dictionary<string, short> SymbolTable()
        {
            // initialization of keyWord
            Dictionary<string, short> temp = new Dictionary<string, short>();

            temp.Add("R0", 0);
            temp.Add("R1", 1);
            temp.Add("R2", 2);
            temp.Add("R3", 3);
            temp.Add("R4", 4);
            temp.Add("R5", 5);
            temp.Add("R6", 6);
            temp.Add("R7", 7);
            temp.Add("R8", 8);
            temp.Add("R9", 9);
            temp.Add("R10", 10);
            temp.Add("R11", 11);
            temp.Add("R12", 12);
            temp.Add("R13", 13);
            temp.Add("R14", 14);
            temp.Add("R15", 15);
            temp.Add("SCREEN", 16384);
            temp.Add("KBD", 24576);
            temp.Add("SP", 0);
            temp.Add("LCL", 1);
            temp.Add("ARG", 2);
            temp.Add("THIS", 3);
            temp.Add("THAT", 4);

            return temp;
        }



        private static string RemoveComments(string line)
        {
            int commentStart = line.IndexOf("//");
            line = line.Remove(commentStart);
            line = line.Trim();

            return line;
        }



        private static short RamTableSearch(string key)
        {
            try
            {
                return RamTable[key];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                RamTable.Add(key, RamStartIndex);
                RamStartIndex++;

                return RamTable[key];
            }
        }
    }
}
