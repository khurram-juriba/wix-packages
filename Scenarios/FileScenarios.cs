using Scenarios.Files;
using Scenarios.Helpers;
using System;
using System.IO;

namespace Scenarios
{
    public static class FileScenarios
    {
        static void writeFile(string folder, string folderTitle, string file, Action<string> log)
        {
            try
            {
                using (var sw = new StreamWriter(Path.Combine(folder, file)))
                {
                    sw.WriteLine($"This is a test entry for {folderTitle}");
                }
            }
            catch (Exception ex)
            {
                log($"Failed to write to file at {folder}");
                log(ex.ToString());
            }
        }

        public static string UnzipToProgramFilesFolder(string sourceArchiveFileName, string[] parts)
        {
            string destinationDirectoryName = FilesHelper.CreateProgramFilesFolder(parts);
            FilesHelper.Unzip(sourceArchiveFileName, destinationDirectoryName);
            return destinationDirectoryName;
        }

        public static void WriteToSpecialFolders(Action<string> log, string company, string product)
        {
            foreach (var entry in SpecialFolderEntry.GetSpecialFolders(log, company, product))
            {
                writeFile(entry.FolderPath, entry.Title, entry.FileName, log);
                log($"{entry.Title} written at {Path.Combine(entry.FolderPath, entry.FileName)}");
            }
        }

        public static void DeleteFromSpecialFolders(Action<string> log, string company, string product)
        {
            foreach (var entry in SpecialFolderEntry.GetSpecialFolders(log, company, product))
            {
                if (!string.IsNullOrEmpty(entry.FolderPath) && !string.IsNullOrEmpty(entry.FileName))
                {
                    var file = Path.Combine(entry.FolderPath, entry.FileName);
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                        log($"{file} deleted");
                    }
                    else
                        log($"{file} doesnt exists");
                }
            }
        }

        public static void VerifySpecialFolders(Action<string> log, string company, string product)
        {
            foreach (var entry in SpecialFolderEntry.GetSpecialFolders(log, company, product))
            {
                if (!string.IsNullOrEmpty(entry.FolderPath) && !string.IsNullOrEmpty(entry.FileName))
                {
                    var file = Path.Combine(entry.FolderPath, entry.FileName);
                    if (File.Exists(file))
                        log($"{file} doesnt exists");
                    else
                        log($"{file} exists");
                }
            }
        }
    }
}
