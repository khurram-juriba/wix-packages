namespace SetupApp
{
    partial class FormSetup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxLogs = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonVerifyConsole = new System.Windows.Forms.Button();
            this.buttonUninstallConsole = new System.Windows.Forms.Button();
            this.buttonInstallConsole = new System.Windows.Forms.Button();
            this.buttonInstallSimpleApp = new System.Windows.Forms.Button();
            this.checkBoxRuntime = new System.Windows.Forms.CheckBox();
            this.backgroundInstallationConsole = new System.ComponentModel.BackgroundWorker();
            this.backgroundVerificationConsole = new System.ComponentModel.BackgroundWorker();
            this.backgroundUninstallationConsole = new System.ComponentModel.BackgroundWorker();
            this.backgroundInstallationSimpleApp = new System.ComponentModel.BackgroundWorker();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxLogs
            // 
            this.textBoxLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogs.Location = new System.Drawing.Point(0, 0);
            this.textBoxLogs.Multiline = true;
            this.textBoxLogs.Name = "textBoxLogs";
            this.textBoxLogs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLogs.Size = new System.Drawing.Size(800, 450);
            this.textBoxLogs.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.buttonClose);
            this.flowLayoutPanel1.Controls.Add(this.buttonVerifyConsole);
            this.flowLayoutPanel1.Controls.Add(this.buttonUninstallConsole);
            this.flowLayoutPanel1.Controls.Add(this.buttonInstallConsole);
            this.flowLayoutPanel1.Controls.Add(this.buttonInstallSimpleApp);
            this.flowLayoutPanel1.Controls.Add(this.checkBoxRuntime);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 397);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 53);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // buttonClose
            // 
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonClose.Location = new System.Drawing.Point(722, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 24);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Finish";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonVerifyConsole
            // 
            this.buttonVerifyConsole.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonVerifyConsole.Location = new System.Drawing.Point(604, 3);
            this.buttonVerifyConsole.Name = "buttonVerifyConsole";
            this.buttonVerifyConsole.Size = new System.Drawing.Size(112, 24);
            this.buttonVerifyConsole.TabIndex = 1;
            this.buttonVerifyConsole.Text = "Verify Console";
            this.buttonVerifyConsole.UseVisualStyleBackColor = true;
            this.buttonVerifyConsole.Click += new System.EventHandler(this.buttonVerifyConsole_Click);
            // 
            // buttonUninstallConsole
            // 
            this.buttonUninstallConsole.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonUninstallConsole.Location = new System.Drawing.Point(486, 3);
            this.buttonUninstallConsole.Name = "buttonUninstallConsole";
            this.buttonUninstallConsole.Size = new System.Drawing.Size(112, 24);
            this.buttonUninstallConsole.TabIndex = 3;
            this.buttonUninstallConsole.Text = "Uninstall Console";
            this.buttonUninstallConsole.UseVisualStyleBackColor = true;
            this.buttonUninstallConsole.Click += new System.EventHandler(this.buttonUninstallConsole_Click);
            // 
            // buttonInstallConsole
            // 
            this.buttonInstallConsole.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonInstallConsole.Location = new System.Drawing.Point(368, 3);
            this.buttonInstallConsole.Name = "buttonInstallConsole";
            this.buttonInstallConsole.Size = new System.Drawing.Size(112, 24);
            this.buttonInstallConsole.TabIndex = 2;
            this.buttonInstallConsole.Text = "Install Console";
            this.buttonInstallConsole.UseVisualStyleBackColor = true;
            this.buttonInstallConsole.Click += new System.EventHandler(this.buttonInstallConsole_Click);
            // 
            // buttonInstallSimpleApp
            // 
            this.buttonInstallSimpleApp.Location = new System.Drawing.Point(287, 3);
            this.buttonInstallSimpleApp.Name = "buttonInstallSimpleApp";
            this.buttonInstallSimpleApp.Size = new System.Drawing.Size(75, 23);
            this.buttonInstallSimpleApp.TabIndex = 4;
            this.buttonInstallSimpleApp.Text = "Install App";
            this.buttonInstallSimpleApp.UseVisualStyleBackColor = true;
            this.buttonInstallSimpleApp.Click += new System.EventHandler(this.buttonInstallSimpleApp_Click);
            // 
            // checkBoxRuntime
            // 
            this.checkBoxRuntime.AutoSize = true;
            this.checkBoxRuntime.Location = new System.Drawing.Point(186, 3);
            this.checkBoxRuntime.Name = "checkBoxRuntime";
            this.checkBoxRuntime.Size = new System.Drawing.Size(95, 17);
            this.checkBoxRuntime.TabIndex = 5;
            this.checkBoxRuntime.Text = "Install Runtime";
            this.checkBoxRuntime.UseVisualStyleBackColor = true;
            // 
            // FormSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.textBoxLogs);
            this.Name = "FormSetup";
            this.Text = "Setting up...";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundInstallationConsole;
        private System.ComponentModel.BackgroundWorker backgroundUninstallationConsole;
        private System.ComponentModel.BackgroundWorker backgroundVerificationConsole;
        private System.ComponentModel.BackgroundWorker backgroundInstallationSimpleApp;

        private System.Windows.Forms.TextBox textBoxLogs;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonVerifyConsole;
        private System.Windows.Forms.Button buttonInstallConsole;
        private System.Windows.Forms.Button buttonUninstallConsole;
        private System.Windows.Forms.Button buttonInstallSimpleApp;
        private System.Windows.Forms.CheckBox checkBoxRuntime;
    }
}

