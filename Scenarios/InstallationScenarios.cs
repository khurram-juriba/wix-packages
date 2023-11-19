using System;

namespace Scenarios
{
    public static class InstallationScenarios
    {
        public static void Install(Action<string> log, string company, string product, string exePath)
        {
            FileScenarios.WriteToSpecialFolders(s => log($"[FileScenarios] {s}"),
                company, product);
            log("Files are created in special folders...");

            ScheduledTaskScenarios.CreateScheduledTasks(s => log($"[ScheduledTaskScenarios] {s}"),
                company, product, exePath);
            log("Tasks are created...");

            SymbolicLinkScenarios.CreateSymbolicLinks(s => log($"[SymbolicLinkScenarios] {s}"),
                company, product);
            log("Symbolic Links are created...");

            EnvironmentVariableScenarios.SetEnvironmentVariables(s => log($"[EnvironmentVariableScenarios] {s}"),
                company, product);
            log("Environment Variables are created...");
        }

        public static void Uninstall(Action<string> log, string company, string product)
        {
            FileScenarios.DeleteFromSpecialFolders(s => log($"[FileScenarios] {s}"),
                company, product);
            log("Files deletion from special folders is completed...");

            if (ScheduledTaskScenarios.DeleteScheduledTasks(s => log($"[ScheduledTaskScenarios] {s}"),
                product))
                log("Tasks are deleted...");
            else
                log("Tasks are not deleted...");

            SymbolicLinkScenarios.DeleteSymbolicLinks(s => log($"[SymbolicLinkScenarios] {s}"),
                product);
            log("Symbolic links deletion is completed...");

            EnvironmentVariableScenarios.DeleteEnvironmentVariables(s => log($"[EnvironmentVariableScenarios] {s}"),
                company);
            log("Environment Variables deletion is completed...");
        }

        public static void Verify(Action<string> log, string company, string product)
        {
            FileScenarios.VerifySpecialFolders(s => log($"[FileScenarios] {s}"),
                company, product);
            log("Files in special folders verification is completed...");

            if (ScheduledTaskScenarios.VerifyScheduledTasks(s => log($"[ScheduledTaskScenarios] {s}"),
                product))
                log("Tasks are verified...");
            else
                log("Tasks verification failed...");

            SymbolicLinkScenarios.VerifySymbolicLinks(s => log($"[SymbolicLinkScenarios] {s}"),
                product);
            log("Symbolic Links verification is completed...");

            EnvironmentVariableScenarios.VerifyEnvironmentVariables(s => log($"[EnvironmentVariableScenarios] {s}"),
                company, product);
            log("Environment Variables verification is completed...");
        }
    }
}
