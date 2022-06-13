using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model;
using CheckerScoreAPI.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CheckerScoreAPI.Data
{
    public class DataContext : IDataContext
    {
        private readonly IMongoCollection<Player> Players;
        private readonly IMongoCollection<Result> Results;

        private IMongoDatabase _database;
        private MongoClient _client;

        public DataContext(IOptions<CheckerScoreDatabaseSettings> dbSettings)
        {
            _client = new MongoClient(dbSettings.Value.ConnectionString);
            _database = _client.GetDatabase(dbSettings.Value.DatabaseName);

            Players = _database.GetCollection<Player>(dbSettings.Value.PlayerDataName);
            Results = _database.GetCollection<Result>(dbSettings.Value.MatchResultsName);
        }

        public async Task<ActionResult> GetAllAsync<T>()
        {
            if (typeof(T) == typeof(Player))
            {
                return new JsonResult(new object()) { Value = await Players.AsQueryable().ToListAsync() };
            }
            else if (typeof(T) == typeof(Result))
            {
                return new JsonResult(new object()) { Value = await Results.AsQueryable().ToListAsync() };
            }

            throw new ArgumentException("This type has no collection");
        }

        public Player GetPlayerByName(string name)
        {
            return Players.Find(x => x.Login == name).FirstOrDefault();
        }

        public async Task<List<Result>> GetMatchesOfPlayer(int playerId)
        {
            var queryResult = await Results.FindAsync(x => x.PlayerOneId == playerId || x.PlayerTwoId == playerId);

            return queryResult.ToList();
        }

        public async Task<PlayerResultModel> GetPlayerInformation(int playerId)
        {
            var playerRecord = GetPlayerByID(playerId);
            var matches = await GetMatchesOfPlayer(playerId);

            var resultModel = new PlayerResultModel() 
            { 
                PlayerId = playerId,
                PlayerName = playerRecord.Login,
                CreatedAt = playerRecord.CreationDate,
                AllMatches = matches.Count,
                Victories = matches.Where(x => x.WinnerId == playerId).Count(),
                LastMatch = matches.OrderByDescending(x => x.PlayedAt).FirstOrDefault()?.PlayedAt
            };

            return resultModel;
        }

        public Player GetPlayerByID(int id) => Players.Find(x => x.PlayerId == id).SortBy(x => x.CreationDate).FirstOrDefault();
        public void AddPlayer(Player player) => Players.InsertOne(player);
        public void UpdatePlayer(Player player) => Players.ReplaceOne(x => x.PlayerId == player.PlayerId, player);
        public void RemovePlayer(Player player) => Players.DeleteOne(x => x.PlayerId == player.PlayerId);

        public int GetLastPlayerID() => (int)Players.CountDocuments(x => x.PlayerId > 0);

        public Result GetMatchResultByID(int id) => Results.Find(x => x.Id == id.ToString()).FirstOrDefault();
        public void AddMatchResult(Result result) => Results.InsertOne(result);
    }
}
