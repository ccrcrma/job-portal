using System;
using System.Dynamic;
using job_portal.Areas.Identity.Models;

namespace job_portal.Util
{
    public class ProxyHelper
    {
        public static object UnProxy<T>(T proxyObject) where T : class
        {
            var type = proxyObject.GetType();
            if (type.Namespace.StartsWith("Castle.Proxies"))
            {
                var baseType = type.BaseType;
                var profile = new Profile();
                foreach (var property in baseType.GetProperties())
                {
                    try
                    {
                        var value = (string)property.GetValue(proxyObject);
                        if (property.CanWrite)
                        {
                            property.SetValue(profile, value);

                        }
                    }
                    catch (InvalidCastException ex)
                    {
                        var value = (Guid)property.GetValue(proxyObject);
                        if (property.CanWrite)
                        {
                            property.SetValue(profile, value);

                        }
                    }

                }
                return profile;
            }
            return proxyObject;
        }
    }
}