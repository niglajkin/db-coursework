using System.ComponentModel.DataAnnotations.Schema;
using RESTfullAPI.Controllers;

namespace RESTfulAPI.Models;

[Table("Artefact")]
public class ArtefactEntity {
    public int Id { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string FilePath { get; set; }
    public string FileType { get; set; }
    [ForeignKey("UploadedByUser")] public int UploadedBy { get; set; }
    [ForeignKey("Project")] public int ProjectId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public UserEntity UploadedByUser { get; set; }
    public ProjectEntity Project { get; set; }
}