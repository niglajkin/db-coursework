using System.ComponentModel.DataAnnotations.Schema;
using RESTfulAPI.Models;

namespace RESTfullAPI.Controllers;

[Table("project")]
public class ProjectEntity {
    public int Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; } 
    [ForeignKey("Owner")] public int OwnerId { get; set; }
    [ForeignKey("Team")] public int TeamId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public UserEntity Owner { get; set; }
    public TeamEntity Team { get; set; }
}