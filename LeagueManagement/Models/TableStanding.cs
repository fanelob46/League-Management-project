using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Models
{
    public class TableStanding
    {
        public int Id { get; set; }

        public int? LeagueId { get; set; }
        public League League { get; set; }

        public int? TeamId { get; set; }
        public Team Team { get; set; }

        public byte Position { get; set; }

        public byte MatchesPlayed { get; set; }
        public byte MatchesWon { get; set; }
        public byte MatchesDrawn { get; set; }
        public byte MatchesLost { get; set; }

        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }

        public int Points { get; set; }

        public int GoalsDifference => GoalsFor - GoalsAgainst;
    }
}
