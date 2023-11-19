using Constants;
using Scenarios;
using System;
using WixSharp;

namespace WixSharpPackages
{
    public static class Program
    {
        static string versionString = "1.2.0.0";

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

        public static void Main(string[] args)
        {
            buildMsi("SimpleRuntime", new Guid("6f330b47-2577-43ad-1000-0861ba258001"),
                new[] { new File(@"Files\runtime.txt") });
            buildMsi("SimpleApp", new Guid("6f330b47-2577-43ad-1000-1861ba258001"),
                new[] { new File(@"Files\1.txt") });
            //buildMsi("SimpleApp2", new Guid("6f330b47-2577-43ad-2000-1861ba258001"),
            //    new[] { new File(@"Files\2.txt") });


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

            project.GUID = new Guid("6f330b47-2577-43ad-1000-1861ba25889b");
            project.Version = new Version(versionString);

            project.UI = WUI.WixUI_ProgressOnly;

            Compiler.BuildMsi(project);
        }
    }
}
