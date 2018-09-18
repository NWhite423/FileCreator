using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Xml.Linq;
using System.Threading;
using System.Text.RegularExpressions;

namespace FileCreator
{
    class Program
    {
        public static string[] ShortVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.');
        public static string MainText = String.Format("MDG File Creation Tool ({0})\nCopyright Melbourne Design Group 2018", ShortVersion[0] + "." + ShortVersion[1] + "." + ShortVersion[2]);
        public static string Seperator = @"\";

        public static class AppStatus
        {
            public static bool Application { get; set; }
            public static bool Job { get; set; }
        }

        static void Main(string[] args)
        {
            MainFunc();
        }

        private static void MainFunc()
        {
            if (!File.Exists("Settings.xml"))
            {
                bool settingsConfirmed = false;
                string directory;
                string files;
                while (!settingsConfirmed)
                {
                    Header();
                    Console.Write("\n\nPlease enter the directory for the customers: ");
                    directory = Console.ReadLine();
                    Header();
                    Console.Write("\n\nPlease enter the folders for a typical job seperated by a comma only (,): ");
                    files = Console.ReadLine();

                    Header();
                    Console.Write("\n\nDirectory: {0}\nFolders: {1}\nAre these values correct? ", directory, files);
                    switch (Console.ReadKey(true).Key.ToString().ToLower())
                    {
                        case "y":
                            {
                                Console.WriteLine("Saving settings...");
                                Thread.Sleep(1000);
                                XDocument doc = new XDocument(
                                    new XElement("Settings",
                                        new XElement("Directory", directory),
                                        new XElement("Folders", files)
                                    )
                                );
                                doc.Save("Settings.xml");
                                settingsConfirmed = true;
                                break;
                            };
                        default:
                            {
                                break;
                            };
                    };
                };
            };

            XDocument settings = XDocument.Load("Settings.xml");
            string AppDirectory = settings.Element("Settings").Element("Directory").Value;
            string[] Folders = settings.Element("Settings").Element("Folders").Value.Split(',');
            
            AppStatus.Application = false;
            AppStatus.Job = false;
            while (!AppStatus.Application)
            {
                Header();
                Console.WriteLine("Available options: help;\n\n");
                Console.Write("Please enter the customer name or another option: ");
                string input = Console.ReadLine();
                switch (input.ToLower())
                {
                    case "help":
                        {
                            Header();
                            Console.WriteLine("\nWelcome to the File Creator for Melbourne Design Group\nThis tool creates all the nessesary files for a job to quickly get off the ground. This tool allows for" +
                                " multiple customer support and automatic job assignment under each customer.\n\nDeveloped by: Nathan White\nVersion: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
                            Console.WriteLine("\nPress any key to return to the main window.");
                            Console.ReadKey();
                            break;
                        }
                    case "exit":
                        {
                            AppStatus.Application = true;
                            break;
                        }
                    default:
                        {
                            while (!AppStatus.Job)
                            {
                                string Customer;
                                string JobName;
                                string JobNumber;
                                string WorkPath;

                                if (!Directory.Exists(AppDirectory + Seperator + input))
                                {
                                    Header();
                                    Console.Write("\n\nError:\nThe Customer \"" + input + "\" does not exist.\n\nCreate new customer? (y/n): ");
                                    string create = Console.ReadKey(true).KeyChar.ToString().ToLower();
                                    switch (create)
                                    {
                                        case "y":
                                            {
                                                WorkPath = AppDirectory + Seperator + input;
                                                Directory.CreateDirectory(WorkPath);
                                                Console.WriteLine("\nCustomer Created at:\n\t" + WorkPath);
                                                Customer = input;
                                                break;
                                            }
                                        case "n":
                                            {
                                                AppStatus.Job = true;
                                                break;
                                            }
                                    };
                                } else
                                {
                                    WorkPath = AppDirectory + Seperator + input;
                                    Customer = input;
                                    JobNumber = DateTime.Today.Year.ToString() + "-" + (Directory.GetDirectories(WorkPath).Length + 1).ToString("D2");

                                    Header();
                                    Console.Write("\n\nCustomer: " + Customer + "\nJob Number: " + JobNumber + "\n\nPlease enter the job name: ");
                                    input = Console.ReadLine();
                                    JobName = input;
                                    WorkPath += Seperator + JobNumber + " " + input;

                                    Header();
                                    Console.Write("\n\nCustomer: " + Customer + "\nJob Number: " + JobNumber + "\nJob Name: " + JobName + "\nDirectory Path:\n\t" + WorkPath + "\n\nAre these values correct? (y/n): ");
                                    input = Console.ReadKey(true).KeyChar.ToString().ToLower();
                                    switch (input)
                                    {
                                        case "y":
                                            {
                                                Console.WriteLine("\nCreating " + WorkPath);
                                                Thread.Sleep(500);
                                                Directory.CreateDirectory(WorkPath);
                                                Console.WriteLine(WorkPath + " Created successfully\nCreating sub-folders...\n");
                                                Thread.Sleep(500);
                                                CreateProjectFolder(WorkPath, Folders);
                                                Console.WriteLine("Sub-folders created. Returning to main menu...");
                                                Thread.Sleep(3000);
                                                AppStatus.Job = true;
                                                break;
                                            }
                                        default:
                                            {
                                                AppStatus.Job = true;
                                                break;
                                            }
                                    }
                                }
                            };                            
                            break;
                        }
                }
            }
        }

        public static void Header()
        {
            Console.Clear();
            Console.WriteLine(MainText);
        }

        public static void CreateProjectFolder(string WorkPath, string[] Folders)
        {
            foreach (string Folder in Folders)
            {
                Console.WriteLine("Creating " + WorkPath + Seperator + Folder);
                Thread.Sleep(1000);
                Directory.CreateDirectory(WorkPath + Seperator + Folder);
                Console.WriteLine(WorkPath + Seperator + Folder + " Created successfully");
            };
        }
    }
}
