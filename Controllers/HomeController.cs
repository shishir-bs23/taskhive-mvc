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

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult CreateForm(TaskModel model)
    {
        _context.Tasks.Add(model);
        _context.SaveChanges();
        return RedirectToAction("Tasks");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
