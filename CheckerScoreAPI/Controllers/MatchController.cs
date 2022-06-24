using Domain.Data.Abstracts;
using Infrastructure.Commands.MatchCommands;
using Infrastructure.Helpers;
using Infrastructure.Model;
using Infrastructure.Queries.MatchQueries;
using Infrastructure.Queries.PlayerQueries;
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
        public async Task<BaseResponse<List<MatchResult>>> GetPlayerResults(int playerId)
        {
            try
            {
                var result = await new GetPlayerResultsQueryAsync(_dataContext, playerId).Get();

                return BaseResponse.GetResponse(true, ResponseMessages.RESULTS_SUCCESS, (List<MatchResult>)result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BaseResponse.GetResponse<List<MatchResult>>(false, ResponseMessages.RESULTS_FAILURE);
            }
        }

        [HttpGet("getPlayerSummary")]
        public async Task<BaseResponse<PlayerResultModel>> GetPlayerSummary(int playerId)
        {
            try
            {
                if (DoesPlayerIDExist(playerId) is false)
                {
                    return BaseResponse.GetResponse<PlayerResultModel>(false, ResponseMessages.PLAYER_ID_INVALID, new());
                }

                var result = await new GetPlayerResultCardQueryAsync(_dataContext, playerId).Get();

                return BaseResponse.GetResponse(true, ResponseMessages.RESULTS_SUCCESS, (PlayerResultModel)result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BaseResponse.GetResponse<PlayerResultModel>(false, ResponseMessages.SUMMARY_FAILURE, new());
            }
        }

        [HttpPost("postresult")]
        public async Task<BaseResponse<object>> PostMatchResult([FromBody] MatchResult matchResult)
        {
            try
            {
                if (IsMatchResultValid(matchResult) is false)
                {
                    return BaseResponse.GetResponse<object>(false, ResponseMessages.PLAYER_ID_INVALID, false);
                }

                var result = await new PostMatchResultCommand(_dataContext, matchResult).Execute();

                return (BaseResponse<object>)result.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BaseResponse.GetResponse<object>(false, ResponseMessages.MATCH_POST_FAILURE, false);
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