using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Models
{
    public class Score
    {
        public int Id { get; set; }
        public int Fixture_Id { get; set; }
        public Fixture Fixture { get; set; }
        public int Team1Id { get; set; }
        public Team Team1 { get; set; }
        public int Team2Id { get; set; }
        public Team Team2 { get; set; }
        public int Player_Id { get; set; }
        public Player player { get; set; }
        public byte Minute { get; set; }
    }
}
