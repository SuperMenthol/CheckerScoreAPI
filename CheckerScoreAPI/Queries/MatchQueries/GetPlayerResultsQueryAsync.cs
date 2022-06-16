using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model;
using CheckerScoreAPI.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CheckerScoreAPI.Queries.MatchQueries
{
    public class GetPlayerResultsQueryAsync : BaseAsyncQuery
    {
        private readonly int _playerId;
        private FilterDefinition<Result> _filter => Builders<Result>.Filter
            .Where(y => y.PlayerOneId == _playerId
                || _playerId == y.PlayerTwoId);

        public GetPlayerResultsQueryAsync(IDataContext dataContext, int playerId) : base(dataContext)
        {
            _playerId = playerId;
            _dataContext = dataContext;
        }

        public override async Task<ObjectResult> Get()
        {
            var playerResults = await _dataContext.Results().FindAsync(_filter).Result.ToListAsync();

            var dtoList = playerResults.OrderByDescending(x => x.PlayedAt).Select(x => new MatchResult()
            {
                MatchTime = x.PlayedAt,
                Player1Id = x.PlayerOneId,
                Player2Id = x.PlayerTwoId,
                WinnerID = x.WinnerId
            }).ToList();

            return new ObjectResult(dtoList);
        }
    }
}
