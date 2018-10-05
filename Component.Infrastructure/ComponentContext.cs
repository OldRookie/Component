using Autofac;
using Component.Infrastructure.DependencyManagement;
using Component.Infrastructure.Startup;
using Component.Infrastructure.SysService;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Infrastructure
{

    public class ComponentContext
    {
        static ComponentContext _instance;

        private static readonly object _syncObj = new object();
        ComponentContext(){

        }

        public static ComponentContext Current
        {
            get
            {
                if (_instance == null)
                {

                    lock (_syncObj)
                    {
                        if (_instance == null) {
                            _instance = new ComponentContext();
                        }
                    }
                }
                return _instance;
            }
        }

        #region Fields

        private ContainerManager _containerManager;

        #endregion

        #region Utilities
        private  void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            //type finder
            var typeFinder = new WebAppTypeFinder();
            //var typeFinder = new AppDomainTypeFinder();
            builder.Register(c => typeFinder);

            //find IDependencyRegistar implementations
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            foreach (var t in drTypes)
            {
                dynamic dependencyRegistar = Activator.CreateInstance(t);
                dependencyRegistar.Register(builder, typeFinder);
            }

            //event
            //OnContainerBuilding(new ContainerBuilderEventArgs(builder));
            var container = builder.Build();
            _containerManager = new ContainerManager(container);
        }

        private void StartSchedule()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            scheduler.Start().ContinueWith(t =>
            {
                scheduler.TriggerJob(new JobKey("SimpleJob", "SimpleJob"));
            }); ;
        }

        private void ExecuteStartup()
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

        public void Initialize()
        {
            StartSchedule();

            RegisterDependencies();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        /// <summary>
        ///  Resolve dependency
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Container manager
        /// </summary>
        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion
    }

}
