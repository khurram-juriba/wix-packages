using Constants;
using Microsoft.Deployment.WindowsInstaller;
using Scenarios;
using System;

namespace WixSharpPackages
{
    public static class CustomActions
    {
        #region Special Folders

        [CustomAction]
        public static ActionResult WriteToSpecialFolders(Session session)
        {
            session.Log("Started WriteToSpecialFolders");
            FileScenarios.WriteToSpecialFolders(s => session.Log(s), ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);
            session.Log("Finished WriteToSpecialFolders");
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult DeleteFromSpecialFolders(Session session)
        {
            session.Log("Started DeleteFromSpecialFolders");
            FileScenarios.DeleteFromSpecialFolders(s => session.Log(s), ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);
            session.Log("Finished DeleteFromSpecialFolders");
            return ActionResult.Success;
        }

        #endregion

        #region Scheduled Tasks

        [CustomAction]
        public static ActionResult CreateScheduledTasks(Session session)
        {
            session.Log("Started CreateScheduledTasks");
            
            var programFiles = Environment.GetEnvironmentVariable("ProgramFiles");
            var exePath = $"{programFiles}\\{ApplicationConstants.COMPANY}\\{ApplicationConstants.PRODUCT}\\{ApplicationConstants.PRODUCT}.exe";
            bool success = ScheduledTaskScenarios.CreateScheduledTasks(s => session.Log(s), ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT, exePath);
            
            session.Log("Finished CreateScheduledTasks");
            return success ? ActionResult.Success : ActionResult.Failure;
        }

        [CustomAction]
        public static ActionResult DeleteScheduledTasks(Session session)
        {
            session.Log("Started DeleteScheduledTasks");
            bool success = ScheduledTaskScenarios.DeleteScheduledTasks(s => session.Log(s), ApplicationConstants.PRODUCT);
            session.Log("Finished DeleteScheduledTasks");
            return success ? ActionResult.Success : ActionResult.Failure;
        }

        #endregion

        #region Symbolic Links

        [CustomAction]
        public static ActionResult CreateSymbolicLinks(Session session)
        {
            session.Log("Started CreateSymbolicLinks");
            SymbolicLinkScenarios.CreateSymbolicLinks(s => session.Log(s), ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);
            session.Log("Finished CreateSymbolicLinks");
            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult DeleteSymbolicLinks(Session session)
        {
            session.Log("Started DeleteSymbolicLinks");
            SymbolicLinkScenarios.DeleteSymbolicLinks(s => session.Log(s), ApplicationConstants.PRODUCT);
            session.Log("Finished DeleteSymbolicLinks");
            return ActionResult.Success;
        }

        #endregion
    }
}
