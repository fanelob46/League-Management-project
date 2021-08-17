using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Helper
{
    public static class TableCalculation
    {
		public static void ApplyFixture(this TeamTableStanding tableStanding, Fixture fixture)
		{
			if (fixture.Team1Score == null || fixture.Team2Score == null)
			{
				throw new Exception("Can't apply match to team table standing. Match result wasn't provided.");
			}

			if (!(fixture.Team1Id == tableStanding.TeamId || fixture.Team2Id == tableStanding.TeamId))
			{
				throw new Exception("Can't apply match to team table standing. Team is not presented in provided match.");
			}

			var isHomeMatch = fixture.Team1Id == tableStanding.TeamId;

			if (fixture.IsDraw())
			{
				tableStanding.Points += Constants.PointsForDraw;
				tableStanding.MatchesDrawn++;
			}
			else
			{
				var isHomeWon = fixture.IsHomeTeamWon();
				if ((isHomeMatch && isHomeWon) || (!isHomeMatch && !isHomeWon))
				{
					tableStanding.Points += Constants.PointsForWin;
					tableStanding.MatchesWon++;
				}
				else
				{
					tableStanding.Points += Constants.PointsForLose;
					tableStanding.MatchesLost++;
				}
			}

			tableStanding.MatchesPlayed++;

			tableStanding.GoalsFor += isHomeMatch ? fixture.Team1Score.Value : fixture.Team2Score.Value;
			tableStanding.GoalsAgainst += isHomeMatch ? fixture.Team2Score.Value : fixture.Team1Score.Value;
		}

		public static bool IsDraw(this Fixture fixture)
		{
			if (fixture.Team1Score.HasValue && fixture.Team2Score.HasValue)
			{
				return fixture.Team1Score == fixture.Team2Score;
			}
			else
			{
				return false;
			}
		}

		public static bool IsHomeTeamWon(this Fixture fixture)
		{
			if (fixture.Team1Score.HasValue && fixture.Team2Score.HasValue)
			{
				return fixture.Team1Score > fixture.Team2Score;
			}
			else
			{
				return false;
			}
		}

		public static bool IsHomeTeamLose(this Fixture fixture)
		{
			if (fixture.Team1Score.HasValue && fixture.Team2Score.HasValue)
			{
				return fixture.Team1Score < fixture.Team2Score;
			}
			else
			{
				return false;
			}
		}
	}
}
