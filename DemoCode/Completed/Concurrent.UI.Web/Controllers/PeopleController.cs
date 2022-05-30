using Microsoft.AspNetCore.Mvc;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace Concurrent.UI.Web.Controllers;

public class PeopleController : Controller
{
    PersonReader reader = new();

    public Task<ViewResult> WithTask()
    {
        ViewData["Title"] = "Using Task";
        ViewData["RequestStart"] = DateTime.Now;

        Task<List<Person>> peopleTask = reader.GetPeopleAsync();
        Task<ViewResult> result = peopleTask.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                var errors = task.Exception!.Flatten().InnerExceptions;
                return View("Error", errors);
            }

            List<Person> people = task.Result;
            ViewData["RequestEnd"] = DateTime.Now;
            return View("Index", people);
        });
        return result;
    }

    public async Task<IActionResult> WithAwait()
    {
        ViewData["Title"] = "Using async/await";
        ViewData["RequestStart"] = DateTime.Now;

        try
        {
            List<Person> people = await reader.GetPeopleAsync();
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
