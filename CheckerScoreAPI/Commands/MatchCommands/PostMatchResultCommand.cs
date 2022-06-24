using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace CheckerScoreAPI.Commands.MatchCommands
{
    public class PostMatchResultCommand : BaseCommand
    {
        private readonly MatchResult _result;

        public PostMatchResultCommand(IDataContext dataContext, MatchResult result) : base(dataContext)
        {
            _dataContext = dataContext;
            _result = result;
        }

        public override async Task<ObjectResult> Execute()
        {
            var entity = _result.ToEntity();

            await _dataContext.Results.InsertOneAsync(entity);

            return new ObjectResult(BaseResponse.GetResponse<object>(true, Helpers.ResponseMessages.MATCH_RESULT_POSTED, true));
        }
    }
}