namespace DataProcessor.Library.Tests;

public class FakeLogger : ILogger
{
    public Task LogMessage(string message, string data)
    {
        return Task.CompletedTask;
    }
}
