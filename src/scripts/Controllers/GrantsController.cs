using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Models;
using RESTfulAPI.DTOs;

namespace RESTfulAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GrantController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public GrantController(ApplicationDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GrantEntity>>> GetGrants() {
        var grants = await _context.Grants
            .Include(g => g.Project)
            .Include(g => g.Project.Team)
            .Include(g => g.User)
            .ToListAsync();

        return grants;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GrantEntity>> GetGrant(int id) {
        var grant = await _context.Grants
            .Include(g => g.Project) // Include Project entity
            .Include(g => g.User) // Include User entity
            .FirstOrDefaultAsync(g => g.Id == id);

        if (grant == null) {
            return NotFound();
        }

        return grant;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGrant([FromBody] CreateGrantDto grantDto) {
        var projectExists = await _context.Projects.AnyAsync(p => p.Id == grantDto.ProjectId);
        if (!projectExists) {
            return BadRequest("Invalid ProjectId.");
        }
        
        var userExists = await _context.Users.AnyAsync(u => u.Id == grantDto.UserId);
        if (!userExists) {
            return BadRequest("Invalid UserId.");
        }

        var grant = new GrantEntity {
            ProjectId = grantDto.ProjectId,
            UserId = grantDto.UserId,
        };

        _context.Grants.Add(grant);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGrant), new { id = grant.Id }, grant);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGrant(int id, [FromBody] CreateGrantDto grantDto) {
        var existingGrant = await _context.Grants.FindAsync(id);
        if (existingGrant == null) {
            return NotFound();
        }
        
        var projectExists = await _context.Projects.AnyAsync(p => p.Id == grantDto.ProjectId);
        if (!projectExists) {
            return BadRequest("Project not found.");
        }

        var userExists = await _context.Users.AnyAsync(u => u.Id == grantDto.UserId);
        if (!userExists) {
            return BadRequest("User not found.");
        }
        
        existingGrant.ProjectId = grantDto.ProjectId;
        existingGrant.UserId = grantDto.UserId;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!_context.Grants.Any(g => g.Id == id)) {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGrant(int id) {
        var grant = await _context.Grants.FindAsync(id);
        if (grant == null) {
            return NotFound();
        }

        _context.Grants.Remove(grant);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
