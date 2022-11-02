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
            List<Person> people = new();

            foreach (int id in ids)
            {
                var person = await reader.GetPersonAsync(id);
                people.Add(person);
            }

            return View("Index", people);
        }
        catch (Exception ex)
        {
            List<Exception> errors = new() { ex };
            return View("Error", errors);
        }
        finally
        {
            ViewData["RequestEnd"] = DateTime.Now;
        }
    }

    // OPTION 2: Task w/ Continuation (runs parallel)
    public async Task<IActionResult> WithTask()
    {
        await Task.Delay(1);
        return Content("This has not been implemented");
    }

    // OPTION 3: Parallel.ForEachAsync (runs parallel)
    public async Task<IActionResult> WithForEach()
    {
        await Task.Delay(1);
        return Content("This has not been implemented");
    }
}
