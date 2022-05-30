using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace ParallelUI.Web.Controllers;

public class PeopleController : Controller
{
    PersonReader reader = new();

    // OPTION 1: Await (runs sequentially)
    public async Task<IActionResult> WithAwait()
    {
        ViewData["Title"] = "Using async/await (not parallel)";
        ViewData["RequestStart"] = DateTime.Now;
        try
        {
            List<int> ids = await reader.GetIdsAsync();
            var people = new List<Person>();

            foreach(int id in ids)
            {
                people.Add(await reader.GetPersonAsync(id));
            }

            return View("Index", people);
        }
        catch (Exception ex)
        {
            var errors = new List<Exception>() { ex };
            return View("Error", errors);
        }
        finally
        {
            ViewData["RequestEnd"] = DateTime.Now;
        }
    }

    // OPTION 2: Task w/ Continuation (runs parallel)
    public async Task<ViewResult> WithTask()
    {
        ViewData["Title"] = "Using Task (parallel)";
        ViewData["RequestStart"] = DateTime.Now;

        try
        {
            List<int> ids = await reader.GetIdsAsync();
            var people = new BlockingCollection<Person>();
            var taskList = new List<Task>();

            foreach (int id in ids)
            {
                Task<Person> personTask = reader.GetPersonAsync(id);
                Task continuation = personTask.ContinueWith(task =>
                {
                    Person person = task.Result;
                    people.Add(person);
                },
                TaskContinuationOptions.OnlyOnRanToCompletion);
                
                taskList.Add(continuation);
            }

            await Task.WhenAll(taskList);
            return View("Index", people);
        }
        catch (Exception ex)
        {
            var errors = new List<Exception>() { ex };
            return View("Error", errors);
        }
        finally
        {
            ViewData["RequestEnd"] = DateTime.Now;
        }
    }

    // OPTION 3: Parallel.ForEachAsync (runs parallel)
    public async Task<ViewResult> WithForEach()
    {
        ViewData["Title"] = "Using ForEachAsync (parallel)";
        ViewData["RequestStart"] = DateTime.Now;

        try
        {
            List<int> ids = await reader.GetIdsAsync();
            var people = new BlockingCollection<Person>();
            await Parallel.ForEachAsync(
                ids,
                async (id, token) =>
                {
                    var person = await reader.GetPersonAsync(id);
                    people.Add(person);
                }
            );
            return View("Index", people);
        }
        catch (Exception ex)
        {
            var errors = new List<Exception>() { ex };
            return View("Error", errors);
        }
        finally
        {
            ViewData["RequestEnd"] = DateTime.Now;
        }
    }
}
