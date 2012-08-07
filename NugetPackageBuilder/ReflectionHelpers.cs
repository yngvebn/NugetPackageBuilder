using System.Linq;
using System.Reflection;

namespace NugetBuilder
{
    public static class ReflectionHelpers
    {
        public static T GetAttribute<T>(this PropertyInfo p) where T : class
        {
            var attribute = p.GetCustomAttributes(typeof(T), true).FirstOrDefault();
            if (attribute == null) return null;
            return attribute as T;
        }
    }
}