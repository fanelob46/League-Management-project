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
        IEnumerable<Fixture> GetAllFixtures();

        Fixture GetFixtureById(int id);

        //Task UpdateFixture(Fixture results);

        void DeleteFixture(Fixture fixture);
    }
}
