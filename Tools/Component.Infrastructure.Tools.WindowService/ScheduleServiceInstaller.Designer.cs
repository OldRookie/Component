namespace Component.Infrastructure.Tools.Job.WindowService
{
    partial class ScheduleServiceInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.scheduleServiceInstallerProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.jobServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // scheduleServiceInstallerProcessInstaller
            // 
            this.scheduleServiceInstallerProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.scheduleServiceInstallerProcessInstaller.Password = null;
            this.scheduleServiceInstallerProcessInstaller.Username = null;
            // 
            // jobServiceInstaller
            // 
            this.jobServiceInstaller.DisplayName = "CTFScheduleService";
            this.jobServiceInstaller.ServiceName = "ScheduleService";
            // 
            // ScheduleServiceInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.scheduleServiceInstallerProcessInstaller,
            this.jobServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller scheduleServiceInstallerProcessInstaller;
        private System.ServiceProcess.ServiceInstaller jobServiceInstaller;
    }
}
