using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Component.Infrastructure.AttributeExt
{
    public static class DescriptionExtension
    {
        /// <summary>
        /// 获取枚举描述特性值
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumerationValue">枚举值</param>
        /// <returns>枚举值的描述/returns>
        public static string GetDescription(this Enum enumerationValue)
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue必须是一个枚举值", "enumerationValue");
            }

            //使用反射获取该枚举的成员信息
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //返回枚举值得描述信息
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //如果没有描述特性的值，返回该枚举值得字符串形式
            return enumerationValue.ToString();
        }


        public static Dictionary<string, string> GetEntityPropertyDict(this Type type)
        {
            var propertyInfos = new Dictionary<string, string>();
            foreach (PropertyInfo item in type.GetProperties())
            {
                var attr = item.GetCustomAttribute<DescriptionAttribute>();

                if (attr != null)
                {
                    var desc = item.GetCustomAttribute<DescriptionAttribute>().Description;
                    propertyInfos.Add(item.Name, desc);
                }
                else
                {
                    var displayAttribute = item.GetCustomAttribute<DisplayAttribute>();
                    if (displayAttribute != null)
                    {
                        propertyInfos.Add(item.Name, displayAttribute.GetName());
                    }
                }
            }
            return propertyInfos;
        }
    }
}
