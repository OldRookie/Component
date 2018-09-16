using Autofac;
using Component.Infrastructure.DependencyManagement;
using Component.Infrastructure.Startup;
using Component.Infrastructure.SysService;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Infrastructure
{
    public class Bootstrapper
    {
        private static void StartSchedule()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            scheduler.Start().ContinueWith(t =>
            {
                scheduler.TriggerJob(new JobKey("SimpleJob", "SimpleJob"));
            }); ;
        }

        public static void Initialize()
        {
            StartSchedule();
        }

        protected static void ExecuteStartup()
        {
            var typeFinder = new WebAppTypeFinder();

            var types = typeFinder.FindClassesOfType<IStartupTask>();
            var instances = new List<IStartupTask>();
            foreach (var mcType in types)
                instances.Add((IStartupTask)Activator.CreateInstance(mcType));

            foreach (var instance in instances)
            {
                Task.Factory.StartNew(() =>
                {
                    instance.Execute();
                });
            }
        }
    }
}