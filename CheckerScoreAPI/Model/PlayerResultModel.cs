namespace CheckerScoreAPI.Model
{
    public class PlayerResultModel
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int AllMatches { get; set; }
        public int Victories { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastMatch { get; set; }
    }
}