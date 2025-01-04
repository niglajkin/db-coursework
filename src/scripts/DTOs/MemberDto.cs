namespace RESTfulAPI.DTOs;

public class MemberDto {
    public int UserId { get; set; }
    public int TeamId { get; set; }
    public string TeamRole { get; set; } = "Developer";
}