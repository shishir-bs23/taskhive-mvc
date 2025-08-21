using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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

    // public IActionResult Tasks(string searchString)
    // {
    //     var allTasks = _context.Tasks.ToList();
    //     if (!String.IsNullOrEmpty(searchString))
    //     {
    //         allTasks = allTasks.Where(n => n.TaskTitle.Contains(searchString)).ToList();
    //     }
    //     return View(allTasks);
    // }

    public IActionResult Tasks(string searchString, string sortOrder)
    {
        var allTasks = _context.Tasks.ToList();
        if (!String.IsNullOrEmpty(searchString))
        {
            allTasks = allTasks.Where(n => n.TaskTitle.Contains(searchString)).ToList();
        }
        allTasks = allTasks.OrderBy(e => e.TaskTitle).ToList();
        allTasks = allTasks.OrderByDescending(e => e.TaskTitle).ToList();

        switch (sortOrder)
        {

            case "id_asc":
                allTasks = allTasks.OrderBy(e => e.TaskId).ToList();
                break;

            case "title_asc":
                allTasks = allTasks.OrderBy(e => e.TaskTitle).ToList();
                break;

            case "creation_date_asc":
                allTasks = allTasks.OrderBy(e => e.TaskCreationDate).ToList();
                break;
            case "due_date_asc":
                allTasks = allTasks.OrderBy(e => e.TaskDueDate).ToList();
                break;
            default:
                allTasks = allTasks.OrderBy(e => e.TaskId).ToList();
                break;
       }

        return View(allTasks);
    }

    public IActionResult Create(int? id)
    {
        if (id != null)
        {
            var taskInDb = _context.Tasks.SingleOrDefault(task => task.TaskId == id);
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

    public IActionResult Details(int id)
    {
        var taskInDb = _context.Tasks.SingleOrDefault(task => task.TaskId == id);
        return View(taskInDb);
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
