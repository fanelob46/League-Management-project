using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Stadium { get; set; }
        public string Location { get; set; }
        public int League_Id { get; set; }
        public virtual League League { get; set; }
    }
}
