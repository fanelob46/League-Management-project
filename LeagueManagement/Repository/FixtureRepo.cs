using LeagueManagement.Data;
using LeagueManagement.Models;
using LeagueManagement.Services;
using LeagueManagement.Helper;
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

            await RecalculateLeagueStandingsAsync(fixture.Id);

            return fixture;
        }

        public async Task<Score> AddScore(Score score)
        {
            var _score = _context.Add(score);
            _context.SaveChanges();
            score.Id = _score.Entity.Id;

            var _fixture = GetFixtureById(score.Fixture_Id);
            var _scores = GetScores();

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


            await RecalculateLeagueStandingsAsync(score.Fixture_Id);

            return score;
        }

        private async Task addFixtureScore(Fixture fixture)
        {
            _context.Update(fixture);
            _context.SaveChangesAsync();
        }

        public void DeleteFixture(Fixture results)
        {
            _context.Remove(results);
            _context.SaveChanges();
        }

        private async Task<League> GetAndValidateItem(int leagueId)
        {
            var league = await _context.League.SingleOrDefaultAsync(l => l.Id == leagueId);

            if (league == null)
            {
                throw new Exception($"League with id={leagueId} doesn't exist");
            }

            return league;
        }

        public async Task RecalculateLeagueStandingsAsync(int leagueId)
        {

            var league = new LeagueRepo(_context).GetLeagueById(leagueId);

            var teams = new TeamRepository(_context).GetAllTeams().Where(t => t.League_Id == leagueId)
                .Select(t => t.Id);

            var currentStandings = new List<TableStanding>();

            foreach (var teamId in teams)
            {
                var playedMatches = new FixtureRepo(_context).GetAllFixtures()
                    .Where(m => m.League_Id == league.Id
                        && (m.Team1Id == teamId || m.Team2Id == teamId)
                        && m.Date < DateTime.Now);

                var tableStanding = new TableStanding()
                {
                    LeagueId = leagueId,
                    TeamId = teamId
                };

                foreach (var match in playedMatches)
                {
                    tableStanding.ApplyFixture(match);
                }
                currentStandings.Add(tableStanding);
            }

            LeagueTableSorter.CalculateTeamPositions(currentStandings);



            var oldStandings = new LeagueRepo(_context).GetStandings(leagueId)
              .Where(t => t.LeagueId == leagueId);

            new LeagueRepo(_context).RemoveTeamStandingAsync(oldStandings);
            System.Threading.Thread.Sleep(10000);
            new LeagueRepo(_context).AddTeamStandingAsync(currentStandings);

        }

        public IEnumerable<Fixture> GetAllFixtures()
        {
            return _context.Fixture.Include(x => x.Team1).Include(x => x.Team2).Include(x => x.Scores).Include(x => x.League).ToList();
        }


        public IEnumerable<Score> GetScores()
        {
            return _context.Scores.ToList();
        }

        public Fixture GetFixtureById(int id)
        {
            return _context.Fixture.Include(x => x.Scores).Include(x => x.Team1).Include(x => x.Team2).Include(x => x.League).SingleOrDefault(x => x.Id == id);
        }

        public List<Player> GetPlayers(int teamId)
        {
            return _context.Player.Where(p => p.Team_Id == teamId).ToList();
        }
        public async Task UpdateFixture(Fixture fixture)
        {
            var _fixture = GetFixtureById(fixture.Id);
            // _fixture.Date = fixture.Date;
            _fixture.Team1Score = fixture.Team1Score;
            _fixture.Team2Score = fixture.Team2Score;
            _context.Update(_fixture);
            _context.SaveChanges();

            await RecalculateLeagueStandingsAsync(_fixture.League_Id);
        }

        public async Task DeleteStandings(int LeagueId, List<TableStanding> currentStandings)
        {
            var standings = _context.Standings.Where(x => x.LeagueId == LeagueId);
            _context.RemoveRange(standings);
            await _context.SaveChangesAsync();


            await new LeagueRepo(_context).AddTeamStandingAsync(currentStandings);
        }

        public IEnumerable<Fixture> GetAllFixturesByLeagueId(int id)
        {
            return _context.Fixture.Include(x => x.Team1).Include(x => x.Team2).ThenInclude(x => x.League).Where(l => l.League_Id == id).ToList();
        }

        public IEnumerable<Fixture> GetAllFixturesByTeamId(int id)
        {
            return _context.Fixture.Include(x => x.Team1).Include(x => x.Team2).ThenInclude(x => x.League).Where(t => t.Team1Id == id || t.Team2Id == id).ToList();
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

                await RecalculateLeagueStandingsAsync(item.League_Id);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
