using CheckerScoreAPI.Commands.PlayerCommands;
using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model;
using CheckerScoreAPI.Queries.PlayerQueries;
using Microsoft.AspNetCore.Mvc;

namespace CheckerScoreAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IDataContext _dataContext;
        private readonly ILogger<PlayerController> _logger;

        public PlayerController(ILogger<PlayerController> logger, IDataContext dataContext)
        {
            _logger = logger;
            _dataContext=dataContext;
        }

        [HttpGet("getInfo")]
        public BaseResponse<PlayerModel> GetPlayerInfo(int playerId)
        {
            try
            {
                if (playerId == 0)
                {
                    return BaseResponse.GetResponse<PlayerModel>(false, Helpers.ResponseMessages.PLAYER_ID_INVALID, new());
                }

                var result = new GetPlayerByIdQuery(_dataContext, playerId).Get();
                return BaseResponse.GetResponse<PlayerModel>(true, Helpers.ResponseMessages.PLAYER_INFO_SUCCESS, (PlayerModel)result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BaseResponse.GetResponse<PlayerModel>(false, Helpers.ResponseMessages.PLAYER_INFO_FAILURE, new PlayerModel());
            }
        }

        [HttpGet("login")]
        public BaseResponse<object> Login(string playerName)
        {
            try
            {
                var result = new GetPlayerByNameQuery(_dataContext, playerName).Get();

                return BaseResponse.GetResponse<object>(result.Value != null, Helpers.ResponseMessages.LOGIN_SUCCEEDED, new());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BaseResponse.GetResponse<object>(false, Helpers.ResponseMessages.LOGIN_FAILED, new());
            }
        }

        [HttpPost("create")]
        public async Task<BaseResponse<object>> CreatePlayer(string playerName)
        {
            try
            {
                if (IsPlayerNameAvailable(playerName) is false)
                {
                    return BaseResponse.GetResponse<object>(false, Helpers.ResponseMessages.PLAYER_NAME_TAKEN);
                }
                var nameValidationResult = Helpers.Validators.IsPlayerNameValid(playerName);
                if (nameValidationResult.Success is false)
                {
                    return nameValidationResult;
                }

                int nextPlayerId = (int)new GetNextPlayerIDQuery(_dataContext).Get().Value;

                var result = await new CreatePlayerCommand(_dataContext, new PlayerModel(nextPlayerId, playerName, DateTime.Now)).Execute();

                return (BaseResponse<object>)result.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BaseResponse.GetResponse<object>(false, Helpers.ResponseMessages.CREATE_PLAYER_FAILED);
            }
        }

        [HttpPut("rename")]
        public async Task<BaseResponse<object>> RenamePlayer([FromBody] PlayerModel player)
        {
            try
            {
                if (player.PlayerId == 0 || DoesPlayerIDExist(player.PlayerId) is false)
                {
                    return BaseResponse.GetResponse<object>(false, Helpers.ResponseMessages.PLAYER_ID_INVALID);
                }

                if (IsPlayerNameAvailable(player.PlayerName) is false)
                {
                    return BaseResponse.GetResponse<object>(false, Helpers.ResponseMessages.PLAYER_NAME_TAKEN);
                }

                var validateName = ValidatePlayerName(player.PlayerName);
                if (validateName.Success is false)
                {
                    return validateName;
                }

                var result = await new RenamePlayerCommand(_dataContext, player).Execute();

                return BaseResponse.GetResponse<object>(true, Helpers.ResponseMessages.RENAME_PLAYER_SUCCEEDED, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BaseResponse.GetResponse<object>(false, Helpers.ResponseMessages.PLAYER_RENAME_FAILURE, false);
            }
        }

        //[HttpDelete("remove")]
        //public ObjectResult RemovePlayer(int playerId)
        //{
        //    if (DoesPlayerIDExist(playerId) is false)
        //    {
        //        return new ObjectResult(new BaseResponse(false, Helpers.ResponseMessages.PLAYER_ID_INVALID));
        //    }

        //    return new ObjectResult(new object());
        //}

        private BaseResponse<object> ValidatePlayerName(string name)
        {
            var nameAvailable = IsPlayerNameAvailable(name);

            if (nameAvailable is false)
            {
                return BaseResponse.GetResponse<object>(true, Helpers.ResponseMessages.PLAYER_NAME_TAKEN, false);
            }

            var nameValidationResult = Helpers.Validators.IsPlayerNameValid(name);
            if (nameValidationResult.Success is false || nameAvailable is false)
            {
                return nameValidationResult;
            }

            return BaseResponse.GetResponse<object>(true, Helpers.ResponseMessages.PLAYER_NAME_SUCCESS_MESSAGE, true);
        }

        private bool IsPlayerNameAvailable(string playerName) => new GetPlayerByNameQuery(_dataContext, playerName).Get().StatusCode == StatusCodes.Status417ExpectationFailed;

        private bool DoesPlayerIDExist(int playerId) => new GetPlayerByIdQuery(_dataContext, playerId).Get().Value != null;
    }
}