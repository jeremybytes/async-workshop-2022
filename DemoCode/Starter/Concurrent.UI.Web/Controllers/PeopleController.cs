using Microsoft.AspNetCore.Mvc;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace Concurrent.UI.Web.Controllers;

public class PeopleController : Controller
{
    PersonReader reader = new();

    public ViewResult WithTask()
    {
        ViewData["Title"] = "Using Task";
        ViewData["RequestStart"] = DateTime.Now;

        ViewData["RequestEnd"] = DateTime.Now;
        return View("Index", new List<Person>());
    }

    public IActionResult WithAwait()
    {
        ViewData["Title"] = "Using async/await";
        ViewData["RequestStart"] = DateTime.Now;
        
        ViewData["RequestEnd"] = DateTime.Now;
        return View("Index", new List<Person>());
    }
}
