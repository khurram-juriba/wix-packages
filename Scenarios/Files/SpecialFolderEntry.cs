using Scenarios.Helpers;
using System;
using System.Collections.Generic;

namespace Scenarios.Files
{
    class SpecialFolderEntry
    {
        public string Title { get; set; }
        public string FolderPath { get; set; }
        public string FileName { get; set; }

        public static IEnumerable<SpecialFolderEntry> GetSpecialFolders(Action<string> log, string company, string product)
        {
            var userProfile = Environment.GetEnvironmentVariable("USERPROFILE");
            var myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            log($"UserProfile: {userProfile}");
            log($"MyDocuments: {myDocumentsPath}");
            log($"CommonApplicationData: {programData}"); // should be C:\ProgramData

            if (!string.IsNullOrEmpty(userProfile))
                yield return new SpecialFolderEntry() { Title = "USERPROFILE", FolderPath = userProfile, FileName = $"{product}-UserProfile.txt" };

            if (!string.IsNullOrEmpty(myDocumentsPath))
                yield return new SpecialFolderEntry() { Title = "MY-DOCUMENTS", FolderPath = myDocumentsPath, FileName = $"{product}-MyDocuments.txt" };

            if (!string.IsNullOrEmpty(programData))
                yield return new SpecialFolderEntry() { Title = "COMMON-APPLICATION-DATA", FolderPath = programData, FileName = $"{product}-CommonApplicationData.txt" };

            /*
             * ICE 30
             * if (programData != @"C:\ProgramData")
             *  yield return new SpecialFolderEntry() { Title = "PROGRAM-DATA", FolderPath = @"C:\ProgramData", FileName = $"{product}-ProgramData.txt" };
             * 
             * yield return new SpecialFolderEntry() { Title = "ALL-USERS", FolderPath = @"C:\Users\All Users", FileName = $"{product}-AllUsers.txt" };
             */

            var programDataApp = FilesHelper.CreateFolderStructure(programData, new[] { company });
            yield return new SpecialFolderEntry() { Title = "COMMON-APPLICATION-DATA-COMPANY", FolderPath = programDataApp, FileName = $"{product}-CommonApplicationData-Company.txt" };

            /*
             * Similar to UserProfiles; we should support All Users folder
             * var allUsersApp = createFolderStructure(@"C:\Users\All Users", new[] { company });
             *  yield return new SpecialFolderEntry() { Title = "ALL-USERS-COMPANY", FolderPath = allUsersApp, FileName = $"{product}-AllUsers-Company.txt" };
             */
        }
    }
}
