using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace FileCreator
{
    class Program
    {
        public static string MainDir = @"C:\Users\Nathan\Dropbox\Customers";
        public static string[] Files = { "Legal", "CAD", "Excel", "Output", "Payment", "Misc" };
        public static string MainText = "MDG File Creation Tool (V1.0)\n\n";
        public static string Seperator = @"\";
        public static string Customer;
        public static string JobName;
        public static int JobNumber;

        static void Main(string[] args)
        {
            MainFunc();
        }

        private static void MainFunc()
        {
            bool finishApp = false;
            while (!finishApp)
            {
                Console.Clear();
                Console.WriteLine(MainText);
                Console.Write("Please enter the Customer: ");
                string Client = Console.ReadLine();
                if (!Directory.Exists(MainDir + Seperator + Client))
                {
                    Console.Clear();
                    Console.WriteLine(MainText);
                    Console.Write("That customer does not exist.\nCreate customer now? (y/n): ");
                    if (Console.ReadKey(true).KeyChar.ToString().ToLower() == "y")
                    {
                        Directory.CreateDirectory(MainDir + Seperator + Client);
                        Console.WriteLine("\nCustomer Created\nPress any key to continue.");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        if (Console.ReadKey(true).KeyChar.ToString().ToLower() == "n")
                        {
                            Console.WriteLine("\n\nERROR: Cannot proceed.\nNo customer by that name exists.\n\nPress any key to continue.");
                            Console.ReadKey(true);
                        }
                    }
                } else
                {
                    var CustomerPass = false;
                    Customer = Client;
                    while (!CustomerPass)
                    {
                        Console.Clear();
                        Console.WriteLine(MainText);
                        JobNumber = Directory.GetDirectories(MainDir + Seperator + Customer).Length + 1;

                        Console.Write("Customer: " + Customer + "\nJob Number: " + DateTime.Today.Year.ToString() + "-" + JobNumber.ToString("D2") + "\nPlease enter the job name: ");
                        string Job = Console.ReadLine();
                        var path = MainDir + Seperator + Customer + Seperator + DateTime.Today.Year.ToString() + "-" + JobNumber.ToString("D2") + " " + Job;
                        if (Directory.Exists(path))
                        {
                            Console.WriteLine("\n\nERROR: Cannot proceed\nJob name already exists.\nPress any key to continue.");
                            Console.ReadKey(true);
                        } else
                        {
                            JobName = Job;
                            Directory.CreateDirectory(path);
                            Console.WriteLine("Job " + Job + " (" + DateTime.Today.Year.ToString() + "-" + JobNumber.ToString("D2") + ") has been created. Creating sub-folders");
                            foreach (string folder in Files)
                            {
                                CreateProjectFolder(folder);
                            }
                            Console.WriteLine("Files created.\n Pres any key to continue");
                            Console.ReadKey(true);
                            CustomerPass = true;
                            finishApp = true;
                        }
                    }
                }
            }
        }

        public static void CreateProjectFolder(string name)
        {
            var path = MainDir + Seperator + Customer + Seperator + DateTime.Today.Year.ToString() + "-" + JobNumber.ToString("D2") + " " + JobName;
            Directory.CreateDirectory(path + Seperator + name);
            Console.WriteLine(JobName + Seperator + name + " created successfully.");
        }
    }
}
