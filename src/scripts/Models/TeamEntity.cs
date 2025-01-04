using System.ComponentModel.DataAnnotations.Schema;

namespace RESTfulAPI.Models;

using System;

[Table("Team")]
public class TeamEntity {
    public int Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.Now;
}