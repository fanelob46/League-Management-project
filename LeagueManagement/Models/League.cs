using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Models
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public List<TeamTableStanding> Standings { get; set; }
        public List<Fixture> Fixtures { get; set; }


    }
}
