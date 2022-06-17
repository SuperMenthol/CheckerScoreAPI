using CheckerScoreAPI.Commands.MatchCommands;
using CheckerScoreAPI.Queries.PlayerQueries;
using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model;
using CheckerScoreAPI.Queries.MatchQueries;
using Microsoft.AspNetCore.Mvc;

namespace CheckerScoreAPI.Controllers
{
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IDataContext _dataContext;
        private readonly ILogger<MatchController> _logger;

        public MatchController(ILogger<MatchController> logger, IDataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        [HttpGet("getMatchResults")]
        public async Task<ObjectResult> GetPlayerResults(int playerId)
        {
            try
            {
                return await new GetPlayerResultsQueryAsync(_dataContext, playerId).Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ObjectResult(
                    new BaseResponse(false, Helpers.ResponseMessages.RESULTS_FAILURE))
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
            }
        }

        [HttpGet("getPlayerSummary")]
        public async Task<ObjectResult> GetPlayerSummary(int playerId)
        {
            try
            {
                if (DoesPlayerIDExist(playerId) is false)
                {
                    return new ObjectResult(new BaseResponse(false, Helpers.ResponseMessages.PLAYER_ID_INVALID));
                }
                return await new GetPlayerResultCardQueryAsync(_dataContext, playerId).Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ObjectResult(
                    new BaseResponse(false, Helpers.ResponseMessages.SUMMARY_FAILURE)) 
                        { 
                            StatusCode = StatusCodes.Status500InternalServerError 
                        };
            }
        }

        [HttpPost("postresult")]
        public async Task<ObjectResult> PostMatchResult(MatchResult result)
        {
            try
            {
                if (IsMatchResultValid(result) is false)
                {
                    return new ObjectResult(new BaseResponse(false, Helpers.ResponseMessages.PLAYER_ID_INVALID));
                }

                return await new PostMatchResultCommand(_dataContext, result).Execute();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ObjectResult(new BaseResponse(false, Helpers.ResponseMessages.MATCH_POST_FAILURE));
            }
        }

        private bool DoesPlayerIDExist(int playerId) => new GetPlayerByIdQuery(_dataContext, playerId).Get().Value != null;

        private bool IsMatchResultValid(MatchResult result)
        {
            var validPlayerIds = new int[] {result.Player1Id, result.Player2Id, 0};
            return DoesPlayerIDExist(result.Player1Id) && DoesPlayerIDExist(result.Player2Id)
                && validPlayerIds.Contains(result.WinnerID);
        }
    }
}