using Common.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Component.Infrastructure.Tools.Job.WindowService
{
    partial class ScheduleService : ServiceBase
    {
        private readonly ILog logger;
        private IScheduler scheduler;

        public ScheduleService()
        {
            InitializeComponent();
            logger = LogManager.GetLogger(GetType());
        }

        protected override void OnStart(string[] args)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            scheduler.Start().GetAwaiter().GetResult();
            var getJobGroupNames = scheduler.GetJobGroupNames().GetAwaiter().GetResult();

            logger.Info(getJobGroupNames);
            logger.Info("Quartz服务成功启动");
        }

        protected override void OnStop()
        {
            scheduler.Shutdown().GetAwaiter().GetResult();
            logger.Info("Quartz服务成功终止");
        }

        protected override void OnPause()
        {
            scheduler.PauseAll().GetAwaiter().GetResult();
        }

        protected override void OnContinue()
        {
            scheduler.ResumeAll().GetAwaiter().GetResult();
        }
    }
}
