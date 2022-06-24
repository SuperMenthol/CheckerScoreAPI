using Domain.Data.Abstracts;
using Infrastructure.Model;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Infrastructure.Queries.PlayerQueries
{
    public class GetPlayerByIdQuery : BaseSyncQuery
    {
        private readonly int _playerId;
        private FilterDefinition<Player> _filter => Builders<Player>.Filter.Where(x => x.PlayerId == _playerId);

        public GetPlayerByIdQuery(IDataContext dataContext, int playerId) : base(dataContext)
        {
            _dataContext = dataContext;
            _playerId = playerId;
        }

        public override ObjectResult Get()
        {
            var player = _dataContext.Players.Find(_filter).SortBy(x => x.CreationDate).FirstOrDefault();
            if (player != null)
            {
                return new ObjectResult(new PlayerModel(player));
            }

            return new ObjectResult(new PlayerModel());
        }
    }
}