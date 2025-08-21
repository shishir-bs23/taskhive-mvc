using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskHive.Models;

namespace TaskHive.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TaskHiveDbContext _context;

    public HomeController(ILogger<HomeController> logger, TaskHiveDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Tasks()
    {
        var allTasks = _context.Tasks.ToList();
        return View(allTasks);
    }

    public IActionResult Create(int? id)
    {
        if (id != null)
        {
            var taskInDb =_context.Tasks.SingleOrDefault(task => task.TaskId == id);
            return View(taskInDb);
        }
       
        return View();
    }

    public IActionResult Detete(int id)
    {
        var taskInDb = _context.Tasks.SingleOrDefault(task => task.TaskId==id);
        _context.Tasks.Remove(taskInDb);
        _context.SaveChanges();
        return RedirectToAction("Tasks");
    }

    public IActionResult CreateForm(TaskModel model)
    {
        if (model.TaskId == 0)
        {
            _context.Tasks.Add(model);
        }
        else
        {
            _context.Tasks.Update(model);
        }
       
        _context.SaveChanges();
        return RedirectToAction("Tasks");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
