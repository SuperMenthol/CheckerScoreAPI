using CheckerScoreAPI.Data.Abstracts;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CheckerScoreAPI.Queries.PlayerQueries
{
    public class GetNextPlayerIDQuery : BaseSyncQuery
    {
        public GetNextPlayerIDQuery(IDataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }
        public override ObjectResult Get()
        {
            int result = _dataContext.Players().AsQueryable()
                .OrderByDescending(p => p.PlayerId)
                .FirstOrDefault()?.PlayerId + 1 ?? 1;

            return new ObjectResult(result);
        }
    }
}