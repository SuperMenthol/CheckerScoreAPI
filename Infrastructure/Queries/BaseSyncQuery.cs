using Domain.Data.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Queries
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