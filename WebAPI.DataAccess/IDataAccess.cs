using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.DataAccess
{
    public interface IDataAccess
    {
        Task<Player> FindPlayerByName(string playerName);
        Task<Player> FindPlayerById(int id);
        Task<int> SavePlayer(string playerName);
        Task<bool> SavePlayerScore(PlayerScore playerScore);
        Task<IList<GameScore>> GetPlayerScores(string playerName);
        Task<IList<GameScore>> GetPlayerScores(int playerId);
        Task<IList<PlayerSummrayResult>> GetTop10PlayersByScoreAndDuration(DateTime dateStart, DateTime dateEnd);
        Task<IList<PlayerImpactReportResult>> GetImpactReport();
    }
}