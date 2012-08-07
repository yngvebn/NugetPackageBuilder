using System;

namespace NugetBuilder
{
    public class NugetTimeOutException : Exception
    {
        public NugetTimeOutException(string timeoutWhileWaitingForNugetToFinish):base(timeoutWhileWaitingForNugetToFinish)
        {
        }
    }

    public class DependentArgumentMissing: ArgumentException
    {
        public DependentArgumentMissing(string message, string paramName =""):base(message, paramName)
        {
            
        }
    }
}