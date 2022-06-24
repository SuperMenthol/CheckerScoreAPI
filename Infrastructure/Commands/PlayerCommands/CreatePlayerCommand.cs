using Domain.Data.Abstracts;
using Infrastructure.Model;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Commands.PlayerCommands
{
    public class CreatePlayerCommand : BaseCommand
    {
        private readonly PlayerModel _model;

        public CreatePlayerCommand(IDataContext dataContext, PlayerModel playerDto) : base(dataContext)
        {
            _dataContext = dataContext;
            _model = playerDto;
        }

        public override async Task<ObjectResult> Execute()
        {
            var player = _model.ToEntity();

            await _dataContext.Players.InsertOneAsync(player);

            return new ObjectResult(BaseResponse.GetResponse<object>(true, Helpers.ResponseMessages.CREATE_PLAYER_SUCCEEDED));
        }
    }
}
