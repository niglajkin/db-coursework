using System.ComponentModel.DataAnnotations.Schema;

namespace RESTfulAPI.Models;

[Table("Role")]
public class RoleEntity {
    public int Id { get; set; }
    public string Name { get; set; }
}