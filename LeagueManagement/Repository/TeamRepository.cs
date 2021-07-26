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
    public class TeamRepository: ITeamRepo
    {
        private readonly LeagueApiContext _context;

        public TeamRepository(LeagueApiContext context)
        {
            _context = context;
        }

        public Team Add(Team team)
        {
            var _team = _context.Add(team);
            _context.SaveChanges();
            team.Id = _team.Entity.Id;

            return team;
        }
        public IEnumerable<Team> GetAllTeams()
        {
            return _context.Team.ToList();
        }
        public Team GetTeamById(int id)
        {
            return _context.Team.Include(x => x.League).SingleOrDefault(x => x.Id == id);

        }
        public IEnumerable<Team> GetAllTeamsByLeagueId(int id)
        {
            return _context.Team.Where(x => x.League_Id == id).ToList();
        }
        public void DeleteTeam(Team team)
        {
            _context.Remove(team);
            _context.SaveChanges();
        }
        public void UpdateTeam(Team team)
        {
            var _team = GetTeamById(team.Id);
            _team.Name = team.Name;
            _team.Stadium = team.Stadium;
            _team.Location = team.Location;
            _team.League_Id = team.League_Id;
            _context.Update(_team);
            _context.SaveChanges();
            
        }
    }
}
