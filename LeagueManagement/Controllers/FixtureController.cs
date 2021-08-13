using LeagueManagement.Models;
using LeagueManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Controllers
{
    public class FixtureController : Controller
    {
        private readonly IFixtureRepo _fixtureRepository;

        public FixtureController(IFixtureRepo fixtureRepository)
        {
            _fixtureRepository = fixtureRepository;
        }

        // GET: fixture
        [HttpGet]
        public IEnumerable<Fixture> Get()
        {
            return _fixtureRepository.GetAllFixtures();
        }

        // GET fixture/5
        [HttpGet("{id}", Name = "GetFixture")]
        public IActionResult Get(int id)
        {
            var league = _fixtureRepository.GetFixtureById(id);
            if (league == null)
            {
                return NotFound();
            }

            return Ok(league);
        }

        // POST fixture
        [HttpPost]
        public IActionResult Post([FromBody] Fixture value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var createdFixture = _fixtureRepository.Add(value);

            return CreatedAtRoute("GetFixture", new { id = createdFixture.Id }, createdFixture);
        }
        // DELETE fixture/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var fixture = _fixtureRepository.GetFixtureById(id);
            if (fixture == null)
            {
                return NotFound();
            }
            _fixtureRepository.DeleteFixture(fixture);

            return NoContent();
        }

    }
}
