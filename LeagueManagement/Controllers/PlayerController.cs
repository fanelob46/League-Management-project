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
    public class PlayerController : Controller
    {
        private readonly IPlayerRepo _playerRepo;

        public PlayerController(IPlayerRepo playerRepo)
        {
            _playerRepo = playerRepo;
        }

        //Get Plyer
        [HttpGet]
        public IEnumerable<Player> Get()
        {
            return _playerRepo.GetAllPlayers();
        }
        [HttpGet("{id}", Name = "GetPlayer")]
        public IActionResult Get(int id)
        {
            var player = _playerRepo.GetPlayerById(id);
            if(player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }
        [HttpGet]
        [Route("teamPlayer/{id}")]
        public IEnumerable<Player> GetPlayerByTeamId(int id)
        {
            return _playerRepo.GetPlayerByTeamId(id);
        }

        // POST player
        [HttpPost]
        public IActionResult Post([FromBody] Player value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var createdPlayer = _playerRepo.Add(value);

            return CreatedAtRoute("GetPlayer", new { id = createdPlayer.Id }, createdPlayer);
        }

        // PUT player/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Player value)
        {
            if (value == null)
            {
                return BadRequest();
            }

            var player = _playerRepo.GetPlayerById(id);

            if (player == null)
            {
                return NotFound();
            }

            value.Id = id;
            _playerRepo.UpdatePlayer(value);

            return NoContent();
        }

        // DELETE player/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var player = _playerRepo.GetPlayerById(id);
            if (player == null)
            {
                return NotFound();
            }
            _playerRepo.DeletePlayer(player);

            return NoContent();
        }
    }
}
