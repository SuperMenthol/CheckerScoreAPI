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
        public ObjectResult PostMatchResult(MatchResult result)
        {
            try
            {
                _dataContext.AddMatchResult(result.ToEntity());
                return new ObjectResult(new BaseResponse(true, Helpers.ResponseMessages.MATCH_RESULT_POSTED));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ObjectResult(new BaseResponse(false, ex.ToString()));
            }
        }
    }
}