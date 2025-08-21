using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
using TaskHive.Enums;
namespace TaskHive.Models;

public class TaskModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TaskId { get; set; }
    [Required(ErrorMessage = "Titile is Required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public required string TaskTitle { get; set; }
    [Required]
    [StringLength(500, ErrorMessage = "Task description cannot exceed 500 characters")]
    public string? TaskDescription { get; set; }
    public DateTime TaskCreationDate { get; set; } = DateTime.Now;
    [Required(ErrorMessage = "Due date is Required")]
    public DateTime TaskDueDate { get; set; }

    [Required(ErrorMessage = "Status is Required")]
    public Status TaskStatus { get; set; } = Status.Pending;

}
