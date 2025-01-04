using RESTfulAPI.Models;
using RESTfulAPI.DTOs;
using RESTfullAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RESTfulAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public TasksController(ApplicationDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks() {
        var task = await _context.Tasks
            .Include(t => t.AssignedUser)
            .Include(t => t.Project)
            .Include(t => t.Project.Team)
            .ToListAsync();

        return Ok(task);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id) {
        var task = await _context.Tasks
            .Include(t => t.AssignedUser)
            .Include(t => t.Project)
            .Include(t => t.Project.Team)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null) {
            return NotFound();
        }

        return Ok(task);
    }
    

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateUpdateTaskDto taskDto) {
        var userExists = taskDto.AssignedTo == null || await _context.Users.AnyAsync(u => u.Id == taskDto.AssignedTo);
        if (!userExists) {
            return BadRequest("Invalid AssignedTo UserId");
        }

        var projectExists = await _context.Projects.AnyAsync(p => p.Id == taskDto.ProjectId);
        if (!projectExists) {
            return BadRequest("Invalid ProjectId");
        }

        var task = new TaskEntity {
            Title = taskDto.Title,
            Description = taskDto.Description,
            AssignedTo = taskDto.AssignedTo,
            ProjectId = taskDto.ProjectId,
            Status = taskDto.Status,
            Priority = taskDto.Priority,
            DueDate = taskDto.DueDate
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] CreateUpdateTaskDto taskDto) {
        var existingTask = await _context.Tasks.FindAsync(id);
        if (existingTask == null) {
            return NotFound();
        }

        var userExists = taskDto.AssignedTo == null || await _context.Users.AnyAsync(u => u.Id == taskDto.AssignedTo);
        if (!userExists) {
            return BadRequest("Invalid AssignedTo UserId");
        }

        var projectExists = await _context.Projects.AnyAsync(p => p.Id == taskDto.ProjectId);
        if (!projectExists) {
            return BadRequest("Invalid ProjectId");
        }

        existingTask.Title = taskDto.Title;
        existingTask.Description = taskDto.Description;
        existingTask.AssignedTo = taskDto.AssignedTo;
        existingTask.ProjectId = taskDto.ProjectId;
        existingTask.Status = taskDto.Status;
        existingTask.Priority = taskDto.Priority;
        existingTask.DueDate = taskDto.DueDate;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!await _context.Tasks.AnyAsync(t => t.Id == id)) {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id) {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
}