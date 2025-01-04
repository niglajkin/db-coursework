using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Models;

namespace RESTfulAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserEntity>>> GetUsers() => 
        await _context.Users.ToListAsync();
    

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserEntity>> GetUser(int id) {
        var user = await _context.Users.FindAsync(id);

        if (user == null) {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<ActionResult<UserEntity>> CreateUser(UserEntity userEntity) {
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUser", new { id = userEntity.Id }, userEntity);
    }

   
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserEntity userEntity) {
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null) {
            return NotFound();
        }
        
        _context.Entry(userEntity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<UserEntity>> DeleteUser(int id) {
        var user = await _context.Users.FindAsync(id);
        if (user == null) {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return user;
    }
}