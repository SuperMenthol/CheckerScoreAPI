using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model;
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
        public ActionResult GetPlayerInfo(int playerId)
        {
            try
            {
                if (playerId == 0)
                {
                    return new JsonResult(new BaseResponse(false, Helpers.ResponseMessages.PLAYER_ID_INVALID));
                }

                return new JsonResult(_dataContext.GetPlayerInformation(playerId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new JsonResult(new BaseResponse(false, ex.ToString()));
            }
        }

        [HttpPost("create")]
        public ActionResult CreatePlayer(string playerName)
        {
            try
            {
                var nameAvailable = IsPlayerNameAvailable(playerName);
                if (!nameAvailable)
                {
                    return new JsonResult(new BaseResponse(false, Helpers.ResponseMessages.PLAYER_NAME_TAKEN));
                }
                var nameValidationResult = Helpers.Validators.IsPlayerNameValid(playerName);
                if (nameValidationResult.Success is false)
                {
                    return new JsonResult(nameValidationResult);
                }

                var playerDto = new PlayerModel()
                {
                    PlayerId = _dataContext.GetLastPlayerID() + 1,
                    PlayerName = playerName
                };

                _dataContext.AddPlayer(playerDto.ToEntity());
                return new JsonResult(new BaseResponse(true, Helpers.ResponseMessages.INSERT_PLAYER_SUCCEEDED));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new JsonResult(new BaseResponse(false, Helpers.ResponseMessages.INSERT_PLAYER_FAILED));
            }
        }

        [HttpPut("rename")]
        public ActionResult RenamePlayer(PlayerModel player)
        {
            if (player.PlayerId == 0 || !DoesPlayerIDExist(player.PlayerId))
            {
                return new JsonResult(new BaseResponse(false, Helpers.ResponseMessages.PLAYER_ID_INVALID));
            }

            var validateName = ValidatePlayerName(player.PlayerName);
            if (validateName.Success is false)
            {
                return new JsonResult(validateName);
            }

            return new JsonResult(new object());
        }

        [HttpDelete("remove")]
        public ActionResult RemovePlayer(int playerId)
        {
            if (DoesPlayerIDExist(playerId) is false)
            {
                return new JsonResult(new BaseResponse(false, Helpers.ResponseMessages.PLAYER_ID_INVALID));
            }

            return new JsonResult(new object());
        }

        private BaseResponse ValidatePlayerName(string name)
        {
            var nameAvailable = IsPlayerNameAvailable(name);

            if (nameAvailable is false)
            {
                return new BaseResponse(false, Helpers.ResponseMessages.PLAYER_NAME_TAKEN);
            }

            var nameValidationResult = Helpers.Validators.IsPlayerNameValid(name);
            if (nameValidationResult.Success is false || nameAvailable is false)
            {
                return nameValidationResult;
            }

            return new BaseResponse(true, Helpers.ResponseMessages.PLAYER_NAME_SUCCESS_MESSAGE);
        }

        private bool IsPlayerNameAvailable(string playerName)
        {
            return true;
        }

        private bool DoesPlayerIDExist(int playerId)
        {
            return true;
        }
    }
}