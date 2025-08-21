using Microsoft.EntityFrameworkCore;
namespace TaskHive.Models;

public class TaskHiveDbContext : DbContext
{

    public DbSet<TaskModel> Tasks { get; set;}
    public TaskHiveDbContext(DbContextOptions options): base(options)
    {

    }
    
}