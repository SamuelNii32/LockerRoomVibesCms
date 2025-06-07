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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeams()
        {
            var teams = await _teamService.GetTeamsAsync();
            return Ok(teams);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDto>> GetTeam(int id)
        {
            var team = await _teamService.GetTeamAsync(id);
            if (team == null) return NotFound();
            return Ok(team);
        }



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



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, TeamDto teamDto)
        {
            var updatedTeam = await _teamService.UpdateTeamAsync(id, teamDto);
            if (updatedTeam == null) return NotFound();
            return Ok(updatedTeam);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var deleted = await _teamService.DeleteTeamAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }





    }

}

