namespace GitFilter
{
    public interface ILogger
    {
        void Log(string value);
        void LogError(string error);
    }
}