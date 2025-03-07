using Constants;
using Scenarios;
using System;
using WixSharp;

namespace WixSharpPackages
{
    public static class Program
    {
        static string versionString = "1.4.1.0";

        static void buildMsi(string name, Guid guid, WixEntity[] files)
        {
            var project = new Project(name,
                new Dir(@"C:\app", files)
            );

            if (Environment.Is64BitOperatingSystem) project.Platform = Platform.x64;

            project.GUID = guid;
            project.Version = new Version(versionString);

            project.UI = WUI.WixUI_ProgressOnly;

            Compiler.BuildMsi(project);
        }

        static void buildExternalCabMsi(string name, Guid guid, WixEntity[] files)
        {
            var project = new Project(name,
                new Dir(@"C:\app", files)
            );

            if (Environment.Is64BitOperatingSystem) project.Platform = Platform.x64;

            project.GUID = guid;
            project.Version = new Version(versionString);

            project.UI = WUI.WixUI_ProgressOnly;

            project.PreserveTempFiles = true;
            project.Media.Clear(); // Clear default Media settings
            project.Media.Add(new Media
            {
                Id = 1,
                Cabinet = $"{name}.cab",      // Required Cabinet attribute
                EmbedCab = false,               // Ensures the CAB file is external
                CompressionLevel = CompressionLevel.none
            });

            string pathDebug = System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
            var directoryProject = System.IO.Directory.GetParent(pathDebug).Parent.Parent;
            string pathProject = directoryProject.FullName;
            //string pathCabs = System.IO.Path.Combine(pathProject, "cabs");

            //if (!System.IO.Directory.Exists(pathCabs)) System.IO.Directory.CreateDirectory(pathCabs);

            //Compiler.LightOptions += $" -b \"{pathProject}\" -b \"{pathCabs}\"";
            //Compiler.LightOptions += $" -b \"{pathProject}\\ExternalFiles\"";
            Compiler.BuildMsi(project);
        }

        public static void Main(string[] args)
        {
            var guid1 = new Guid("6f330b47-2577-43ad-1000-0861ba258001");
            var guid2 = new Guid("6f330b47-2577-43ad-1000-1861ba258001");
            var guid3 = new Guid("6f330b47-2577-43ad-1000-2861ba258001");
            var guid4 = new Guid("6f330b47-2577-43ad-1000-3861ba258001");

            buildMsi("SimpleRuntime", guid1,
                new[] { new File(@"Files\runtime.txt") });
            buildMsi("SimpleApp", guid2,
                new[] { new File(@"Files\1.txt") });
            //buildMsi("SimpleApp2", new Guid("6f330b47-2577-43ad-2000-2861ba258001"),
            //    new[] { new File(@"Files\2.txt") });
            buildExternalCabMsi("ExternalCabMsi", guid3,
                new[] { new File(@"Files\2.txt") });


            var project = new Project(ApplicationConstants.PRODUCT,
                new Dir($"%ProgramFiles%\\{ApplicationConstants.COMPANY}\\{ApplicationConstants.PRODUCT}", //we can use also use AppDataFolder
                    new File(@"Files\Scenarios.dll"),
                    new File(@"Files\Manual.txt"),
                    new File(@"Files\TheConsoleApp.exe")
                    //  new FileShortcut(ApplicationConstants.PRODUCT, $"%ProgramMenu%"),
                    //  new FileShortcut(ApplicationConstants.PRODUCT, @"%Desktop%")// { Advertise = true }),
                ),
                new Dir(@"%Desktop%",
                    new ExeFileShortcut(ApplicationConstants.PRODUCT, "[INSTALLDIR]TheConsoleApp.exe", arguments: "")
                    //we can also have conditions defined
                    //  Condition = new Condition("INSTALLDESKTOPSHORTCUT=\"yes\"")
                ),
                new EnvironmentVariable("WixSharpApp", ApplicationConstants.PRODUCT),
                new ManagedAction(CustomActions.WriteToSpecialFolders, CustomActions.DeleteFromSpecialFolders)
                {
                    RefAssemblies = new[] { typeof(FileScenarios).Assembly.Location }
                },
                new ManagedAction(CustomActions.CreateScheduledTasks, CustomActions.DeleteScheduledTasks),
                new ManagedAction(CustomActions.CreateSymbolicLinks, CustomActions.DeleteSymbolicLinks)
            );
            
            if (Environment.Is64BitOperatingSystem) project.Platform = Platform.x64;

            project.GUID = guid4;
            project.Version = new Version(versionString);

            project.UI = WUI.WixUI_ProgressOnly;

            Compiler.BuildMsi(project);
        }
    }
}
