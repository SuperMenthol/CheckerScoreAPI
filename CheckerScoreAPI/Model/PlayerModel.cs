using CheckerScoreAPI.Model.Entity;

namespace CheckerScoreAPI.Model
{
    public class PlayerModel
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }

        public Player ToEntity()
        {
            return new Player()
            {
                PlayerId = PlayerId,
                Login = PlayerName,
                CreationDate = DateTime.Now
            };
        }
    }
}