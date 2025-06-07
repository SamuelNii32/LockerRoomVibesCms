using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LockerRoomVibesCms.Data;
using LockerRoomVibesCms.Models;
using LockerRoomVibesCms.Interfaces;

namespace LockerRoomVibesCms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        /// <summary>
        /// Returns a list of all teams.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{TeamDto}, {TeamDto}, ...]
        /// </returns>
        /// <example>
        /// GET: api/Teams -> [{TeamDto}, {TeamDto}, ...]
        /// </example>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeams()
        {
            var teams = await _teamService.GetTeamsAsync();
            return Ok(teams);
        }


        /// <summary>
        /// Returns a single team by its ID.
        /// </summary>
        /// <param name="id">The ID of the team.</param>
        /// <returns>
        /// 200 OK
        /// {TeamDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Teams/3 -> {TeamDto}
        /// </example>

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDto>> GetTeam(int id)
        {
            var team = await _teamService.GetTeamAsync(id);
            if (team == null) return NotFound();
            return Ok(team);
        }


        /// <summary>
        /// Creates a new team.
        /// </summary>
        /// <param name="teamDto">The information for the new team.</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Teams/{id}
        /// {TeamDto}
        /// or
        /// 400 Bad Request
        /// </returns>
        /// <example>
        /// POST: api/Teams
        /// Request Body: {TeamDto}
        /// -> Response: 201 Created
        /// </example>

        [HttpPost]
        public async Task<ActionResult<TeamDto>> CreateTeam(TeamDto teamDto)
        {
            try
            {
                var createdTeam = await _teamService.CreateTeamAsync(teamDto);
                return CreatedAtAction(nameof(GetTeam), new { id = createdTeam.id }, createdTeam);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        /// <summary>
        /// Updates an existing team.
        /// </summary>
        /// <param name="id">The ID of the team to update.</param>
        /// <param name="teamDto">The updated team data.</param>
        /// <returns>
        /// 200 OK
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// PUT: api/Teams/3
        /// Request Body: {TeamDto}
        /// -> Response: 200 OK
        /// </example>

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, TeamDto teamDto)
        {
            var updatedTeam = await _teamService.UpdateTeamAsync(id, teamDto);
            if (updatedTeam == null) return NotFound();
            return Ok(updatedTeam);
        }


        /// <summary>
        /// Deletes a team by its ID.
        /// </summary>
        /// <param name="id">The ID of the team to delete.</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/Teams/3
        /// -> Response: 204 No Content
        /// </example>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var deleted = await _teamService.DeleteTeamAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }





    }

}

