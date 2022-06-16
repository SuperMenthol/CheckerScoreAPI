using CheckerScoreAPI.Data.Abstracts;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CheckerScoreAPI.Queries
{
    public abstract class BaseSyncQuery
    {
        protected IDataContext _dataContext;
        public abstract ObjectResult Get();

        public BaseSyncQuery(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}