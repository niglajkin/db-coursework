namespace RESTfulAPI.DTOs;

public class CreateArtefactDto {
    public string Title { get; set; }
    public string Description { get; set; }
    public string FilePath { get; set; }
    public string FileType { get; set; }
    public int UploadedById { get; set; }
    public int ProjectId { get; set; }
}