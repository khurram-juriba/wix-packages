using Microsoft.Win32;
using System;

namespace Scenarios
{
    public static class RegistryScenarios
    {
        static string getKeyPath(string company, string product)
        {
            Action check = () =>
            {
                if (string.IsNullOrEmpty(company)) throw new ArgumentException("company");
                if (string.IsNullOrEmpty(product)) throw new ArgumentException("product");
            };

            check();
            company = company.Trim().Replace(" ", "");
            product = product.Trim().Replace(" ", "");
            check();

            return $"SOFTWARE\\{company}\\{product}";
        }

        static bool readRegistryValue(Action<string> log, RegistryHive hive, string keyPath, string valueName)
        {
            RegistryKey registryKey = null;

            try
            {
                bool writeable = false;
                RegistryView view = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
                registryKey = RegistryKey.OpenBaseKey(hive, view)
                    .OpenSubKey(keyPath, writeable);

                if (registryKey != null)
                {
                    object value = registryKey.GetValue(valueName);
                    if (null != value)
                        log($"{hive}\\{keyPath}\\{valueName} is {value}");
                    return true;
                }
                else
                    log($"{hive}\\{keyPath} not found");
            }
            catch (Exception ex)
            {
                log($"Failed to read registry {hive}\\{keyPath}");
                log(ex.ToString());
            }
            finally
            {
                if (registryKey != null)
                    registryKey.Close(); //lets close
            }

            return false;
        }

        static bool writeRegistryValue(Action<string> log, RegistryHive hive, string[] keyPaths, string valueName, object value)
        {
            RegistryKey registryKey = null;

            var finalPath = $"SOFTWARE";

            try
            {
                bool writeable = true;
                RegistryView view = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;

                foreach(var keyPath in keyPaths)
                {
                    var newPath = $"{finalPath}\\{keyPath}";
                    registryKey = RegistryKey.OpenBaseKey(hive, view)
                        .OpenSubKey(newPath, writeable);
                    
                    if (registryKey == null)
                    {
                        registryKey = RegistryKey.OpenBaseKey(hive, view)
                            .OpenSubKey(finalPath, writeable);
                        if (null != registryKey)
                            registryKey.CreateSubKey(keyPath);
                        else
                            log($"Failed to open {finalPath}");
                    }

                    finalPath += $"\\{keyPath}";
                }

                registryKey = RegistryKey.OpenBaseKey(hive, view)
                    .OpenSubKey(finalPath, writeable);

                if (registryKey != null)
                {
                    registryKey.SetValue(valueName, value);
                    log($"{hive}\\{finalPath}\\{valueName} is set to {value}");
                    return true;
                }
                else
                    log($"{hive}\\{finalPath} not found");
            }
            catch (Exception ex)
            {
                log($"Failed to add registry {hive}\\{finalPath}");
                log(ex.ToString());
            }
            finally
            {
                if (registryKey != null)
                    registryKey.Close(); //lets close
            }

            return false;
        }

        public static void WriteRegistries(Action<string> log, string company, string product)
        {
            if (!writeRegistryValue(log, RegistryHive.CurrentUser,
                new[] { company, product },
                "TimeStamp", DateTime.Now.ToString()))
                log("Failed to write user registry");

            if (!writeRegistryValue(log, RegistryHive.LocalMachine,
                new[] { company, product },
                "TimeStamp", DateTime.Now.ToString()))
                log("Failed to write system registry");
        }

        public static void ReadRegistries(Action<string> log, string company, string product)
        {
            if (!readRegistryValue(log, RegistryHive.CurrentUser,
                    getKeyPath(company, product),
                    "TimeStamp"))
                log("Failed to read user registry");

            if (!readRegistryValue(log, RegistryHive.LocalMachine,
                getKeyPath(company, product),
                "TimeStamp"))
                log("Failed to read system registry");
        }
    }
}
