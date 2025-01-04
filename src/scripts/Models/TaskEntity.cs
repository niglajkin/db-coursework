using System.ComponentModel.DataAnnotations.Schema;
using RESTfulAPI.Models;
using RESTfullAPI.Controllers;

namespace RESTfullAPI.Models;

[Table("Task")]
public class TaskEntity {
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    [ForeignKey("AssignedUser")] public int? AssignedTo { get; set; }
    [ForeignKey("Project")] public int ProjectId { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public UserEntity? AssignedUser { get; set; }
    public ProjectEntity Project { get; set; }
}