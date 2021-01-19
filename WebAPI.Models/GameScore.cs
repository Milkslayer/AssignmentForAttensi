using System;

namespace WebAPI.Models
{
    public class GameScore
    {
        public int Score { get; set; }
        public DateTime SessionStart { get; set; }
        public DateTime SessionEnd { get; set; }
    }
}