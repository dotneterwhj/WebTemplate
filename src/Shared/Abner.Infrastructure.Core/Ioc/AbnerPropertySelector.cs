using System.Reflection;
using Autofac.Core;

namespace Abner.Infrastructure.Core
{
    public class AbnerPropertySelector : IPropertySelector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool InjectProperty(PropertyInfo propertyInfo, object instance)
        {
            return propertyInfo.CustomAttributes.Any(p => p.AttributeType == typeof(PropertyInjectionAttribute));
            // var propertyInjectionAttribute = propertyInfo.GetCustomAttribute<PropertyInjectionAttribute>();
        }
    }
}