using Scenarios.Helpers;
using System;
using System.IO;

namespace Scenarios
{
    public static class SymbolicLinkScenarios
    {
        public static void CreateSymbolicLinks(Action<string> log, string company, string product)
        {
            string system32 = Environment.SystemDirectory; // C:\windows\system32
            string winDrive = Path.GetPathRoot(Environment.SystemDirectory); // C:\
            var bin = FilesHelper.CreateFolderStructure(winDrive, new[] { "bin" });
            //var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var programFiles = Environment.GetEnvironmentVariable("ProgramFiles");
            var notepadPath = Path.Combine(system32, "notepad.exe");
            var chromePath = Path.Combine(programFiles, "Google", "Chrome", "Application", "chrome.exe");

            log($"Bin: {bin}");
            //log($"Desktop: {desktop}");
            log($"Notepad: {notepadPath}");
            log($"Chrome: {chromePath}");

            Action<string> createDirectoryLink = link =>
            {
                var target = Path.Combine(programFiles, company, product);
                bool folderResult = SymbolicLinkHelper.CreateSymbolicLink(log, link,
                    target, true /*isDirectory*/);
                if (folderResult)
                    log($"{link} symbolic link is created for {target}");
                else
                    log($"{link} symbolic link was not created for {target}");
            };

            createDirectoryLink(Path.Combine(bin, product));
            //createDirectoryLink(Path.Combine(desktop, product));

            Action<string, string> createFireLink = (file, link) =>
            {
                if (File.Exists(file))
                {
                    bool fileResult = SymbolicLinkHelper.CreateSymbolicLink(log, link,
                        file, false /*isDirectory*/);
                    if (fileResult)
                        log($"{link} symbolic link is created for {file}");
                    else
                        log($"{link} symbolic link was not created for {file}");
                }
            };

            createFireLink(notepadPath, Path.Combine(bin, "notepad.lnk"));
            //createFireLink(notepadPath, Path.Combine(desktop, "notepad.lnk"));

            createFireLink(chromePath, Path.Combine(bin, "chrome.lnk"));
            //createFireLink(chromePath, Path.Combine(desktop, "chrome.lnk"));
        }

        public static void DeleteSymbolicLinks(Action<string> log, string product)
        {
            Action<string> deleteFolder = (parent) =>
            {
                var folder = Path.Combine(parent, product);
                if (Directory.Exists(folder))
                {
                    Directory.Delete(folder);
                    log($"{folder} is deleted");
                }
                else
                    log($"{folder} doesnt exists");
            };

            Action<string> deleteFile = (file) =>
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                    log($"{file} is deleted");
                }
                else
                    log($"{file} doesnt exists");
            };

            string winDrive = Path.GetPathRoot(Environment.SystemDirectory);
            var bin = Path.Combine(winDrive, "bin");

            deleteFolder(bin);
            deleteFile(Path.Combine(bin, "notepad.lnk"));
            deleteFile(Path.Combine(bin, "chrome.lnk"));

            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            deleteFolder(desktop);
            deleteFile(Path.Combine(desktop, "notepad.lnk"));
            deleteFile(Path.Combine(desktop, "chrome.lnk"));
        }

        public static void VerifySymbolicLinks(Action<string> log, string product)
        {
            Action<string> verifyFolder = (parent) =>
            {
                var folder = Path.Combine(parent, product);
                if (Directory.Exists(folder))
                {
                    try
                    {
                        var target = SymbolicLinkHelper.ResolveSymbolicLink(folder);
                        log($"{folder} resolved to {target}");
                    }
                    catch (Exception ex)
                    {
                        log($"Failed to resolve {folder} as symbolic link; {ex.GetType()}: {ex.Message}");
                    }
                }
                else
                    log($"{folder} doesnt exists");
            };

            Action<string> verifyFile = (file) =>
            {
                if (File.Exists(file))
                {
                    try
                    {
                        var target = SymbolicLinkHelper.ResolveSymbolicLink(file);
                        log($"{file} resolved to {target}");
                    }
                    catch (Exception ex)
                    {
                        log($"Failed to resolve {file} as symbolic link; {ex.GetType()}: {ex.Message}");
                    }
                }
                else
                    log($"{file} doesnt exists");
            };

            string winDrive = Path.GetPathRoot(Environment.SystemDirectory);
            var bin = Path.Combine(winDrive, "bin");

            verifyFolder(bin);
            verifyFile(Path.Combine(bin, "notepad.lnk"));
            verifyFile(Path.Combine(bin, "chrome.lnk"));

            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            verifyFolder(desktop);
            verifyFile(Path.Combine(desktop, "notepad.lnk"));
            verifyFile(Path.Combine(desktop, "chrome.lnk"));
        }
    }
}
