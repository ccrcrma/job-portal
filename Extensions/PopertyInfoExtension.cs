using System;
using System.Reflection;

namespace job_portal.Extensions
{
    public static class PopertyInfoExtension
    {
        public static TPropertyType ConvertToChildObject<TInstanceType, TPropertyType>(this PropertyInfo propertyInfo,
            TInstanceType instance) where TInstanceType : class, new()
        {
            if (instance == null)
                instance = Activator.CreateInstance<TInstanceType>();
            return (TPropertyType)propertyInfo.GetValue(instance);

        }
    }
}