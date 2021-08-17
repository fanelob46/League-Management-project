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

     public async Task<Score> AddScore(Score score)
        {
            var _Score = _context.Add(score);
            _context.SaveChanges();
            score.Id = _Score.Entity.Id;

            var _fixture = GetFixtureById(score.Fixture_Id);
            var _scores = GetAllScores();

            var players = GetPlayers(_fixture.Team1Id);
            var awayPlayers = GetPlayers(_fixture.Team2Id);

            var homeTeamScores = 0;
            var awayTeamScores = 0;

            foreach (Score s in _scores)
            {
                homeTeamScores = _fixture.Scores.Where(s => s.Team1Id == s.player.Team_Id).Count();
                awayTeamScores = _fixture.Scores.Where(s => s.Team2Id == s.player.Team_Id).Count();
            }


            _fixture.Team1Score = (byte?)homeTeamScores;
            _fixture.Team2Score = (byte?)awayTeamScores;

            await addFixtureScore(_fixture);


            //await RecalculateLeagueStandingsAsync(score.FixtureId);

            return score;
        }
        public IEnumerable<Score> GetAllScores()
        {
            return _context.Scores.ToList();
        }
        public List<Player> GetPlayers(int teamId)
        {
            return _context.Player.Where(p => p.Team_Id == teamId).ToList();
        }
        private async Task addFixtureScore(Fixture fixture)
        {
            _context.Update(fixture);
            _context.SaveChangesAsync();
        }
        public async Task UpdateFixtureResults(Fixture results)
        {
            try
            {
                var item = await _context.Fixture.Include(f => f.Scores).FirstOrDefaultAsync(f => f.Id == results.Id);

                if (item == null)
                {
                    throw new Exception($"Fixture with id={results.Id} doesn't exist");
                }

                var homeTeamScores = results.Scores.Where(s => s.Team1Id == item.Team1Id).Count();
                var awayTeamScores = results.Scores.Where(s => s.Team2Id == item.Team2Id).Count();

                if (results.Team1Score != homeTeamScores || results.Team2Score != awayTeamScores)
                {
                    throw new Exception($"Provided scores mismatch fixture results.");
                }

                // Update results
                item.Team1Score = results.Team1Score;
                item.Team2Score = results.Team2Score;


                // Remove existed score items that are not presented in updated results
                item.Scores.RemoveAll(oldScore => !results.Scores.Any(s => s.Id == oldScore.Id));

                // Add new scores
                var newScores = results.Scores.Where(s => s.Id == null);

                foreach (var score in newScores)
                {
                    item.Scores.Add(score);
                }

                _context.Update(item);

               // await RecalculateLeagueStandingsAsync(item.LeagueId);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
