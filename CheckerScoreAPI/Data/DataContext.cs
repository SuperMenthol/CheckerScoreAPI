using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model.Entity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CheckerScoreAPI.Data
{
    public class DataContext : IDataContext
    {
        public IMongoCollection<Player> Players() => _players;
        public IMongoCollection<Result> Results() => _results;

        private readonly IMongoCollection<Player> _players;
        private readonly IMongoCollection<Result> _results;

        private IMongoDatabase _database;
        private MongoClient _client;

        public DataContext(IOptions<CheckerScoreDatabaseSettings> dbSettings)
        {
            _client = new MongoClient(dbSettings.Value.ConnectionString);
            _database = _client.GetDatabase(dbSettings.Value.DatabaseName);

            _players = _database.GetCollection<Player>(dbSettings.Value.PlayerDataName);
            _results = _database.GetCollection<Result>(dbSettings.Value.MatchResultsName);
        }

        public void AddPlayer(Player player) => Players().InsertOne(player);
        public void UpdatePlayer(Player player) => Players().ReplaceOne(x => x.PlayerId == player.PlayerId, player);
        public void RemovePlayer(Player player) => Players().DeleteOne(x => x.PlayerId == player.PlayerId);
        public void AddMatchResult(Result result) => Results().InsertOne(result);
    }
}
