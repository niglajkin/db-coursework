using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Models;

namespace RESTfulAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public RoleController(ApplicationDbContext context) {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleEntity>>> GetRoles() 
        => await _context.Roles.ToListAsync();
        
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<RoleEntity>> GetRole(int id) {
        var role = await _context.Roles.FindAsync(id);

        if (role == null) {
            return NotFound();
        }

        return role;
    }

    
    [HttpPost]
    public async Task<IActionResult> CreateRole(RoleEntity role) {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(int id, RoleEntity role) {
        var existingRole = await _context.Roles.FindAsync(id);
        if (existingRole == null) {
            return NotFound();
        }

        existingRole.Name = role.Name;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id) {
        var role = await _context.Roles.FindAsync(id);
        if (role == null) {
            return NotFound();
        }

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}