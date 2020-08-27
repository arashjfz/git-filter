using System;

namespace GitFilter
{
    internal class ConsoleLogger : ILogger
    {
        #region Implementation of ILogger

        public void Log(string value)
        {
            Console.WriteLine(value);
        }

        public void LogError(string error)
        {
            Console.Error.WriteLine(error);
        }

        #endregion
    }
}