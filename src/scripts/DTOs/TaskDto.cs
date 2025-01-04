namespace RESTfulAPI.DTOs;

public class CreateUpdateTaskDto {
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? AssignedTo { get; set; }
    public int ProjectId { get; set; }
    public string Status { get; set; } = "PENDING"; 
    public string Priority { get; set; } = "MEDIUM"; 
    public DateTime? DueDate { get; set; }
}