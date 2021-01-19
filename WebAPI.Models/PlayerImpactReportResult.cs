using System;

namespace WebAPI.Models
{
    public class PlayerImpactReportResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FirstScore { get; set; }
        public int BestScore { get; set; }
        public int Playthroughs { get; set; }
        public int TotalPlayDurationSeconds { get; set; }
        public string TotalPlayDurationString => PlayDurationToString();

        public string PlayDurationToString()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(TotalPlayDurationSeconds);
            return timeSpan.ToString(@"hh\:mm\:ss") + " HH:MM:SS";
        }
    }
}