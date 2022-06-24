using Domain.Data.Abstracts;
using Domain.Entity;
using Infrastructure.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Infrastructure.Commands.PlayerCommands
{
    public class RenamePlayerCommand : BaseCommand
    {
        private readonly PlayerModel _playerModel;

        public RenamePlayerCommand(IDataContext dataContext, PlayerModel player) : base(dataContext)
        {
            _dataContext = dataContext;
            _playerModel = player;
        }

        public override async Task<ObjectResult> Execute()
        {
            var upd = new UpdateDefinitionBuilder<Player>();
            upd.Set(x => x.Login, _playerModel.PlayerName);

            await _dataContext.Players.UpdateOneAsync(_filter, upd.Combine());

            return new ObjectResult(BaseResponse.GetResponse<object>(true, Helpers.ResponseMessages.RENAME_PLAYER_SUCCEEDED));
        }

        private FilterDefinition<Player> _filter => Builders<Player>.Filter.Where(x => x.PlayerId == _playerModel.PlayerId);
    }
}
