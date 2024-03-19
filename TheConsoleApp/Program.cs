using Constants;
using Scenarios;
using System;
using System.Diagnostics;
using System.Reflection;

namespace TheConsoleApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var f = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            string appVersion = f.ProductVersion;
            Console.WriteLine($"{ApplicationConstants.PRODUCT} v{appVersion}");

            while(true)
            {
                Console.WriteLine("Choose action");
                Console.WriteLine("[1] Verify [2] Write Registries [3] Read Registries [4] Write to Folders [5] Read from Folders");
                Console.Write("[q] to quit\t");
                var input = Console.ReadLine();

                if (input == "1")
                    InstallationScenarios.Verify(s => Console.WriteLine(s),
                        ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);
                else if (input == "2")
                    RegistryScenarios.WriteRegistries(s => Console.WriteLine(s),
                        ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);
                else if (input == "3")
                    RegistryScenarios.ReadRegistries(s => Console.WriteLine(s),
                        ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);
                else if (input == "4")
                    FileScenarios.WriteToSpecialFolders(s => Console.WriteLine(s),
                        ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);
                else if (input == "5")
                    FileScenarios.VerifySpecialFolders(s => Console.WriteLine(s),
                        ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);
                else if (input == "q")
                    break;
            }


            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
