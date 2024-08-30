using Constants;
using Scenarios;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using static System.Console;

namespace TheConsoleApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var f = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            string appVersion = f.ProductVersion;
            WriteLine($"{ApplicationConstants.PRODUCT} v{appVersion}");

            while(true)
            {
                string input = "";

                Action getMenuOption = () =>
                {
                    WriteLine("Choose action");
                    WriteLine("[1] Verify [2] Write Registries [3] Read Registries [4] Write to Folders [5] Read from Folders");
                    WriteLine("[6 or eventlogs] Eventlogs [7 or exitcode] Exit Code [8 or reboot] Reboot");
                    Console.Write("[q] to quit\t");

                    input = Console.ReadLine();
                };

                if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
                    input = args[0].Trim();
                else
                    getMenuOption();

                if (input == "1")
                    InstallationScenarios.Verify(s => WriteLine(s),
                        ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);
                else if (input == "2")
                    RegistryScenarios.WriteRegistries(s => WriteLine(s),
                        ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);
                else if (input == "3")
                    RegistryScenarios.ReadRegistries(s => WriteLine(s),
                        ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);
                else if (input == "4")
                    FileScenarios.WriteToSpecialFolders(s => WriteLine(s),
                        ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);
                else if (input == "5")
                    FileScenarios.VerifySpecialFolders(s => WriteLine(s),
                        ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);
                else if (input == "6" || input.Equals("eventlogs", StringComparison.InvariantCultureIgnoreCase))
                    writeEventLogs();
                else if (input == "7" || input.Equals("exitcode", StringComparison.InvariantCultureIgnoreCase))
                {
                    int code = 0;
                    if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]) && int.TryParse(args[1], out code))
                        exitCodes(code);
                    else
                        exitCodes();
                    
                    return;
                }
                else if (input =="8" || input.Equals("reboot", StringComparison.InvariantCultureIgnoreCase))
                {
                    //RebootHelper.Reboot();
                    LibClass.ExitWindows(RestartOptions.Reboot, force: true);

                    return;
                }
                else if (input == "q")
                    break;
                else
                    getMenuOption();
            }
        }

        static void writeEventLogs()
        {
            WriteLine("Attempting to write test Event Logs");

            var info = "This event log was emitted by application as an INFORMATION.";
            var error = "This event log was emitted by application as an ERROR.";
            var warning = "This event log was emitted by application as an WARNING.";

            try
            {
                using (var eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "EventLogger";
                    eventLog.WriteEntry(info, EventLogEntryType.Information, 101, 1);
                    eventLog.WriteEntry(error, EventLogEntryType.Error, 101, 1);
                    eventLog.WriteEntry(warning, EventLogEntryType.Warning, 101, 1);
                }

                WriteLine("3 events raised. Waiting for 30 seconds...");
                Thread.Sleep(30 * 1000);
                WriteLine("Completed!");
            }
            catch (Exception ex)
            {
                WriteLine($"Failed, {ex.Message}");
            }
        }

        static void exitCodes(int code)
        {
            WriteLine($"Exiting with code {code}");
            Environment.Exit(code);
        }

        static void exitCodes()
        {
            WriteLine("Enter the exit code\t");
            exitCodes(int.Parse(Console.ReadLine()));
        }
    }
}
