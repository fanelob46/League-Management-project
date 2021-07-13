using LeagueManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Services
{
    public interface ILeagueRepo
    {
        League Add(League league);
        IEnumerable<League> GetAllLeagues();
        League GetLeagueById(int Id);
        void DeleteLeague(League league);
        void UpdateLeague(League league);
    }
}
