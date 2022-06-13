using CheckerScoreAPI.Model.Entity;

namespace CheckerScoreAPI.Model
{
    public class MatchResult
    {
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
        public int WinnerID { get; set; }
        public DateTime MatchTime { get; set; }

        public Result ToEntity()
        {
            return new Result()
            {
                PlayerOneId = Player1Id,
                PlayerTwoId = Player2Id,
                WinnerId = WinnerID,
                PlayedAt = MatchTime
            };
        }
    }
}