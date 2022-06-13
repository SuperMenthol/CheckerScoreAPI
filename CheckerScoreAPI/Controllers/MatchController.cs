using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model;
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
        public ActionResult GetPlayerResults(int playerId)
        {
            return new JsonResult(new object());
        }

        [HttpPost("postresult")]
        public ActionResult PostMatchResult(MatchResult result)
        {
            try
            {
                _dataContext.AddMatchResult(result.ToEntity());
                return new JsonResult(new BaseResponse(true, Helpers.ResponseMessages.MATCH_RESULT_POSTED));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new JsonResult(new BaseResponse(false, ex.ToString()));
            }
        }
    }
}