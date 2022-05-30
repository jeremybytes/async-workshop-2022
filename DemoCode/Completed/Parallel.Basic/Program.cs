using System.Threading.Channels;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace ParallelBasic;

class Program
{
    static PersonReader reader = new();

    static async Task Main(string[] args)
    {
        var start = DateTimeOffset.Now;
        Console.Clear();

        var ids = await reader.GetIdsAsync();

        Console.WriteLine(ids.ToDelimitedString(","));

        // Option 1 = Run Sequentially
        //await RunSequentially(ids);

        // Option 2 = Task w/ Continuation
        //await RunWithContinuation(ids);

        // Option 3 = Channels
        //await RunWithChannel(ids);

        // Option 4 = ForEachAsync
        await RunWithForEachAsync(ids);

        var elapsed = DateTimeOffset.Now - start;
        Console.WriteLine($"\nTotal time: {elapsed}");

        Console.ReadLine();
    }

    // Option 1
    static async Task RunSequentially(List<int> ids)
    {
        foreach (var id in ids)
        {
            var person = await reader.GetPersonAsync(id);
            DisplayPerson(person);
        }
    }

    // Option 2
    static async Task RunWithContinuation(List<int> ids)
    {
        var allTasks = new List<Task>();

        foreach (var id in ids)
        {
            Task<Person> currentTask = reader.GetPersonAsync(id);

            Task continuation = currentTask.ContinueWith(t =>
            {
                var person = t.Result;
                lock (allTasks)
                {
                    DisplayPerson(person);
                }
            });

            allTasks.Add(continuation);
        }
        await Task.WhenAll(allTasks);
    }

    // Option 3
    static async Task RunWithChannel(List<int> ids)
    {
        var channel = Channel.CreateBounded<Person>(10);

        var consumer = ShowData(channel.Reader);
        var producer = ProduceData(ids, channel.Writer);

        await producer;
        await consumer;
    }

    static async Task ShowData(ChannelReader<Person> channelReader)
    {
        await foreach (var person in channelReader.ReadAllAsync())
        {
            DisplayPerson(person);
        }
    }

    static async Task ProduceData(List<int> ids, ChannelWriter<Person> channelWriter)
    {
        var allTasks = new List<Task>();
        foreach (int id in ids)
        {
            var currentTask = FetchRecord(id, channelWriter);
            allTasks.Add(currentTask);
        }
        await Task.WhenAll(allTasks);
        channelWriter.Complete();
    }

    static async Task FetchRecord(int id, ChannelWriter<Person> channelWriter)
    {
        var person = await reader.GetPersonAsync(id);
        await channelWriter.WriteAsync(person);
    }

    // Option 4: Parallel.ForEachAsync
    static async Task RunWithForEachAsync(List<int> ids)
    {
        await Parallel.ForEachAsync(
            ids,
            async(id, _) =>
            {
                var person = await reader.GetPersonAsync(id);
                lock (ids)
                {
                    DisplayPerson(person);
                }
            });
    }


    static void DisplayPerson(Person person)
    {
        Console.WriteLine("--------------");
        Console.WriteLine($"{person.Id}: {person}");
        Console.WriteLine($"{person.StartDate:D}");
        Console.WriteLine($"Rating: {new string('*', person.Rating)}");
    }
}