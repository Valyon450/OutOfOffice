using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Options;
using BusinessLogic.Requests;
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
        public async Task<IActionResult> CreateProject([FromBody] CreateOrUpdateProject request, CancellationToken cancellationToken)
        {
            int id = await _projectService.CreateProjectAsync(request, cancellationToken);

            if (id != 0)
            {
                return CreatedAtAction(nameof(GetProjectById), new { id }, request);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] CreateOrUpdateProject request, CancellationToken cancellationToken)
        {
            bool success = await _projectService.UpdateProjectAsync(id, request, cancellationToken);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPatch("{id}")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id, CancellationToken cancellationToken)
        {
            bool success = await _projectService.DeleteProjectAsync(id, cancellationToken);

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

        [HttpGet("filter")]
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
