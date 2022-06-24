using Domain.Data.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Queries
{
    public abstract class BaseAsyncQuery
    {
        protected IDataContext _dataContext;
        public abstract Task<ObjectResult> Get();

        public BaseAsyncQuery(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}