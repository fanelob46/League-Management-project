using LeagueManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Data
{
    public class LeagueApiContext : DbContext
    {
        public LeagueApiContext(DbContextOptions options) : base(options) 

        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<League>();
                
        }
        public DbSet<League> League { get; set; }
    }
}
