using System;
using System.Collections.Generic;

namespace NugetBuilder
{
    public static class ArgumentParser<T> where T: class, new()
    {
        private static List<string> _arguments;
        public static T Parse(string[] args)
        {
            T nb = new T();
            _arguments = new List<string>(args);
            var returnType = typeof (T);
            foreach(var property in returnType.GetProperties())
            {
                var attribute = property.GetAttribute<ArgumentAttribute>();
                if (attribute == null) continue;
                var argumentIndex = _arguments.IndexOf("-"+attribute.Name);
                if(argumentIndex == -1 && attribute.Required)
                {
                    throw new ArgumentException(string.Format("Required argument missing - {0}", attribute.Name));
                    
                }
                else if(argumentIndex == -1 && !attribute.Required && String.IsNullOrEmpty(attribute.DefaultValue))
                {
                    continue;
                }
                if(attribute.HasValue)
                {
                    var argumentValue = GetArgumentAt(argumentIndex + 1);
                    if(attribute.Required && argumentValue == null)
                    {
                        throw new ArgumentException(string.Format("Required argument missing - {0}", attribute.Name));
                    }
                        
                    property.SetValue(nb, argumentValue ?? attribute.DefaultValue, null);
                }
                else
                {
                    if(attribute.Required)
                    {
                        throw new ArgumentException(string.Format("Required argument missing - {0}", attribute.Name));
                    }
                    property.SetValue(nb, Convert.ChangeType(attribute.DefaultValue, property.PropertyType), null);
                }
            }
            return nb;
        }

        private static object GetArgumentAt(int i)
        {
            if (i >= _arguments.Count) return null;
            if (_arguments[i].StartsWith("-")) return null;
            return _arguments[i];
        }
    }
}