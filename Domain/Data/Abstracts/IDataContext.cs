using Domain.Entity;
using MongoDB.Driver;

namespace Domain.Data.Abstracts
{
    public interface IDataContext
    {
        IMongoCollection<Result> Results { get; }
        IMongoCollection<Player> Players { get; }
    }
}