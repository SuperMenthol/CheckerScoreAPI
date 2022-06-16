using CheckerScoreAPI.Model.Entity;
using MongoDB.Driver;

namespace CheckerScoreAPI.Data.Abstracts
{
    public interface IDataContext
    {
        IMongoCollection<Result> Results();
        IMongoCollection<Player> Players();
        void AddPlayer(Player player);
        void UpdatePlayer(Player player);
        void RemovePlayer(Player player);
        void AddMatchResult(Result result);
    }
}