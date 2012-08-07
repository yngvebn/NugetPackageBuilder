namespace NugetBuilder
{
    public class NugetBuilderArguments
    {
        [Argument(HasValue = false, Name = "ReleaseOnly", Required = false, DefaultValue = "true", DependentOn="BuildConfiguration")]
        public bool ReleaseOnly { get; set; }

        [Argument(Name = "TargetAssembly", Required = true)]
        public string TargetAssembly { get; set; }

        [Argument(Name = "NugetDestination", Required = false)]
        public string NugetDestination { get; set; }

        [Argument(HasValue = true, Name="BuildConfiguration", Required=false)]
        public string BuildConfiguration { get; set; }
    }
}