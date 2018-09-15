using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Component.Infrastructure.Resources
{
    public static class ResourcesExtension
    {
        public static string ResourceText<TModel>(this HtmlHelper<TModel> helper, string resourceKey)
        {
            var text = string.Empty;
            var baseName = "Resources." + resourceKey.Split('.')[0];
            var resourceName = resourceKey.Split('.')[1];
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            var resource = new ResourceManager(baseName, global::System.Reflection.Assembly.Load("App_GlobalResources"));
            text = resource.GetString(resourceName);
            return text;
        }

        public static string ResourceText(this string resourceKey)
        {
            var text = string.Empty;
            var baseName = "Resources." + resourceKey.Split('.')[0];
            var resourceName = resourceKey.Split('.')[1];
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            var resource = new ResourceManager(baseName, global::System.Reflection.Assembly.Load("App_GlobalResources"));
            text = resource.GetString(resourceName);
            return text;
        }
    }
}
