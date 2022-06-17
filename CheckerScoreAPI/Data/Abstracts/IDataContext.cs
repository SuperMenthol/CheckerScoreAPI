using CheckerScoreAPI.Model.Entity;
using MongoDB.Driver;

namespace CheckerScoreAPI.Data.Abstracts
{
    public interface IDataContext
    {
        IMongoCollection<Result> Results { get; }
        IMongoCollection<Player> Players { get; }
    }
}