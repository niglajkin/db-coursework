using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Models;

namespace RESTfulAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeamController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public TeamController(ApplicationDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamEntity>>> GetTeams()
        => await _context.Teams.ToListAsync();
    

    [HttpGet("{id}")]
    public async Task<ActionResult<TeamEntity>> GetTeam(int id) {
        var team = await _context.Teams.FindAsync(id);

        if (team == null) {
            return NotFound();
        }

        return team;
    }

    [HttpPost]
    public async Task<ActionResult<TeamEntity>> CreateTeam(TeamEntity teamEntity) {
        _context.Teams.Add(teamEntity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTeam), new { id = teamEntity.Id }, teamEntity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeam(int id, TeamEntity teamEntity) {
        if (id != teamEntity.Id) {
            return BadRequest();
        }

        _context.Entry(teamEntity).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!TeamExists(id)) {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeam(int id) {
        var team = await _context.Teams.FindAsync(id);
        if (team == null) {
            return NotFound();
        }

        _context.Teams.Remove(team);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TeamExists(int id) {
        return _context.Teams.Any(e => e.Id == id);
    }
}