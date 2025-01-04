using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Models;
using RESTfullAPI.Controllers;
using RESTfulAPI.DTOs;


namespace RESTfulAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public ProjectController(ApplicationDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectEntity>>> GetProjects() {
        var projects = await _context.Projects
            .Include(p => p.Owner) 
            .Include(p => p.Team)
            .ToListAsync();

        return Ok(projects);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectEntity>> GetProject(int id) {
        var project = await _context.Projects
            .Include(p => p.Owner)
            .Include(p => p.Team)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null) return NotFound();


        return Ok(project);
    }


    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] ProjectDto projectDto) {
        var userExist = await _context.Users.AnyAsync(u => u.Id == projectDto.OwnerId);
        if (!userExist) {
            return BadRequest("Invalid OwnerId");
        }

        var teamExist = await _context.Teams.AnyAsync(t => t.Id == projectDto.TeamId);
        if (!teamExist) {
            return BadRequest("Invalid TeamId");
        }

        var project = new ProjectEntity {
            Name = projectDto.Name,
            Description = projectDto.Description,
            OwnerId = projectDto.OwnerId,
            TeamId = projectDto.TeamId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, ProjectDto projectDto) {
        var existingProject = await _context.Projects.FindAsync(id);
        if (existingProject == null) {
            return NotFound();
        }
        
        var ownerExists = await _context.Users.AnyAsync(u => u.Id == projectDto.OwnerId);
        if (!ownerExists) {
            return BadRequest("Owner not found.");
        }
        
        var teamExists = await _context.Teams.AnyAsync(t => t.Id == projectDto.TeamId);
        if (!teamExists) {
            return BadRequest("Team not found.");
        }

        
        existingProject.Name = projectDto.Name;
        existingProject.Description = projectDto.Description;
        existingProject.OwnerId = projectDto.OwnerId;
        existingProject.TeamId = projectDto.TeamId;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!_context.Projects.Any(p => p.Id == id)) {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id) {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) {
            return NotFound();
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}