using Domain.Data.Abstracts;
using Infrastructure.Model;
using Domain.Entity;
using Infrastructure.Queries.PlayerQueries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Queries.MatchQueries
{
    public class GetPlayerResultCardQueryAsync : BaseAsyncQuery
    {
        private readonly int _playerId;
        private FilterDefinition<Result> _filter => Builders<Result>.Filter
            .Where(y => y.PlayerOneId == _playerId
                || _playerId == y.PlayerTwoId);

        public GetPlayerResultCardQueryAsync(IDataContext dataContext, int playerId) : base(dataContext)
        {
            _playerId = playerId;
            _dataContext = dataContext;
        }

        public override async Task<ObjectResult> Get()
        {
            var playerInfo = (Player)new GetPlayerByIdQuery(_dataContext, _playerId).Get().Value;

            if (playerInfo == null)
            {
                return new ObjectResult(new PlayerModel()) { StatusCode = StatusCodes.Status417ExpectationFailed };
            }

            var resultModel = new PlayerResultModel()
            {
                PlayerId = _playerId,
                PlayerName = playerInfo.Login,
                CreatedAt = playerInfo.CreationDate
            };

            var allMatches = await _dataContext.Results.FindAsync(_filter).Result.ToListAsync();

            if (allMatches.Any() is false)
            {
                return new ObjectResult(resultModel);
            }

            resultModel.AllMatches = allMatches.Count;
            resultModel.Victories = allMatches.Where(x => x.WinnerId == _playerId).Count();
            resultModel.Draws = allMatches.Where(x => x.WinnerId == 0).Count();
            resultModel.Losses = resultModel.AllMatches - (resultModel.Victories + resultModel.Draws);
            resultModel.LastMatch = allMatches.OrderByDescending(x => x.PlayedAt).First()?.PlayedAt;

            return new ObjectResult(resultModel);
        }
    }
}