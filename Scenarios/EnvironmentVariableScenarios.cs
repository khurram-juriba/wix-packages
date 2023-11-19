using System;

namespace Scenarios
{
    public static class EnvironmentVariableScenarios
    {
        public static void SetEnvironmentVariables(Action<string> log, string company, string product)
        {
            var machine = $"{company.ToUpper()}_MACHINE";
            var user = $"{company.ToUpper()}_USER";
            log($"Setting {machine} to {company} for Machine");
            log($"Setting {user} to {product} for User");

            Environment.SetEnvironmentVariable(machine, company, EnvironmentVariableTarget.Machine);
            Environment.SetEnvironmentVariable(user, product, EnvironmentVariableTarget.User);
        }

        public static void DeleteEnvironmentVariables(Action<string> log, string company)
        {
            var machine = $"{company.ToUpper()}_MACHINE";
            var user = $"{company.ToUpper()}_USER";
            log($"Unsetting {machine} for Machine");
            log($"Unsetting {user} for User");

            Environment.SetEnvironmentVariable($"{company.ToUpper()}_MACHINE", null, EnvironmentVariableTarget.Machine);
            Environment.SetEnvironmentVariable($"{company.ToUpper()}_USER", null, EnvironmentVariableTarget.User);
        }

        public static void VerifyEnvironmentVariables(Action<string> log, string company, string product)
        {
            var machine = $"{company.ToUpper()}_MACHINE";
            var user = $"{company.ToUpper()}_USER";
            var machineValue = Environment.GetEnvironmentVariable(machine, EnvironmentVariableTarget.Machine);
            var userValue = Environment.GetEnvironmentVariable(user, EnvironmentVariableTarget.User);

            if (machineValue == company)
                log($"Variable: {machine}, expected value: {machineValue} for machine");
            else
                log($"Variable: {machine}, unexpected value: {machineValue} for machine; expected value: {company}");

            if (userValue == product)
                log($"Variable: {user}, expected value: {userValue} for user");
            else
                log($"Variable: {user}, unexpected value: {userValue} for user; expected value: {product}");

        }
    }
}
