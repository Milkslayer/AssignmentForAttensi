using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Logic
{
    public interface IPlayerService
    {
        Task<Player> GetPlayerByName(string playerName);
        Task<Player> AddNewPlayer(string playerName);
        Task<PlayerScore> AddPlayerScore(PlayerScore playerScore);
        Task<IList<GameScore>>GetPlayerScores(int playerId);

        Task<IList<GameScore>>GetPlayerScores(string playerName);
    }
}