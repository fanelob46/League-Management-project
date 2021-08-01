using LeagueManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueManagement.Services
{
    public interface IPlayerRepo
    {
        Player Add(Player player);
        IEnumerable<Player> GetAllPlayers();
        IEnumerable<Player> GetPlayerByTeamId(int id);
        Player GetPlayerById(int id);
        void DeletePlayer(Player player);
        void UpdatePlayer(Player player);

    }
}
