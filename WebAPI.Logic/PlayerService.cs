using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.DataAccess;
using WebAPI.Models;

namespace WebAPI.Logic
{
    public class PlayerService : IPlayerService
    {
        public IDataAccess _dataAccess;
        public PlayerService(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<Player> GetPlayerByName(string playerName)
        {
            return await _dataAccess.FindPlayerByName(playerName);
        }

        public async Task<Player> AddNewPlayer(string playerName)
        {
            int result = await _dataAccess.SavePlayer(playerName);
            if (result > 0)
            {
                return new Player()
                {
                    Id = result,
                    Name = playerName
                };
            }

            return null;
        }

        public async Task<PlayerScore> AddPlayerScore(PlayerScore playerScore)
        {
            var result = await _dataAccess.SavePlayerScore(playerScore);
            if (result) 
                return playerScore;
            return null;
        }

        public async Task<IList<GameScore>> GetPlayerScores(string playerName)
        {
            var result = await _dataAccess.GetPlayerScores(playerName);
            return result;
        }
        
        public async Task<IList<GameScore>> GetPlayerScores(int playerId)
        {
            var result = await _dataAccess.GetPlayerScores(playerId);
            return result;
        }
    }
}