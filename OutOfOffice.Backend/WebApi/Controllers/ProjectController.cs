using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectDTO>>> GetProjects(CancellationToken cancellationToken)
        {
            var projects = await _projectService.GetProjectsAsync(cancellationToken);

            if (projects == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(projects);
            }           
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDTO>> GetProjectById(int id, CancellationToken cancellationToken)
        {
            var project = await _projectService.GetProjectByIdAsync(id, cancellationToken);

            if (project == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(project);
            }            
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateProject([FromBody] ProjectDTO projectDTO, CancellationToken cancellationToken)
        {
            bool success = await _projectService.AddOrUpdateProjectAsync(projectDTO, cancellationToken);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateProject(int id, CancellationToken cancellationToken)
        {
            bool success = await _projectService.DeactivateProjectAsync(id, cancellationToken);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ProjectDTO>>> SearchProjects([FromQuery] string searchTerm, CancellationToken cancellationToken)
        {
            var projects = await _projectService.SearchProjectsAsync(searchTerm, cancellationToken);

            if (projects == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(projects);
            }            
        }

        [HttpPost("filter")]
        public async Task<ActionResult<List<ProjectDTO>>> FilterProjects([FromBody] FilterOptions options, CancellationToken cancellationToken)
        {
            var projects = await _projectService.FilterProjectsAsync(options, cancellationToken);

            if (projects == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(projects);
            }            
        }
    }
}
