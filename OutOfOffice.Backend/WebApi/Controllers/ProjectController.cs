using BusinessLogic.DTOs;
using BusinessLogic.Options;
using BusinessLogic.Requests;
using BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all actions in this controller
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
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
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        public async Task<ActionResult<ProjectDTO>> GetProjectById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _projectService.GetProjectByIdAsync(id, cancellationToken);

                return Ok(project);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Project Manager, Administrator")] // Project Managers and Administrators can access this
        public async Task<IActionResult> CreateProject([FromBody] CreateOrUpdateProject request, CancellationToken cancellationToken)
        {
            try
            {
                int id = await _projectService.CreateProjectAsync(request, cancellationToken);

                return CreatedAtAction(nameof(GetProjectById), new { id }, request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Project Manager, Administrator")] // Project Managers and Administrators can access this
        public async Task<IActionResult> UpdateProject(int id, [FromBody] CreateOrUpdateProject request, CancellationToken cancellationToken)
        {
            try
            {
                await _projectService.UpdateProjectAsync(id, request, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Project Manager, Administrator")] // Project Managers and Administrators can access this
        public async Task<IActionResult> ActivateOrDeactivateProject(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _projectService.ActivateOrDeactivateProjectAsync(id, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")] // Administrators can access this
        public async Task<IActionResult> DeleteProject(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _projectService.DeleteProjectAsync(id, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
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
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
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
