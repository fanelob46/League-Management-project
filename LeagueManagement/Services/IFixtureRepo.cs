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
        IEnumerable<Fixture> GetAllFixtures();
        IEnumerable<Score> GetAllScores();

        Fixture GetFixtureById(int id);
        Task UpdateFixtureResults(Fixture fixture);

        //Task UpdateFixture(Fixture results);

        void DeleteFixture(Fixture fixture);
    }
}
