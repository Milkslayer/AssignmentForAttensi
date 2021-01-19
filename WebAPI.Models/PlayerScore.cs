using System.Collections.Generic;

namespace WebAPI.Models
{
    public class PlayerScore
    {
        public int PlayerId { get; set; }
        public List<GameScore> Scores { get; set; }

        public PlayerScore()
        {
            Scores = new List<GameScore>();
        }
    }
}