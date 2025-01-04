namespace RESTfulAPI.DTOs;

public class ProjectDto {
    public required string Name { get; init; }
    public required string Description { get; init; }
    public int OwnerId { get; init; }  
    public int TeamId { get; init; }  
}