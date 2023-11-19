using Constants;
using Scenarios;
using Scenarios.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Threading;

namespace SetupApp
{
    public partial class FormSetup : Form
    {
        Dispatcher uiDispatcher;

        void writeToTextBox(string s)
        {
            if (null != uiDispatcher)
                uiDispatcher.Invoke(() => this.textBoxLogs.Text += $"{s}{Environment.NewLine}");
        }

        void setText(string s)
        {
            if (null != uiDispatcher)
                uiDispatcher.Invoke(() => this.Text = s);
        }

        void setEnabled(bool value)
        {
            if (null != uiDispatcher)
                uiDispatcher.Invoke(() =>
                {
                    this.buttonClose.Enabled = this.buttonInstallSimpleApp.Enabled =
                    this.buttonInstallConsole.Enabled = this.buttonUninstallConsole.Enabled = this.buttonVerifyConsole.Enabled = value;
                });
        }

        void installMsi(string msi)
        {
            var argument = $"/i {msi}"; // we can append /q here to install msi silently
            this.writeToTextBox($"Prepared Argument: {argument}");

            var installerProcess = new Process();
            var processInfo = new ProcessStartInfo();
            processInfo.Arguments = argument;
            processInfo.FileName = "msiexec";
            installerProcess.StartInfo = processInfo;

            installerProcess.Start();
            installerProcess.WaitForExit();
            var exitCode = installerProcess.ExitCode;

            this.writeToTextBox($"Return code: {exitCode}");
        }

        public FormSetup()
        {
            InitializeComponent();

            this.uiDispatcher = Dispatcher.CurrentDispatcher;

            var startupPath = Application.StartupPath;
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var programFiles = Environment.GetEnvironmentVariable("ProgramFiles");
            var companyFolder = Path.Combine(programFiles, ApplicationConstants.COMPANY);
            var appFolder = Path.Combine(companyFolder, ApplicationConstants.PRODUCT);

            this.backgroundInstallationSimpleApp.DoWork += (s1, e1) =>
            {
                this.setText("Installing MSIs...");

                var tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                if (!Directory.Exists(tempFolder)) Directory.CreateDirectory(tempFolder);
                this.writeToTextBox($"Working folder: {tempFolder}");

                FilesHelper.Unzip(Path.Combine(startupPath, "SimpleApp-Msi.zip"), tempFolder);
                this.writeToTextBox($"Msis.zip is unzipped to {tempFolder}");

                if (this.checkBoxRuntime.Checked)
                {
                    try
                    {
                        this.installMsi(Path.Combine(tempFolder, "SimpleRuntime.msi"));
                        this.writeToTextBox("Runtime is installed");
                    }
                    catch (Exception ex)
                    {
                        this.writeToTextBox("Runtime failed to install");
                        this.writeToTextBox(ex.ToString());
                    }
                }

                try
                {
                    this.installMsi(Path.Combine(tempFolder, "SimpleApp.msi"));
                    this.writeToTextBox("App is installed");

                    using (var sw = new StreamWriter(@"C:\app\app.txt"))
                    {
                        sw.WriteLine($"This is a test entry for an app");
                    }
                }
                catch (Exception ex)
                {
                    this.writeToTextBox("App failed to install");
                    this.writeToTextBox(ex.ToString());
                }

                if (Directory.Exists(tempFolder))
                {
                    Directory.Delete(tempFolder, recursive: true);
                    this.writeToTextBox($"Deleted {tempFolder}");
                }

                this.setText("Finished...");
                this.setEnabled(true); // enabling all buttons
            };
            
            this.backgroundInstallationConsole.DoWork += (s1, e1) =>
            {
                var exePath = $"{programFiles}\\{ApplicationConstants.COMPANY}\\{ApplicationConstants.PRODUCT}\\{ApplicationConstants.PRODUCT}.exe";

                var destination = FileScenarios.UnzipToProgramFilesFolder(Path.Combine(startupPath, "TheConsoleApp.zip"),
                    new[] { ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT });

                ShortcutHelper.CreateShortcut(Path.Combine(desktop, $"{ApplicationConstants.PRODUCT}.lnk"),
                    Path.Combine(destination, ApplicationConstants.PRODUCT_EXE),
                    destination);
                this.writeToTextBox("Shortcut is created...");

                InstallationScenarios.Install(this.writeToTextBox, ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT, exePath);

                this.setText("Finished...");
                this.setEnabled(true); // enabling all buttons
            };

            this.backgroundUninstallationConsole.DoWork += (s1, e1) =>
            {
                if (Directory.Exists(companyFolder))
                {
                    if (Directory.Exists(appFolder))
                    {
                        if (File.Exists(Path.Combine(appFolder, "Scenarios.dll")))
                            File.Delete(Path.Combine(appFolder, "Scenarios.dll"));
                        if (File.Exists(Path.Combine(appFolder, "Manual.txt")))
                            File.Delete(Path.Combine(appFolder, "Manual.txt"));
                        if (File.Exists(Path.Combine(appFolder, ApplicationConstants.PRODUCT_EXE)))
                            File.Delete(Path.Combine(appFolder, ApplicationConstants.PRODUCT_EXE));

                        if (!Directory.EnumerateFileSystemEntries(appFolder).Any())
                            Directory.Delete(appFolder);
                    }
                    if (!Directory.EnumerateFileSystemEntries(companyFolder).Any())
                        Directory.Delete(companyFolder);
                }
                this.writeToTextBox("Program Files are deleted...");

                var shortcut = $"{ApplicationConstants.PRODUCT}.lnk";
                if (File.Exists(Path.Combine(desktop, shortcut)))
                {
                    File.Delete(Path.Combine(desktop, shortcut));
                    this.writeToTextBox("Shortcut is deleted...");
                }

                InstallationScenarios.Uninstall(this.writeToTextBox, ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);

                this.setText("Finished...");
                this.setEnabled(true); // enabling all buttons
            };

            this.backgroundVerificationConsole.DoWork += (s1, e1) =>
            {
                if (!Directory.Exists(companyFolder))
                    this.writeToTextBox($"{companyFolder} doesnt exist");
                else if (!Directory.Exists(appFolder))
                    this.writeToTextBox($"{appFolder} doesnt exist");
                else if (!File.Exists(Path.Combine(appFolder, "Scenarios.dll")))
                    this.writeToTextBox($"{appFolder} doesnt have Scenarios.dll");
                else if (!File.Exists(Path.Combine(appFolder, "Manual.txt")))
                    this.writeToTextBox($"{appFolder} doesnt have Manual.txt");
                else if (!File.Exists(Path.Combine(appFolder, ApplicationConstants.PRODUCT_EXE)))
                    this.writeToTextBox($"{appFolder} doesnt have {ApplicationConstants.PRODUCT_EXE}");

                if (!File.Exists(Path.Combine(desktop, $"{ApplicationConstants.PRODUCT}.lnk")))
                    this.writeToTextBox($"{desktop} doesnt have {ApplicationConstants.PRODUCT} shortcut");

                InstallationScenarios.Verify(this.writeToTextBox, ApplicationConstants.COMPANY, ApplicationConstants.PRODUCT);

                this.setText("Finished...");
                this.setEnabled(true); // enabling all buttons
            };
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonVerifyConsole_Click(object sender, EventArgs e)
        {
            this.setEnabled(false); // disabling all buttons
            this.textBoxLogs.Text = string.Empty;
            this.Text = "Verifying...";
            this.backgroundVerificationConsole.RunWorkerAsync();
        }

        private void buttonInstallConsole_Click(object sender, EventArgs e)
        {
            this.setEnabled(false); // disabling all buttons
            this.textBoxLogs.Text = string.Empty;
            this.Text = "Installing...";
            this.backgroundInstallationConsole.RunWorkerAsync();
        }

        private void buttonUninstallConsole_Click(object sender, EventArgs e)
        {
            this.setEnabled(false); // disabling all buttons
            this.textBoxLogs.Text = string.Empty;
            this.Text = "Uninstalling...";
            this.backgroundUninstallationConsole.RunWorkerAsync();
        }

        private void buttonInstallSimpleApp_Click(object sender, EventArgs e)
        {
            this.setEnabled(false); // disabling all buttons
            this.textBoxLogs.Text = string.Empty;
            this.Text = "Installing...";
            this.backgroundInstallationSimpleApp.RunWorkerAsync();
        }

        public void HandleArguments(string[] args)
        {
            //setup.exe install theconsole and setup.exe install simpleapp should install them silently and exit

            if (null != args && args.Length > 1
                && !string.IsNullOrEmpty(args[0]) && args[0].ToLower() == "install"
                && !string.IsNullOrEmpty(args[1]))
            {
                var installWhat = args[1].ToLower();
                bool? runtime = null;

                if (installWhat == "simpleapp" && args.Length > 2 && !string.IsNullOrEmpty(args[2]))
                    runtime = true;

                this.writeToTextBox($"Install argument passed; installing {installWhat}");

                if (installWhat == "theconsole")
                {
                    this.backgroundInstallationConsole.RunWorkerCompleted += delegate { this.Close(); };
                    this.buttonInstallConsole_Click(this, EventArgs.Empty);
                }
                else if (installWhat == "simpleapp")
                {
                    if (runtime.HasValue && runtime.Value)
                        this.checkBoxRuntime.Checked = true;

                    this.backgroundInstallationSimpleApp.RunWorkerCompleted += delegate { this.Close(); };
                    this.buttonInstallSimpleApp_Click(this, EventArgs.Empty);
                }
            }
        }
    }
}
