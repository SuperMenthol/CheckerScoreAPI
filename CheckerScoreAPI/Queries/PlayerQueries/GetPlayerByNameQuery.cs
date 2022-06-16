using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model;
using CheckerScoreAPI.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CheckerScoreAPI.Queries.PlayerQueries
{
    public class GetPlayerByNameQuery : BaseSyncQuery
    {
        private readonly string _playerName;
        private FilterDefinition<Player> _filter => Builders<Player>.Filter.Where(x => x.Login == _playerName);

        public GetPlayerByNameQuery(IDataContext dataContext, string playerName) : base(dataContext)
        {
            _dataContext = dataContext;
            _playerName = playerName;
        }

        public override ObjectResult Get()
        {
            var player = _dataContext.Players().FindSync(_filter).FirstOrDefault();
            if (player != null)
            {
                return new ObjectResult(new PlayerModel(player));
            }

            return new ObjectResult(null);
        }
    }
}