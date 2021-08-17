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
    public class LeagueRepo : ILeagueRepo
    {
        private readonly LeagueApiContext _context;
        public LeagueRepo(LeagueApiContext context)
        {
            _context = context;
        }
        public League Add(League league)
        {
            var _league = _context.Add(league);
            _context.SaveChanges();
            league.Id = _league.Entity.Id;

            return league;
        }
        public async Task<int> AddTeamStandingAsync(IEnumerable<TableStanding> items)
        {


            await _context.Standings.AddRangeAsync(items);
            return await _context.SaveChangesAsync();
        }

        public IEnumerable<League> GetAllLeagues()
        {
            return _context.League.Include(x => x.Teams).ToList();
        }
        public League GetLeagueById(int id)
        {
            return _context.League.Include(x => x.Teams).SingleOrDefault(x => x.Id == id);

        }

        public IEnumerable<TableStanding> GetStandings(int leagueId)
        {
            return _context.Standings.Include(t => t.Team).Where(ts => ts.LeagueId == leagueId);
        }

        public IEnumerable<TableStanding> GetStandings()
        {
            return _context.Standings.Include(t => t.Team);
        }

        public async Task<int> RemoveTeamStandingAsync(IEnumerable<TableStanding> items)
        {
            _context.Standings.RemoveRange(items);
            return await _context.SaveChangesAsync();
        }
        public void DeleteLeague(League league)
        {
            _context.Remove(league);
            _context.SaveChanges();
        }
        public void UpdateLeague(League league)
        {
            var _league = GetLeagueById(league.Id);
            _league.Name = league.Name;
            _league.Country = league.Country;
            _context.Update(_league);
            _context.SaveChanges();
                
        }
    }
}
