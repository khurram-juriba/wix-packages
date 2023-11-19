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

            InstallationScenarios.Verify(s => Console.WriteLine(s),
                ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);

            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }
}
