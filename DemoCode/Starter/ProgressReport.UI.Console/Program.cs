using TaskAwait.Library;
using TaskAwait.Shared;

namespace ProgressReport.UI;

class Program
{
    private static PersonReader reader = new();
    private static CancellationTokenSource tokenSource = new();

    static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine("Please make a selection:");
        Console.WriteLine("1) Use Task (with progress reporting)");
        Console.WriteLine("2) Use await (with progress reporting)");
        Console.WriteLine("(any other key to exit)");

        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                Console.WriteLine();
                Task.Run(() => UseTask(tokenSource.Token));
                HandleExit();
                break;
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                Console.WriteLine();
                Task.Run(() => UseAwait(tokenSource.Token));
                HandleExit();
                break;
            default:
                Console.WriteLine();
                Environment.Exit(0);
                break;
        }
    }

    private static void UseTask(CancellationToken cancelToken)
    {
        Console.WriteLine("One Moment Please ('x' to Cancel, 'q' to Quit)");

        var progress = new Progress<int>();
        progress.ProgressChanged += (_, e) => Console.WriteLine($"{e}% Complete");

        Task<List<Person>> personTask =
            reader.GetPeopleAsync(progress, cancelToken);

        personTask.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("\nThere was a problem retrieving data");
                    foreach (var ex in task.Exception!.Flatten().InnerExceptions)
                        Console.WriteLine($"\nERROR\n{ex.GetType()}\n{ex.Message}");
                    Environment.Exit(1);
                }
                if (task.IsCanceled)
                {
                    Console.WriteLine("\nThe operation was canceled");
                    Environment.Exit(0);
                }
                if (task.IsCompletedSuccessfully)
                {
                    List<Person> people = task.Result;
                    foreach (var person in people)
                        Console.WriteLine(person.ToString());
                    Environment.Exit(0);
                }
            });
    }

    private async static Task UseAwait(CancellationToken cancelToken)
    {
        Console.WriteLine("One Moment Please ('x' to Cancel, 'q' to Quit)");

        var progress = new Progress<int>();
        progress.ProgressChanged += (_, e) => Console.WriteLine($"{e}% Complete");

        try
        {
            List<Person> people =
                await reader.GetPeopleAsync(progress, cancelToken);

            foreach (var person in people)
                Console.WriteLine(person);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("\nThe operation was canceled");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nThere was a problem retrieving data");
            Console.WriteLine($"\nERROR\n{ex.GetType()}\n{ex.Message}");
            Environment.Exit(1);
        }
        finally
        {
            Environment.Exit(0);
        }
    }

    private static void HandleExit()
    {
        while (true)
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.X:
                    tokenSource.Cancel();
                    break;
                case ConsoleKey.Q:
                    Console.WriteLine();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Waiting...");
                    break;
            }
    }
}