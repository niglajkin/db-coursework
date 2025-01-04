using System.ComponentModel.DataAnnotations.Schema;

namespace RESTfulAPI.Models;

[Table("User")]
public class UserEntity {
    public int Id { get; set; }
    public required string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}