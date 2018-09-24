using System;
using System.Linq;
using System.Web;

namespace Component.UI.MVC.Framework
{
    public static class CookieHelper
    {
        private static string _cookieName = "Component";
        private static string _langName = "Lang";

        public static void DeleteUserInfoCookie(HttpResponse response)
        {
            HttpCookie _cookie = new HttpCookie(_cookieName);
            _cookie.Values.Add("Expires", DateTime.Now.AddYears(-1).ToString());
            _cookie.Expires = DateTime.Now.AddYears(-1);//定义有效期为1年,不起作用,不保存Expires值
            response.Cookies.Add(_cookie);
        }


        public static HttpCookie UpdateLanguageToCookie(string lang, HttpCookieCollection cks)
        {
            var cookies = cks[_cookieName];
            if (cookies != null)
            {
                cookies.Values.Set("Lang", lang);
                return cookies;
            }
            return new HttpCookie(_cookieName); //返回空的 cookie
        }

        public static string GetLanguageFromCookie(HttpCookieCollection cks)
        {
            var cookies = cks[_cookieName];
            if (cookies != null && cookies[_langName] != null)
            {
                return cookies[_langName];
            }
            return string.Empty;
        }

        /// <summary>
        /// 检查Cookie是否存在
        /// </summary>
        /// <param name="cks"></param>
        /// <returns></returns>
        public static bool CheckCookieExist(HttpCookieCollection cks)
        {
            var cookies = cks[_cookieName];
            if (cookies != null)
            {//有cookie,并且没有过期
                if (cookies.Values.AllKeys.Contains("Expires"))
                {
                    if (DateTime.Parse(cookies["Expires"]) >= DateTime.Now)
                    {//未过期
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}