namespace WebAPI.Models
{
    public class PlayerSummrayResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RankingScore { get; set; }
        public int TotalScore { get; set; }
        public int TotalPlayDurationSeconds { get; set; }
    }
}