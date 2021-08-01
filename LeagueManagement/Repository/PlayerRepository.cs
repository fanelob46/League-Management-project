using LeagueManagement.Data;
using LeagueManagement.Models;
using LeagueManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LeagueManagement.Repository
{
    public class PlayerRepository : IPlayerRepo
    {
        private readonly LeagueApiContext _context;

        public PlayerRepository(LeagueApiContext context)
        {
            _context = context;
        }

        public Player Add(Player player)
        {
            var _player = _context.Add(player);
            _context.SaveChanges();
            player.Id = _player.Entity.Id;

            return player;
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _context.Player.Include(x => x.Team).ToList();
        }

        public IEnumerable<Player> GetPlayerByTeamId(int id)
        {
            return _context.Player.Where(x => x.Team_Id == id).ToList();
        }
        public Player GetPlayerById(int id)
        {
            return _context.Player.Include(x => x.Team).SingleOrDefault(x => x.Id == id);
        }
        public void DeletePlayer(Player player)
        {
            _context.Remove(player);
            _context.SaveChanges();
        }

        public void UpdatePlayer(Player player)
        {
            var _player = GetPlayerById(player.Id);
            _player.Name = player.Name;
            _player.Surname = player.Surname;
            _player.Position = player.Position;
            _player.Age = player.Age;
            _player.Rating = player.Rating;
            _player.Team_Id = player.Team_Id;
            _context.Update(_player);
            _context.SaveChanges();
        }
    }
}
