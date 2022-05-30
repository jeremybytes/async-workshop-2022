namespace DataProcessor.Library;

public interface ILogger
{
    Task LogMessage(string message, string data);
}