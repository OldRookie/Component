using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Component.Infrastructure.Utility
{
    public static class JsonHelper
    {
        #region JSON Convert and Object Mapping

        /// <summary>
        /// Object to Json String
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String JsonSerialize(this object obj, bool isExternalFormat = false)
        {
            if (obj == null)
            {
                //throw new JsonConvertException("Null Object can not serialize to Json");
            }
            JsonSerializerSettings js = new JsonSerializerSettings();
            if (isExternalFormat)
            {
                js = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    //StringEscapeHandling = StringEscapeHandling.EscapeHtml,// handle html
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,

                };
                return JsonConvert.SerializeObject(obj, Formatting.Indented, js);
            }
            else
            {
                js.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                return JsonConvert.SerializeObject(obj, Formatting.None, js);
            }

            //js.NullValueHandling = NullValueHandling.Ignore;
        }


        /// <summary>
        /// Json String to Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(this string json, bool isExternalFormat = false)
        {
            if (string.IsNullOrEmpty(json))
            {
                //throw new JsonConvertException("Null Or empty String can not Deserialize to Model Object");
            }

            if (isExternalFormat)
            {
                var js = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    //StringEscapeHandling = StringEscapeHandling.EscapeHtml,// handle html
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,

                };
                return JsonConvert.DeserializeObject<T>(json, js);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(json);
            }

        }

        public static bool TryJsonDeserialize<T>(this string json, out T jsonData) where T : new()
        {
            jsonData = default(T);
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            try
            {
                jsonData = JsonConvert.DeserializeObject<T>(json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obj Map To new Object Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T MapTo<T>(this object obj)
        {
            if (obj == null) return default(T);
            String sourceJson = obj.JsonSerialize();
            T targetModel = sourceJson.JsonDeserialize<T>();
            return targetModel;
        }
        #endregion
    }
}