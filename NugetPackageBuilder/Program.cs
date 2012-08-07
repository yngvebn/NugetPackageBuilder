using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;

namespace NugetBuilder
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                NugetBuilderArguments ap = ArgumentParser<NugetBuilderArguments>.Parse(args);
                if(ap.ReleaseOnly && String.IsNullOrEmpty(ap.BuildConfiguration))
                {
                    throw new DependentArgumentMissing("-BuildConfiguration must be supplied when using -ReleaseOnly");
                }
                Console.WriteLine("Building NuGet Package");
                Build(ap);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Environment.Exit(0);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Environment.Exit(1);
            }
            
        }

        public static void Build(NugetBuilderArguments args)
        {

            //if (!configuration.Equals("release", StringComparison.InvariantCultureIgnoreCase)) return;
            string targetDir = Path.GetDirectoryName(args.TargetAssembly);
            string nugetDir = Path.Combine(targetDir, "NuGet");
            Assembly asm = Assembly.LoadFile(args.TargetAssembly);
            if (!Directory.Exists(nugetDir)) Directory.CreateDirectory(nugetDir);
            AssemblyProductAttribute asmProduct = asm.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0] as AssemblyProductAttribute;
            AssemblyFileVersionAttribute asmVersion = asm.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0] as AssemblyFileVersionAttribute;
            string nugetPackageName = string.Format("{0}.{1}", asmProduct.Product, asmVersion.Version);
            string versionDir = Path.Combine(nugetDir,nugetPackageName);
            if (!Directory.Exists(versionDir)) Directory.CreateDirectory(versionDir);
            string libDir = Path.Combine(versionDir, "lib");
            if (!Directory.Exists(libDir)) Directory.CreateDirectory(libDir);
            var nugetLibAssembly = Path.Combine(libDir, Path.GetFileName(args.TargetAssembly));
            if (File.Exists(nugetLibAssembly))
                File.Delete(nugetLibAssembly);
            File.Copy(args.TargetAssembly, nugetLibAssembly);
            XmlSerializer xs = new XmlSerializer(typeof(Package));
            var nuspecFile = Path.Combine(versionDir, string.Format("{0}.nuspec", asmProduct.Product));
            using (StreamWriter sw = new StreamWriter(nuspecFile))
            {
                xs.Serialize(sw, new Package(asm));
            }
            var nuget = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), "nuget.exe");
            string arguments = string.Format("pack \"{0}\" -BasePath \"{1}\" -OutputDirectory \"{2}\"", nuspecFile, versionDir, versionDir);
            
            Process process = new Process();
            elapsedTime = 0;
            eventHandled = false;
            process.StartInfo = new ProcessStartInfo(nuget, arguments);
            process.EnableRaisingEvents = true;
            process.StartInfo.WorkingDirectory = versionDir;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Exited += new EventHandler(process_Exited);
            process.Start();
            //System.Threading.Thread.Sleep(1000);
            //File.Delete(nuspecFile);

            // Wait for Exited event, but not more than 30 seconds.
            const int SLEEP_AMOUNT = 100;
            while (!eventHandled)
            {
                elapsedTime += SLEEP_AMOUNT;
                if (elapsedTime > 30000)
                {
                    throw new NugetTimeOutException("Timeout while waiting for Nuget to finish!");
                    break;
                }
                Thread.Sleep(SLEEP_AMOUNT);
            }
            string finalNugetDirectory = versionDir;
            File.Delete(nuspecFile);
            if(!String.IsNullOrEmpty(args.NugetDestination))
            {
                if (Directory.Exists(Path.Combine(args.NugetDestination, nugetPackageName)))
                {
                    throw new NugetPackageExistsException("Package "+nugetPackageName+" already exists in repository. Did you forget to bump assembly version?");
                }
                
                Directory.Move(versionDir, Path.Combine(args.NugetDestination, nugetPackageName));
                finalNugetDirectory = Path.Combine(args.NugetDestination,nugetPackageName);
            }
            Console.WriteLine("NuGet package created: {0}", finalNugetDirectory);
        }
        private static int elapsedTime;
        private static bool eventHandled;
        static void process_Exited(object sender, EventArgs e)
        {
            eventHandled = true;
            
        }




    }

    public class NugetPackageExistsException : Exception
    {
        public NugetPackageExistsException(string message):base(message)
        {
        }
    }
}