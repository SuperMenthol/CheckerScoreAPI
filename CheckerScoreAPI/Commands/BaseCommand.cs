using CheckerScoreAPI.Data.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace CheckerScoreAPI.Commands
{
    public abstract class BaseCommand
    {
        protected IDataContext _dataContext;

        public BaseCommand(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public abstract Task<ObjectResult> Execute();
    }
}