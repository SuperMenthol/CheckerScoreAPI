﻿using CheckerScoreAPI.Commands.PlayerCommands;
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
        public ObjectResult GetPlayerInfo(int playerId)
        {
            try
            {
                if (playerId == 0)
                {
                    return new ObjectResult(new BaseResponse(false, Helpers.ResponseMessages.PLAYER_ID_INVALID));
                }

                return new GetPlayerByIdQuery(_dataContext, playerId).Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ObjectResult(new BaseResponse(false, Helpers.ResponseMessages.PLAYER_INFO_FAILURE))
                    {
                    StatusCode = StatusCodes.Status500InternalServerError
                    };
            }
        }

        [HttpGet("login")]
        public async Task<ObjectResult> Login(string playerName)
        {
            try
            {
                return new GetPlayerByNameQuery(_dataContext, playerName).Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ObjectResult(new BaseResponse(false, Helpers.ResponseMessages.LOGIN_FAILED));
            }
        }

        [HttpPost("create")]
        public async Task<ObjectResult> CreatePlayer(string playerName)
        {
            try
            {
                if (IsPlayerNameAvailable(playerName) is false)
                {
                    return new ObjectResult(new BaseResponse(false, Helpers.ResponseMessages.PLAYER_NAME_TAKEN));
                }
                var nameValidationResult = Helpers.Validators.IsPlayerNameValid(playerName);
                if (nameValidationResult.Success is false)
                {
                    return new ObjectResult(nameValidationResult);
                }

                int nextPlayerId = (int)new GetNextPlayerIDQuery(_dataContext).Get().Value;

                return await new CreatePlayerCommand(_dataContext, new PlayerModel(nextPlayerId, playerName, DateTime.Now)).Execute();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ObjectResult(new BaseResponse(false, Helpers.ResponseMessages.CREATE_PLAYER_FAILED))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpPut("rename")]
        public async Task<ObjectResult> RenamePlayer([FromBody] PlayerModel player)
        {
            try
            {
                if (player.PlayerId == 0 || DoesPlayerIDExist(player.PlayerId) is false)
                {
                    return new ObjectResult(new BaseResponse(false, Helpers.ResponseMessages.PLAYER_ID_INVALID));
                }

                if (IsPlayerNameAvailable(player.PlayerName) is false)
                {
                    return new ObjectResult(new BaseResponse(false, Helpers.ResponseMessages.PLAYER_NAME_TAKEN));
                }

                var validateName = ValidatePlayerName(player.PlayerName);
                if (validateName.Success is false)
                {
                    return new ObjectResult(validateName);
                }

                return await new RenamePlayerCommand(_dataContext, player).Execute();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ObjectResult(new BaseResponse(false, Helpers.ResponseMessages.PLAYER_RENAME_FAILURE))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpDelete("remove")]
        public ObjectResult RemovePlayer(int playerId)
        {
            if (DoesPlayerIDExist(playerId) is false)
            {
                return new ObjectResult(new BaseResponse(false, Helpers.ResponseMessages.PLAYER_ID_INVALID));
            }

            return new ObjectResult(new object());
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

        private bool IsPlayerNameAvailable(string playerName) => new GetPlayerByNameQuery(_dataContext, playerName).Get().Value == null;

        private bool DoesPlayerIDExist(int playerId) => new GetPlayerByIdQuery(_dataContext, playerId).Get().Value != null;
    }
}