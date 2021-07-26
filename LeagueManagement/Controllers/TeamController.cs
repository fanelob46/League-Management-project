using LeagueManagement.Models;
using LeagueManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        private readonly ITeamRepo _teamRepo;
        public TeamController(ITeamRepo teamRepo)
        {
            _teamRepo = teamRepo;
        }
        // GET: team
        [HttpGet]
        public IEnumerable<Team> Get()
        {
            return _teamRepo.GetAllTeams();
        }

        [HttpGet]
        [Route("leagueTeams/{id}")]
        public IEnumerable<Team> GetTeamsByLeagueId(int id)
        {
            return _teamRepo.GetAllTeamsByLeagueId(id);
        }
        // GET team/5
        [HttpGet("{id}", Name = "GetTeam")]
        public IActionResult Get(int id)
        {
            var team = _teamRepo.GetTeamById(id);
            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        // POST team
        [HttpPost]
        public IActionResult Post([FromBody] Team value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var createdTeam = _teamRepo.Add(value);

            return CreatedAtRoute("GetTeam", new { id = createdTeam.Id }, createdTeam);
        }

        // PUT team/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Team value)
        {
            if (value == null)
            {
                return BadRequest();
            }

            var team = _teamRepo.GetTeamById(id);

            if (team == null)
            {
                return NotFound();
            }

            value.Id = id;
            _teamRepo.UpdateTeam(value);

            return NoContent();
        }

        // DELETE team/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var team = _teamRepo.GetTeamById(id);
            if (team == null)
            {
                return NotFound();
            }
            _teamRepo.DeleteTeam(team);

            return NoContent();
        }
        
    }
}
