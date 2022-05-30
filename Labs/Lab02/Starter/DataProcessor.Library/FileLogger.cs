using System.Configuration;

namespace DataProcessor.Library;

public class FileLogger : ILogger
{
    private string logPath;

    public FileLogger()
    {
        var logFile = ConfigurationManager.AppSettings["LogLocation"];
        logPath = AppDomain.CurrentDomain.BaseDirectory + logFile;

        using (var writer = new StreamWriter(logPath, true))
        {
            writer.WriteLine("==================");
        }
    }

    public async Task LogMessage(string message, string data)
    {
        using var writer = new StreamWriter(logPath, true);
        await writer.WriteLineAsync(
            $"{DateTime.Now:s}: {message} - {data}")
            .ConfigureAwait(false);
    }
}
