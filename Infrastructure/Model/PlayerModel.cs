using Domain.Entity;

namespace Infrastructure.Model
{
    public class PlayerModel
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public DateTime CreatedAt { get; set; }

        public PlayerModel()
        {

        }

        public PlayerModel(int id, string playerName, DateTime createdAt)
        {
            PlayerId = id;
            PlayerName = playerName;
            CreatedAt = createdAt;
        }

        public PlayerModel(Player entity)
        {
            PlayerId = entity.PlayerId;
            PlayerName = entity.Login;
            CreatedAt = entity.CreationDate;
        }

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