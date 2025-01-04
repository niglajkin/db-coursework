using RESTfulAPI.Models;

namespace RESTfullAPI.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Member")]
public class MemberEntity {
    public int Id { get; set; }
    [ForeignKey("User")] public int UserId { get; set; }
    [ForeignKey("Team")]public int TeamId { get; set; }
    public string TeamRole { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.Now;
    
    public UserEntity User { get; set; }
    public TeamEntity Team { get; set; }
}
