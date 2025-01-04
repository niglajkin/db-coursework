using System.ComponentModel.DataAnnotations.Schema;
using RESTfulAPI.Models;
using RESTfullAPI.Controllers;

namespace RESTfullAPI.Models;

[Table("Grant")]
public class GrantEntity {
    public int Id { get; init; }
    [ForeignKey("Project")] public int ProjectId { get; set; }
    [ForeignKey("User")] public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public ProjectEntity Project { get; set; }
    public UserEntity User { get; set; }
}