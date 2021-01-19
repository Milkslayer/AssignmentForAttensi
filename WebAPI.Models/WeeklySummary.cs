using System.Collections.Generic;

namespace WebAPI.Models
{
    public class WeeklySummary
    {
        public int WeekNumber { get; set; }
        
        public List<PlayerSummrayResult> TopPlayers { get; set; }
    }
}