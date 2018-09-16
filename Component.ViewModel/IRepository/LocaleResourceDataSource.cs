using Component.Infrastructure.DependencyManagement;
using Component.Model.Entity;
using Component.Model.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Infrastructure.BaseDataModel.Service
{
    public class LocaleResourceDataSource
    {
        static object syncObj = new object();
        static LocaleResourceDataSource _instance;
        LocaleResourceDataSource()
        {

        }
        public static LocaleResourceDataSource SingleTon
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new LocaleResourceDataSource();
                        }
                    }
                }
                return _instance;
            }
        }
    }

    public static class LocaleResourceExtension
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="code"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string ResourceValue(this string moduleName, string code, string language = "zh-cn")
        {
            var localeResourceRepository = ComponentContext.Current.Resolve<IBaseRepository<LocaleResource>>();
            var localeResource = localeResourceRepository.FirstOrDefault(x => x.ResourceModule == moduleName && code == x.ResourceCode && language == x.Language);
            if (localeResource != null)
            {
                return localeResource.ResourceValue;
            }
            return null;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        /// <param name="fullModuleCode"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string ResourceValue(this string fullModuleCode, string language = "zh-cn")
        {
            var localeResourceRepository = ComponentContext.Current.Resolve<IBaseRepository<LocaleResource>>();
            var localeResource = localeResourceRepository.FirstOrDefault(x => (x.ResourceModule + "." + x.ResourceCode) == fullModuleCode
                    && language == x.Language);
            if (localeResource != null)
            {
                return localeResource.ResourceValue;
            }
            return null;
        }

        public static string FormatMsg(this string msgTemplate, params object[] args)
        {
            if (!string.IsNullOrEmpty(msgTemplate))
            {
                return string.Format(msgTemplate, args);
            }
            else
            {
                return string.Empty;
            }

        }
    }
}