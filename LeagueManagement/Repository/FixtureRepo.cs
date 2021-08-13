using LeagueManagement.Data;
using LeagueManagement.Models;
using LeagueManagement.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Repository
{
    public class FixtureRepo : IFixtureRepo
    {
        private readonly LeagueApiContext _context;

        public FixtureRepo(LeagueApiContext context)
        {
            _context = context;
        }

        public async Task<Fixture> Add(Fixture fixture)
        {
            var _fixture = _context.Add(fixture);
            _context.SaveChanges();
            fixture.Id = _fixture.Entity.Id;

            ///await RecalculateLeagueStandingsAsync(fixture.Id);

            return fixture;
        }

        public void DeleteFixture(Fixture results)
        {
            _context.Remove(results);
            _context.SaveChanges();
        }

        public IEnumerable<Fixture> GetAllFixtures()
        {
            return _context.Fixture.Include(x => x.Team1).Include(x => x.Team2).ThenInclude(x => x.League).ToList();
        }

        public Fixture GetFixtureById(int id)
        {
            return _context.Fixture.Include(x => x.Team1).Include(x => x.Team2).Include(x => x.League).SingleOrDefault(x => x.Id == id);
        }

     

    }
}
