using System.Diagnostics;
using System.Threading.Tasks;
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

    public async Task<IActionResult> Tasks(string searchString, string sortOrder, int pageNumber)
    {
        var allTasks = from t in _context.Tasks
               select t;

        if (!String.IsNullOrEmpty(searchString))
        {
            allTasks = allTasks.Where(n => n.TaskTitle.Contains(searchString));
        }
        allTasks = allTasks.OrderBy(e => e.TaskTitle);
        allTasks = allTasks.OrderByDescending(e => e.TaskTitle);

        switch (sortOrder)
        {

            case "id_asc":
                allTasks = allTasks.OrderBy(e => e.TaskId);
                break;

            case "title_asc":
                allTasks = allTasks.OrderBy(e => e.TaskTitle);
                break;

            case "creation_date_asc":
                allTasks = allTasks.OrderBy(e => e.TaskCreationDate);
                break;
            case "due_date_asc":
                allTasks = allTasks.OrderBy(e => e.TaskDueDate);
                break;
            default:
                allTasks = allTasks.OrderBy(e => e.TaskId);
                break;
       }
//Pagination
        if (pageNumber < 1)
        {
            pageNumber = 1;
        }
        int pageSize = 5;
        return View(await PaginatedList<TaskModel>.CreateAsync(allTasks,pageNumber,pageSize));
        // return View(allTasks);
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
