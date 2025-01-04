# Реалізація інформаційного та програмного забезпечення

## SQL-скрипт для створення на початкового наповнення бази даних

```sql
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `mydb` ;

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8mb4 ;
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`Role`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`User` ;

CREATE TABLE IF NOT EXISTS `User` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(45) NOT NULL,
  `email` VARCHAR(45) NOT NULL,
  `password` VARCHAR(255) NOT NULL,
  `roleId` INT UNSIGNED NOT NULL,
  `status` ENUM('ACTIVE', 'BANNED') NOT NULL DEFAULT 'ACTIVE',
  `createdAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `email_UNIQUE` (`email`),
  INDEX `roleId_idx` (`roleId`),
  CONSTRAINT `fk_roleId`
      FOREIGN KEY (`roleId`)
          REFERENCES `Role` (`id`)
          ON DELETE NO ACTION
          ON UPDATE NO ACTION
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `mydb`.`Role`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Role` ;

CREATE TABLE IF NOT EXISTS `Role` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(45) NOT NULL,
    PRIMARY KEY (`id`),
    UNIQUE INDEX `idRole_UNIQUE` (`id`),
    UNIQUE INDEX `name_UNIQUE` (`name`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `mydb`.`Project`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Project`;

CREATE TABLE IF NOT EXISTS `Project` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(100) NOT NULL,
    `description` TEXT,
    `ownerId` INT UNSIGNED NOT NULL,
    `teamId` INT UNSIGNED NOT NULL,
    `createdAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    UNIQUE INDEX `name_UNIQUE` (`name`),
    INDEX `ownerId_idx` (`ownerId`),
    CONSTRAINT `fk_ownerId`
        FOREIGN KEY (`ownerId`)
            REFERENCES `User` (`id`)
            ON DELETE NO ACTION
            ON UPDATE CASCADE,
    CONSTRAINT `fk_teamId`
        FOREIGN KEY (`teamId`)
            REFERENCES `Team` (`id`)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `mydb`.`Team`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Team`;

CREATE TABLE IF NOT EXISTS `Team` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `createdAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `mydb`.`Member`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Member`;

CREATE TABLE IF NOT EXISTS `Member` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `userId` INT UNSIGNED NOT NULL,
    `teamId` INT UNSIGNED NOT NULL,
    `teamRole` ENUM('Developer', 'Project Leader') NOT NULL DEFAULT 'Developer',
    `joinedAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    INDEX `userId_idx` (`userId`),
    INDEX `teamId_idx` (`teamId`),
    CONSTRAINT `fk_userId`
        FOREIGN KEY (`userId`)
            REFERENCES `User` (`id`)
            ON DELETE CASCADE
            ON UPDATE CASCADE,
    CONSTRAINT `fk_member_teamId`
        FOREIGN KEY (`teamId`)
            REFERENCES `Team` (`id`)
            ON DELETE CASCADE
            ON UPDATE CASCADE
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `mydb`.`Task`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Task`;

CREATE TABLE IF NOT EXISTS `Task` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(100) NOT NULL,
  `description` TEXT NULL,
  `assignedTo` INT UNSIGNED DEFAULT NULL,
  `projectId` INT UNSIGNED NOT NULL,
  `status` ENUM('PENDING', 'IN_PROGRESS', 'COMPLETED', 'ON_HOLD', 'CANCELLED') NOT NULL DEFAULT 'PENDING',
  `priority` ENUM('LOW', 'MEDIUM', 'HIGH') NOT NULL DEFAULT 'MEDIUM',
  `dueDate` DATETIME NULL,
  `createdAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  INDEX `assignedTo_idx` (`assignedTo`),
  INDEX `projectId_idx` (`projectId`),
  CONSTRAINT `fk_assignedTo_user`
      FOREIGN KEY (`assignedTo`)
          REFERENCES `User` (`id`)
          ON DELETE SET NULL
          ON UPDATE CASCADE,
  CONSTRAINT `fk_projectId`
      FOREIGN KEY (`projectId`)
          REFERENCES `Project` (`id`)
          ON DELETE CASCADE
          ON UPDATE CASCADE
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `mydb`.`Artefact`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Artefact`;

CREATE TABLE IF NOT EXISTS `Artefact` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(100) NOT NULL,
  `description` TEXT NULL,
  `filePath` VARCHAR(255) NOT NULL,
  `fileType` VARCHAR(45) NOT NULL,
  `uploadedBy` INT UNSIGNED NOT NULL,
  `projectId` INT UNSIGNED NOT NULL,
  `createdAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  INDEX `projectId_idx` (`projectId`),
  CONSTRAINT `fk_uploadedBy_user`
      FOREIGN KEY (`uploadedBy`)
          REFERENCES `User` (`id`)
          ON DELETE NO ACTION
          ON UPDATE NO ACTION,
  CONSTRAINT `fk_projectId_artefact`
      FOREIGN KEY (`projectId`)
          REFERENCES `Project` (`id`)
          ON DELETE CASCADE
          ON UPDATE CASCADE
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `mydb`.`Grant`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Grant`;

CREATE TABLE IF NOT EXISTS `Grant` (
   `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
   `projectId` INT UNSIGNED NOT NULL,
   `userId` INT UNSIGNED NOT NULL,
   `createdAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
   PRIMARY KEY (`id`),
   INDEX `projectId_idx` (`projectId`),
   INDEX `userId_idx` (`userId`),
   CONSTRAINT `fk_grant_project`
       FOREIGN KEY (`projectId`)
           REFERENCES `Project` (`id`)
           ON DELETE CASCADE
           ON UPDATE CASCADE,
   CONSTRAINT `fk_grant_user`
       FOREIGN KEY (`userId`)
           REFERENCES `User` (`id`)
           ON DELETE CASCADE
           ON UPDATE CASCADE
) ENGINE = InnoDB;

-- Filling the tables with data
START TRANSACTION;

INSERT INTO `Role` (`name`) VALUES
('Admin'),
('Developer'),
('Manager');

INSERT INTO `User` (`username`, `email`, `password`, `roleId`, `status`) VALUES
('john_doe', 'john.doe@example.com', 'password123', 1, 'ACTIVE'),
('jane_smith', 'jane.smith@example.com', 'password123', 2, 'ACTIVE'),
('alex_williams', 'alex.williams@example.com', 'password123', 3, 'ACTIVE'),
('michael_brown', 'michael.brown@example.com', 'password123', 2, 'BANNED');

INSERT INTO `Team` () VALUES
(),
(),
(),
();

INSERT INTO `Member` (`userId`, `teamId`, `teamRole`) VALUES
(1, 1, 'Project Leader'),
(2, 1, 'Developer'),
(3, 2, 'Developer'),
(4, 3, 'Developer');

INSERT INTO `Project` (`name`, `description`, `ownerId`, `teamId`) VALUES
('Project A', 'Description for Project A', 1, 1),
('Project B', 'Description for Project B', 3, 2),
('Project C', 'Description for Project C', 1, 3);

INSERT INTO `Task` (`title`, `description`, `assignedTo`, `projectId`, `status`, `priority`, `dueDate`) VALUES
('Task 1 for Project A', 'Task 1 description', 2, 1, 'PENDING', 'HIGH', '2024-11-20 10:00:00'),
('Task 2 for Project A', 'Task 2 description', 3, 1, 'IN_PROGRESS', 'MEDIUM', '2024-11-25 12:00:00'),
('Task 1 for Project B', 'Task 1 description', 4, 2, 'PENDING', 'LOW', '2024-11-22 09:00:00'),
('Task 1 for Project C', 'Task 1 description', 2, 3, 'COMPLETED', 'HIGH', '2024-11-15 16:00:00');

INSERT INTO `Artefact` (`title`, `description`, `filePath`, `fileType`, `uploadedBy`, `projectId`) VALUES
('Artefact 1 for Project A', 'Initial design file', '/files/project_a/design_v1.pdf', 'PDF', 2, 1),
('Artefact 2 for Project B', 'Final report for Project B', '/files/project_b/report_final.pdf', 'PDF', 4, 2),
('Artefact 1 for Project C', 'Codebase for Project C', '/files/project_c/code.zip', 'ZIP', 2, 3);

INSERT INTO `Grant` (`projectId`, `userId`) VALUES
(1, 2),
(1, 3),
(2, 4);

COMMIT;
```
## RESTfull сервіс для управління даними

Models
```C#
[Table("User")]
public class UserEntity {
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
```

```C#

[Table("Team")]
public class TeamEntity {
    public int Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.Now;
}

```

```C#
[Table("Task")]
public class TaskEntity {
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    [ForeignKey("AssignedUser")] public int? AssignedTo { get; set; }
    [ForeignKey("Project")] public int ProjectId { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public UserEntity? AssignedUser { get; set; }
    public ProjectEntity Project { get; set; }
}

```

```C#

[Table("Role")]
public class RoleEntity {
    public int Id { get; set; }
    public string Name { get; set; }
}

```

```C#

[Table("Project")]
public class ProjectEntity {
    public int Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; } 
    [ForeignKey("Owner")] public int OwnerId { get; set; }
    [ForeignKey("Team")] public int TeamId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public UserEntity Owner { get; set; }
    public TeamEntity Team { get; set; }
}

```

```C#

[Table("Member")]
public class MemberEntity {
    public int Id { get; set; }
    [ForeignKey("User")] public int UserId { get; set; }
    [ForeignKey("Team")]public int TeamId { get; set; }
    public string TeamRole { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.Now;
    
    public UserEntity User { get; set; }
    public TeamEntity Team { get; set; }
}

```

```C#

[Table("Grant")]
public class GrantEntity {
    public int Id { get; init; }
    [ForeignKey("Project")] public int ProjectId { get; set; }
    [ForeignKey("User")] public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public ProjectEntity Project { get; set; }
    public UserEntity User { get; set; }
}

```

```C#

[Table("Artefact")]
public class ArtefactEntity {
    public int Id { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string FilePath { get; set; }
    public string FileType { get; set; }
    [ForeignKey("UploadedByUser")] public int UploadedBy { get; set; }
    [ForeignKey("Project")] public int ProjectId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public UserEntity UploadedByUser { get; set; }
    public ProjectEntity Project { get; set; }
}

```

Database context class

```C#

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

```
Controlers

```C#

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

```

```C#

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

```

```C#

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

```

```C#

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

```

```C#

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

```

```C#



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

```


```C#

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

```C#

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

```
