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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Retrieves all projects.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>List of projects.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Retrieves a specific project.
        /// </summary>
        /// <param name="id">Project Id.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>The project.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="request">Project data.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>Created project Id.</returns>
        /// <response code="201">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpPost]
        [Authorize(Roles = "Project Manager, Administrator")] // Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
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

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="id">Project Id.</param>
        /// <param name="request">Updated project data.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Project Manager, Administrator")] // Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Activates or deactivates a project.
        /// </summary>
        /// <param name="id">Project Id.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpPatch("{id}")]
        [Authorize(Roles = "Project Manager, Administrator")] // Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Deletes a project.
        /// </summary>
        /// <param name="id">Project Id.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">User is unauthorized</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")] // Administrators can access this
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Searches projects based on a search term.
        /// </summary>
        /// <param name="searchTerm">Term to search for.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>List of projects matching the search term.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet("search")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Filters projects based on filter options.
        /// </summary>
        /// <param name="options">Filter options.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>List of projects matching the filter options.</returns>
        /// <response code="200">Success</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpPost("filter")]
        [Authorize(Roles = "HR Manager, Project Manager, Administrator")] // HR Managers, Project Managers and Administrators can access this
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
