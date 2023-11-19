using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Scenarios
{
    public static class ScheduledTaskScenarios
    {
        static int runSchTasks(string arguments, Action<string> output = null)
        {
            string EXE_SCHTASKS = "schtasks.exe";
            var system32 = Environment.GetFolderPath(Environment.SpecialFolder.System);
            var process = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true, RedirectStandardError = true,
                    FileName = Path.Combine(system32, EXE_SCHTASKS),
                    Arguments = arguments
                }
            };

            process.Start();
            if (null != output)
            {
                //we can additionaly have an event handler for process.ErrorDataReceived and log e.Data
                output(process.StandardOutput.ReadToEnd());
            }

            process.WaitForExit();
            var processExitCode = process.ExitCode;
            process.Dispose();
            
            return processExitCode;
        }

        static string getTaskName(string product)
        {
            return $"Launch {product} App";
        }

        public static bool CreateScheduledTasks(Action<string> log, string company, string product, string exePath)
        {
            try
            {
                //for user specific task we can get userName using Environment.GetEnvironmentVariable("USERNAME")
                //we can run the task on each logon using something like /Create /TN \"{TASK_NAME}\" /SC ONLOGON /TR \"{filePath}\" /RU \"{userName}\" /IT
                var arguments = $"/Create /TN \"{getTaskName(product)}\" /SC ONCE /ST 00:00 /TR \"'{exePath}'\" /IT";
                log($"Calculated Arguments: {arguments}");

                var processExitCode = runSchTasks(arguments);
                return processExitCode == 0;
            }
            catch (Exception ex)
            {
                log("Failed to create task");
                log(ex.ToString());
                return false;
            }
        }

        public static bool DeleteScheduledTasks(Action<string> log, string product)
        {
            try
            {
                var arguments = $"/Delete /TN \"{getTaskName(product)}\" /F";
                var processExitCode = runSchTasks(arguments);
                return processExitCode == 0;
            }
            catch (Exception ex)
            {
                log("Failed to delete task");
                log(ex.ToString());
                return false;
            }
        }

        public static bool VerifyScheduledTasks(Action<string> log, string product)
        {
            try
            {
                var arguments = $"/Query /FO CSV /NH"; //we will get output like "\Microsoft\Windows\Workplace Join\Recovery-Check","N/A","Disabled"
                // Task Path
                // Last Runtime: N/A or 9/9/2023 11:28:04 PM
                // Status: Disabled, Ready etc

                bool found = false;
                string output = null;
                var processExitCode = runSchTasks(arguments, s => output = s);

#pragma warning disable S2583
                if (null != output)
                {
                    var lines = output.Split(Environment.NewLine.ToCharArray());

                    
                    foreach (var line in lines.OrderBy(o => o))
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var tokens = line.Split(",".ToCharArray());
                            if (tokens.Length >= 3) //we have all the needed info
                            {
                                var task = tokens[0]; //something\somefolder\task
                                if (task == string.Format(@"""\{0}""", getTaskName(product)))
                                {
                                    log($"Task found: {task}");
                                    found = true;
                                    break;
                                }
                            }
                        }
                    }
                }
#pragma warning restore S2583 // Conditionally executed code should be reachable

                return processExitCode == 0 && found;
            }
            catch (Exception ex)
            {
                log("Failed to verify task");
                log(ex.ToString());
                return false;
            }
        }
    }
}
