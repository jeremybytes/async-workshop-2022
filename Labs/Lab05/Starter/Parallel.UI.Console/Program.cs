using System.Threading.Channels;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace ParallelUI;

class Program
{
    private static PersonReader reader = new();
    private static CancellationTokenSource tokenSource = new();

    static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine("Please make a selection:");
        Console.WriteLine("1) Use await (non-parallel)");
        Console.WriteLine("2) Use Task (parallel)");
        Console.WriteLine("3) Use Channels (parallel)");
        Console.WriteLine("4) Use ForEachAsync (parallel)");
        Console.WriteLine("(any other key to exit)");

        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                Console.WriteLine();
                Task.Run(() => UseAwait(tokenSource.Token));
                HandleExit();
                break;
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                Console.WriteLine();
                Task.Run(() => UseTask(tokenSource.Token));
                HandleExit();
                break;
            case ConsoleKey.D3:
            case ConsoleKey.NumPad3:
                Console.WriteLine();
                Task.Run(() => UseChannel(tokenSource.Token));
                HandleExit();
                break;
            case ConsoleKey.D4:
            case ConsoleKey.NumPad4:
                Console.WriteLine();
                Task.Run(() => UseForEachAsync(tokenSource.Token));
                HandleExit();
                break;
            default:
                Console.WriteLine();
                Environment.Exit(0);
                break;
        }
    }

    // OPTION 1: Await (runs sequentially)
    private async static Task UseAwait(CancellationToken cancelToken)
    {
        Console.WriteLine("One Moment Please ('x' to Cancel, 'q' to Quit)");
        DateTime start = DateTime.Now;

        try
        {
            List<int> ids = await reader.GetIdsAsync().ConfigureAwait(false);

            foreach (int id in ids)
            {
                Console.WriteLine(await reader.GetPersonAsync(id, cancelToken).ConfigureAwait(false));
            }
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
            Console.WriteLine($"Total Time: {DateTime.Now - start}");
            Environment.Exit(0);
        }
    }

    // OPTION 2: Task w/ Continuation (runs parallel)
    private static async Task UseTask(CancellationToken cancelToken)
    {
        Console.WriteLine("One Moment Please ('x' to Cancel, 'q' to Quit)");
        DateTime start = DateTime.Now;

        try
        {
            List<int> ids = await reader.GetIdsAsync().ConfigureAwait(false);

            var taskList = new List<Task>();

            foreach (int id in ids)
            {
                Task<Person> personTask = reader.GetPersonAsync(id, cancelToken);

                Task continuation = personTask.ContinueWith(task =>
                {
                    Person person = task.Result;
                    Console.WriteLine(person.ToString());
                },
                TaskContinuationOptions.OnlyOnRanToCompletion);

                taskList.Add(continuation);
            }

            await Task.WhenAll(taskList).ConfigureAwait(false);
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
            Console.WriteLine($"Total Time: {DateTime.Now - start}");
            Environment.Exit(0);
        }
    }

    // OPTION 3: Task w/ Channel (runs parallel)
    private static async Task UseChannel(CancellationToken cancelToken)
    {
        Console.WriteLine("One Moment Please ('x' to Cancel, 'q' to Quit)");
        DateTime start = DateTime.Now;

        try
        {
            List<int> ids = await reader.GetIdsAsync().ConfigureAwait(false);

            var channel = Channel.CreateUnbounded<Person>();
            Task consumer = ShowData(channel.Reader);
            Task producer = ProduceData(ids, channel.Writer, cancelToken);
            await producer;
            await consumer;
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
            Console.WriteLine($"Total Time: {DateTime.Now - start}");
            Environment.Exit(0);
        }
    }

    private static async Task ShowData(ChannelReader<Person> channelReader)
    {
        await foreach (var person in channelReader.ReadAllAsync())
        {
            Console.WriteLine(person.ToString());
        }
    }

    private static async Task ProduceData(List<int> ids, ChannelWriter<Person> channelWriter,
        CancellationToken cancelToken = new())
    {
        var taskList = new List<Task>();
        foreach (var id in ids)
        {
            taskList.Add(FetchPerson(id, channelWriter, cancelToken));
        }
        await Task.WhenAll(taskList);
        channelWriter.Complete();
    }

    private static async Task FetchPerson(int id, ChannelWriter<Person> channelWriter,
        CancellationToken cancelToken)
    {
        var person = await reader.GetPersonAsync(id, cancelToken);
        await channelWriter.WriteAsync(person);
    }

    // OPTION 4: Parallel.ForEachAsync (runs parallel)
    private static async Task UseForEachAsync(CancellationToken cancelToken)
    {
        Console.WriteLine("One Moment Please ('x' to Cancel, 'q' to Quit)");
        DateTime start = DateTime.Now;

        try
        {
            List<int> ids = await reader.GetIdsAsync().ConfigureAwait(false);

            await Parallel.ForEachAsync(
                ids,
                new ParallelOptions()
                {
                    CancellationToken = cancelToken,
                    MaxDegreeOfParallelism = 10
                },
                async (id, token) =>
                {
                    var person = await reader.GetPersonAsync(id, token);
                    Console.WriteLine(person.ToString());
                });
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
            Console.WriteLine($"Total Time: {DateTime.Now - start}");
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