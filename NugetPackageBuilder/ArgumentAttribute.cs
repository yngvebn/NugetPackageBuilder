using System;

namespace NugetBuilder
{
    public class ArgumentAttribute : Attribute
    {
        public string DefaultValue { get; set; }

        public bool Required { get; set; }

        public string Name { get; set; }

        public bool HasValue { get; set; }

        public string DependentOn { get; set; }

        public ArgumentAttribute()
        {
            HasValue = true;
        }
    }
}