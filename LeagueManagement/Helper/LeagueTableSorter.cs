using LeagueManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Helper
{
    public static class LeagueTableSorter
    {
		public static void CalculateTeamPositions(List<TableStanding> tableRows)
		{
			tableRows.Sort(TablePositionComparer);
			tableRows.Reverse();

			for (byte i = 0; i < tableRows.Count; i++)
			{
				tableRows[i].Position = (byte)(i + 1);
			}
		}

		private static int TablePositionComparer(TableStanding team1, TableStanding team2)
		{
			if (team1.Points > team2.Points) return 1;
			if (team1.Points < team2.Points) return -1;

			var team1GD = team1.GoalsDifference;
			var team2GD = team2.GoalsDifference;

			if (team1GD > team2GD) return 1;
			if (team1GD < team2GD) return -1;

			if (team1.GoalsFor > team2.GoalsFor) return 1;
			if (team1.GoalsFor < team2.GoalsFor) return -1;

			return 0;
		}
	}
}
