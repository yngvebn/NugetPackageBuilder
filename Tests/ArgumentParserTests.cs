using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NugetBuilder;

namespace Tests
{
    [TestFixture]
    public class ArgumentParserTests
    {
        [Test]
        public void CanParseArguments()
        {
            string[] args = new []{"-ReleaseOnly", "-TargetAssembly", "SomeAssembly.dll", "-BuildConfiguration", "Release"};
            NugetBuilderArguments arguments = ArgumentParser<NugetBuilderArguments>.Parse(args);
            Assert.That(arguments.ReleaseOnly, Is.True);
            Assert.That(arguments.TargetAssembly, Is.EqualTo("SomeAssembly.dll"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void NoArgumentsThrowsException()
        {
            NugetBuilderArguments arguments = ArgumentParser<NugetBuilderArguments>.Parse(new string[]{});

        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RequiredArgumentMissingThrowsException()
        {
            string[] args = new []{ "-TargetAssembly", "-ReleaseOnly" };
            NugetBuilderArguments arguments = ArgumentParser<NugetBuilderArguments>.Parse(args);
        }
        [Test]
        public void ArgumentsGetDefaultValue()
        {
            string[] args = new[] { "-TargetAssembly", "SomeAssembly.dll" };
            NugetBuilderArguments arguments = ArgumentParser<NugetBuilderArguments>.Parse(args);
            Assert.That(arguments.ReleaseOnly, Is.True);
        }

    }
}
