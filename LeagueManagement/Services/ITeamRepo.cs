using LeagueManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Services
{
    public interface ITeamRepo
    {
        Team Add(Team team);
        IEnumerable<Team> GetAllTeams();
        Team GetTeamById(int id);
        IEnumerable<Team> GetAllTeamsByLeagueId(int id);
        void DeleteTeam(Team team);
        void UpdateTeam(Team team);
    }
}
