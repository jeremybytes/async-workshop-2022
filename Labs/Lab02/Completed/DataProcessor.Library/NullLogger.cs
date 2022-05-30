namespace DataProcessor.Library;

public class NullLogger : ILogger
{
    public Task LogMessage(string message, string data)
    {
        // does nothing
        return Task.CompletedTask;
    }
}
