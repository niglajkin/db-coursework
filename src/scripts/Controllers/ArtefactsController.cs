using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Models;
using RESTfulAPI.DTOs;

namespace RESTfulAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArtefactsController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public ArtefactsController(ApplicationDbContext context) {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArtefactEntity>>> GetArtefacts() {
        var artefacts = await _context.Artefacts
            .Include(a => a.UploadedByUser)
            .Include(a => a.Project)
            .Include(a => a.Project.Team)
            .ToListAsync();

        return Ok(artefacts);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ArtefactEntity>> GetArtefact(int id) {
        var artefact = await _context.Artefacts
            .Include(a => a.UploadedByUser)
            .Include(a => a.Project)
            .Include(a => a.Project.Team)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (artefact == null) {
            return NotFound();
        }

        return Ok(artefact);
    }
    
    [HttpPost]
    public async Task<ActionResult<ArtefactEntity>> CreateArtefact([FromBody] CreateArtefactDto artefactDto) {
        var userExists = await _context.Users.AnyAsync(u => u.Id == artefactDto.UploadedById);
        if (!userExists) {
            return BadRequest("Invalid UploadedById");
        }
        
        var projectExists = await _context.Projects.AnyAsync(p => p.Id == artefactDto.ProjectId);
        if (!projectExists) {
            return BadRequest("Invalid ProjectId");
        }
        
        var artefact = new ArtefactEntity {
            Title = artefactDto.Title,
            Description = artefactDto.Description,
            FilePath = artefactDto.FilePath,
            FileType = artefactDto.FileType,
            UploadedBy = artefactDto.UploadedById,
            ProjectId = artefactDto.ProjectId,
        };

        _context.Artefacts.Add(artefact);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetArtefact), new { id = artefact.Id }, artefact);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateArtefact(int id, [FromBody] CreateArtefactDto artefactDto) {
        var artefact = await _context.Artefacts.FindAsync(id);
        if (artefact == null) {
            return NotFound();
        }
        
        var userExists = await _context.Users.AnyAsync(u => u.Id == artefactDto.UploadedById);
        if (!userExists) {
            return BadRequest("Invalid UploadedById");
        }
        
        var projectExists = await _context.Projects.AnyAsync(p => p.Id == artefactDto.ProjectId);
        if (!projectExists) {
            return BadRequest("Invalid ProjectId");
        }
        
        artefact.Title = artefactDto.Title;
        artefact.Description = artefactDto.Description;
        artefact.FilePath = artefactDto.FilePath;
        artefact.FileType = artefactDto.FileType;
        artefact.UploadedBy = artefactDto.UploadedById;
        artefact.ProjectId = artefactDto.ProjectId;

        _context.Entry(artefact).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!_context.Artefacts.Any(a => a.Id == id)) {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtefact(int id) {
        var artefact = await _context.Artefacts.FindAsync(id);
        if (artefact == null) {
            return NotFound();
        }

        _context.Artefacts.Remove(artefact);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}