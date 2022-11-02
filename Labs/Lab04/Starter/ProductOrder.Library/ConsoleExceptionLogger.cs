namespace ProductOrder.Library;

public class ConsoleExceptionLogger : IExceptionLogger
{
    public void LogException(Exception exception)
    {
        Console.WriteLine($"---");
        Console.WriteLine($"Exception Type:");
        Console.WriteLine($"    {exception.GetType()}");
        Console.WriteLine($"Exception Message:");
        Console.WriteLine($"    {exception.Message}");
        Console.WriteLine($"Inner Exception:");
        if (exception.InnerException == null)
        {
            Console.WriteLine($"    NONE");
        }
        else
        {
            Console.WriteLine($"    {exception.InnerException.GetType()}");
            Console.WriteLine($"    {exception.InnerException.Message}");
        }

        var aggEx = exception as AggregateException;
        if (aggEx != null)
        {
            Console.WriteLine($"Inner Exceptions Count: {aggEx.InnerExceptions.Count}");
            int current = 0;
            foreach (var ex in aggEx.Flatten().InnerExceptions)
            {
                Console.WriteLine($"\nInnerExceptions[{current++}]:");
                Console.WriteLine($"    {ex.GetType()}");
                Console.WriteLine($"    {ex.Message}");
            }
        }

        Console.WriteLine($"---");
    }
}
