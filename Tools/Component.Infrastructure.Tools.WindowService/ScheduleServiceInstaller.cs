using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Component.Infrastructure.Tools.Job.WindowService
{
    [RunInstaller(true)]
    public partial class ScheduleServiceInstaller : System.Configuration.Install.Installer
    {
        public ScheduleServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
