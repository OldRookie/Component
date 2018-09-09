using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Component.UI.MVC.Framework
{
    public static class ExtensionsHelper
    {
        public static string LocalResources(this WebViewPage page, string key)
        {//
         // 使用：@this.LocalResources("IntroductionText")
         //  资源文件要放在 View/controler/App_LocalResources 下面
            string str = "";
            try
            {
                str = (string)page.ViewContext.HttpContext.GetLocalResourceObject(page.VirtualPath, key);
                return str;
            }
            catch (Exception ex)
            {
                return str;
            }
        }
        public static string GlobalResources(this WebViewPage page, string resourceName, string keyName)
        {
            try
            {
                string str = (string)page.ViewContext.HttpContext.GetGlobalResourceObject("resourceName", "keyName");
                return str;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }

    /// <summary>
    /// 自定义 HtmlHelper 辅助方法
    /// </summary>
    public static class ExHtmlHelper {
        /// <summary>
        /// 将换行符(\n)换成<br />符 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static MvcHtmlString ConverEntryStringToHtmlString(this HtmlHelper helper,
            string strData)
        {
            string htmlString = strData.Replace(" ", "&nbsp").Replace("\n", "<br />");//换行符,变成<br />
            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString Conver64StringToHtmlString(this HtmlHelper helper,
            string stringdata)
        {
            byte[] data = Convert.FromBase64String(stringdata);
            string msg = System.Text.Encoding.Default.GetString(data);
            return MvcHtmlString.Create(msg.Replace(" ", "&nbsp").Replace("\n", "<br />"));//换行符,变成<br />
        }
    }
}