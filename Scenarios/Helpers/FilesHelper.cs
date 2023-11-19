using System;
using System.IO;
using System.IO.Compression;

namespace Scenarios.Helpers
{
    public static class FilesHelper
    {
        internal static string CreateFolderStructure(string parentFolder, string[] parts)
        {
            if (!Directory.Exists(parentFolder)) return null;

            var path = parentFolder;
            foreach (var part in parts)
            {
                path = Path.Combine(path, part);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            }

            return path;
        }

        internal static string CreateProgramFilesFolder(string[] parts)
        {
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            return CreateFolderStructure(programFiles, parts);
        }

        internal static void MoveFile(string source, string destination)
        {
            if (File.Exists(source))
            {
                if (File.Exists(destination)) File.Delete(destination);
                File.Move(source, destination);
            }
        }

        public static void Unzip(string sourceArchiveFileName, string destinationDirectoryName)
        {
            if (File.Exists(sourceArchiveFileName) && Directory.Exists(destinationDirectoryName))
                ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
        }
    }
}
