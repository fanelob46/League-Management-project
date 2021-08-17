using LeagueManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Services
{
   public interface IFixtureRepo
    {
        Task<Fixture> Add(Fixture fixture);
        Task<Score> AddScore(Score score);

        //Task<Venue> AddVenue(Venue venue);
        IEnumerable<Fixture> GetAllFixtures();

        IEnumerable<Score> GetScores();

        IEnumerable<Fixture> GetAllFixturesByLeagueId(int id);

        IEnumerable<Fixture> GetAllFixturesByTeamId(int id);

       // IEnumerable<Fixture> GetAllFixtureByVenueId(int id);

        Fixture GetFixtureById(int id);

        //Task UpdateFixture(Fixture results);

        Task UpdateFixtureResults(Fixture fixture);

        void DeleteFixture(Fixture fixture);

        Task DeleteStandings(int LeagueId, List<TableStanding> currentStandings);
    }
}
