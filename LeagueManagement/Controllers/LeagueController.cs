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
    public class LeagueController : Controller
    {
        private readonly ILeagueRepo _leagueRepo;

        public LeagueController(ILeagueRepo leagueRepo)
        {
            _leagueRepo = leagueRepo;
        }

        //Get Leagues
        [HttpGet]
        public IEnumerable<League> Get()
        {
            return _leagueRepo.GetAllLeagues();
        }
        [HttpGet("{id}", Name = "GetLeague")]
        public IActionResult Get(int id)
        {
            var league = _leagueRepo.GetLeagueById(id);
            if(league == null)
            {
                return NotFound();
            }
            return Ok(league);
        }
        // POST league
        [HttpPost]
        public IActionResult Post([FromBody] League value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var createdLeague = _leagueRepo.Add(value);

            return CreatedAtRoute("GetLeague", new { id = createdLeague.Id }, createdLeague);
        }

        // PUT league/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] League value)
        {
            if (value == null)
            {
                return BadRequest();
            }

            var league = _leagueRepo.GetLeagueById(id);

            if (league == null)
            {
                return NotFound();
            }

            value.Id = id;
            _leagueRepo.UpdateLeague(value);

            return NoContent();
        }

        // DELETE league/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var league = _leagueRepo.GetLeagueById(id);
            if (league == null)
            {
                return NotFound();
            }
            _leagueRepo.DeleteLeague(league);

            return NoContent();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
