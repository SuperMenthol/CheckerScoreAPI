using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CheckerScoreAPI.Queries.PlayerQueries
{
    public class GetPlayerByNameQuery : BaseSyncQuery
    {
        private readonly string _playerName;
        private FilterDefinition<Model.Entity.Player> _filter => Builders<Model.Entity.Player>.Filter.Where(x => x.Login == _playerName);

        public GetPlayerByNameQuery(IDataContext dataContext, string playerName) : base(dataContext)
        {
            _dataContext = dataContext;
            _playerName = playerName;
        }

        public override ObjectResult Get()
        {
            var player = _dataContext.Players.FindSync(_filter).FirstOrDefault();
            if (player != null)
            {
                return new ObjectResult(new PlayerModel(player));
            }

            return new ObjectResult(null);
        }
    }
}