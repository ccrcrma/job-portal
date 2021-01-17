using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace job_portal.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((DisplayAttribute)_Attribs.ElementAt(0)).Name;
                }
            }
            return GenericEnum.ToString();
        }
    }
}