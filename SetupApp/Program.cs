using System;
using System.Windows.Forms;

namespace SetupApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var f = new FormSetup();
            if (null != args && args.Length > 0)
                f.Load += delegate { f.HandleArguments(args); };

            Application.Run(f);
        }
    }
}
