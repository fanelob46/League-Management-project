using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Models
{
    public class Fixture
    {
        public int Id { get; set; }
        public DateTime  Date { get; set; }
        public int League_Id { get; set; }
        public League League { get; set; }
        public int Team1Id { get; set; }
        public Team Team1 { get; set; }
        public byte? Team1Score { get; set; }
        public int Team2Id { get; set; }
        public Team Team2 { get; set; }
        public byte? Team2Score { get; set; }

        public List<Score> Scores { get; set; }
    }
}
