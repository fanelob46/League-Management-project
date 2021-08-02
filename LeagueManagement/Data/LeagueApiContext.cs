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


            modelBuilder.Entity<Team>()
                .HasOne<League>(t => t.League)
                .WithMany(l => l.Teams)
                .HasForeignKey(t => t.League_Id)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Player>()
               .HasOne<Team>(p => p.Team)
               .WithMany(t => t.Players)
               .HasForeignKey(p => p.Team_Id)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();



        }

       
        public DbSet<League> League { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Player> Player { get; set; }
    }
}
