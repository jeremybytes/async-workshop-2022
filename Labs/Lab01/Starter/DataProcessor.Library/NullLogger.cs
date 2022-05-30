namespace DataProcessor.Library;

public class NullLogger : ILogger
{
    public void LogMessage(string message, string data)
    {
        // does nothing
    }
}
