using RESTfulAPI.Models;
using RESTfulAPI.DTOs;
using RESTfullAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace RESTfulAPI.Controllers;


[Route("api/[controller]")]
[ApiController]
public class MembersController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public MembersController(ApplicationDbContext context) {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberEntity>>> GetMembers() {
        var members = await _context.Members
            .Include(m => m.User)
            .Include(m => m.Team)
            .ToListAsync();

        return Ok(members);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<MemberEntity>> GetMember(int id) {
        var member = await _context.Members
            .Include(m => m.User)
            .Include(m => m.Team)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (member == null) {
            return NotFound();
        }

        return Ok(member);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMember([FromBody] MemberDto memberDto) {
        var userExist = await _context.Users.AnyAsync(u => u.Id == memberDto.UserId);
        if (!userExist) {
            return BadRequest("Invalid UserId");
        }

        var teamExist = await _context.Teams.AnyAsync(t => t.Id == memberDto.TeamId);
        if (!teamExist) {
            return BadRequest("Invalid TeamId");
        }
        
        var member = new MemberEntity {
            UserId = memberDto.UserId,
            TeamId = memberDto.TeamId,
            TeamRole = memberDto.TeamRole,
        };
        
        _context.Members.Add(member);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMember(int id, [FromBody] MemberDto memberDto) {
        var existingMember = await _context.Members.FindAsync(id);
        if (existingMember == null) {
            return NotFound();
        }

        var userExists = await _context.Users.AnyAsync(u => u.Id == memberDto.UserId);
        if (!userExists) {
            return BadRequest("Invalid UserId");
        }

        var teamExist = await _context.Teams.AnyAsync(t => t.Id == memberDto.TeamId);
        if (!teamExist) {
            return BadRequest("Invalid TeamId");
        }
        
        existingMember.UserId = memberDto.UserId;
        existingMember.TeamId = memberDto.TeamId;
        existingMember.TeamRole = memberDto.TeamRole;

        
        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!await _context.Members.AnyAsync(m => m.Id == id)) {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMember(int id) {
        var member = await _context.Members.FindAsync(id);
        if (member == null) {
            return NotFound();
        }

        _context.Members.Remove(member);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}