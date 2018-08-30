using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Infrastructure
{
    public class Bootstrapper {
        private static void StartSchedule() {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            scheduler.Start();
        }

        public static void Initialize()
        {

        }
    }
}