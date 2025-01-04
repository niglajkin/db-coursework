using Microsoft.EntityFrameworkCore;
using RESTfullAPI.Controllers;
using RESTfullAPI.Models;


namespace RESTfulAPI.Models;

public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users { get; init; }
    public DbSet<ProjectEntity> Projects { get; init; }
    public DbSet<TeamEntity> Teams { get; init; }
    public DbSet<MemberEntity> Members { get; init; }
    public DbSet<TaskEntity> Tasks { get; init; }
    public DbSet<ArtefactEntity> Artefacts { get; init; }
    public DbSet<GrantEntity> Grants { get; init; }
    public DbSet<RoleEntity> Roles { get; init; }
}