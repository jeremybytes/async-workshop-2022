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
        await RunSequentially(ids);

        // Option 2 = Task w/ Continuation
        //await RunWithContinuation(ids);

        // Option 3 = Channels
        //await RunWithChannel(ids);

        // Option 4 = ForEachAsync
        //await RunWithForEachAsync(ids);

        var elapsed = DateTimeOffset.Now - start;
        Console.WriteLine($"\nTotal time: {elapsed}");

        Console.ReadLine();
    }

    // Option 1
    static async Task RunSequentially(List<int> ids)
    {
        foreach (var id in ids)
        {
            var person = await Task.Run(() => reader.GetPerson(id));
            DisplayPerson(person);
        }
    }

    // Option 2
    static async Task RunWithContinuation(List<int> ids)
    {
        await Task.Delay(1);
    }

    // Option 3
    static async Task RunWithChannel(List<int> ids)
    {
        await Task.Delay(1);
    }

    // Option 4: Parallel.ForEachAsync
    static async Task RunWithForEachAsync(List<int> ids)
    {
        await Task.Delay(1);
    }


    static void DisplayPerson(Person person)
    {
        Console.WriteLine("--------------");
        Console.WriteLine($"{person.Id}: {person}");
        Console.WriteLine($"{person.StartDate:D}");
        Console.WriteLine($"Rating: {new string('*', person.Rating)}");
    }
}